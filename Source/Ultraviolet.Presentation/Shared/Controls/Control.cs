using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Presentation.Documents;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which consists of multiple component elements.
    /// </summary>
    [UvmlKnownType]
    public abstract class Control : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        static Control()
        {
            FocusableProperty.OverrideMetadata(typeof(Control), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Control(UltravioletContext uv, String name)
            : base(uv, name)
        {
            var upf = uv.GetUI().GetPresentationFoundation();
            dataSourceWrapper = upf.CreateDataSourceWrapperForControl(this);

            LoadComponentRoot();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is included in tab navigation.
        /// </summary>
        /// <value><see langword="true"/> if the control is included in tab navigation; otherwise,
        /// <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsTabStopProperty"/></dpropField>
        ///     <dpropStylingName>tab-stop</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsTabStop
        {
            get { return GetValue<Boolean>(IsTabStopProperty); }
            set { SetValue(IsTabStopProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font used to draw the control's text.
        /// </summary>
        /// <value>A <see cref="SourcedResource{T}"/> value that specifies the font to
        /// use when rendering the control's text. The default value is an invalid resource.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FontProperty"/></dpropField>
        ///     <dpropStylingName>font</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedResource<UltravioletFont> Font
        {
            get { return GetValue<SourcedResource<UltravioletFont>>(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font style which is used to draw the control's text.
        /// </summary>
        /// <value>A <see cref="UltravioletFontStyle"/> value that specifies the style
        /// with which to draw the control's text. The default value is <see cref="UltravioletFontStyle.Regular"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FontStyleProperty"/></dpropField>
        ///     <dpropStylingName>font-style</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public UltravioletFontStyle FontStyle
        {
            get { return GetValue<UltravioletFontStyle>(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the control's foreground color.
        /// </summary>
        /// <value>A <see cref="Color"/> value that specifies the color with which
        /// to draw the control's foreground elements, such as text. The default 
        /// value is <see cref="Color.Black"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ForegroundProperty"/></dpropField>
        ///     <dpropStylingName>foreground</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color Foreground
        {
            get { return GetValue<Color>(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the control's background color.
        /// </summary>
        /// <value>A <see cref="Color"/> value that specifies the color with which
        /// to draw the control's background elements. The default 
        /// value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="BackgroundProperty"/></dpropField>
        ///     <dpropStylingName>background</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color Background
        {
            get { return GetValue<Color>(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content.
        /// </summary>
        /// <value>A <see cref="HorizontalAlignment"/> value that specifies the alignment of the control's content
        /// within its layout area. The default value is <see cref="HorizontalAlignment.Left"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HorizontalContentAlignmentProperty"/></dpropField>
        ///     <dpropStylingName>content-halign</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the control's content.
        /// </summary>
        /// <value>A <see cref="VerticalAlignment"/> value that specifies the alignment of the control's content
        /// within its layout area. The default value is <see cref="VerticalAlignment.Top"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="VerticalContentAlignmentProperty"/></dpropField>
        ///     <dpropStylingName>content-valign</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Identifies the IsTabStop dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsTabStop"/> dependency property.</value>
        public static readonly DependencyProperty IsTabStopProperty = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Font"/> dependency property.</value>
        public static readonly DependencyProperty FontProperty = TextElement.FontProperty.AddOwner(typeof(Control),
            new PropertyMetadata<SourcedResource<UltravioletFont>>(null, PropertyMetadataOptions.AffectsArrange, HandleFontChanged));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FontStyle"/> dependency property.</value>
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(Control),
            new PropertyMetadata<UltravioletFontStyle>(null, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Foreground"/> dependency property.</value>
        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Background"/> dependency property.</value>
        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(Control));
        
        /// <summary>
        /// Identifies the <see cref="HorizontalContentAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HorizontalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", "content-halign",
            typeof(HorizontalAlignment), typeof(Control), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Left, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", "content-valign",
            typeof(VerticalAlignment), typeof(Control), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Top, PropertyMetadataOptions.AffectsArrange));
        
        /// <summary>
        /// Gets the data source wrapper which exposes the compiled binding expressions for the control.
        /// </summary>
        internal Object DataSourceWrapper
        {
            get { return dataSourceWrapper; }
        }

        /// <summary>
        /// Gets the namescope for the control's component template.
        /// </summary>
        internal Namescope ComponentTemplateNamescope
        {
            get { return componentTemplateNamescope; }
        }

        /// <summary>
        /// Gets or sets the root of the control's component tree.
        /// </summary>
        internal UIElement ComponentRoot
        {
            get { return componentRoot; }
            set
            {
                if (componentRoot == value)
                    return;

                if (componentRoot != null)
                    componentRoot.ChangeLogicalAndVisualParents(null, null);

                componentRoot = value;

                if (componentRoot != null)
                    componentRoot.ChangeLogicalAndVisualParents(this, this);

                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected internal override void RemoveLogicalChild(UIElement child)
        {
            if (child == ComponentRoot)
            {
                ComponentRoot = null;
            }
            base.RemoveLogicalChild(child);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            if (ComponentRoot == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return ComponentRoot;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            if (ComponentRoot == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return ComponentRoot;
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get
            {
                return ComponentRoot == null ? 0 : 1;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get
            {
                return ComponentRoot == null ? 0 : 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control supports custom scrolling behavior.
        /// </summary>
        protected internal virtual Boolean HandlesScrolling
        {
            get { return false; }
        }

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadFont();

            base.ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected override void StyleOverride(UvssDocument styleSheet)
        {
            if (componentRoot != null)
                componentRoot.InvalidateStyle(true);

            base.StyleOverride(styleSheet);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (componentRoot == null)
            {
                var desiredWidth = Double.IsPositiveInfinity(availableSize.Width) ? 0 : availableSize.Width;
                var desiredHeight = Double.IsPositiveInfinity(availableSize.Height) ? 0 : availableSize.Height;
                return new Size2D(desiredWidth, desiredHeight);
            }
            componentRoot.Measure(availableSize);
            return componentRoot.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (componentRoot == null)
            {
                var finalWidth = Double.IsPositiveInfinity(finalSize.Width) ? 0 : finalSize.Width;
                var finalHeight = Double.IsPositiveInfinity(finalSize.Height) ? 0 : finalSize.Height;
                return new Size2D(finalWidth, finalHeight);
            }
            var finalRect = new RectangleD(Point2D.Zero, finalSize);
            componentRoot.Arrange(finalRect, options);
            return componentRoot.RenderSize;
        }

        /// <summary>
        /// Reloads the <see cref="Font"/> resource.
        /// </summary>
        protected void ReloadFont()
        {
            LoadResource(Font);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> dependency property changes.
        /// </summary>
        private static void HandleFontChanged(DependencyObject dobj, SourcedResource<UltravioletFont> oldValue, SourcedResource<UltravioletFont> newValue)
        {
            ((Control)dobj).ReloadFont();
        }

        /// <summary>
        /// Loads the control's component root from the control's associated template.
        /// </summary>
        private void LoadComponentRoot()
        {
            if (componentRoot != null)
                throw new InvalidOperationException(PresentationStrings.ComponentRootAlreadyLoaded);
            
            this.ComponentRoot = UvmlLoader.LoadComponentTemplate(this);
        }

        // The control's data source wrapper, which exposes its compiled binding expressions.
        private readonly Object dataSourceWrapper;

        // The control's component template.
        private readonly Namescope componentTemplateNamescope = new Namescope();
        private UIElement componentRoot;
    }
}
