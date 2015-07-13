using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a <see cref="Control"/> which is defined by the user with a UVML document
    /// that is loaded at runtime.
    /// </summary>
    [UvmlKnownType]
    public abstract class UserControl : Control
    {
        /// <summary>
        /// Initializes the <see cref="UserControl"/> type.
        /// </summary>
        static UserControl()
        {
            FocusableProperty.OverrideMetadata(typeof(UserControl), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public UserControl(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
