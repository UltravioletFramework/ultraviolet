using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element which is used to indicate the position of child content within a component template.
    /// </summary>
    [UIElement("ContentViewer")]
    public sealed class ContentViewer : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ContentViewer(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }
    }
}
