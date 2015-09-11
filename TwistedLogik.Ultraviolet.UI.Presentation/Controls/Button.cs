using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Button.xml")]
    public class Button : ButtonBase
    {
        /// <summary>
        /// Initializes the button type.
        /// </summary>
        static Button()
        {
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(Button), new PropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(Button), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Button(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
