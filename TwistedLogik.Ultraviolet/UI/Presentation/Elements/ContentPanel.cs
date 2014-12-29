using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a panel which is used to indicate the position of content within container component templates.
    /// </summary>
    [UIElement("ContentPanel")]
    public sealed class ContentPanel : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ContentPanel(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }
    }
}
