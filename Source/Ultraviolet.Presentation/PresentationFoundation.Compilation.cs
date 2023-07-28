using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Gets the data source wrapper that exposes the specified object's compiled binding expressions, if it has one.
        /// </summary>
        /// <param name="obj">The object for which to retrieve a data source wrapper.</param>
        /// <returns>The data source wrapper for the specified object, or a reference to the original object if it does not have a data source wrapper.</returns>
        public static Object GetDataSourceWrapper(Object obj)
        {
            if (obj is Control control)
            {
                return control.DataSourceWrapper;
            }
            return obj;
        }

        /// <summary>
        /// Creates a data source wrapper instance for a view with the specified view model, if such a wrapper exists.
        /// </summary>
        /// <param name="viewModel">The view model instance that will be wrapped by the data source wrapper.</param>
        /// <param name="namescope">The view model namescope.</param>
        /// <returns>The data source wrapper that was created, or a reference to <paramref name="viewModel"/> if no such wrapper exists.</returns>
        public Object CreateDataSourceWrapperForView(Object viewModel, Namescope namescope)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(namescope, nameof(namescope));

            if (viewModel == null)
                return null;

            Type wrapperType;
            if (!compiledDataSourceWrappersByWrappedType.TryGetValue(viewModel.GetType(), out wrapperType))
            {
                var vmWrapperAttr = viewModel.GetType().GetCustomAttributes(typeof(ViewModelWrapperAttribute), false).Cast<ViewModelWrapperAttribute>().SingleOrDefault();
                wrapperType = (vmWrapperAttr == null) ? null : vmWrapperAttr.WrapperType;
                compiledDataSourceWrappersByWrappedType[viewModel.GetType()] = wrapperType;
            }

            try
            {
                return (wrapperType == null) ? viewModel : Activator.CreateInstance(wrapperType, new Object[] { viewModel, namescope });
            }
            catch (MissingMethodException e)
            {
                var dataSourceField = wrapperType.GetField("value", BindingFlags.NonPublic | BindingFlags.Instance);
                if (dataSourceField != null)
                {
                    var typeExpected = dataSourceField.FieldType;
                    var typeProvided = viewModel.GetType();
                    throw new InvalidOperationException(PresentationStrings.ViewModelMismatch.Format(typeExpected, typeProvided), e);
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a new view model wrapper instance for the specified control's component template.
        /// </summary>
        /// <param name="viewModel">The control for which to create a view model wrapper.</param>
        /// <returns>The view model wrapper that was created, or a reference to <paramref name="viewModel"/> if no valid wrapper exists.</returns>
        public Object CreateDataSourceWrapperForControl(Control viewModel)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(viewModel, nameof(viewModel));

            if (viewModel == null)
                return null;

            var wrapperType = default(Type);
            var templateType = viewModel.GetType();
            var templateInherited = false;

            for (var current = templateType; current != null; current = current.BaseType)
            {
                if (compiledDataSourceWrappersByWrappedType.TryGetValue(current, out wrapperType))
                    break;

                templateInherited = true;
            }

            if (wrapperType != null && templateInherited)
                compiledDataSourceWrappersByWrappedType[templateType] = wrapperType;

            return (wrapperType == null) ? viewModel : Activator.CreateInstance(wrapperType, new Object[] { viewModel, viewModel.ComponentTemplateNamescope });
        }

        /// <summary>
        /// Gets the data source wrapper type for the specified wrapped type.
        /// </summary>
        /// <param name="dataSourceType">The data source type which is wrapped.</param>
        /// <returns>The data source wrapper type for the specified wrapped type.</returns>
        public Type GetDataSourceWrapperType(Type dataSourceType)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dataSourceType, nameof(dataSourceType));

            Type wrapperType;
            compiledDataSourceWrappersByWrappedType.TryGetValue(dataSourceType, out wrapperType);
            return wrapperType;
        }

        /// <summary>
        /// Compiles the binding expressions in the specified content directory tree.
        /// Compilation is performed on a background thread.
        /// </summary>
        /// <param name="root">The root of the content directory tree to search for binding expressions to compile.</param>
        /// <param name="flags">A set of <see cref="CompileExpressionsFlags"/> values specifying how the expressions should be compiled.</param>
        /// <returns>A <see cref="Task"/> representing the compilation process.</returns>
        public async Task CompileExpressionsAsync(String root, CompileExpressionsFlags flags = CompileExpressionsFlags.None)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.RequireNotEmpty(root, nameof(root));

            if (!IsSupportedPlatform(Ultraviolet.Runtime, Ultraviolet.Platform))
                throw new NotSupportedException();

            LoadBindingExpressionCompiler();

            var options = CreateCompilerOptions(root, flags);
            try
            {
                var result = await Task.Run(() => bindingExpressionCompiler.Compile(Ultraviolet, options));
                if (result.Failed)
                    throw new BindingExpressionCompilationFailedException(result.Message, result);

                inMemoryBindingExpressionsAsm = result.Assembly;                
            }
            catch (Exception e)
            {
                LogExceptionToBuildOutputConsole(root, e,
                    (flags & CompileExpressionsFlags.ResolveContentFiles) == CompileExpressionsFlags.ResolveContentFiles);
                throw;
            }

            GC.Collect(2, GCCollectionMode.Forced);
        }

        /// <summary>
        /// Compiles the binding expressions in the specified content directory tree if the application is running
        /// one one of the platforms which supports doing so. Otherwise, this method has no effect.
        /// Compilation is performed on a background thread.
        /// </summary>
        /// <param name="root">The root of the content directory tree to search for binding expressions to compile.</param>
        /// <param name="flags">A set of <see cref="CompileExpressionsFlags"/> values specifying how the expressions should be compiled.</param>
        public async Task CompileExpressionsIfSupportedAsync(String root, CompileExpressionsFlags flags = CompileExpressionsFlags.None)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.RequireNotEmpty(root, nameof(root));

            if (!IsSupportedPlatform(Ultraviolet.Runtime, Ultraviolet.Platform))
                return;

            await CompileExpressionsAsync(root, flags);
        }

        /// <summary>
        /// Compiles the binding expressions in the specified content directory tree.
        /// </summary>
        /// <param name="root">The root of the content directory tree to search for binding expressions to compile.</param>
        /// <param name="flags">A set of <see cref="CompileExpressionsFlags"/> values specifying how the expressions should be compiled.</param>
        public void CompileExpressions(String root, CompileExpressionsFlags flags = CompileExpressionsFlags.None)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.RequireNotEmpty(root, nameof(root));

            if (!IsSupportedPlatform(Ultraviolet.Runtime, Ultraviolet.Platform))
                throw new NotSupportedException();

            LoadBindingExpressionCompiler();

            var options = CreateCompilerOptions(root, flags);
            try
            {
                var result = bindingExpressionCompiler.Compile(Ultraviolet, options);
                if (result.Failed)
                    throw new BindingExpressionCompilationFailedException(result.Message, result);

                inMemoryBindingExpressionsAsm = result.Assembly;
            }
            catch (Exception e)
            {
                LogExceptionToBuildOutputConsole(root, e,
                    (flags & CompileExpressionsFlags.ResolveContentFiles) == CompileExpressionsFlags.ResolveContentFiles);
                throw;
            }

            GC.Collect(2, GCCollectionMode.Forced);
        }
        
        /// <summary>
        /// Compiles the binding expressions in the specified content directory tree if the application is running
        /// one one of the platforms which supports doing so. Otherwise, this method has no effect.
        /// </summary>
        /// <param name="root">The root of the content directory tree to search for binding expressions to compile.</param>
        /// <param name="flags">A set of <see cref="CompileExpressionsFlags"/> values specifying how the expressions should be compiled.</param>
        public void CompileExpressionsIfSupported(String root, CompileExpressionsFlags flags = CompileExpressionsFlags.None)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.RequireNotEmpty(root, nameof(root));

            if (!IsSupportedPlatform(Ultraviolet.Runtime, Ultraviolet.Platform))
                return;

            CompileExpressions(root, flags);
        }

        /// <summary>
        /// Loads the assembly that contains the application's compiled binding expressions.
        /// </summary>
        public void LoadCompiledExpressions()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Assembly compiledExpressionsAssembly;
            try
            {
                if (inMemoryBindingExpressionsAsm != null)
                {
                    compiledExpressionsAssembly = inMemoryBindingExpressionsAsm;
                }
                else
                {
                    var loader = AssemblyLoaderService.Create();
                    compiledExpressionsAssembly = loader.Load(CompiledExpressionsAssemblyName);
                }
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException(PresentationStrings.CompiledExpressionsAssemblyNotFound, e);
            }

            compiledDataSourceWrappersByWrappedType.Clear();
            foreach (var dataSourceWrapperType in compiledExpressionsAssembly.GetExportedTypes())
            {
                var wrapperAttr = (WrappedDataSourceAttribute)dataSourceWrapperType.GetCustomAttributes(typeof(WrappedDataSourceAttribute), false).SingleOrDefault();
                if (wrapperAttr != null)
                    compiledDataSourceWrappersByWrappedType[wrapperAttr.WrappedType] = dataSourceWrapperType;
            }
        }
        
        /// <summary>
        /// Gets or sets the name of the assembly from which to load the binding expressions compiler.
        /// </summary>
        public String BindingExpressionCompilerAssemblyName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingExpressionCompilerOptions"/> class from the specified set of flags.
        /// </summary>
        private static BindingExpressionCompilerOptions CreateCompilerOptions(String root, CompileExpressionsFlags flags)
        {
            var options = new BindingExpressionCompilerOptions()
            {
                GenerateInMemory = flags.HasFlag(CompileExpressionsFlags.GenerateInMemory),
                WriteErrorsToFile = true,
                WriteCompiledFilesToWorkingDirectory = flags.HasFlag(CompileExpressionsFlags.WriteCompiledFilesToWorkingDirectory),
                Input = root,
                Output = CompiledExpressionsAssemblyName,
                IgnoreCache = flags.HasFlag(CompileExpressionsFlags.GenerateInMemory) || flags.HasFlag(CompileExpressionsFlags.IgnoreCache),
                WorkInTemporaryDirectory = flags.HasFlag(CompileExpressionsFlags.WorkInTemporaryDirectory),
            };
            return options;
        }

        /// <summary>
        /// Gets a value indicating whether the specified platform supports expression compilation.
        /// </summary>
        private static Boolean IsSupportedPlatform(UltravioletRuntime runtime, UltravioletPlatform platform)
        {
            // NOTE: This is checking to see if we're a .NET Native application.
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null && String.IsNullOrEmpty(entryAssembly.Location))
                return false;

            return platform != UltravioletPlatform.Android && platform != UltravioletPlatform.iOS;
        }

        /// <summary>
        /// Logs an exception to the console so that it will be displayed by Visual Studio's Error List window.
        /// </summary>
        private void LogExceptionToBuildOutputConsole(String root, Exception exception, Boolean resolveContentFiles)
        {
            if (exception is AggregateException)
            {
                foreach (var innerException in ((AggregateException)exception).InnerExceptions)
                {
                    LogExceptionToBuildOutputConsole(root, innerException, resolveContentFiles);
                }
            }
            else
            {
                var exeAssembly = Assembly.GetEntryAssembly();
                var exeName = (exeAssembly != null) ? Assembly.GetEntryAssembly().GetName().Name :
                    System.Diagnostics.Process.GetCurrentProcess().ProcessName;

                if (exception is BindingExpressionCompilationFailedException)
                {
                    foreach (var error in ((BindingExpressionCompilationFailedException)exception).Result.Errors)
                    {
                        var filename = !String.IsNullOrEmpty(error.Filename) && resolveContentFiles ?
                            UltravioletDebugUtil.GetOriginalContentFilePath(root, error.Filename) : error.Filename ?? exeName;

                        Console.WriteLine("{0}({1},{2}): expression compiler error {3}: {4}", filename, error.Line, error.Column, error.ErrorNumber, error.ErrorText);
                    }
                    return;
                }

                if (exception is BindingExpressionCompilationErrorException)
                {
                    var error = ((BindingExpressionCompilationErrorException)exception).Error;
                    var filename = !String.IsNullOrEmpty(error.Filename) && resolveContentFiles ?
                        UltravioletDebugUtil.GetOriginalContentFilePath("Content", error.Filename) : error.Filename ?? exeName;

                    Console.WriteLine("{0}({1},{2}): expression compiler error {3}: {4}", filename, error.Line, error.Column, error.ErrorNumber, error.ErrorText);
                    return;
                }

                Console.WriteLine("{0}: expression compiler error 1: {1}", exeName, exception.Message);
            }
        }

        /// <summary>
        /// Loads the assembly which provides binding expression compilation services.
        /// </summary>
        private void LoadBindingExpressionCompiler()
        {
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
                throw new NotSupportedException();

            if (bindingExpressionCompiler != null)
                return;

            var compilerAsmName = BindingExpressionCompilerAssemblyName;
            if (String.IsNullOrEmpty(compilerAsmName))
                throw new InvalidOperationException(PresentationStrings.InvalidBindingExpressionCompilerAsm);

            Assembly compilerAsm = null;
            try
            {
                compilerAsm = Assembly.Load(compilerAsmName);
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException(PresentationStrings.ExpressionCompilerNotFound.Format(compilerAsmName), e);
            }

            var compilerType = compilerAsm.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IBindingExpressionCompiler)))
                .Where(x => x.GetCustomAttribute(typeof(ObsoleteAttribute)) == null)
                .FirstOrDefault();
            if (compilerType == null || compilerType.IsAbstract)
            {
                throw new InvalidOperationException(PresentationStrings.ExpressionCompilerTypeNotValid);
            }

            try
            {
                bindingExpressionCompiler = (IBindingExpressionCompiler)Activator.CreateInstance(compilerType);
            }
            catch (MissingMethodException e)
            {
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(compilerType.Name), e);
            }
        }

        /// <summary>
        /// Performs platform-specific steps to load the compiled expresisons assembly.
        /// </summary>
        partial void LoadCompiledExpressions_Android(ref Assembly asm);        

        // The registry of compiled data source wrappers.
        private readonly Dictionary<Type, Type> compiledDataSourceWrappersByWrappedType =
            new Dictionary<Type, Type>();

        // The compiler used to compile the game's binding expressions.
        private const String CompiledExpressionsAssemblyName = "Ultraviolet.Presentation.CompiledExpressions.dll";
        private IBindingExpressionCompiler bindingExpressionCompiler;

        // The assembly which contains the game's binding expressions.
        private Assembly inMemoryBindingExpressionsAsm;
    }
}
