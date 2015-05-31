using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Platform;

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
            var fss = FileSystemService.Create();

            var defaultOpenTransitionDuration  = input.Root.AttributeValueDouble("DefaultOpenTransitionDuration") ?? 0.0;
            var defaultCloseTransitionDuration = input.Root.AttributeValueDouble("DefaultCloseTransitionDuration") ?? 0.0;

            var styleSheetRoot     = Path.GetDirectoryName(metadata.AssetPath);
            var styleSheetElements = input.Root.Elements("StyleSheet");
            var styleSheetPaths    = styleSheetElements.Select(x => manager.ResolveAssetFilePath(Path.Combine(styleSheetRoot, x.Value)));
            var styleSheetSources  = new List<String>();

            foreach (var styleSheetPath in styleSheetPaths)
            {
                using (var stream = fss.OpenRead(styleSheetPath))
                {
                    using(var reader = new StreamReader(stream))
                    {
                        var source = reader.ReadToEnd();
                        styleSheetSources.Add(source);
                    }
                }
            }

            return new UIPanelDefinition()
            {
                DefaultOpenTransitionDuration  = TimeSpan.FromMilliseconds(defaultOpenTransitionDuration),
                DefaultCloseTransitionDuration = TimeSpan.FromMilliseconds(defaultCloseTransitionDuration),
                RootElement                    = input.Root,
                ViewElement                    = input.Root.Element("View"),
                StyleSheets                    = styleSheetSources,
            };
        }    
    }
}
