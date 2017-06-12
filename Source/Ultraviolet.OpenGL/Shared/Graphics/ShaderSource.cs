using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Ultraviolet.Content;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents shader source code after processing has been performed.
    /// </summary>
    public sealed class ShaderSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderSource"/> class.
        /// </summary>
        /// <param name="source"></param>
        private ShaderSource(String source)
        {
            this.Source = source;
        }

        /// <summary>
        /// Processes raw shader source into a new <see cref="ShaderSource"/> object.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> that is loading the shader source.</param>
        /// <param name="metadata">The content processor metadata for the shader source that is being loaded.</param>
        /// <param name="source">The raw shader source to process.</param>
        /// <returns>A <see cref="ShaderSource"/> object that represents the processed shader source.</returns>
        public static ShaderSource ProcessRawSource(ContentManager manager, IContentProcessorMetadata metadata, String source)
        {
            var output = new StringBuilder();
            var line = default(String);

            using (var reader = new StringReader(source))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (ProcessIncludeDirective(manager, metadata, line, output))
                        continue;

                    if (ProcessIncludeResourceDirective(manager, metadata, line, output))
                        continue;

                    if (ProcessIfVerDirective(manager, metadata, line, output))
                        continue;

                    output.AppendLine(line);
                }
            }

            return new ShaderSource(output.ToString());
        }
        
        /// <summary>
        /// Implicitly converts a <see cref="ShaderSource"/> object to its underlying source string.
        /// </summary>
        /// <param name="source">The underlying source string of the specified <see cref="ShaderSource"/> object.</param>
        public static implicit operator String(ShaderSource source) => source.Source;

        /// <summary>
        /// Gets the processed shader source.
        /// </summary>
        public String Source { get; }

        /// <summary>
        /// Processes #include directives.
        /// </summary>
        private static Boolean ProcessIncludeDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output)
        {
            var includeMatch = regexIncludeDirective.Match(line);
            if (includeMatch.Success)
            {
                if (manager == null || metadata == null)
                    throw new InvalidOperationException(OpenGLStrings.CannotIncludeShaderHeadersInStream);

                var includePath = includeMatch.Groups["file"].Value;
                includePath = ContentManager.NormalizeAssetPath(Path.Combine(Path.GetDirectoryName(metadata.AssetPath), includePath));
                metadata.AddAssetDependency(includePath);

                var includeSrc = manager.Load<ShaderSource>(includePath, cache: false);
                output.AppendLine(includeSrc);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #includeres directives.
        /// </summary>
        private static Boolean ProcessIncludeResourceDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output)
        {
            var includeResourceMatch = regexincludeResourceDirective.Match(line);
            if (includeResourceMatch.Success)
            {
                var includeResource = includeResourceMatch.Groups["resource"].Value;
                var includeAsm = includeResourceMatch.Groups["asm"]?.Value ?? "entry";
                var asm =
                    String.Equals("entry", includeAsm, StringComparison.OrdinalIgnoreCase) ? Assembly.GetEntryAssembly() :
                    String.Equals("executing", includeAsm, StringComparison.OrdinalIgnoreCase) ? Assembly.GetExecutingAssembly() : null;

                var info = asm.GetManifestResourceInfo(includeResource);
                if (info == null)
                    throw new InvalidOperationException(OpenGLStrings.InvalidIncludedResource.Format(includeResource));

                using (var resStream = asm.GetManifestResourceStream(includeResource))
                using (var resReader = new StreamReader(resStream))
                {
                    var includeSrc = ProcessRawSource(manager, metadata, resReader.ReadToEnd());
                    output.AppendLine(includeSrc);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #ifver directives.
        /// </summary>
        private static Boolean ProcessIfVerDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output)
        {
            var ifVerMatch = regexIfVerDirective.Match(line);
            if (ifVerMatch.Success)
            {
                var source = ifVerMatch.Groups["source"].Value;

                var dirVersionIsGLES = !String.IsNullOrEmpty(ifVerMatch.Groups["gles"].Value);
                var dirVersionMajor = Int32.Parse(ifVerMatch.Groups["version_maj"].Value);
                var dirVersionMinor = Int32.Parse(ifVerMatch.Groups["version_min"].Value);
                var dirVersion = new Version(dirVersionMajor, dirVersionMinor);
                var dirSatisfied = false;

                var uvVersionIsGLES = gl.IsGLES;
                var uvVersionMajor = gl.MajorVersion;
                var uvVersionMinor = gl.MinorVersion;
                var uvVersion = new Version(uvVersionMajor, uvVersionMinor);

                if (dirVersionIsGLES != uvVersionIsGLES)
                    return true;

                switch (ifVerMatch.Groups["op"].Value)
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

                if (dirSatisfied)
                    output.AppendLine(ProcessRawSource(manager, metadata, source));

                return true;
            }
            return false;
        }

        // Regular expressions used to identify directives
        private static readonly Regex regexIncludeDirective =
            new Regex(@"^\s*#include\s+""(?<file>.*)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexincludeResourceDirective =
            new Regex(@"^\s*#includeres\s+""(?<resource>.*)""\s*(?<asm>entry|executing)?\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexIfVerDirective =
            new Regex(@"^\s*#(?<op>ifver(_gt|_gte|_lt|_lte)?)\s+\""(?<gles>es)?(?<version_maj>\d+).(?<version_min>\d+)\""\s+\{\s*(?<source>.+)\s*\}\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
