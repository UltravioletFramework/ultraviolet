using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a combo box with a drop down list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ComboBox.xml")]
    public class ComboBox : Selector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ComboBox(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Called to inform the combo box that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(ComboBoxItem container)
        {
            var item = ItemContainerGenerator.ItemFromContainer(container);
            if (item == null)
                return;

            var dobj = item as DependencyObject;
            if (dobj == null || !GetIsSelected(dobj))
            {
                BeginChangeSelection();

                UnselectAllItems();
                SelectItem(item);

                EndChangeSelection();
            }
        }

        private Double ActualWidth
        {
            get { return AbsoluteBounds.Width; }
        }

        /// <inheritdoc/>
        protected internal override Panel CreateItemsPanel()
        {
            return new StackPanel(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ComboBoxItem(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainer(DependencyObject element)
        {
            return element is ComboBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(DependencyObject container, Object item)
        {
            var cbi = container as ComboBoxItem;
            if (cbi == null)
                return false;

            return cbi.Content == item;
        }

        protected override void OnSelectedItemAdded(object item)
        {
            base.OnSelectedItemAdded(item);
        }

        protected override void OnSelectedItemRemoved(object item)
        {
            base.OnSelectedItemRemoved(item);
        }

        /// <summary>
        /// Handles the <see cref="UIElement.PreviewMouseDown"/> event for the PART_Input component.
        /// </summary>
        private void PART_Input_PreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (PART_Popup != null)
            {
                PART_Popup.IsOpen = !PART_Popup.IsOpen;
            }
        }

        // Component references.
        private readonly Popup PART_Popup = null;
    }
}
