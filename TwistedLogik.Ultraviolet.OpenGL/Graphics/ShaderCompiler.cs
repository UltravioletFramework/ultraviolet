using System;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the base class for shaders.
    /// </summary>
    public static class ShaderCompiler
    {
        /// <summary>
        /// Compiles the specified shader.
        /// </summary>
        /// <param name="shader">The shader handle.</param>
        /// <param name="source">The shader source.</param>
        /// <param name="log">The compiler log.</param>
        /// <returns>true if the shader compiled; otherwise, false.</returns>
        public static Boolean Compile(UInt32 shader, String[] source, out String log)
        {
            Contract.Require(source, "source");

            unsafe
            {
                var pSource = stackalloc IntPtr[source.Length];
                var pLength = stackalloc Int32[source.Length];
                for (var i = 0; i < source.Length; i++)
                {
                    pSource[i] = Marshal.StringToHGlobalAnsi(source[i]);
                    pLength[i] = source[i].Length;
                }

                try
                {
                    gl.ShaderSource(shader, source.Length, (sbyte**)pSource, (int*)pLength);
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
