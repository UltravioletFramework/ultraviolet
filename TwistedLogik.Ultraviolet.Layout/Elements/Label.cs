using System;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a label on a user interface.
    /// </summary>
    public class Label : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public Label(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }
    }
}
