using System;
using System.Text;
using System.Text.RegularExpressions;
using TwistedLogik.Ultraviolet.Content;
using System.IO;

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

        // Regular expressions used to identify directives
        private static readonly Regex regexIncludeDirective =
            new Regex(@"^\s*#include ""(?<file>.*)""\s*$", RegexOptions.Singleline);      
    }
}
