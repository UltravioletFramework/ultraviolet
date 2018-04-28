using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="ComboBox"/> control.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.ComboBoxItem.xml")]
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
            HighlightOnSelect = false;
            HighlightOnMouseOver = true;
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left && !data.Handled)
            {
                Select();
                OnSelectedByUser();

                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchTap(TouchDevice device, Int64 id, Double x, Double y, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id) && !data.Handled)
                {
                    Select();
                    OnSelectedByUser();

                    data.Handled = true;
                }
            }
            base.OnTouchTap(device, id, x, y, data);
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

        /// <summary>
        /// Selects the item.
        /// </summary>
        private void Select()
        {
            var comboBox = ItemsControl.ItemsControlFromItemContainer(this) as ComboBox;
            if (comboBox != null)
            {
                comboBox.HandleItemClicked(this);
                OnSelected();
            }
        }
    }
}
