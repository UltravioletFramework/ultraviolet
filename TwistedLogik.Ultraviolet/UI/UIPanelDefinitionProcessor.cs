using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a content processor which processes XML panel
    /// definitions into instances of the <see cref="UIPanelDefinition"/> class.
    /// </summary>
    [ContentProcessor]
    internal sealed class UIPanelDefinitionProcessor : ContentProcessor<XDocument, UIPanelDefinition>
    {
        /// <inheritdoc/>
        public override UIPanelDefinition Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var defaultOpenTransitionDuration  = input.Root.AttributeValueDouble("DefaultOpenTransitionDuration") ?? 0.0;
            var defaultCloseTransitionDuration = input.Root.AttributeValueDouble("DefaultCloseTransitionDuration") ?? 0.0;

            return new UIPanelDefinition()
            {
                DefaultOpenTransitionDuration  = TimeSpan.FromMilliseconds(defaultOpenTransitionDuration),
                DefaultCloseTransitionDuration = TimeSpan.FromMilliseconds(defaultCloseTransitionDuration),
                Stylesheet                     = ProcessStylesheets(manager, metadata, input),
                ViewElement                    = input.Root.Element("View"),
            };
        }

        /// <summary>
        /// Processes the panel definition's stylesheet.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The <see cref="UvssDocument"/> that was loaded.</returns>
        private UvssDocument ProcessStylesheets(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var fss = FileSystemService.Create();

            var stylesheetRoot     = Path.GetDirectoryName(metadata.AssetPath);
            var stylesheetElements = input.Root.Elements("Stylesheet");
            var stylesheetPaths    = stylesheetElements.Select(x => manager.ResolveAssetFilePath(Path.Combine(stylesheetRoot, x.Value)));
            var stylesheetSource   = new StringBuilder();

            foreach (var path in stylesheetPaths)
            {
                using (var stream = fss.OpenRead(path))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var source = reader.ReadToEnd();
                        stylesheetSource.AppendLine(source);
                    }
                }
            }

            var stylesheetDocument = UvssDocument.Parse(stylesheetSource.ToString());
            return stylesheetDocument;
        }
    }
}
