using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ListBox.xml")]
    public class ListBox : Selector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ListBox(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected internal override Panel CreateItemsPanel()
        {
            return new StackPanel(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override UIElement CreateItemContainer()
        {
            return new ListBoxItem(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainer(UIElement element)
        {
            return element is ListBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(UIElement container, Object item)
        {
            var lbi = container as ListBoxItem;
            if (lbi == null)
                return false;

            return lbi.Content == item;
        }

        /// <inheritdoc/>
        protected override void AssociateItemContainerWithItem(UIElement container, Object item)
        {
            var lbi = container as ListBoxItem;
            if (lbi == null)
                return;

            lbi.Content = item;
        }
    }
}
