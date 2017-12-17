using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains methods for parsing and compiling shaders.
    /// </summary>
    internal static class ShaderCompiler
    {
        /// <summary>
        /// Compiles the specified shader.
        /// </summary>
        /// <param name="shader">The shader handle.</param>
        /// <param name="source">The shader source.</param>
        /// <param name="log">The compiler log.</param>
        /// <param name="ssmd">The source metadata for this shader.</param>
        /// <returns>true if the shader compiled; otherwise, false.</returns>
        public static Boolean Compile(UInt32 shader, ShaderSource[] source, out String log, out ShaderSourceMetadata ssmd)
        {
            Contract.Require(source, nameof(source));

            ssmd = new ShaderSourceMetadata();
            foreach (var s in source)
                ssmd.Concat(s.Metadata);

            unsafe
            {
                var pSource = stackalloc IntPtr[source.Length];
                var pLength = stackalloc Int32[source.Length];
                for (var i = 0; i < source.Length; i++)
                {
                    pSource[i] = Marshal.StringToHGlobalAnsi(source[i].Source);
                    pLength[i] = source[i].Source.Length;
                }

                try
                {
                    gl.ShaderSource(shader, source.Length, (sbyte**)pSource, pLength);
                    gl.ThrowIfError();

                    gl.CompileShader(shader);
                    gl.ThrowIfError();

                    gl.GetShaderInfoLog(shader, out log);
                    gl.ThrowIfError();
                }
                finally
                {
                    for (var i = 0; i < source.Length; i++)
                    {
                        Marshal.FreeHGlobal(pSource[i]);
                    }
                }

                var status = gl.GetShaderi(shader, gl.GL_COMPILE_STATUS);
                gl.ThrowIfError();
                
                return status != 0;
            }
        }
    }
}
