using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a UI panel's definition file.
    /// </summary>
    public sealed class UIPanelDefinition
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
        /// Gets the stylesheet which is applied to the panel's view.
        /// </summary>
        public UvssDocument Stylesheet { get; internal set; }

        /// <summary>
        /// Gets the set of XML elements which contain stylesheet references.
        /// </summary>
        public IEnumerable<XElement> StylesheetElements { get; internal set; }

        /// <summary>
        /// Gets the set of XML elements which contain custom constrol registrations.
        /// </summary>
        public IEnumerable<XElement> ControlElements { get; internal set; }

        /// <summary>
        /// Gets the XML element which describes the panel's view layout.
        /// </summary>
        public XElement ViewElement { get; internal set; }
    }
}
