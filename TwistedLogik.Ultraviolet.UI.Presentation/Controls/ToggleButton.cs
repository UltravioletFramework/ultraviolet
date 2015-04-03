using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface which can be toggled between its states.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ToggleButton.xml")]
    public class ToggleButton : ButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ToggleButton(UltravioletContext uv, String id)
            : base(uv, id)
        {
            VisualStateGroups.Create("checkstate", new[] { "unchecked", "checked" });

            SetDefaultValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            SetDefaultValue<VerticalAlignment>(VerticalContentAlignmentProperty, VerticalAlignment.Center);
        }

        /// <summary>
        /// Gets a value indicating whether the button is checked.
        /// </summary>
        public Boolean Checked
        {
            get { return GetValue<Boolean>(CheckedProperty); }
            set { SetValue<Boolean>(CheckedProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Checked"/> property changes.
        /// </summary>
        public event UpfEventHandler CheckedChanged;

        /// <summary>
        /// Identifies the Checked dependency property.
        /// </summary>
        public static readonly DependencyProperty CheckedProperty = DependencyProperty.Register("Checked", typeof(Boolean), typeof(ToggleButton),
            new PropertyMetadata(CommonBoxedValues.Boolean.False, HandleCheckedChanged));

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event.
        /// </summary>
        protected virtual void OnCheckedChanged()
        {
            var temp = CheckedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Toggles the value of the <see cref="Checked"/> property.
        /// </summary>
        protected virtual void ToggleChecked()
        {
            Checked = !Checked;
        }

        /// <inheritdoc/>
        protected override void OnButtonPressed()
        {
            ToggleChecked();

            View.CaptureMouse(this);
            base.OnButtonPressed();
        }

        /// <inheritdoc/>
        protected override void OnButtonReleased()
        {
            View.ReleaseMouse(this);
            base.OnButtonReleased();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Checked"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckedChanged(DependencyObject dobj)
        {
            var element = (ToggleButton)dobj;
            element.OnCheckedChanged();
            element.UpdateCheckState();
        }

        /// <summary>
        /// Transitions the button into the appropriate check state.
        /// </summary>
        private void UpdateCheckState()
        {
            if (Checked)
            {
                VisualStateGroups.GoToState("checkstate", "checked");
            }
            else
            {
                VisualStateGroups.GoToState("checkstate", "unchecked");
            }
        }
    }
}
