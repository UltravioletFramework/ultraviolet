using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a control that allows the user to select items.
    /// </summary>
    [UvmlKnownType]
    public abstract class Selector : ItemsControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Selector"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within a layout.</param>
        public Selector(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Sets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="isSelected">A value indicating whether the specified element is selected.</param>
        public void SetIsSelected(DependencyObject element, Boolean isSelected)
        {
            Contract.Require(element, "element");

            element.SetValue<Boolean>(IsSelectedProperty, isSelected);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is selected; otherwise, <c>false</c>.</returns>
        public Boolean GetIsSelected(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsSelectedProperty);
        }

        /// <summary>
        /// Gets or sets the index of the first item in the current selection. If nothing is currently
        /// selected, this property will return negative one (-1).
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return GetValue<Int32>(SelectedIndexProperty); }
            set { SetValue<Int32>(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the first item in the current selection.
        /// </summary>
        public Object SelectedItem
        {
            get { return GetValue<Object>(SelectedItemProperty); }
            set { SetValue<Object>(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex"/> property changes.
        /// </summary>
        public event UpfEventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedItem"/> property changes.
        /// </summary>
        public event UpfEventHandler SelectedItemChanged;

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(Selector),
            new DependencyPropertyMetadata(HandleSelectedIndexChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(Selector),
            new DependencyPropertyMetadata(HandleSelectedItemChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the IsSelected attached property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(Boolean), typeof(Selector), 
            new DependencyPropertyMetadata(null, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Raises the <see cref="SelectedIndexChanged"/> event.
        /// </summary>
        protected virtual void OnSelectedIndexChanged()
        {
            var temp = SelectedIndexChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectedItemChanged"/> event.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            var temp = SelectedItemChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleSelectedIndexChanged(DependencyObject dobj)
        {
            var selector = (Selector)dobj;
            selector.OnSelectedIndexChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedItem"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleSelectedItemChanged(DependencyObject dobj)
        {
            var selector = (Selector)dobj;
            selector.OnSelectedItemChanged();
        }
    }
}
