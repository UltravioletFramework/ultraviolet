using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
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
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Control(UltravioletContext uv, String name)
            : base(uv, name)
        {
            LoadComponentRoot();
        }

        /// <summary>
        /// Gets or sets the font used to draw the control's text.
        /// </summary>
        public SourcedResource<SpriteFont> Font
        {
            get { return GetValue<SourcedResource<SpriteFont>>(FontProperty); }
            set { SetValue<SourcedResource<SpriteFont>>(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font style which is used to draw the control's text.
        /// </summary>
        public SpriteFontStyle FontStyle
        {
            get { return GetValue<SpriteFontStyle>(FontStyleProperty); }
            set { SetValue<SpriteFontStyle>(FontStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the control's foreground color.
        /// </summary>
        public Color Foreground
        {
            get { return GetValue<Color>(ForegroundProperty); }
            set { SetValue<Color>(ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the control's background color.
        /// </summary>
        public Color Background
        {
            get { return GetValue<Color>(BackgroundProperty); }
            set { SetValue<Color>(BackgroundProperty, value); }
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
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'font'.</remarks>
        public static readonly DependencyProperty FontProperty = TextElement.FontProperty.AddOwner(typeof(Control),
            new PropertyMetadata<SourcedResource<SpriteFont>>(null, PropertyMetadataOptions.AffectsArrange, HandleFontChanged));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'font-style'.</remarks>
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(Control),
            new PropertyMetadata<SpriteFontStyle>(null, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'foreground'.</remarks>
        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'background'.</remarks>
        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Identifies the <see cref="HorizontalContentAlignment"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-halign'.</remarks>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", "content-halign",
            typeof(HorizontalAlignment), typeof(ContentControl), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Left, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-valign'.</remarks>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", "content-valign",
            typeof(VerticalAlignment), typeof(ContentControl), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Top, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Populates any fields of this object which represent references
        /// to components in the current component tree.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            componentTemplateNamescope.PopulateFieldsFromRegisteredElements(this);
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

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            ReloadFont();

            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override void StyleCore(UvssDocument styleSheet)
        {
            if (componentRoot != null)
                componentRoot.InvalidateStyle(true);

            base.StyleCore(styleSheet);
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
        private static void HandleFontChanged(DependencyObject dobj, SourcedResource<SpriteFont> oldValue, SourcedResource<SpriteFont> newValue)
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

            var template = Ultraviolet.GetUI().GetPresentationFoundation().ComponentTemplates.Get(this);
            if (template == null)
                return;

            UvmlLoader.LoadComponentTemplate(this, template);
        }

        // Property values.
        private UIElement componentRoot;
        private readonly Namescope componentTemplateNamescope = new Namescope();
    }
}
