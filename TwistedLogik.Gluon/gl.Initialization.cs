using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        /// <summary>
        /// Initializes the gl type.
        /// </summary>
        static gl()
        {
            miCreateDefaultDelegateException = typeof(gl).GetMethod(
                "CreateDefaultDelegateException", BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        /// Initializes OpenGL.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        public static void Initialize(IOpenGLInitializer initializer)
        {
            Contract.Require(initializer, "initializer");
            Contract.EnsureNot(initialized, GluonStrings.OpenGLAlreadyInitialized);

            initializing = true;

            LoadFunction(initializer, "glGetError");
            LoadFunction(initializer, "glGetIntegerv");
            LoadFunction(initializer, "glGetString");
            LoadFunction(initializer, "glGetStringi");

            LoadVersion();
            LoadExtensions();

            gl.EXT_direct_state_access_available = IsExtensionSupported("GL_EXT_direct_state_access");
            
            var functions = GetOpenGLFunctionFields();
            foreach (var function in functions)
            {
                var name = function.Name;
                var requirements = function.GetCustomAttributes(typeof(RequireAttribute), false).Cast<RequireAttribute>().FirstOrDefault();
                if (!LoadFunction(initializer, function, requirements))
                {
                    Debug.WriteLine(GluonStrings.CouldNotLoadFunction.Format(function));
                }
            }

            Debug.WriteLine(GluonStrings.LoadedOpenGLVersion.Format(GetString(GL_VERSION), GetString(GL_VENDOR)));

            initializing = false;
            initialized = true;
        }

        /// <summary>
        /// Clears out Gluon's bindings to the OpenGL context with which is currently associated. 
        /// </summary>
        public static void Uninitialize()
        {
            Contract.Ensure(initialized, GluonStrings.OpenGLNotInitialized);

            gl.majorVersion = 0;
            gl.minorVersion = 0;
            gl.extensions.Clear();

            gl.EXT_direct_state_access_available = false;

            var functions = GetOpenGLFunctionFields();
            foreach (var function in functions)
            {
                function.SetValue(null, null);
            }

            Debug.WriteLine(GluonStrings.UnloadedOpenGL);

            initialized = false;
        }

        /// <summary>
        /// Gets a value indicating whether the OpenGL version is greater than or equal to the specified version.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <returns>true if the OpenGL version is greater than or equal to the specified version; otherwise, false.</returns>
        public static bool IsVersionAtLeast(Version version)
        {
            Contract.Require(version, "version");

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
            Contract.EnsureRange(major >= 1, "major");
            Contract.EnsureRange(minor >= 0, "minor");

            if (!initialized && !initializing)
                throw new InvalidOperationException(GluonStrings.OpenGLNotInitialized);

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
            Contract.Require(version, "version");

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
            Contract.EnsureRange(major >= 1, "major");
            Contract.EnsureRange(minor >= 0, "minor");

            if (!initialized && !initializing)
                throw new InvalidOperationException(GluonStrings.OpenGLNotInitialized);

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
            Contract.RequireNotEmpty(extension, "extension");

            if (!initialized && !initializing)
                throw new InvalidOperationException(GluonStrings.OpenGLNotInitialized);

            return extensions.Contains(extension);
        }

        /// <summary>
        /// Throws an exception if OpenGL is in an error state.
        /// </summary>
        /// <remarks>This method will be compiled out in release mode.</remarks>
        [Conditional("DEBUG")]
        public static void ThrowIfError()
        {
            var error = gl.GetError();
            switch (error)
            {
                case gl.GL_NO_ERROR:
                    return;

                case gl.GL_INVALID_ENUM:
                    throw new ArgumentException("GL_INVALID_ENUM");

                case gl.GL_INVALID_VALUE:
                    throw new ArgumentOutOfRangeException("GL_INVALID_VALUE");

                case gl.GL_INVALID_OPERATION:
                    throw new InvalidOperationException("GL_INVALID_OPERATION");

                case gl.GL_INVALID_FRAMEBUFFER_OPERATION:
                    throw new InvalidOperationException("GL_INVALID_FRAMEBUFFER_OPERATION");

                case gl.GL_OUT_OF_MEMORY:
                    throw new OutOfMemoryException("GL_OUT_OF_MEMORY");
            }
        }

        /// <summary>
        /// Gets a value indicating whether OpenGL has been initialized.
        /// </summary>
        public static Boolean Initialized
        {
            get { return initialized; }
        }

        /// <summary>
        /// Gets all of the fields of the <see cref="gl"/> type which represent bindings to OpenGL functions.
        /// </summary>
        /// <returns>A collection containing the function binding fields.</returns>
        private static IEnumerable<FieldInfo> GetOpenGLFunctionFields()
        {
            var functions = 
                from field in typeof(gl).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                where
                    typeof(Delegate).IsAssignableFrom(field.FieldType) && !field.FieldType.IsGenericType
                select field;

            return functions;
        }

        /// <summary>
        /// Loads the specified function.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        /// <param name="name">The name of the field that represents the function to load.</param>
        /// <param name="reqs">The function's requirements.</param>
        /// <returns>true if the function was loaded; otherwise, false.</returns>
        private static bool LoadFunction(IOpenGLInitializer initializer, String name, RequireAttribute reqs = null)
        {
            var field = typeof(gl).GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
                throw new MissingMethodException(name);

            return LoadFunction(initializer, field, reqs);
        }

        /// <summary>
        /// Loads the specified function.
        /// </summary>
        /// <param name="initializer">The OpenGL initializer.</param>
        /// <param name="field">The field that represents the function to load.</param>
        /// <param name="reqs">The function's requirements.</param>
        /// <returns>true if the function was loaded successfully, or wasn't loaded because its extension is not supported; otherwise, false.</returns>
        private static bool LoadFunction(IOpenGLInitializer initializer, FieldInfo field, RequireAttribute reqs = null)
        {
            var name = field.Name.StartsWith("gl") ? field.Name : "gl" + field.Name;

            // If this isn't a core function, attempt to load it as an extension.
            if (reqs != null && !reqs.IsCore(majorVersion, minorVersion))
            {
                if (!IsExtensionSupported(reqs.Extension))
                {
                    LoadDefaultDelegate(field, reqs);
                    return false;
                }
                name = reqs.ExtensionFunction ?? name;
            }

            // Load the function pointer from the OpenGL initializer.
            var fnptr = initializer.GetProcAddress(name);
            if (fnptr == IntPtr.Zero)
            {
                LoadDefaultDelegate(field, reqs);
                return false;
            }
            var fndel = Marshal.GetDelegateForFunctionPointer(fnptr, field.FieldType);
            field.SetValue(null, fndel);
            return true;
        }

        /// <summary>
        /// Loads the OpenGL version number.
        /// </summary>
        private static void LoadVersion()
        {
            int majorVersion;
            int minorVersion;

            gl.GetIntegerv(gl.GL_MAJOR_VERSION, &majorVersion);
            if (gl.GetError() == gl.GL_INVALID_ENUM)
            {
                var version = gl.GetString(gl.GL_VERSION);

                var ixSpace = version.IndexOf(' ');
                if (ixSpace >= 0)
                {
                    version = version.Substring(0, ixSpace);
                }

                var versionComponents = version.Split('.');
                gl.majorVersion = Int32.Parse(versionComponents[0]);
                gl.minorVersion = versionComponents.Length > 1 ? Int32.Parse(versionComponents[1]) : 0;
            }
            else
            {
                gl.glGetIntegerv(GL_MINOR_VERSION, &minorVersion);
                gl.majorVersion = majorVersion;
                gl.minorVersion = minorVersion;
            }
        }

        /// <summary>
        /// Loads the list of supported extensions.
        /// </summary>
        private static void LoadExtensions()
        {
            int numExtensions;
            gl.glGetIntegerv(gl.GL_NUM_EXTENSIONS, &numExtensions);

            extensions.Clear();
            for (uint i = 0; i < numExtensions; i++)
            {
                var extension = gl.GetStringi(gl.GL_EXTENSIONS, i);
                extensions.Add(extension);
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
            var valIsCore = Expression.Constant(requirements != null && requirements.IsCore(majorVersion, minorVersion));
            var valExt = Expression.Constant(requirements.Extension);
            var valExtFn = Expression.Constant(requirements.ExtensionFunction);

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
            message.AppendFormat(GluonStrings.FunctionNotProvidedByDriver.Format(function));
            message.AppendFormat(" ");
            if (hasReqs)
            {
                if (!isCore)
                {
                    if (!IsExtensionSupported(ext))
                    {
                        message.AppendFormat(GluonStrings.MissingRequiredExtension.Format(ext));
                        message.AppendFormat(" ");
                    }
                    else
                    {
                        message.AppendFormat(GluonStrings.DriverReturnedNullPointer.Format(
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
        private static Int32 majorVersion;
        private static Int32 minorVersion;
        private static readonly HashSet<String> extensions = 
            new HashSet<String>();
    }
}
