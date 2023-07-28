using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics;

namespace Ultraviolet.OpenGL
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
            Contract.RequireNotEmpty(name, nameof(name));

            var asm = Assembly.GetExecutingAssembly();

            using (var stream = asm.GetManifestResourceStream("Ultraviolet.OpenGL.Resources." + name))
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
        public static ShaderSource ReadShaderResourceString(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            if (GL.IsGLES)
            {
                var glesName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(name) + "ES", Path.GetExtension(name));
                if (manifestResourceNames.Contains("Ultraviolet.OpenGL.Resources." + glesName))
                {
                    name = glesName;
                }
            }

            ShaderStage stage = ShaderStage.Unknown;
            if (name.Contains(".frag"))
            {
                stage = ShaderStage.Fragment;
            }
            else if (name.Contains(".vert"))
            {
                stage = ShaderStage.Vertex;
            }

            return ShaderSource.ProcessRawSource(null, null, ReadResourceString(name), stage);
        }

        // The manifest resource names for this assembly.
        private static readonly HashSet<String> manifestResourceNames;
    }
}
