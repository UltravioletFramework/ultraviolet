using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="ComboBox"/> control.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ComboBoxItem.xml")]
    public class ComboBoxItem : ListBoxItem
    {
        /// <summary>
        /// Initializes the <see cref="ComboBoxItem"/> type.
        /// </summary>
        static ComboBoxItem()
        {
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ComboBoxItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Once));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ComboBoxItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Local));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ComboBoxItem(UltravioletContext uv, String name)
            : base(uv, name)
        {
            HighlightOnSelect    = false;
            HighlightOnMouseOver = !Generic.IsTouchDeviceAvailable;
        }

        /// <inheritdoc/>
        protected override void OnGenericInteraction(UltravioletResource device, RoutedEventData data)
        {
            if (!data.Handled)
            {
                var comboBox = ItemsControl.ItemsControlFromItemContainer(this) as ComboBox;
                if (comboBox != null)
                {
                    comboBox.HandleItemClicked(this);
                }
                data.Handled = true;
            }
            base.OnGenericInteraction(device, data);
        }
        
        /// <inheritdoc/>
        protected override void OnContentChanged(Object oldValue, Object newValue)
        {
            var comboBox = ItemsControl.ItemsControlFromItemContainer(this) as ComboBox;
            if (comboBox != null)
            {
                comboBox.HandleItemChanged(this);
            }
            base.OnContentChanged(oldValue, newValue);
        }
    }
}
