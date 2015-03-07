using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an item in a <see cref="ListBox"/> control.
    /// </summary>
    [UvmlKnownType("ListBoxItem", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ListBoxItem.xml")]
    public class ListBoxItem : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ListBoxItem(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }
    }
}
