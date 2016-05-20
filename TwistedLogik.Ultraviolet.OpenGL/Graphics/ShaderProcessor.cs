using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the base class for shader content processors.
    /// </summary>
    /// <typeparam name="TOutput">The output type which is produced by the processor.</typeparam>
    public abstract class ShaderProcessor<TOutput> : ContentProcessor<String, TOutput>
    {
        /// <summary>
        /// Replaces any #include directives in the specified shader source with the
        /// contents of the included files.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="source">The shader source to process.</param>
        /// <returns>The processed shader source.</returns>
        protected string ReplaceIncludes(ContentManager manager, IContentProcessorMetadata metadata, String source)
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

                    var includeMatch = regexIncludeDirective.Match(line);
                    if (includeMatch.Success)
                    {
                        var includePath = includeMatch.Groups["file"].Value;
                        includePath = ResolveDependencyAssetPath(metadata, includePath);
                        includePath = manager.ResolveAssetFilePath(includePath);

                        var includeSrc = manager.Import<String>(includePath);
                        output.AppendLine(includeSrc);
                    }
                    else
                    {
                        output.AppendLine(line);
                    }
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
        private static readonly Regex regexIncludeDirective =
            new Regex(@"^\s*#include ""(?<file>.*)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex regexIfVerDirective =
            new Regex(@"^\s*#(?<op>ifver(_gt|_gte|_lt|_lte)?) \""(?<gles>es)?(?<version_maj>\d+).(?<version_min>\d+)\"" \{\s+(?<source>.+)\s+\}\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
