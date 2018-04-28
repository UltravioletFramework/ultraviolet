using System;
using Ultraviolet.Presentation.Controls.Primitives;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a check box.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.CheckBox.xml")]
    public class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes the <see cref="CheckBox"/> type.
        /// </summary>
        static CheckBox()
        {
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(CheckBox), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Top));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public CheckBox(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
