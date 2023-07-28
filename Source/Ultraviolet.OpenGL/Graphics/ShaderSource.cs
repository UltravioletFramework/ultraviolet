using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Ultraviolet.Content;
using Ultraviolet.Core;
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
        /// <param name="source">The source code for this shader.</param>
        /// <param name="metadata">The metadata for this shader.</param>
        private ShaderSource(String source, ShaderSourceMetadata metadata)
        {
            this.Source = source;
            this.Metadata = metadata;
        }

        /// <summary>
        /// Processes raw shader source into a new <see cref="ShaderSource"/> object.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> that is loading the shader source.</param>
        /// <param name="metadata">The content processor metadata for the shader source that is being loaded.</param>
        /// <param name="source">The raw shader source to process.</param>
        /// <param name="stage">The shader stage.</param>
        /// <returns>A <see cref="ShaderSource"/> object that represents the processed shader source.</returns>
        public static ShaderSource ProcessRawSource(ContentManager manager, IContentProcessorMetadata metadata, String source, ShaderStage stage)
        {
            var ssmd = new ShaderSourceMetadata();

            return ProcessInternal(ssmd, source, (line, output) =>
            {
                if (ProcessIncludeDirective(manager, metadata, line, output, ssmd))
                    return true;

                if (ProcessIncludeResourceDirective(manager, metadata, line, output, ssmd, stage))
                    return true;

                if (ProcessIfVerDirective(manager, metadata, line, output, ssmd, stage))
                    return true;

                if (ProcessIfStageDirective(manager, metadata, line, output, ssmd, stage))
                    return true;

                if (ProcessSamplerDirective(manager, metadata, line, output, ssmd))
                    return true;

                if (ProcessParamDirective(manager, metadata, line, output, ssmd))
                    return true;

                if (ProcessCameraDirective(manager, metadata, line, output, ssmd))
                    return true;

                return false;
            });
        }

        /// <summary>
        /// Processes a <see cref="ShaderSource"/> instance to produce a new <see cref="ShaderSource"/> instance
        /// with expanded #extern definitions. Externs which do not exist in <paramref name="externs"/> are removed from
        /// the shader source entirely, and if <paramref name="externs"/> is <see langword="null"/>, all externs are removed.
        /// </summary>
        /// <param name="source">The source instance to process.</param>
        /// <param name="externs">The collection of defined extern values, or <see langword="null"/>.</param>
        /// <returns>A new <see cref="ShaderSource"/> instance with expanded #extern definitions.</returns>
        public static ShaderSource ProcessExterns(ShaderSource source, Dictionary<String, String> externs)
        {
            Contract.Require(source, nameof(source));

            return ProcessInternal(source.Metadata, source.Source, (line, output) =>
            {
                if (ProcessExternDirective(line, output, source.Metadata, externs))
                    return true;

                return false;
            });
        }

        /// <summary>
        /// Implicitly converts a <see cref="String"/> object to a new <see cref="ShaderSource"/> instance.
        /// </summary>
        /// <param name="source">The underlying source string of the <see cref="ShaderSource"/> object to create.</param>
        public static implicit operator ShaderSource(String source) => new ShaderSource(source, new ShaderSourceMetadata());

        /// <summary>
        /// Gets the processed shader source.
        /// </summary>
        public String Source { get; }

        /// <summary>
        /// Gets the metadata for this shader.
        /// </summary>
        public ShaderSourceMetadata Metadata { get; }

        /// <summary>
        /// Performs line-by-line processing of raw shader source code.
        /// </summary>
        private static ShaderSource ProcessInternal(ShaderSourceMetadata ssmd, String source, Func<String, StringBuilder, Boolean> processor)
        {
            var output = new StringBuilder();
            var line = default(String);

            using (var reader = new StringReader(source))
            {
                var insideComment = false;

                while ((line = reader.ReadLine()) != null)
                {
                    TrackComments(line, ref insideComment);

                    if (!insideComment)
                    {
                        if (processor(line, output))
                            continue;
                    }

                    output.AppendLine(line);
                }
            }

            return new ShaderSource(output.ToString(), ssmd);
        }

        /// <summary>
        /// Parses a line of GLSL for comments and keeps track of whether we're
        /// currently inside of one that spans multiple lines.
        /// </summary>
        private static void TrackComments(String line, ref Boolean insideComment)
        {
            var ixCurrent = 0;

            // If we're inside of a C-style comment, look for a */ somewhere on this line...
            if (insideComment)
            {
                var ixCStyleEndToken = line.IndexOf("*/");
                if (ixCStyleEndToken >= 0)
                {
                    ixCurrent = ixCStyleEndToken + "*/".Length;
                    insideComment = false;
                }
            }

            if (insideComment)
                return;

            // Remove any complete C-style comments from the line
            var cStyleComments = regexCStyleComment.Matches(line, ixCurrent);
            if (cStyleComments.Count > 0)
            {
                var lastMatch = cStyleComments[cStyleComments.Count - 1];
                ixCurrent = lastMatch.Index + lastMatch.Length;
            }

            // Look for comments that finish out the line
            var ixCppCommentToken = line.IndexOf("//", ixCurrent);
            var ixCStyleStartToken = line.IndexOf("/*", ixCurrent);
            if (ixCStyleStartToken >= 0 && (ixCppCommentToken < 0 || ixCStyleStartToken < ixCppCommentToken))
            {
                line = line.Substring(0, ixCStyleStartToken);
                insideComment = true;
            }
        }

        /// <summary>
        /// Processes #extern directives.
        /// </summary>
        private static Boolean ProcessExternDirective(String line, StringBuilder output, ShaderSourceMetadata ssmd, Dictionary<String, String> externs)
        {
            var externMatch = regexExternDirective.Match(line);
            if (externMatch.Success)
            {
                var externName = externMatch.Groups["name"].Value;
                if (String.IsNullOrWhiteSpace(externName))
                    throw new InvalidOperationException(OpenGLStrings.ShaderExternHasInvalidName);

                var externValue = String.Empty;
                if (externs?.TryGetValue(externName, out externValue) ?? false)
                {
                    output.AppendLine($"#define {externName} {externValue}");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #include directives.
        /// </summary>
        private static Boolean ProcessIncludeDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd)
        {
            var includeMatch = regexIncludeDirective.Match(line);
            if (includeMatch.Success)
            {
                if (manager == null || metadata == null)
                    throw new InvalidOperationException(OpenGLStrings.CannotIncludeShaderHeadersInStream);

                var includePath = includeMatch.Groups["file"].Value;
                includePath = ContentManager.NormalizeAssetPath(Path.Combine(Path.GetDirectoryName(metadata.AssetPath), includePath));
                metadata.AddAssetDependency(includePath);

                var includeSrc = manager.Load<ShaderSource>(includePath, metadata.AssetDensity, false, metadata.IsLoadedFromSolution);
                ssmd.Concat(includeSrc.Metadata);
                output.AppendLine(includeSrc.Source);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #includeres directives.
        /// </summary>
        private static Boolean ProcessIncludeResourceDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd, ShaderStage stage)
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
                    var includeSrc = ProcessRawSource(manager, metadata, resReader.ReadToEnd(), stage);
                    ssmd.Concat(includeSrc.Metadata);
                    output.Append(includeSrc.Source);

                    if (!includeSrc.Source.EndsWith("\n"))
                        output.AppendLine();
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #ifver directives.
        /// </summary>
        private static Boolean ProcessIfVerDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd, ShaderStage stage)
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

                var uvVersionIsGLES = GL.IsGLES;
                var uvVersionMajor = GL.MajorVersion;
                var uvVersionMinor = GL.MinorVersion;
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
                {
                    var includedSource = ProcessRawSource(manager, metadata, source, stage);
                    ssmd.Concat(includedSource.Metadata);
                    output.Append(includedSource.Source);

                    if (!includedSource.Source.EndsWith("\n"))
                        output.AppendLine();
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #sampler directives.
        /// </summary>
        private static Boolean ProcessSamplerDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd)
        {
            var samplerMatch = regexSamplerDirective.Match(line);
            if (samplerMatch.Success)
            {
                var sampler = Int32.Parse(samplerMatch.Groups["sampler"].Value);
                var uniform = samplerMatch.Groups["uniform"].Value;

                ssmd.AddPreferredSamplerIndex(uniform, sampler);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #param directives.
        /// </summary>
        private static Boolean ProcessParamDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd)
        {
            var paramMatch = regexParamDirective.Match(line);
            if (paramMatch.Success)
            {
                var parameter = paramMatch.Groups["parameter"].Value;

                ssmd.AddParameterHint(parameter);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #camera directives.
        /// </summary>
        private static Boolean ProcessCameraDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd)
        {
            var cameraMatch = regexCameraDirective.Match(line);
            if (cameraMatch.Success)
            {
                var parameter = cameraMatch.Groups["parameter"].Value;
                var uniform = cameraMatch.Groups["uniform"].Value;

                ssmd.AddCameraHint(parameter, uniform);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes #ifstage directives.
        /// </summary>
        private static Boolean ProcessIfStageDirective(ContentManager manager, IContentProcessorMetadata metadata, String line, StringBuilder output, ShaderSourceMetadata ssmd, ShaderStage shaderStage)
        {
            var ifStageMatch = regexIfStageDirective.Match(line);
            if (ifStageMatch.Success)
            {
                var source = ifStageMatch.Groups["source"].Value;
                var stage = ifStageMatch.Groups["stage"].Value?.ToLower();

                ShaderStage dirStage = ShaderStage.Unknown;
                if (stage == "vertex")
                {
                    dirStage = ShaderStage.Vertex;
                }
                else if (stage == "fragment")
                {
                    dirStage = ShaderStage.Fragment;
                }

                if (dirStage == shaderStage)
                {
                    var includedSource = ProcessRawSource(manager, metadata, source, shaderStage);
                    ssmd.Concat(includedSource.Metadata);
                    output.Append(includedSource.Source);

                    if (!includedSource.Source.EndsWith("\n"))
                        output.AppendLine();
                }

                return true;
            }
            return false;
        }

        // Regular expressions used to identify directives
        private static readonly Regex regexCStyleComment =
            new Regex(@"/\*.*?\*/", RegexOptions.Compiled);
        private static readonly Regex regexExternDirective =
            new Regex(@"^\s*(\/\/)?#extern(\s+""(?<name>.*)""\s*)?$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexIncludeDirective =
            new Regex(@"^\s*(\/\/)?#include\s+""(?<file>.*)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexincludeResourceDirective =
            new Regex(@"^\s*(\/\/)?#includeres\s+""(?<resource>.*)""\s*(?<asm>entry|executing)?\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexIfVerDirective =
            new Regex(@"^\s*(\/\/)?#(?<op>ifver(_gt|_gte|_lt|_lte)?)\s+\""(?<gles>es)?(?<version_maj>\d+).(?<version_min>\d+)\""\s+\{\s*(?<source>.+)\s*\}\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexSamplerDirective =
            new Regex(@"^\s*(\/\/)?#sampler\s+(?<sampler>\d+)\s+""(?<uniform>.*)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexParamDirective =
            new Regex(@"^\s*(\/\/)?#param\s+""(?<parameter>.*?)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexCameraDirective =
            new Regex(@"^\s*(\/\/)?#camera\((?<parameter>\w+)\)\s*""(?<uniform>\w+)""\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex regexIfStageDirective =
            new Regex(@"^\s*(\/\/)?#ifstage\s+\""(?<stage>\w+)?\""\s+\{\s*(?<source>.+)\s*\}\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
