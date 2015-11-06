using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a control that contains multiple tabbed pages.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.TabControl.xml")]
    [UvmlPlaceholder("ItemsPanel", typeof(TabPanel))]
    public class TabControl : Selector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TabControl(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets a <see cref="Dock"/> value which specifies how the tab headers are positioned relative to their content.
        /// </summary>
        public Dock TabStripPlacement
        {
            get { return GetValue<Dock>(TabStripPlacementProperty); }
            set { SetValue(TabStripPlacementProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabStripPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(TabControl),
            new PropertyMetadata<Dock>(Dock.Top));

        /// <summary>
        /// Called to inform the tab control that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(TabItem container)
        {
            var item = ItemContainerGenerator.ItemFromContainer(container);
            if (item == null)
                return;

            var dobj = item as DependencyObject;
            if (dobj == null)
                return;

            BeginChangeSelection();

            UnselectAllItems();
            SelectItem(item);

            EndChangeSelection();
        }

        /// <summary>
        /// Called to inform the tab control that one of its items changed its content.
        /// </summary>
        /// <param name="item">The item that was changed.</param>
        internal void HandleItemContentChanged(TabItem item)
        {
            if (SelectedIndex < 0 || SelectedIndex >= Items.Count)
                return;

            if (item != Items[SelectedIndex])
                return;

            UpdateTabContent();
        }

        /// <inheritdoc/>
        protected internal override Panel CreateItemsPanel()
        {
            return new TabPanel(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainer(DependencyObject element)
        {
            return element is TabItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(DependencyObject container, Object item)
        {
            var ti = container as TabItem;
            if (ti == null)
                return false;

            return ti.Content == item;
        }

        /// <inheritdoc/>
        protected override void OnSelectionChanged()
        {
            UpdateTabContent();

            base.OnSelectionChanged();
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged()
        {
            if ((SelectedIndex < 0 || SelectedIndex > Items.Count) && Items.Count > 0)
            {
                SelectedIndex = 0;
            }
            base.OnItemsChanged();
        }

        /// <summary>
        /// Updates the displayed tab content.
        /// </summary>
        private void UpdateTabContent()
        {
            if (PART_ContentPresenter != null)
            {
                var item = SelectedItem as TabItem;
                if (item != null)
                {
                    var content = item.Content;
                    PART_ContentPresenter.Content = content;
                }
                else
                {
                    PART_ContentPresenter.Content = null;
                }
            }
        }

        // Component references.
        private readonly ContentPresenter PART_ContentPresenter = null;
    }
}
