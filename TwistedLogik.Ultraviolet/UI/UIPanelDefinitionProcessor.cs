using System;
using System.IO;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a content processor which processes XML panel
    /// definitions into instances of the <see cref="UIPanelDefinition"/> class.
    /// </summary>
    [ContentProcessor]
    internal sealed class UIPanelDefinitionProcessor : ContentProcessor<XDocument, UIPanelDefinition>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override UIPanelDefinition Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var defaultOpenTransitionDuration  = input.Root.AttributeValueDouble("DefaultOpenTransitionDuration") ?? 0.0;
            var defaultCloseTransitionDuration = input.Root.AttributeValueDouble("DefaultCloseTransitionDuration") ?? 0.0;

            var layout        = input.Root.Element("Layout");
            var layoutSource  = (layout == null) ? null : layout.AttributeValueString("Source");
            var layoutRootDir = Path.GetDirectoryName(metadata.AssetPath);

            return new UIPanelDefinition()
            {
                DefaultOpenTransitionDuration  = TimeSpan.FromMilliseconds(defaultOpenTransitionDuration),
                DefaultCloseTransitionDuration = TimeSpan.FromMilliseconds(defaultCloseTransitionDuration),
                Layout                         = layout,
                LayoutSource                   = layoutSource,
                LayoutRootDirectory            = layoutRootDir
            };
        }
    }
}
