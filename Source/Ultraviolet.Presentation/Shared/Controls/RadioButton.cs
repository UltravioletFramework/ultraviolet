using System;
using Ultraviolet.Presentation.Controls.Primitives;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.RadioButton.xml")]
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes the <see cref="RadioButton"/> type.
        /// </summary>
        static RadioButton()
        {
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(RadioButton), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Top));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public RadioButton(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override void OnToggle(Boolean toggledByUser)
        {
            if (!IsChecked.GetValueOrDefault())
            {
                IsChecked = true;
                if (toggledByUser)
                {
                    OnCheckedByUser();
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnChecked()
        {
            var parent = LogicalTreeHelper.GetParent(this);
            if (parent != null)
            {
                var children = LogicalTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < children; i++)
                {
                    var sibling = LogicalTreeHelper.GetChild(parent, i);
                    if (sibling == this)
                        continue;

                    var radioButton = sibling as RadioButton;
                    if (radioButton == null)
                        continue;

                    radioButton.IsChecked = false;
                }
            }

            base.OnChecked();
        }
    }
}
