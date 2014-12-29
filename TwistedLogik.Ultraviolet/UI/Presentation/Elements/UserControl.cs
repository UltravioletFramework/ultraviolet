using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which is defined by the application using a layout definition.
    /// </summary>
    public abstract class UserControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        protected UserControl(UltravioletContext uv, String id)
            : base(uv, id)
        { }
    }
}
