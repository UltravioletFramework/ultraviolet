using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which displays a particular sprite animation.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Source")]
    public class Sprite : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Sprite(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>A <see cref="SourcedResource{T}"/> which represents the sprite that is drawn
        /// by this control. The default value is an invalid sprite.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceProperty"/></dpropField>
        ///     <dpropStylingName>source-sprite</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedResource<Graphics.Graphics2D.Sprite> Source
        {
            get { return GetValue<SourcedResource<Graphics.Graphics2D.Sprite>>(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the sprite is drawn.
        /// </summary>
        /// <value>A <see cref="Color"/> value that specifies the color with which
        /// the control draws its sprite. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceColorProperty"/></dpropField>
        ///     <dpropStylingName>source-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color SourceColor
        {
            get { return GetValue<Color>(SourceColorProperty); }
            set { SetValue(SourceColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the animation which is drawn.
        /// </summary>
        /// <value>A <see cref="SpriteAnimationName"/> value that specifies which of the loaded
        /// sprite's animations will be drawn by the control.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceAnimationProperty"/></dpropField>
        ///     <dpropStylingName>source-animation</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SpriteAnimationName SourceAnimation
        {
            get { return GetValue<SpriteAnimationName>(SourceAnimationProperty); }
            set { SetValue(SourceAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the sprite's set of rendering effects.
        /// </summary>
        /// <value>A <see cref="SpriteEffects"/> value that specifies the sprite's rendering effects.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceEffectsProperty"/></dpropField>
        ///     <dpropStylingName>source-effects</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SpriteEffects SourceEffects
        {
            get { return GetValue<SpriteEffects>(SourceEffectsProperty); }
            set { SetValue(SourceEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the animation which is drawn.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value that specifies whether to use the display animation's
        /// global controller (if <see langword="true"/>) or a controller which is local to the control (if <see langword="false"/>).</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="UseGlobalAnimationControllerProperty"/></dpropField>
        ///     <dpropStylingName>use-global-animation-controller</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean UseGlobalAnimationController
        {
            get { return GetValue<Boolean>(UseGlobalAnimationControllerProperty); }
            set { SetValue(UseGlobalAnimationControllerProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control performs clipping.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value that specifies whether the control performs clipping.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ClippingEnabledProperty"/></dpropField>
        ///     <dpropStylingName>clipping-enabled</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean ClippingEnabled
        {
            get { return GetValue<Boolean>(ClippingEnabledProperty); }
            set { SetValue(ClippingEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the rendered sprite within the control.
        /// </summary>
        /// <value>A <see cref="HorizontalAlignment"/> value which specifies the horizontal alignment of
        /// the control's rendered sprite. The default value is <see cref="HorizontalAlignment.Left"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="HorizontalContentAlignmentProperty"/></dpropField>
        ///		<dpropStylingName>content-halign</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.None"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the rendered sprite within the control.
        /// </summary>
        /// <value>A <see cref="VerticalAlignment"/> value which specifies the vertical alignment of
        /// the control's rendered sprite. The default value is <see cref="VerticalAlignment.Top"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="VerticalContentAlignmentProperty"/></dpropField>
        ///		<dpropStylingName>content-valign</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.None"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="Source"/> dependency property.</value>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", "source-sprite",
            typeof(SourcedResource<Graphics.Graphics2D.Sprite>), typeof(Sprite), new PropertyMetadata<SourcedResource<Graphics.Graphics2D.Sprite>>(HandleSourceChanged));

        /// <summary>
        /// Identifies the <see cref="SourceColor"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="SourceColor"/> dependency property.</value>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Color), typeof(Sprite),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the <see cref="SourceAnimation"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="SourceAnimation"/> dependency property.</value>
        public static readonly DependencyProperty SourceAnimationProperty = DependencyProperty.Register("SourceAnimation", typeof(SpriteAnimationName), typeof(Sprite),
            new PropertyMetadata<SpriteAnimationName>(HandleSourceAnimationChanged));

        /// <summary>
        /// Identifies the <see cref="SourceEffects"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="SourceEffects"/> dependency property.</value>
        public static readonly DependencyProperty SourceEffectsProperty = DependencyProperty.Register("SourceEffects", typeof(SpriteEffects), typeof(Sprite),
            new PropertyMetadata<SpriteEffects>());

        /// <summary>
        /// Identifies the <see cref="UseGlobalAnimationController"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="UseGlobalAnimationController"/> dependency property.</value>
        public static readonly DependencyProperty UseGlobalAnimationControllerProperty = DependencyProperty.Register("UseGlobalAnimationController", typeof(Boolean), typeof(Sprite),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, HandleUseGlobalAnimationControllerChanged));

        /// <summary>
        /// Identifies the <see cref="ClippingEnabled"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="ClippingEnabled"/> dependency property.</value>
        public static readonly DependencyProperty ClippingEnabledProperty = DependencyProperty.Register("ClippingEnabled", typeof(Boolean), typeof(Sprite),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleClippingEnabledChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalContentAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HorizontalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", "content-halign",
            typeof(HorizontalAlignment), typeof(Sprite), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Center));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", "content-valign",
            typeof(VerticalAlignment), typeof(Sprite), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Center));

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadSource();
            ResetAnimationController();

            base.ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (localAnimationController != null)
                localAnimationController.Update(time);

            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var position = Point2D.Zero;
            var width = 0.0;
            var height = 0.0;

            if (localAnimationController != null)
            {
                CalculateSpritePosition(localAnimationController, out position, out width, out height);
                DrawSprite(dc, localAnimationController, position, width, height, SourceColor, 0f, SourceEffects, 0f);
            }
            else
            {
                var sprite = Source;
                var spriteAnimation = SourceAnimation;
                if (sprite.IsLoaded && sprite.Resource.Value != null && sprite.Resource.Value.IsValidAnimationName(spriteAnimation))
                {
                    var globalAnimationController = sprite.Resource.Value[spriteAnimation].Controller;
                    CalculateSpritePosition(globalAnimationController, out position, out width, out height);
                    DrawSprite(dc, globalAnimationController, position, width, height, SourceColor, 0f, SourceEffects, 0f);
                }
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipOverride()
        {
            if (ClippingEnabled)
                return UntransformedAbsoluteBounds;

            return base.ClipOverride();
        }

        /// <summary>
        /// Reloads the element's source image.
        /// </summary>
        protected void ReloadSource()
        {
            LoadResource(Source);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Source"/> dependency property changes.
        /// </summary>
        private static void HandleSourceChanged(DependencyObject dobj, SourcedResource<Graphics.Graphics2D.Sprite> oldValue, SourcedResource<Graphics.Graphics2D.Sprite> newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.ReloadSource();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SourceAnimation"/> dependency property changes.
        /// </summary>
        private static void HandleSourceAnimationChanged(DependencyObject dobj, SpriteAnimationName oldValue, SpriteAnimationName newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.ResetAnimationController();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UseGlobalAnimationController"/> dependency property changes.
        /// </summary>
        private static void HandleUseGlobalAnimationControllerChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.localAnimationController = newValue ? null : new SpriteAnimationController();
            sprite.ResetAnimationController();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ClippingEnabled"/> dependency property changes.
        /// </summary>
        private static void HandleClippingEnabledChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.Clip();
        }

        /// <summary>
        /// Resets the state of the control's local animation controller, if it has one.
        /// </summary>
        private void ResetAnimationController()
        {
            if (localAnimationController == null)
                return;

            var sprite = Source;
            var spriteAnimationName = SourceAnimation;
            if (!sprite.IsLoaded || sprite.Resource.Value == null || !sprite.Resource.Value.IsValidAnimationName(spriteAnimationName))
            {
                localAnimationController.StopAnimation();
            }
            else
            {
                var animation = sprite.Resource.Value[spriteAnimationName];
                localAnimationController.PlayAnimation(animation);
            }
        }

        /// <summary>
        /// Calculates the position and size of the control's sprite based on the control's current property values.
        /// </summary>
        private void CalculateSpritePosition(SpriteAnimationController animation, out Point2D position, out Double width, out Double height)
        {
            var x = 0.0;
            var y = 0.0;

            var frame = animation.GetFrame();
            width = Display.PixelsToDips(frame.Width);
            height = Display.PixelsToDips(frame.Height);

            switch (HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:
                    x = 0.0;
                    break;

                case HorizontalAlignment.Right:
                    x = RenderSize.Width;
                    break;

                case HorizontalAlignment.Center:
                    x = RenderSize.Width / 2.0;
                    break;

                case HorizontalAlignment.Stretch:
                    x = RenderSize.Width / 2.0;
                    width = RenderSize.Width;
                    break;
            }

            switch (VerticalContentAlignment)
            {
                case VerticalAlignment.Top:
                    y = 0.0;
                    break;

                case VerticalAlignment.Bottom:
                    y = RenderSize.Height;
                    break;

                case VerticalAlignment.Center:
                    y = RenderSize.Height / 2.0;
                    break;

                case VerticalAlignment.Stretch:
                    y = RenderSize.Width / 2.0;
                    height = RenderSize.Height;
                    break;
            }

            position = new Point2D(x, y);
        }

        // State values.
        private SpriteAnimationController localAnimationController;
    }
}
