using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the configuration settings for the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationConfiguration"/> class.
        /// </summary>
        public PresentationFoundationConfiguration()
        {
            OutOfBandRenderTargetSize = 1024;
        }

        /// <summary>
        /// Gets or sets the size of the Foundation's out-of-band render targets. These are used to render any
        /// elements which need to be drawn "out-of-band," which is to say, prior to the rest of the visual tree.
        /// This is primarily used for elements which have a visual transformation applied to them; such elements are drawn
        /// onto out-of-band render targets, and then these render targets are transformed and rendered to the back buffer.
        /// If you need to transform very large elements, make this value bigger; if you don't need to transform elements,
        /// you can make this value smaller to save video memory.
        /// </summary>
        public Int32 OutOfBandRenderTargetSize
        {
            get;
            set;
        }
    }
}
