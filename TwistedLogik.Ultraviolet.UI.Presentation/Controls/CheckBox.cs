using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a check box.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.CheckBox.xml")]
    public class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public CheckBox(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets a <see cref="Visibility"/> value that describes the visibility state
        /// of the radio button's mark.
        /// </summary>
        private Visibility MarkVisibility
        {
            get { return Checked ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
