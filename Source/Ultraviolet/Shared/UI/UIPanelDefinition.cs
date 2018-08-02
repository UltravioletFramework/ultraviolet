using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a UI panel's definition file.
    /// </summary>
    public class UIPanelDefinition
    {
        /// <summary>
        /// Gets the path to the asset file from which the panel definition was loaded.
        /// </summary>
        public String AssetFilePath { get; internal set; }

        /// <summary>
        /// Gets the amount of time over which the panel will transition to
        /// its open state if no time is explicitly specified.
        /// </summary>
        public TimeSpan DefaultOpenTransitionDuration { get; internal set; }

        /// <summary>
        /// Gets the amount of time over which the panel will transition to
        /// its closed state if no time is explicitly specified.
        /// </summary>
        public TimeSpan DefaultCloseTransitionDuration { get; internal set; }

        /// <summary>
        /// Gets the root element of the panel definition.
        /// </summary>
        public XElement RootElement { get; internal set; }

        /// <summary>
        /// Gets the XML element which describes the panel's view layout.
        /// </summary>
        public XElement ViewElement { get; internal set; }

        /// <summary>
        /// Gets the asset paths of the panel's associated style sheets, if it has any.
        /// </summary>
        public IEnumerable<UIStyleSheetAsset> StyleSheetAssets { get; internal set; }

        /// <summary>
        /// Gets the contents of the panel's associated style sheets, if it has any.
        /// </summary>
        public IEnumerable<UIStyleSheetSource> StyleSheetSources { get; internal set; }

        /// <summary>
        /// Gets the panel's collection of directives, if it has any.
        /// </summary>
        public IEnumerable<UIPanelDirective> Directives { get; internal set; }
    }
}
