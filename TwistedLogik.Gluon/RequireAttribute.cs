using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Gluon
{
    /// <summary>
    /// Represents an attribute indicating that an OpenGL function requires a specified OpenGL version or extension to exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
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
        /// <returns>true if this is a core function; otherwise, false.</returns>
        public Boolean IsCore(Int32 major, Int32 minor)
        {
            if (String.IsNullOrEmpty(Extension))
                return true;
            else
            {
                if (MinVersionValue == null && MaxVersionValue == null)
                    return false;
            }

            var version = new Version(major, minor, 0, 0);

            var satisfiesMin = true;
            if (MinVersionValue != null)
            {
                satisfiesMin = version >= MinVersionValue;
            }

            var satisfiesMax = true;
            if (MaxVersionValue != null)
            {
                satisfiesMax = version <= MaxVersionValue;
            }

            return satisfiesMin && satisfiesMax;
        }

        /// <summary>
        /// Gets a value indicating whether this function is part of the core profile in the specified version of OpenGL.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <returns>true if this is a core function; otherwise, false.</returns>
        public Boolean IsCore(Version version)
        {
            Contract.Require(version, "version");

            return IsCore(version.Major, version.Minor);
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
        /// Gets the minimum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public Version MinVersionValue
        {
            get { return minVersionValue; }
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
        /// Gets the maximum OpenGL version in which the function appears as part of the core API.
        /// </summary>
        public Version MaxVersionValue
        {
            get { return maxVersionValue; }
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
        private Version minVersionValue;
        private String maxVersion;
        private Version maxVersionValue;
        private String extension;
        private String extensionFunction;
    }
}
