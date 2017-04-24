using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    /// <summary>
    /// Represents an attribute indicating that an OpenGL function requires a specified OpenGL version or extension to exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class RequireAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the RequireAttribute class.
        /// </summary>
        public RequireAttribute()
        {

        }

        /// <summary>
        /// Gets a value indicating whether this function is part of the core profile in the specified version of OpenGL.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <param name="gles">A value indicating whether the OpenGL profile being evaluated is an OpenGL ES profile.</param>
        /// <returns>true if this is a core function; otherwise, false.</returns>
        public Boolean IsCore(Int32 major, Int32 minor, Boolean gles)
        {
            var minVersion = (gles ? MinVersionESValue : MinVersionValue) ?? MinVersionValue;
            var maxVersion = (gles ? MaxVersionESValue : MaxVersionValue) ?? MaxVersionValue;

            if (String.IsNullOrEmpty(Extension))
                return true;
            else
            {
                if (minVersion == null && maxVersion == null)
                    return false;
            }

            var version = new Version(major, minor, 0, 0);

            var satisfiesMin = true;
            if (minVersion != null)
            {
                satisfiesMin = version >= minVersion;
            }

            var satisfiesMax = true;
            if (maxVersion != null)
            {
                satisfiesMax = version <= maxVersion;
            }

            return satisfiesMin && satisfiesMax;
        }

        /// <summary>
        /// Gets a value indicating whether this function is part of the core profile in the specified version of OpenGL.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <param name="gles">A value indicating whether the OpenGL profile being evaluated is an OpenGL ES profile.</param>
        /// <returns>true if this is a core function; otherwise, false.</returns>
        public Boolean IsCore(Version version, Boolean gles)
        {
            Contract.Require(version, nameof(version));

            return IsCore(version.Major, version.Minor, gles);
        }

        /// <summary>
        /// Gets or sets the minimum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public String MinVersion
        {
            get { return minVersion; }
            set
            {
                minVersion = value;
                minVersionValue = String.IsNullOrEmpty(value) ? null : Version.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum OpenGL ES version in which the function appears as part of the core API.
        /// </summary>
        public String MinVersionES
        {
            get { return minVersionES; }
            set
            {
                minVersionES = value;
                minVersionESValue = String.IsNullOrEmpty(value) ? null : Version.Parse(value);
            }
        }

        /// <summary>
        /// Gets the minimum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public Version MinVersionValue
        {
            get { return minVersionValue; }
        }

        /// <summary>
        /// Gets the minimum OpenGL ES version in which the function appears as part of the core API.
        /// </summary>
        public Version MinVersionESValue
        {
            get { return minVersionESValue; }
        }

        /// <summary>
        /// Gets or sets the maximum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public String MaxVersion
        {
            get { return maxVersion; }
            set
            {
                maxVersion = value;
                maxVersionValue = String.IsNullOrEmpty(value) ? null : Version.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum OpenGL ES version in which the function appears as part of the core API.
        /// </summary>
        public String MaxVersionES
        {
            get { return maxVersionES; }
            set
            {
                maxVersionES = value;
                maxVersionESValue = String.IsNullOrEmpty(value) ? null : Version.Parse(value);
            }
        }

        /// <summary>
        /// Gets the maximum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public Version MaxVersionValue
        {
            get { return maxVersionValue; }
        }

        /// <summary>
        /// Gets the maximum OpenGL ES version in which the function appears as part of the core API.
        /// </summary>
        public Version MaxVersionESValue
        {
            get { return maxVersionESValue; }
        }

        /// <summary>
        /// Gets or sets the name of the required extension, if this is not a core function.
        /// </summary>
        public String Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        /// <summary>
        /// Gets or sets the function's name when loaded as an extension, if this is not a core function.
        /// </summary>
        public String ExtensionFunction
        {
            get { return extensionFunction; }
            set { extensionFunction = value; }
        }

        // Property values.
        private String minVersion;
        private String minVersionES;
        private Version minVersionValue;
        private Version minVersionESValue;
        private String maxVersion;
        private String maxVersionES;
        private Version maxVersionValue;
        private Version maxVersionESValue;
        private String extension;
        private String extensionFunction;
    }
}
