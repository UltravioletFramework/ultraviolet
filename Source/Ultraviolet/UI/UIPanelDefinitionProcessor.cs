using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core.Xml;
using Ultraviolet.Platform;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a content processor which processes XML panel
    /// definitions into instances of the <see cref="UIPanelDefinition"/> class.
    /// </summary>
    [ContentProcessor]
    internal sealed class UIPanelDefinitionProcessor : ContentProcessor<XDocument, UIPanelDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanelDefinitionProcessor"/> class.
        /// </summary>
        public UIPanelDefinitionProcessor() { }

        /// <inheritdoc/>
        public override UIPanelDefinition Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var fss = FileSystemService.Create();

            var defaultOpenTransitionDuration = input.Root.AttributeValueDouble("DefaultOpenTransitionDuration") ?? 0.0;
            var defaultCloseTransitionDuration = input.Root.AttributeValueDouble("DefaultCloseTransitionDuration") ?? 0.0;

            var styleSheetRoot = Path.GetDirectoryName(metadata.AssetPath);
            var styleSheetElements = input.Root.Elements("StyleSheet");
            var styleSheetInfos = styleSheetElements.Select(x => new
            {
                AssetPath = Path.Combine(styleSheetRoot, x.Value),
                Cultures = ((String)x.Attribute("Cultures"))?.Split(',').Select(y => y.Trim()),
                Languages = ((String)x.Attribute("Languages"))?.Split(',').Select(y => y.Trim()),
            });

            var styleSheetAssets = styleSheetInfos.Select(x => new UIStyleSheetAsset(x.AssetPath));
            var styleSheetSources = new List<UIStyleSheetSource>();

            foreach (var styleSheetInfo in styleSheetInfos)
            {
                var styleSheetFilePath = manager.ResolveAssetFilePath(styleSheetInfo.AssetPath, metadata.AssetDensity, metadata.IsLoadedFromSolution);
                using (var stream = fss.OpenRead(styleSheetFilePath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var source = reader.ReadToEnd();
                        styleSheetSources.Add(new UIStyleSheetSource(source, styleSheetInfo.Cultures, styleSheetInfo.Languages));
                    }
                }
            }

            var directives = new List<UIPanelDirective>();

            var xmlDirectives = input.Root.Elements("Directive");
            foreach (var xmlDirective in xmlDirectives)
            {
                var directiveType = (String)xmlDirective.Attribute("Type");
                if (String.IsNullOrEmpty(directiveType))
                    throw new InvalidDataException(UltravioletStrings.ViewDirectiveMustHaveType.Format(metadata.AssetFilePath));

                directives.Add(new UIPanelDirective(directiveType, xmlDirective.Value));
            }

            foreach (var styleSheetAsset in styleSheetAssets)
                metadata.AddAssetDependency(styleSheetAsset.Asset);

            return new UIPanelDefinition()
            {
                AssetFilePath = metadata.AssetFilePath,
                DefaultOpenTransitionDuration = TimeSpan.FromMilliseconds(defaultOpenTransitionDuration),
                DefaultCloseTransitionDuration = TimeSpan.FromMilliseconds(defaultCloseTransitionDuration),
                RootElement = input.Root,
                ViewElement = input.Root.Element("View"),
                StyleSheetAssets = styleSheetAssets,
                StyleSheetSources = styleSheetSources,
                Directives = directives,
            };
        }
    }
}
