using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a UI panel's definition.
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
        /// Gets the root directory of the panel's layout files.
        /// </summary>
        public String LayoutRootDirectory { get; internal set; }

        /// <summary>
        /// Gets the source file containing the panel's layout information.
        /// </summary>
        public String LayoutSource { get; internal set; }

        /// <summary>
        /// Gets the XML element that contains the panel's layout information.
        /// </summary>
        public XElement Layout { get; internal set; }
    }
}
