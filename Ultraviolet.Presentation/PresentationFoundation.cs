using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the state of the Ultraviolet Presentation Foundation.
    /// </summary>
    public partial class PresentationFoundation : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="PresentationFoundation"/> type.
        /// </summary>
        static PresentationFoundation()
        {
            CommandManager.RegisterValueResolvers();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundation"/> class.
        /// </summary>
        internal PresentationFoundation(UltravioletContext uv)
            : base(uv)
        {
            RuntimeHelpers.RunClassConstructor(typeof(Tweening).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(SimpleClockPool).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(StoryboardClockPool).TypeHandle);

            RegisterCoreTypes();

            this.outOfBandRenderer = uv.IsRunningInServiceMode ? null : new OutOfBandRenderer(uv);

            this.styleQueue = new LayoutQueue(InvalidateStyle, false);
            this.measureQueue = new LayoutQueue(InvalidateMeasure);
            this.arrangeQueue = new LayoutQueue(InvalidateArrange);          
        }

        /// <summary>
        /// Modifies the specified <see cref="UltravioletConfiguration"/> instance so that the Ultraviolet
        /// Presentation Foundation will be registered as the context's view provider.
        /// </summary>
        /// <param name="ultravioletConfig">The <see cref="UltravioletConfiguration"/> instance to modify.</param>
        /// <param name="presentationConfig">Configuration settings for the Ultraviolet Presentation Foundation.</param>
        public static void Configure(UltravioletConfiguration ultravioletConfig, PresentationFoundationConfiguration presentationConfig = null)
        {
            Contract.Require(ultravioletConfig, nameof(ultravioletConfig));

            ultravioletConfig.ViewProviderAssembly = typeof(PresentationFoundation).Assembly.FullName;
            ultravioletConfig.ViewProviderConfiguration = presentationConfig;
        }

        /// <summary>
        /// Draws a diagnostics panel containing various Presentation Foundation performance metrics.
        /// </summary>
        public void DrawDiagnosticsPanel()
        {
            if (diagnosticsPanel == null)
                diagnosticsPanel = new DiagnosticsPanel(Ultraviolet);

            diagnosticsPanel.Draw();
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
        /// </summary>
        /// <param name="root">The root of the content directory tree to search for binding expressions to compile.</param>
        /// <param name="flags">A set of <see cref="CompileExpressionsFlags"/> values specifying how the expressions should be compiled.</param>
        public void CompileExpressions(String root, CompileExpressionsFlags flags = CompileExpressionsFlags.None)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.RequireNotEmpty(root, nameof(root));

            LoadBindingExpressionCompiler();

            var options = new BindingExpressionCompilerOptions();
            options.GenerateInMemory = (flags & CompileExpressionsFlags.GenerateInMemory) == CompileExpressionsFlags.GenerateInMemory;
            options.WriteErrorsToFile = true;
            options.Input = root;
            options.Output = CompiledExpressionsAssemblyName;
            options.IgnoreCache = options.GenerateInMemory || (flags & CompileExpressionsFlags.IgnoreCache) == CompileExpressionsFlags.IgnoreCache;

            try
            {
                var result = bindingExpressionCompiler.Compile(Ultraviolet, options);
                if (result.Failed)
                    throw new BindingExpressionCompilationFailedException(result.Message, result);

                inMemoryBindingExpressionsAsm = result.Assembly;
            }
            catch (Exception e)
            {
                var resolveContentFiles = (flags & CompileExpressionsFlags.ResolveContentFiles) == CompileExpressionsFlags.ResolveContentFiles;
                LogExceptionToBuildOutputConsole(root, e, resolveContentFiles);
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

            if (Ultraviolet.Platform == UltravioletPlatform.Android ||
                Ultraviolet.Platform == UltravioletPlatform.iOS)
            {
                return;
            }
            CompileExpressions(root, flags);
        }

        /// <summary>
        /// Loads the assembly that contains the application's compiled binding expressions.
        /// </summary>
        public void LoadCompiledExpressions()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Assembly compiledExpressionsAssembly = null;
            try
            {
                if (inMemoryBindingExpressionsAsm != null)
                {
                    compiledExpressionsAssembly = inMemoryBindingExpressionsAsm;
                }
                else
                {
                    switch (Ultraviolet.Platform)
                    {
                        case UltravioletPlatform.Windows:
                        case UltravioletPlatform.Linux:
                        case UltravioletPlatform.macOS:
                            compiledExpressionsAssembly = Assembly.LoadFrom(CompiledExpressionsAssemblyName);
                            break;

                        case UltravioletPlatform.Android:
                        case UltravioletPlatform.iOS:
                            compiledExpressionsAssembly = Assembly.Load(CompiledExpressionsAssemblyName);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException(PresentationStrings.CompiledExpressionsAssemblyNotFound, e);
            }
            
            compiledDataSourceWrappersByWrappedType.Clear();
            foreach (var dataSourceWrapperType in compiledExpressionsAssembly.GetTypes())
            {
                var wrapperAttr = (WrappedDataSourceAttribute)dataSourceWrapperType.GetCustomAttributes(typeof(WrappedDataSourceAttribute), false).SingleOrDefault();
                if (wrapperAttr != null)
                    compiledDataSourceWrappersByWrappedType[wrapperAttr.WrappedType] = dataSourceWrapperType;
            }
        }

        /// <summary>
        /// Gets the collection of types which are known to the Presentation Foundation.
        /// </summary>
        /// <returns>A dictionary containing the types which are known to the Presentation Foundation, using
        /// the UVML names of the types as the dictionary key.</returns>
        public IDictionary<String, Type> GetKnownTypes()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var types = new Dictionary<String, Type>();

            foreach (var kvp in coreTypes)
                types[kvp.Key] = kvp.Value.Type;

            foreach (var kvp in registeredTypes)
                types[kvp.Key] = kvp.Value.Type;

            return types;
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to which the element will be bound.</typeparam>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <returns>The element that was created, or <see langword="null"/> if the element could not be created.</returns>
        public UIElement InstantiateElementByName<TViewModel>(String typeName, String name)
        {
            return InstantiateElementByName(typeName, name, typeof(TViewModel));
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <returns>The element that was created, or <see langword="null"/> if the element could not be created.</returns>
        public UIElement InstantiateElementByName(String typeName, String name, Type viewModelType)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            
            KnownElement registration;
            if (!GetKnownElementRegistration(typeName, out registration))
                return null;

            var isFrameworkElement = typeof(FrameworkElement).IsAssignableFrom(registration.Type);

            var ctor = isFrameworkElement ?                
                registration.Type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) }) :
                registration.Type.GetConstructor(new[] { typeof(UltravioletContext) });

            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(registration.Type));

            var instance = (UIElement)ctor.Invoke(isFrameworkElement ? 
                new Object[] { Ultraviolet, name } : 
                new Object[] { Ultraviolet });
            
            return instance;
        }
        
        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownType(String name, out Type type)
        {
            return GetKnownType(name, true, out type);
        }

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="isCaseSensitive">A value indicating whether the resolution of the type name is case-sensitive.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownType(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.EnsureNotDisposed(this, Disposed);

            type = null;

            KnownType registration;
            if (!GetKnownTypeRegistration(name, out registration))
                return false;

            if (isCaseSensitive && !String.Equals(name, registration.Name, StringComparison.Ordinal))
                return false;

            type = registration.Type;
            return true;
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><see langword="true"/> if the specified element type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownElement(String name, out Type type)
        {
            return GetKnownElement(name, true, out type);
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="isCaseSensitive">A value indicating whether the resolution of the element name is case-sensitive.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><see langword="true"/> if the specified element type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownElement(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.EnsureNotDisposed(this, Disposed);

            type = null;

            KnownElement registration;
            if (!GetKnownElementRegistration(name, out registration))
                return false;

            if (isCaseSensitive && !String.Equals(name, registration.Name, StringComparison.Ordinal))
                return false;

            type = registration.Type;
            return true;
        }

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="name">The name of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetElementDefaultProperty(String name, out String property)
        {
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.EnsureNotDisposed(this, Disposed);

            property = null;

            KnownElement registration;
            if (!GetKnownElementRegistration(name, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="type">The type of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetElementDefaultProperty(Type type, out String property)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            property = null;

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }

        /// <summary>
        /// Registers any UVML-known types in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly for which to register known types.</param>
        public void RegisterKnownTypes(Assembly asm)
        {
            Contract.Require(asm, nameof(asm));
            
            var knownTypes = from t in asm.GetTypes()
                             let attr = t.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault()
                             where
                              attr != null
                             select t;

            foreach (var knownType in knownTypes)
            {
                RegisterElementInternal(registeredTypes, knownType, null);
            }
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        public void RegisterElement(Type type)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            RegisterElementInternal(registeredTypes, type, null);
        }
        
        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <returns><see langword="true"/> if the custom element was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterKnownType(Type type)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            KnownType registration;
            if (!GetKnownTypeRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The element type to unregister.</param>
        /// <returns><see langword="true"/> if the custom element was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterKnownElement(Type type)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Attempts to set the global style sheet used by all Presentation Foundation views. If any exceptions are thrown
        /// during this process, the previous style sheet will be automatically restored.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        /// <returns><see langword="true"/> if the style sheet was set successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean TrySetGlobalStyleSheet(UvssDocument styleSheet)
        {
            var previous = GlobalStyleSheet;
            try
            {
                SetGlobalStyleSheet(styleSheet);
            }
            catch
            {
                SetGlobalStyleSheet(previous);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the global style sheet used by all Presentation Foundation views.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        public void SetGlobalStyleSheet(UvssDocument styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.globalStyleSheet = styleSheet;
            OnGlobalStyleSheetChanged();
        }        

        /// <summary>
        /// Initializes the specified collection of the Presentation Foundation's internal object pools.
        /// </summary>
        /// <param name="pools">A collection of <see cref="InternalPool"/> values indicating which pools to initialize.</param>
        /// <remarks>In order to save memory, the Presentation Foundation does not initialize any of its internal object pools until 
        /// it determines that they are required. This method allows an application to manually initialize any pools which it
        /// knows ahead of time that it's going to need.</remarks>
        public void InitializeInternalPools(params InternalPool[] pools)
        {
            if (pools == null)
                return;

            foreach (var pool in pools)
            {
                switch (pool)
                {
                    case InternalPool.SimpleClocks:
                        SimpleClockPool.Instance.Initialize();
                        break;

                    case InternalPool.StoryboardInstances:
                        StoryboardInstancePool.Instance.Initialize();
                        break;

                    case InternalPool.StoryboardClocks:
                        StoryboardClockPool.Instance.Initialize();
                        break;

                    case InternalPool.OutOfBandRenderer:
                        OutOfBandRenderer.InitializePools();
                        break;

                    case InternalPool.WeakReferences:
                        WeakReferencePool.Instance.Initialize();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the performance statistics which have been collected by the Ultraviolet Presentation Foundation.
        /// </summary>
        public PresentationFoundationPerformanceStats PerformanceStats
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return performanceStats; 
            }
        }

        /// <summary>
        /// Gets the Presentation Foundation's component template manager.
        /// </summary>
        public ComponentTemplateManager ComponentTemplates
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return componentTemplateManager; 
            }
        }

        /// <summary>
        /// Gets the current global style sheet.
        /// </summary>
        public UvssDocument GlobalStyleSheet
        {
            get { return globalStyleSheet; }
        }

        /// <summary>
        /// Occurs when the Presentation Foundation's global style sheet is changed.
        /// </summary>
        public event EventHandler GlobalStyleSheetChanged;

        /// <summary>
        /// Gets the data source wrapper that exposes the specified object's compiled binding expressions, if it has one.
        /// </summary>
        /// <param name="obj">The object for which to retrieve a data source wrapper.</param>
        /// <returns>The data source wrapper for the specified object, or a reference to the original object if it does not have a data source wrapper.</returns>
        internal static Object GetDataSourceWrapper(Object obj)
        {
            var control = obj as Control;
            if (control != null)
            {
                return control.DataSourceWrapper;
            }
            return obj;
        }

        /// <summary>
        /// Performs the layout process for any elements which currently need it.
        /// </summary>
        internal void PerformLayout()
        {
            using (UltravioletProfiler.Section(PresentationProfilerSections.Layout))
            {
                while (ElementNeedsLayout)
                {
                    // 1. Style
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Style))
                    {
                        while (ElementNeedsStyle)
                        {
                            var element = styleQueue.Dequeue();
                            if (element.IsStyleValid || element.View == null)
                                continue;

                            element.Style(element.View.StyleSheet);
                            element.InvalidateMeasure();
                        }
                    }

                    // 2. Measure
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Measure))
                    {
                        while (ElementNeedsMeasure && !ElementNeedsStyle)
                        {
                            var element = measureQueue.Dequeue();
                            if (element.IsMeasureValid || element.View == null)
                                continue;

                            element.Measure(element.MostRecentAvailableSize);
                            element.InvalidateArrange();
                        }
                    }

                    // 3. Arrange
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Arrange))
                    {
                        while (ElementNeedsArrange && !ElementNeedsStyle && !ElementNeedsMeasure)
                        {
                            var element = arrangeQueue.Dequeue();
                            if (element.IsArrangeValid || element.View == null)
                                continue;

                            element.Arrange(element.MostRecentFinalRect, element.MostRecentArrangeOptions);
                        }
                    }

                    // 4. Raise events.
                    if (ElementNeedsLayout)
                        continue;

                    RaiseRenderSizeChanged();

                    if (ElementNeedsLayout)
                        continue;

                    RaiseLayoutUpdated();
                }
            }
        }

        /// <summary>
        /// Removes the specified UI element from all of the Foundation's processing queues.
        /// </summary>
        /// <param name="element">The element to remove from the queues.</param>
        internal void RemoveFromQueues(UIElement element)
        {
            Contract.Require(element, nameof(element));

            StyleQueue.Remove(element);
            MeasureQueue.Remove(element);
            ArrangeQueue.Remove(element);
        }

        /// <summary>
        /// Adds the specified element to the queue of elements which will receive a RenderSizeChanged event.
        /// </summary>
        /// <param name="element">The element to register.</param>
        /// <param name="previousSize">The previous size of the element.</param>
        internal void RegisterRenderSizeChanged(UIElement element, Size2D previousSize)
        {
            Contract.Require(element, nameof(element));

            var entry = new RenderSizeChangedEntry(element, previousSize);
            elementsPendingRenderSizeChanged.AddLast(entry);
        }

        /// <summary>
        /// Indicates that the specified element is interested in receiving <see cref="UIElement.LayoutUpdated"/> events.
        /// </summary>
        /// <param name="element">The element to register.</param>
        internal void RegisterForLayoutUpdated(UIElement element)
        {
            elementsWithLayoutUpdatedHandlers.AddLast(element);
        }

        /// <summary>
        /// Indicates that the specified element is no longer interested in receiving <see cref="UIElement.LayoutUpdated"/> events.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        internal void UnregisterForLayoutUpdated(UIElement element)
        {
            elementsWithLayoutUpdatedHandlers.Remove(element);
        }

        /// <summary>
        /// Gets the singleton instance of the Presentation Foundation.
        /// </summary>
        internal static PresentationFoundation Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the identifier of the current digest cycle.
        /// </summary>
        internal Int64 DigestCycleID
        {
            get { return digestCycleID; }
        }

        /// <summary>
        /// Gets the renderer which is used to draw elements out-of-band.
        /// </summary>
        internal OutOfBandRenderer OutOfBandRenderer
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return outOfBandRenderer;
            }
        }

        /// <summary>
        /// Gets the queue of elements with invalid styling states.
        /// </summary>
        internal LayoutQueue StyleQueue
        {
            get { return styleQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid measurement states.
        /// </summary>
        internal LayoutQueue MeasureQueue
        {
            get { return measureQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid arrangement states.
        /// </summary>
        internal LayoutQueue ArrangeQueue
        {
            get { return arrangeQueue; }
        }

        /// <summary>
        /// Gets or sets the name of the assembly from which to load the binding expressions compiler.
        /// </summary>
        internal String BindingExpressionCompilerAssemblyName
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(outOfBandRenderer);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Logs an exception to the console so that it will be displayed by Visual Studio's Error List window.
        /// </summary>
        private static void LogExceptionToBuildOutputConsole(String root, Exception exception, Boolean resolveContentFiles)
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

            var compilerType = compilerAsm.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IBindingExpressionCompiler))).FirstOrDefault();
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
        /// Called when the Ultraviolet context blah blah blah
        /// </summary>
        /// <param name="uv"></param>
        private void OnFrameStart(UltravioletContext uv)
        {
            PerformanceStats.OnFrameStart();
        }

        /// <summary>
        /// Called when the Ultraviolet context is about to update its subsystems.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdatingSubsystems(UltravioletContext uv, UltravioletTime time)
        {
            digestCycleID++;
        }

        /// <summary>
        /// Called when the Ultraviolet UI subsystem is being updated.
        /// </summary>
        /// <param name="subsystem">The Ultraviolet subsystem.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdatingUI(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            PerformanceStats.BeginUpdate();

            PerformLayout();

            if (OutOfBandRenderer != null)
                OutOfBandRenderer.Update();

            PerformanceStats.EndUpdate();
        }

        /// <summary>
        /// Called when the Ultraviolet context is about to draw a frame.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void OnDrawing(UltravioletContext uv, UltravioletTime time)
        {
            if (OutOfBandRenderer != null)
                OutOfBandRenderer.DrawRenderTargets(time);
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="registry">The registry to which to add the element.</param>
        /// <param name="type">The type that implements the custom element.</param>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        private void RegisterElementInternal(Dictionary<String, KnownType> registry, Type type, XDocument layout)
        {
            var knownTypeAttr = (UvmlKnownTypeAttribute)type.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault();
            if (knownTypeAttr == null)
                throw new InvalidOperationException(PresentationStrings.KnownTypeMissingAttribute.Format(type.Name));

            var knownTypeName = knownTypeAttr.Name ?? type.Name;

            KnownType existingRegistration;
            if (GetKnownTypeRegistration(knownTypeName, out existingRegistration))
                throw new InvalidOperationException(PresentationStrings.KnownTypeAlreadyRegistered.Format(knownTypeName));

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            KnownType registration;
            if (typeof(UIElement).IsAssignableFrom(type))
            {
                registration = CreateKnownElementRegistration(type, knownTypeAttr);
            }
            else
            {
                registration = CreateKnownTypeRegistration(type, knownTypeAttr);
            }
            
            registry[registration.Name] = registration;
        }

        /// <summary>
        /// Registers any known types which are defined in the Presentation Foundation's core assembly.
        /// </summary>
        private void RegisterCoreTypes()
        {
            var knownTypes = from t in typeof(UIElement).Assembly.GetTypes()
                             let attr = t.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault()
                             where
                              attr != null
                             select t;

            foreach (var knownType in knownTypes)
            {
                RegisterElementInternal(coreTypes, knownType, null);
            }
        }

        /// <summary>
        /// Creates a known type registration for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a registration.</param>
        /// <param name="attr">The attribute that marks the type as a known type.</param>
        /// <returns>The <see cref="KnownType"/> registration that was created.</returns>
        private KnownType CreateKnownTypeRegistration(Type type, UvmlKnownTypeAttribute attr)
        {
            var registration = new KnownType(attr.Name ?? type.Name, type);
            return registration;
        }

        /// <summary>
        /// Creates a known element registration for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a registration.</param>
        /// <param name="attr">The attribute that marks the type as a known type.</param>
        /// <returns>The <see cref="KnownType"/> registration that was created.</returns>
        private KnownType CreateKnownElementRegistration(Type type, UvmlKnownTypeAttribute attr)
        {
            var defaultPropertyAttr  = (UvmlDefaultPropertyAttribute)type.GetCustomAttributes(typeof(UvmlDefaultPropertyAttribute), true).SingleOrDefault();
            var defaultProperty      = default(String);
            if (defaultPropertyAttr != null)
            {
                defaultProperty = defaultPropertyAttr.Name;
            }
            
            var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null && !type.IsAbstract)
                throw new InvalidOperationException(PresentationStrings.UIElementInvalidCtor.Format(type.Name));

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            var registration = new KnownElement(attr.Name ?? type.Name, type, defaultProperty);
            RegisterDefaultComponentTemplate(type, attr);

            return registration;
        }

        /// <summary>
        /// Gets the registration for the specified known type.
        /// </summary>
        /// <param name="name">The name of the known type for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known type with the specified name.</param>
        /// <returns><see langword="true"/> if a known type with the specified name exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownTypeRegistration(String name, out KnownType registration)
        {
            if (coreTypes.TryGetValue(name, out registration))
                return true;

            if (registeredTypes.TryGetValue(name, out registration))
                return true;

            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known type.
        /// </summary>
        /// <param name="type">The CLR type of the known type for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element associated with the specified CLR type.</param>
        /// <returns><see langword="true"/> if a known type associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownTypeRegistration(Type type, out KnownType registration)
        {
            foreach (var value in coreTypes.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            foreach (var value in registeredTypes.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known element.
        /// </summary>
        /// <param name="name">The name of the known element for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element with the specified name.</param>
        /// <returns><see langword="true"/> if a known element with the specified name exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownElementRegistration(String name, out KnownElement registration)
        {
            KnownType typeRegistration;
            if (GetKnownTypeRegistration(name, out typeRegistration) && typeRegistration is KnownElement)
            {
                registration = (KnownElement)typeRegistration;
                return true;
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known element.
        /// </summary>
        /// <param name="type">The CLR type of the known element for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element associated with the specified CLR type.</param>
        /// <returns><see langword="true"/> if a known element associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownElementRegistration(Type type, out KnownElement registration)
        {
            KnownType typeRegistration;
            if (GetKnownTypeRegistration(type, out typeRegistration) && typeRegistration is KnownElement)
            {
                registration = (KnownElement)typeRegistration;
                return true;
            }
            registration = null;
            return false;
        }
        
        /// <summary>
        /// Raises the <see cref="GlobalStyleSheetChanged"/> event.
        /// </summary>
        private void OnGlobalStyleSheetChanged() =>
            GlobalStyleSheetChanged?.Invoke(this, EventArgs.Empty);
        
        /// <summary>
        /// Invalidates the specified element's style.
        /// </summary>
        private void InvalidateStyle(UIElement element)
        {
            if (element.IsStyleValid)
            {
                element.InvalidateStyleInternal();
                PerformanceStats.InvalidateStyleCount++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's measure.
        /// </summary>
        private void InvalidateMeasure(UIElement element)
        {
            if (element.IsMeasureValid)
            {
                element.InvalidateMeasureInternal();
                PerformanceStats.InvalidateMeasureCount++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's arrangement.
        /// </summary>
        private void InvalidateArrange(UIElement element)
        {
            if (element.IsArrangeValid)
            {
                element.InvalidateArrangeInternal();
                PerformanceStats.InvalidateArrangeCount++;
            }
        }

        /// <summary>
        /// Registers the specified type's default component template, if it has one.
        /// </summary>
        /// <param name="type">The type that represents the element for which to register a component template.</param>
        /// <param name="uiElementAttr">The <see cref="UvmlKnownTypeAttribute"/> instance which is associated with the element type.</param>
        private void RegisterDefaultComponentTemplate(Type type, UvmlKnownTypeAttribute uiElementAttr)
        {
            if (String.IsNullOrEmpty(uiElementAttr.ComponentTemplate))
                return;

            var asm = type.Assembly;

            using (var stream = asm.GetManifestResourceStream(uiElementAttr.ComponentTemplate))
            {
                if (stream == null)
                    return;

                var template = XDocument.Load(stream, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                ComponentTemplates.SetDefault(type, template);
            }
        }

        /// <summary>
        /// Raises the RenderSizeChanged event for elements which have changed size.
        /// </summary>
        private void RaiseRenderSizeChanged()
        {
            try
            {
                if (elementsPendingRenderSizeChangedTemp.Capacity < elementsPendingRenderSizeChanged.Count)
                    elementsPendingRenderSizeChangedTemp.Capacity = elementsPendingRenderSizeChanged.Count;

                foreach (var entry in elementsPendingRenderSizeChanged)
                    elementsPendingRenderSizeChangedTemp.Add(entry);

                elementsPendingRenderSizeChanged.Clear();

                foreach (var entry in elementsPendingRenderSizeChangedTemp)
                {
                    var sizeChangedInfo = new SizeChangedInfo(entry.PreviousSize, entry.CurrentSize);
                    entry.Element.OnRenderSizeChanged(sizeChangedInfo);
                }
            }
            finally
            {
                elementsPendingRenderSizeChangedTemp.Clear();
            }
        }

        /// <summary>
        /// Raises the <see cref="UIElement.LayoutUpdated"/> event for elements which have registered handlers.
        /// </summary>
        private void RaiseLayoutUpdated()
        {
            elementsWithLayoutUpdatedHandlers.ForEach((element) =>
            {
                element.RaiseLayoutUpdated();
                return !Instance.ElementNeedsLayout;
            });
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting layout.
        /// </summary>
        private Boolean ElementNeedsLayout
        {
            get { return ElementNeedsStyle || ElementNeedsMeasure || ElementNeedsArrange; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting styling.
        /// </summary>
        private Boolean ElementNeedsStyle
        {
            get { return styleQueue.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting measurement.
        /// </summary>
        private Boolean ElementNeedsMeasure
        {
            get { return measureQueue.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting arrangement.
        /// </summary>
        private Boolean ElementNeedsArrange
        {
            get { return arrangeQueue.Count > 0; }
        }

        // The singleton instance of the Ultraviolet Presentation Foundation.
        private static readonly UltravioletSingleton<PresentationFoundation> instance =
            new UltravioletSingleton<PresentationFoundation>(uv =>
            {
                var instance = new PresentationFoundation(uv);
                uv.FrameStart += instance.OnFrameStart;
                uv.UpdatingSubsystems += instance.OnUpdatingSubsystems;
                uv.GetUI().Updating += instance.OnUpdatingUI;
                uv.Drawing += instance.OnDrawing;
                return instance;
            });

        // The diagnostics panel used to display performance metrics.
        private DiagnosticsPanel diagnosticsPanel;

        // Performance stats.
        private readonly PresentationFoundationPerformanceStats performanceStats = 
            new PresentationFoundationPerformanceStats();

        // The component template manager.
        private readonly ComponentTemplateManager componentTemplateManager = 
            new ComponentTemplateManager();

        // The registry of known types.
        private readonly Dictionary<String, KnownType> coreTypes = 
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<String, KnownType> registeredTypes = 
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);

        // The registry of compiled data source wrappers.
        private readonly Dictionary<Type, Type> compiledDataSourceWrappersByWrappedType =
            new Dictionary<Type, Type>();
        
        // The out-of-band element renderer.
        private readonly OutOfBandRenderer outOfBandRenderer;

        // The queues of elements with invalid layouts.
        private readonly LayoutQueue styleQueue;
        private readonly LayoutQueue measureQueue;
        private readonly LayoutQueue arrangeQueue;
        private readonly WeakLinkedList<UIElement> elementsWithLayoutUpdatedHandlers =  
            new WeakLinkedList<UIElement>();

        // The list of elements waiting for a RenderSizeChanged event.
        private readonly PooledLinkedList<RenderSizeChangedEntry> elementsPendingRenderSizeChanged =
            new PooledLinkedList<RenderSizeChangedEntry>();
        private readonly List<RenderSizeChangedEntry> elementsPendingRenderSizeChangedTemp =
            new List<RenderSizeChangedEntry>();

        // The global style sheet.
        private UvssDocument globalStyleSheet;

        // The compiler used to compile the game's binding expressions.
        private const String CompiledExpressionsAssemblyName = "Ultraviolet.Presentation.CompiledExpressions.dll";
        private IBindingExpressionCompiler bindingExpressionCompiler;
        private Assembly inMemoryBindingExpressionsAsm;

        // The identifier of the current digest cycle.
        private Int64 digestCycleID = 1;
    }
}
