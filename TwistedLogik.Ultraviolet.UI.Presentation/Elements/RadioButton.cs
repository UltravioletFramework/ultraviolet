using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UvmlKnownType("RadioButton", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.RadioButton.xml")]
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public RadioButton(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override void ToggleChecked()
        {
            if (!Checked)
            {
                Checked = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnCheckedChanged()
        {
            if (Parent != null && Checked)
            {
                for (int i = 0; i < Parent.LogicalChildren; i++)
                {
                    var sibling = Parent.GetLogicalChild(i);
                    if (sibling == this)
                        continue;

                    var radioButton = sibling as RadioButton;
                    if (radioButton == null)
                        continue;

                    radioButton.Checked = false;
                }
            }
            base.OnCheckedChanged();
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
