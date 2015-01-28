using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a <see cref="Control"/> which is defined by the user with a UVML document
    /// that is loaded at runtime.
    /// </summary>
    public abstract class UserControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public UserControl(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }
    }
}
