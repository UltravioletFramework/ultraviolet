using System;
using System.IO;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Contains utility methods for accessing the library's resource files.
    /// </summary>
    internal static class ResourceUtil
    {
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
    }
}
