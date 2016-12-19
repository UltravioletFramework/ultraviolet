﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanelDefinitionProcessor"/> class.
        /// </summary>
        [Preserve]
        public UIPanelDefinitionProcessor() { }

        /// <inheritdoc/>
        public override UIPanelDefinition Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var fss = FileSystemService.Create();

            var defaultOpenTransitionDuration = (Double?)input.Root.Attribute("DefaultOpenTransitionDuration") ?? 0.0;
            var defaultCloseTransitionDuration = (Double?)input.Root.Attribute("DefaultCloseTransitionDuration") ?? 0.0;

            var styleSheetRoot = Path.GetDirectoryName(metadata.AssetPath);
            var styleSheetElements = input.Root.Elements().Where(x => x.Name.LocalName == "StyleSheet");
            var styleSheetPaths = styleSheetElements.Select(x => manager.ResolveAssetFilePath(Path.Combine(styleSheetRoot, x.Value)));
            var styleSheetSources = new List<String>();

            foreach (var styleSheetPath in styleSheetPaths)
            {
                using (var stream = fss.OpenRead(styleSheetPath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var source = reader.ReadToEnd();
                        styleSheetSources.Add(source);
                    }
                }
            }

            var directives = new List<UIPanelDirective>();

            var xmlDirectives = input.Root.Elements().Where(x => x.Name.LocalName == "Directive");
            foreach (var xmlDirective in xmlDirectives)
            {
                var directiveType = (String)xmlDirective.Attribute("Type");
                if (String.IsNullOrEmpty(directiveType))
                    throw new InvalidDataException(UltravioletStrings.ViewDirectiveMustHaveType.Format(metadata.AssetFilePath));

                directives.Add(new UIPanelDirective(directiveType, xmlDirective.Value));
            }

            return new UIPanelDefinition()
            {
                AssetFilePath = metadata.AssetFilePath,
                DefaultOpenTransitionDuration = TimeSpan.FromMilliseconds(defaultOpenTransitionDuration),
                DefaultCloseTransitionDuration = TimeSpan.FromMilliseconds(defaultCloseTransitionDuration),
                RootElement = input.Root,
                ViewElement = input.Root.Elements().Where(x => x.Name.LocalName == "View").SingleOrDefault(),
                StyleSheets = styleSheetSources,
                Directives = directives,
            };
        }
    }
}
