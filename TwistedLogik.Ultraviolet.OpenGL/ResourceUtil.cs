using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Contains utility methods for accessing the library's resource files.
    /// </summary>
    internal static class ResourceUtil
    {
        /// <summary>
        /// Initializes the <see cref="ResourceUtil"/> class.
        /// </summary>
        static ResourceUtil()
        {
            manifestResourceNames = new HashSet<String>(Assembly.GetExecutingAssembly().GetManifestResourceNames(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Reads a resource file as a string of text.
        /// </summary>
        /// <param name="name">The name of the file to read.</param>
        /// <returns>A string of text that contains the file data.</returns>
        public static String ReadResourceString(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            var asm = Assembly.GetExecutingAssembly();

            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.OpenGL.Resources." + name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads a resource file containing a shader program's source code.
        /// </summary>
        /// <param name="name">The name of the file to read.</param>
        /// <returns>A string of text that contains the file data.</returns>
        public static String ReadShaderResourceString(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            if (gl.IsGLES)
            {
                var glesName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(name) + "ES", Path.GetExtension(name));
                if (manifestResourceNames.Contains(glesName))
                {
                    name = glesName;
                }
            }

            return ReadResourceString(name);
        }

        // The manifest resource names for this assembly.
        private static readonly HashSet<String> manifestResourceNames;
    }
}
