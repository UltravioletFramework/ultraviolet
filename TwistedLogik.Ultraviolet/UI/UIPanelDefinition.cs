using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a UI panel's definition file.
    /// </summary>
    public class UIPanelDefinition
    {
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
        /// The contents of the panel's associated style sheets, if it has any.
        /// </summary>
        public IEnumerable<String> StyleSheets { get; internal set; }
    }
}
