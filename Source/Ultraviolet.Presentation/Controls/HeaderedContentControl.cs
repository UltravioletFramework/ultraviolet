using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a content control with a header.
    /// </summary>
    [UvmlKnownType]
    public class HeaderedContentControl : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderedContentControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public HeaderedContentControl(UltravioletContext uv, String name) 
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the control's header.
        /// </summary>
        /// <value>An <see cref="Object"/> which represents the control's header content. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HeaderProperty"/></dpropField>
        ///     <dpropStylingName>header</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.CoerceObjectToString"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Object Header
        {
            get { return GetValue<Object>(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the control's header when that header
        /// is being displayed as string.
        /// </summary>
        /// <value>A format string that specifies how to format the control's header content. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HeaderStringFormatProperty"/></dpropField>
        ///     <dpropStylingName>header-string-format</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.CoerceObjectToString"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String HeaderStringFormat
        {
            get { return GetValue<String>(HeaderStringFormatProperty); }
            set { SetValue(HeaderStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the control has header content.
        /// </summary>
        /// <value><see langword="true"/> if the control has header content; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HasHeaderProperty"/></dpropField>
        ///     <dpropStylingName>has-header</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean HasHeader
        {
            get { return GetValue<Boolean>(HasHeaderProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Header"/> dependency property.</value>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(Object), typeof(HeaderedContentControl),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString, HandleHeaderChanged));

        /// <summary>
        /// Identifies the <see cref="HeaderStringFormat"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HeaderStringFormat"/> dependency property.</value>
        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(String), typeof(HeaderedContentControl),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString));

        /// <summary>
        /// The private access key for the <see cref="HasHeader"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly("HasHeader", typeof(Boolean), typeof(HeaderedContentControl),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="HasHeader"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HasHeader"/> dependency property.</value>
        public static readonly DependencyProperty HasHeaderProperty = HasHeaderPropertyKey.DependencyProperty;

        /// <summary>
        /// Occurs when the value of the <see cref="Header"/> property is changed.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="Header"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Header"/> property.</param>
        protected virtual void OnHeaderChanged(Object oldValue, Object newValue)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Header"/> dependency property changes.
        /// </summary>
        private static void HandleHeaderChanged(DependencyObject dobj, Object oldValue, Object newValue)
        {

        }
    }
}
