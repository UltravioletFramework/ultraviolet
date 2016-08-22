using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
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
        /// <returns>true if the shader compiled; otherwise, false.</returns>
        public static Boolean Compile(UInt32 shader, String[] source, out String log)
        {
            Contract.Require(source, nameof(source));

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

        /// <summary>
        /// Replaces any Ultraviolet-specific preprocessor directives in the specified shader source.
        /// </summary>
        /// <param name="source">The shader source to process.</param>
        /// <returns>The processed shader source.</returns>
        public static String ProcessShaderDirectives(String source)
        {
            return ProcessShaderDirectives(source, null);
        }

        /// <summary>
        /// Replaces any Ultraviolet-specific preprocessor directives in the specified shader source.
        /// </summary>
        /// <param name="source">The shader source to process.</param>
        /// <param name="custom">A function which implements custom directives.</param>
        /// <returns>The processed shader source.</returns>
        public static String ProcessShaderDirectives(String source, Func<String, StringBuilder, Boolean> custom)
        {
            var output = new StringBuilder();
            var line = default(String);

            using (var reader = new StringReader(source))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var ifVerMatch = regexIfVerDirective.Match(line);
                    if (ifVerMatch.Success)
                    {
                        line = ProcessIfVerDirective(ifVerMatch);
                        if (String.IsNullOrEmpty(line))
                            continue;
                    }

                    if (custom == null || !custom(line, output))
                        output.AppendLine(line);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Processes an #ifver shader directive.
        /// </summary>
        private static String ProcessIfVerDirective(Match match)
        {
            var source = match.Groups["source"].Value;

            var dirVersionIsGLES = !String.IsNullOrEmpty(match.Groups["gles"].Value);
            var dirVersionMajor = Int32.Parse(match.Groups["version_maj"].Value);
            var dirVersionMinor = Int32.Parse(match.Groups["version_min"].Value);
            var dirVersion = new Version(dirVersionMajor, dirVersionMinor);
            var dirSatisfied = false;

            var uvVersionIsGLES = gl.IsGLES;
            var uvVersionMajor = gl.MajorVersion;
            var uvVersionMinor = gl.MinorVersion;
            var uvVersion = new Version(uvVersionMajor, uvVersionMinor);

            if (dirVersionIsGLES != uvVersionIsGLES)
                return null;

            switch (match.Groups["op"].Value)
            {
                case "ifver":
                    dirSatisfied = (uvVersion == dirVersion);
                    break;

                case "ifver_lt":
                    dirSatisfied = (uvVersion < dirVersion);
                    break;

                case "ifver_lte":
                    dirSatisfied = (uvVersion <= dirVersion);
                    break;

                case "ifver_gt":
                    dirSatisfied = (uvVersion > dirVersion);
                    break;

                case "ifver_gte":
                    dirSatisfied = (uvVersion >= dirVersion);
                    break;
            }

            return dirSatisfied ? source : null;
        }

        // Regular expressions used to identify directives
        private static readonly Regex regexIfVerDirective =
            new Regex(@"^\s*#(?<op>ifver(_gt|_gte|_lt|_lte)?)\s+\""(?<gles>es)?(?<version_maj>\d+).(?<version_min>\d+)\""\s+\{\s*(?<source>.+)\s*\}\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
