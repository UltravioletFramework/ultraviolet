using System;
using System.ComponentModel;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which displays a single item of content.
    /// </summary>
    [DefaultProperty("Content")]
    public abstract class ContentControl : Control, IItemContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ContentControl(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        void IItemContainer.PrepareItemContainer(Object item)
        {
            treatContentAsLogicalChild = false;
        }

        /// <summary>
        /// Gets or sets the control's content.
        /// </summary>
        public Object Content
        {
            get { return GetValue<Object>(ContentProperty); }
            set { SetValue<Object>(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty); }
            set { SetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the control's content.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalContentAlignmentProperty); }
            set { SetValue<VerticalAlignment>(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment"/> property changes.
        /// </summary>
        public event UIElementEventHandler HorizontalContentAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment"/> property changes.
        /// </summary>
        public event UIElementEventHandler VerticalContentAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Content"/> property changes.
        /// </summary>
        public event UIElementEventHandler ContentChanged;

        /// <summary>
        /// Identifies the <see cref="HorizontalContentAlignment"/> dependency property.
        /// </summary>
        [Styled("content-halign")]
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(ContentControl),
            new DependencyPropertyMetadata(HandleHorizontalContentAlignmentChanged, () => HorizontalAlignment.Left, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        [Styled("content-valign")]
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(ContentControl),
            new DependencyPropertyMetadata(HandleVerticalContentAlignmentChanged, () => VerticalAlignment.Top, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Content"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(ContentControl), 
            new DependencyPropertyMetadata(HandleContentChanged, null, DependencyPropertyOptions.AffectsMeasure | DependencyPropertyOptions.CoerceObjectToString));

        /// <summary>
        /// Gets a value indicating whether the content control treats its content as a logical child.
        /// </summary>
        internal Boolean TreatContentAsLogicalChild
        {
            get { return treatContentAsLogicalChild; }
        }

        /// <summary>
        /// Gets the control's content presenter.
        /// </summary>
        internal ContentPresenter ContentPresenter
        {
            get { return contentPresenter; }
            set { contentPresenter = value; }
        }

        /// <inheritdoc/>
        protected internal override void RemoveLogicalChild(UIElement child)
        {
            if (TreatContentAsLogicalChild && Content == child)
            {
                Content = null;
            }
            base.RemoveLogicalChild(child);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            if (TreatContentAsLogicalChild && contentElement != null)
            {
                if (childIndex == 0)
                {
                    return contentElement;
                }
                return base.GetLogicalChild(childIndex - 1);
            }
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            return base.GetVisualChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return (TreatContentAsLogicalChild && contentElement != null ? 1 : 0) + base.LogicalChildrenCount; }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return base.VisualChildrenCount; }
        }

        /// <summary>
        /// Raises the <see cref="HorizontalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalContentAlignmentChanged()
        {
            var temp = HorizontalContentAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VerticalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalContentAlignmentChanged()
        {
            var temp = VerticalContentAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ContentChanged"/> event.
        /// </summary>
        protected virtual void OnContentChanged()
        {
            var temp = ContentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleHorizontalContentAlignmentChanged(DependencyObject dobj)
        {
            var control = (ContentControl)dobj;
            control.OnHorizontalContentAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleVerticalContentAlignmentChanged(DependencyObject dobj)
        {
            var control = (ContentControl)dobj;
            control.OnVerticalContentAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Content"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleContentChanged(DependencyObject dobj)
        {
            var control = (ContentControl)dobj;

            if (control.TreatContentAsLogicalChild)
            {
                var oldElement = control.contentElement;
                if (oldElement != null)
                    oldElement.ChangeLogicalAndVisualParents(null, null);
            }

            control.contentElement = control.Content as UIElement;

            if (control.TreatContentAsLogicalChild)
            {
                var newElement = control.contentElement;
                if (newElement != null)
                    newElement.ChangeLogicalAndVisualParents(control, control.ContentPresenter);
            }

            control.UpdateTextParserCache();
            control.OnContentChanged();
        }

        /// <summary>
        /// Updates the cache which contains the element's parsed text.
        /// </summary>
        private void UpdateTextParserCache()
        {
            textParserResult.Clear();

            if (View == null)
                return;

            var content = Content;
            if (content != null && contentElement == null)
            {
                var contentAsString = content.ToString();
                View.Resources.TextRenderer.Parse(contentAsString, textParserResult);
            }

            InvalidateArrange();
        }

        /// <summary>
        /// Updates the cache which contains the element's laid-out text.
        /// </summary>
        /// <param name="availableSize">The amount of space in which the element's text can be laid out.</param>
        private void UpdateTextLayoutCache(Size2D availableSize)
        {
            textLayoutResult.Clear();

            if (View == null)
                return;

            if (textParserResult.Count > 0 && Font.IsLoaded)
            {
                var availableWidth  = (Int32)Display.DipsToPixels(availableSize.Width);
                var availableHeight = (Int32)Display.DipsToPixels(availableSize.Height);

                var flags    = LayoutUtil.ConvertAlignmentsToTextFlags(HorizontalContentAlignment, VerticalContentAlignment);                
                var settings = new TextLayoutSettings(Font, availableWidth, availableHeight, flags, FontStyle);
                View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutResult, settings);
            }
        }

        // State values.
        private UIElement contentElement;
        private ContentPresenter contentPresenter;
        private Boolean treatContentAsLogicalChild = true;

        // Cached parser/layout results for content text.
        private readonly TextParserResult textParserResult = new TextParserResult();
        private readonly TextLayoutResult textLayoutResult = new TextLayoutResult();
    }
}
