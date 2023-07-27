using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.OpenGL.Bindings
{
    [SuppressUnmanagedCodeSecurity]
    public static unsafe partial class GL
    {
        /// <summary>
        /// Initializes the gl type.
        /// </summary>
        static GL()
        {
            miCreateDefaultDelegateException = typeof(GL).GetMethod(
                "CreateDefaultDelegateException", BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        /// Initializes OpenGL.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        public static void Initialize(IOpenGLInitializer initializer)
        {
            Contract.Require(initializer, nameof(initializer));
            Contract.EnsureNot(initialized, BindingsStrings.OpenGLAlreadyInitialized);

            initializing = true;

            initializer.Prepare();

            LoadFunction(initializer, "glGetError", false);
            LoadFunction(initializer, "glGetIntegerv", false);
            LoadFunction(initializer, "glGetString", false);
            LoadFunction(initializer, "glGetStringi", false);

            LoadVersion();
            LoadExtensions();

            GL.InitializeDSA();
            GL.InitializeFeatureFlags();

            var functions = GetOpenGLFunctionFields();
            foreach (var function in functions)
            {
                if (!LoadFunction(initializer, function))
                {
                    VerboseLog(BindingsStrings.CouldNotLoadFunction.Format(function));
                }
            }

            GL.DefaultFramebuffer = (UInt32)GL.GetInteger(GL.GL_FRAMEBUFFER_BINDING);
            GL.ThrowIfError();

            GL.DefaultRenderbuffer = (UInt32)GL.GetInteger(GL.GL_RENDERBUFFER_BINDING);
            GL.ThrowIfError();

            Debug.WriteLine(BindingsStrings.LoadedOpenGLVersion.Format(GetString(GL_VERSION), GetString(GL_VENDOR)));

            initializer.Cleanup();

            initializing = false;
            initialized = true;
        }

        /// <summary>
        /// Clears out Ultraviolet's bindings to the OpenGL context with which is currently associated. 
        /// </summary>
        public static void Uninitialize()
        {
            Contract.Ensure(initialized, BindingsStrings.OpenGLNotInitialized);

            GL.DefaultFramebuffer = 0;

            GL.isGLES = false;
            GL.isEmulated = false;
            GL.majorVersion = 0;
            GL.minorVersion = 0;
            GL.extensions.Clear();

            GL.dsaimpl = null;
            GL.IsARBDirectStateAccessAvailable = false;
            GL.IsEXTDirectStateAccessAvailable = false;
            GL.IsTextureStorageAvailable = false;
            GL.IsVertexAttribBindingAvailable = false;

            var functions = GetOpenGLFunctionFields();
            foreach (var function in functions)
            {
                function.SetValue(null, null);
            }

            Debug.WriteLine(BindingsStrings.UnloadedOpenGL);

            initialized = false;
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL version is greater than or equal to the specified version.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <returns>true if the OpenGL version is greater than or equal to the specified version; otherwise, false.</returns>
        public static bool IsVersionAtLeast(Version version)
        {
            Contract.Require(version, nameof(version));

            return IsVersionAtLeast(version.Major, version.Minor);
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL version is greater than or equal to the specified version.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <returns>true if the OpenGL version is greater than or equal to the specified version; otherwise, false.</returns>
        public static bool IsVersionAtLeast(Int32 major, Int32 minor)
        {
            Contract.EnsureRange(major >= 1, nameof(major));
            Contract.EnsureRange(minor >= 0, nameof(minor));

            if (!initialized && !initializing)
                throw new InvalidOperationException(BindingsStrings.OpenGLNotInitialized);

            if (majorVersion == major)
                return minorVersion >= minor;
            return majorVersion > major;
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL version is less than or equal to the specified version.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <returns>true if the OpenGL version is less than or equal to the specified version; otherwise, false.</returns>
        public static bool IsVersionAtMost(Version version)
        {
            Contract.Require(version, nameof(version));

            return IsVersionAtMost(version.Major, version.Minor);
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL version is less than or equal to the specified version.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <returns>true if the OpenGL version is less than or equal to the specified version; otherwise, false.</returns>
        public static bool IsVersionAtMost(Int32 major, Int32 minor)
        {
            Contract.EnsureRange(major >= 1, nameof(major));
            Contract.EnsureRange(minor >= 0, nameof(minor));

            if (!initialized && !initializing)
                throw new InvalidOperationException(BindingsStrings.OpenGLNotInitialized);

            if (majorVersion == major)
                return minorVersion <= minor;
            return majorVersion < major;
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL driver supports the specified extension.
        /// </summary>
        /// <param name="extension">The name of the extension to evaluate.</param>
        /// <returns>true if the OpenGL driver supports the specified extension; otherwise, false.</returns>
        public static bool IsExtensionSupported(String extension)
        {
            Contract.RequireNotEmpty(extension, nameof(extension));

            if (!initialized && !initializing)
                throw new InvalidOperationException(BindingsStrings.OpenGLNotInitialized);

            return extensions.Contains(extension);
        }

        /// <summary>
        /// Throws an exception if OpenGL is in an error state.
        /// </summary>
        /// <remarks>This method will be compiled out in release mode.</remarks>
        [Conditional("DEBUG")]
        public static void ThrowIfError()
        {
            var error = GL.GetError();
            switch (error)
            {
                case GL.GL_NO_ERROR:
                    return;

                case GL.GL_INVALID_ENUM:
                    throw new ArgumentException("GL_INVALID_ENUM");

                case GL.GL_INVALID_VALUE:
                    throw new ArgumentOutOfRangeException("GL_INVALID_VALUE");

                case GL.GL_INVALID_OPERATION:
                    throw new InvalidOperationException("GL_INVALID_OPERATION");

                case GL.GL_INVALID_FRAMEBUFFER_OPERATION:
                    throw new InvalidOperationException("GL_INVALID_FRAMEBUFFER_OPERATION");

                case GL.GL_OUT_OF_MEMORY:
                    throw new OutOfMemoryException("GL_OUT_OF_MEMORY");
            }
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> if the current OpenGL context uses the OpenGL ES profile.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public static void ThrowIfGLES(String message)
        {
            if (IsGLES)
            {
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> if the current OpenGL context uses the OpenGL ES profile.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public static void ThrowIfGLES(StringResource message)
        {
            if (IsGLES)
            {
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the major version of the OpenGL driver.
        /// </summary>
        public static Int32 MajorVersion
        {
            get { return majorVersion; }
        }

        /// <summary>
        /// Gets the minor version of the OpenGL driver.
        /// </summary>
        public static Int32 MinorVersion
        {
            get { return minorVersion; }
        }

        /// <summary>
        /// Gets a value indicating whether OpenGL has been initialized.
        /// </summary>
        public static Boolean Initialized
        {
            get { return initialized; }
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL context is an OpenGL ES context.
        /// </summary>
        public static Boolean IsGLES
        {
            get { return isGLES; }
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL context is an OpenGL ES 2 context.
        /// </summary>
        public static Boolean IsGLES2
        {
            get { return IsGLES && majorVersion == 2; }
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL context is an OpenGL ES 3 context.
        /// </summary>
        public static Boolean IsGLES3
        {
            get { return IsGLES && majorVersion == 3; }
        }

        /// <summary>
        /// Gets a value indicating whether Ultraviolet thinks it's running inside of an emulator.
        /// </summary>
        public static Boolean IsEmulated
        {
            get { return isEmulated; }
        }

        /// <summary>
        /// Gets the resource name for the context's default framebuffer.
        /// </summary>
        public static UInt32 DefaultFramebuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the resource name for the context's default renderbuffer.
        /// </summary>
        public static UInt32 DefaultRenderbuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets all of the fields of the <see cref="GL"/> type which represent bindings to OpenGL functions.
        /// </summary>
        /// <returns>A collection containing the function binding fields.</returns>
        private static IEnumerable<FieldInfo> GetOpenGLFunctionFields()
        {
            var functions = 
                from field in typeof(GL).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                where
                    typeof(Delegate).IsAssignableFrom(field.FieldType) && !field.FieldType.IsGenericType && field.Name.StartsWith("gl")
                select field;

            return functions.ToList();
        }

        /// <summary>
        /// Loads the specified function.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        /// <param name="name">The name of the field that represents the function to load.</param>
        /// <param name="checkRequirements">A value indicating whether to check the function's requirements.</param>
        /// <returns>true if the function was loaded; otherwise, false.</returns>
        private static Boolean LoadFunction(IOpenGLInitializer initializer, String name, Boolean checkRequirements = true)
        {
            var field = typeof(GL).GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
                throw new MissingMethodException(name);

            return LoadFunction(initializer, field, checkRequirements);
        }

        /// <summary>
        /// Loads the specified function.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        /// <param name="field">The field that represents the function to load.</param>
        /// <param name="checkRequirements">A value indicating whether to check the function's requirements.</param>
        /// <returns>true if the function was loaded successfully, or wasn't loaded because its extension is not supported; otherwise, false.</returns>
        private static Boolean LoadFunction(IOpenGLInitializer initializer, FieldInfo field, Boolean checkRequirements = true)
        {
            var name = field.Name.StartsWith("gl") ? field.Name : "gl" + field.Name;
            var reqs = checkRequirements ? field.GetCustomAttributes(typeof(RequireAttribute), false).Cast<RequireAttribute>() : null;

            // If this isn't a core function, attempt to load it as an extension.
            if (reqs != null && reqs.Any())
            {
                foreach (var req in reqs)
                {
                    if (!req.IsCore(majorVersion, minorVersion, isGLES))
                    {
                        var extensions = req.Extension.Split(new[] { "&&" }, StringSplitOptions.None).Select(x => x.Trim());
                        if (!extensions.All(IsExtensionSupported))
                            continue;

                        name = req.ExtensionFunction ?? name;
                    }
                }
            }

            // Load the function pointer from the OpenGL initializer.
            var fnptr = initializer.GetProcAddress(name);
            if (fnptr == IntPtr.Zero)
            {
                LoadDefaultDelegate(field, reqs?.FirstOrDefault());
                return false;
            }
            var fndel = Marshal.GetDelegateForFunctionPointer(fnptr, field.FieldType);
            field.SetValue(null, fndel);
            return true;
        }

        /// <summary>
        /// Attempts to parse the version of the loaded OpenGL implementation from the string
        /// returned by glGetString(GL_VERSION).
        /// </summary>
        private static Boolean TryParseOpenGLVersionFromString(String str, out Int32 major, out Int32 minor)
        {
            str = isGLES ? str.Substring("OpenGL ES ".Length) : str.Substring("OpenGL ".Length);

            var components = str.Split(new[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length < 2 || !Int32.TryParse(components[0], out major) || !Int32.TryParse(components[1], out minor))
            {
                major = 0;
                minor = 0;
                return false;
            }
            return true;            
        }

        /// <summary>
        /// Writes a message to debug output, but only if the VERBOSE_LOGGING compilation symbol is specified.
        /// </summary>
        [Conditional("VERBOSE_LOGGING")]
        private static void VerboseLog(String str)
        {
            Debug.WriteLine(str);
        }

        /// <summary>
        /// Loads the OpenGL version number.
        /// </summary>
        private static void LoadVersion()
        {
            var majorVersion = 0;
            var minorVersion = 0;

            var version = GL.GetString(GL.GL_VERSION);
            
            isGLES = version.StartsWith("OpenGL ES");
            isEmulated = false;

            GL.GetIntegerv(GL.GL_MAJOR_VERSION, &majorVersion);

            if (GL.GetError() == GL.GL_INVALID_ENUM) 
            {
                if (!TryParseOpenGLVersionFromString(version, out GL.majorVersion, out GL.minorVersion))
                {
                    // Something is horribly wrong with our version string; be optimistic!
                    GL.majorVersion = 4;
                    GL.minorVersion = 0;
                }
            }
            else
            {
                GL.glGetIntegerv(GL_MINOR_VERSION, (IntPtr)(&minorVersion));
                GL.majorVersion = majorVersion;
                GL.minorVersion = minorVersion;
            }

            // In the case of GLES, it's possible for the value reported by glGetIntegerv() to
            // differ from the value specified in GL_VERSION if we're running inside of an emulator
            // with native GPU enabled - so try to account for that case.
            if (isGLES)
            {
                Int32 glesMajorVersion;
                Int32 glesMinorVersion;
                if (TryParseOpenGLVersionFromString(GL.GetString(GL.GL_VERSION), out glesMajorVersion, out glesMinorVersion))
                {
                    if (glesMajorVersion != GL.majorVersion || glesMinorVersion != GL.minorVersion)
                    {
                        GL.isEmulated = true;
                        GL.majorVersion = glesMajorVersion;
                        GL.minorVersion = glesMinorVersion;
                    }
                }
            }
        }

        /// <summary>
        /// Loads the list of supported extensions.
        /// </summary>
        private static void LoadExtensions()
        {
            extensions.Clear();

            if (isGLES && majorVersion < 3)
            {
                var reportedExtensions = GL.GetString(GL.GL_EXTENSIONS).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var extension in reportedExtensions)
                {
                    extensions.Add(extension);
                }
            }
            else
            {
                Int32 numExtensions;
                GL.glGetIntegerv(GL.GL_NUM_EXTENSIONS, (IntPtr)(&numExtensions));

                for (var i = 0; i < numExtensions; i++)
                {
                    var extension = GL.GetStringi(GL.GL_EXTENSIONS, (UInt32)i);
                    extensions.Add(extension);
                }
            }
        }
        
        /// <summary>
        /// Loads the default delegate for the specified field.
        /// </summary>
        /// <param name="field">The field for which to load a default delegate.</param>
        /// <param name="requirements">The OpenGL function's specified requirements.</param>
        private static void LoadDefaultDelegate(FieldInfo field, RequireAttribute requirements)
        {
            var fndefault = CreateDefaultDelegate(field.FieldType, field.Name, requirements);
            field.SetValue(null, fndefault);
        }

        /// <summary>
        /// Creates a default delegate for an OpenGL function in the event that it is not implemented by the driver.
        /// </summary>
        /// <param name="type">The OpenGL delegate type.</param>
        /// <param name="function">The name of the OpenGL function.</param>
        /// <param name="requirements">The OpenGL function's specified requirements.</param>
        /// <returns>The default delegate that was created.</returns>
        private static Delegate CreateDefaultDelegate(Type type, String function, RequireAttribute requirements)
        {
            var valFunction = Expression.Constant(function);
            var valHasReqs = Expression.Constant(requirements != null);
            var valIsCore = Expression.Constant(requirements != null && requirements.IsCore(majorVersion, minorVersion, isGLES));
            var valExt = Expression.Constant(requirements == null ? null : requirements.Extension);
            var valExtFn = Expression.Constant(requirements == null ? null : requirements.ExtensionFunction);

            var delegateInvoke = type.GetMethod("Invoke");
            var delegateParameters = delegateInvoke.GetParameters().Select(x => Expression.Parameter(x.ParameterType, x.Name));

            var throwExpression = Expression.Throw(Expression.Call(miCreateDefaultDelegateException, 
                valFunction, 
                valIsCore,
                valHasReqs,
                Expression.Convert(valExt, typeof(String)),
                Expression.Convert(valExtFn, typeof(String))), delegateInvoke.ReturnType);
            var throwLambda = Expression.Lambda(type, throwExpression, delegateParameters);

            return throwLambda.Compile();
        }

        /// <summary>
        /// Creates the exception object that is thrown by the default delegate used to replace missing OpenGL functions.
        /// </summary>
        private static Exception CreateDefaultDelegateException(String function, Boolean isCore, Boolean hasReqs, String ext, String extFn)
        {
            var message = new StringBuilder();
            message.AppendFormat(BindingsStrings.FunctionNotProvidedByDriver.Format(function));
            message.AppendFormat(" ");
            if (hasReqs)
            {
                if (!isCore)
                {
                    if (!IsExtensionSupported(ext))
                    {
                        message.AppendFormat(BindingsStrings.MissingRequiredExtension.Format(ext));
                        message.AppendFormat(" ");
                    }
                    else
                    {
                        message.AppendFormat(BindingsStrings.DriverReturnedNullPointer.Format(
                            ext, extFn ?? function));
                        message.AppendFormat(" ");
                    }
                }
            }
            return new NotSupportedException(message.ToString());
        }

        // The cached method info for the method that creates default delegate exceptions.
        private static readonly MethodInfo miCreateDefaultDelegateException;

        // Have we been initialized?
        private static Boolean initializing;
        private static Boolean initialized;

        // OpenGL API information.
        private static Boolean isGLES;
        private static Boolean isEmulated;
        private static Int32 majorVersion;
        private static Int32 minorVersion;
        private static readonly HashSet<String> extensions = 
            new HashSet<String>();
    }
}
