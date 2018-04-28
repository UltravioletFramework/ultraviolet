using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a control which displays a single item of content.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Content")]
    public abstract class ContentControl : Control, IItemContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ContentControl(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
        
        /// <inheritdoc/>
        void IItemContainer.PrepareItemContainer(Object item)
        {
            treatContentAsLogicalChild = false;
            Content = item;
        }

        /// <summary>
        /// Gets or sets the control's content.
        /// </summary>
        /// <value>The <see cref="Object"/> that is displayed by the control. The
        /// default value is <see langword="null"/></value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentProperty"/></dpropField>
        ///     <dpropStylingName>content</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.CoerceObjectToString"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Object Content
        {
            get { return GetValue<Object>(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the content control's content when that content
        /// is being displayed as string.
        /// </summary>
        /// <value>A format string that specifies how to format the control's content. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentStringFormatProperty"/></dpropField>
        ///     <dpropStylingName>content-string-format</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.CoerceObjectToString"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String ContentStringFormat
        {
            get { return GetValue<String>(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the control has any content.
        /// </summary>
        /// <value><see langword="true"/> if the control currently has content; otherwise, <see langword="false"/>. The
        /// default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HasContentProperty"/></dpropField>
        ///     <dpropStylingName>has-content</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean HasContent
        {
            get { return GetValue<Boolean>(HasContentProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="Content"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content'.</remarks>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(ContentControl),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString, HandleContentChanged));

        /// <summary>
        /// Identifies the <see cref="ContentStringFormat"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-string-format'.</remarks>
        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(String), typeof(ContentControl),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString));

        /// <summary>
        /// The private access key for the <see cref="HasContent"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey HasContentPropertyKey = DependencyProperty.RegisterReadOnly("HasContent", typeof(Boolean), typeof(ContentControl),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="HasContent"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'has-content'.</remarks>
        public static readonly DependencyProperty HasContentProperty = HasContentPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value indicating whether the content control treats its content as a logical child.
        /// </summary>
        internal Boolean TreatContentAsLogicalChild
        {
            get { return treatContentAsLogicalChild; }
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
        /// Occurs when the value of the <see cref="Content"/> property is changed.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="Content"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Content"/> property.</param>
        protected virtual void OnContentChanged(Object oldValue, Object newValue)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Content"/> dependency property changes.
        /// </summary>
        private static void HandleContentChanged(DependencyObject dobj, Object oldValue, Object newValue)
        {
            var control = (ContentControl)dobj;

            var oldElement = control.contentElement;
            if (oldElement != null)
            {
                if (control.TreatContentAsLogicalChild)
                {
                    oldElement.ChangeLogicalParent(null);
                }
            }

            control.contentElement = control.Content as UIElement;
            control.SetValue<Boolean>(HasContentPropertyKey, control.Content != null);

            var newElement = control.contentElement;
            if (newElement != null)
            {
                if (control.TreatContentAsLogicalChild)
                {
                    newElement.ChangeLogicalParent(control);
                }
            }

            control.OnContentChanged(oldValue, newValue);
        }

        // State values.
        private UIElement contentElement;
        private Boolean treatContentAsLogicalChild = true;
    }
}
