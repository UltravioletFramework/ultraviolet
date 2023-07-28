using System;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation.Media.Effects
{
    /// <summary>
    /// Represents an effect that draws a drop shadow on the target element.
    /// </summary>
    [UvmlKnownType]
    public sealed class DropShadowEffect : Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropShadowEffect"/> class.
        /// </summary>
        public DropShadowEffect()
        {
            var effectValue = effect.Value;
            if (effectValue != null)
            {
                effectValue.Mix = 1f;
            }
        }

        /// <inheritdoc/>
        public override Int32 AdditionalRenderTargetsRequired
        {
            get { return 2; }
        }

        /// <summary>
        /// Gets or sets the angle in degrees at which the drop shadow is offset from its element.
        /// </summary>
        public Single Direction
        {
            get { return GetValue<Single>(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow's opacity.
        /// </summary>
        public Single Opacity
        {
            get { return GetValue<Single>(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the radius of the blur which is applied to the drop shadow.
        /// </summary>
        public Double BlurRadius
        {
            get { return GetValue<Double>(BlurRadiusProperty); }
            set { SetValue(BlurRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance between the element and its drop shadow.
        /// </summary>
        public Double ShadowDepth
        {
            get { return GetValue<Double>(ShadowDepthProperty); }
            set { SetValue(ShadowDepthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the drop shadow.
        /// </summary>
        public Color Color
        {
            get { return GetValue<Color>(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Direction"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Single), typeof(DropShadowEffect),
            new PropertyMetadata<Single>(315f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(Single), typeof(DropShadowEffect),
            new PropertyMetadata<Single>(1f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="BlurRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register("BlurRadius", typeof(Double), typeof(DropShadowEffect),
            new PropertyMetadata<Double>(5.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ShadowDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register("ShadowDepth", typeof(Double), typeof(DropShadowEffect),
            new PropertyMetadata<Double>(5.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(DropShadowEffect),
            new PropertyMetadata<Color>(Color.Black, PropertyMetadataOptions.None));

        /// <inheritdoc/>
        protected internal override void DrawRenderTargets(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var gfx = dc.Ultraviolet.GetGraphics();
            
            // Calculate the shadow's offset from the element
            var cumulativeTransform = target.VisualTransform;

            var shadowVectorStart = new Vector2(0, 0);
            Vector2.Transform(ref shadowVectorStart, ref cumulativeTransform, out shadowVectorStart);

            var shadowVectorEnd = Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(Radians.FromDegrees(-Direction)));
            Vector2.Transform(ref shadowVectorEnd, ref cumulativeTransform, out shadowVectorEnd);

            var shadowDepth = (Int32)element.View.Display.DipsToPixels(ShadowDepth);
            var shadowVector = Vector2.Normalize(shadowVectorEnd - shadowVectorStart) * shadowDepth;

            // Draw the horizontal blur pass
            var pass1Target = target.Next;
            gfx.SetRenderTarget(pass1Target.RenderTarget, Color.Transparent);

            effect.Value.Radius = GetBlurRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Horizontal;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);
            dc.RawDraw(target.ColorBuffer, Vector2.Zero, Color);
            dc.End();

            // Draw the vertical blur pass
            var pass2Target = target.Next.Next;
            gfx.SetRenderTarget(pass2Target.RenderTarget, Color.Transparent);

            effect.Value.Radius = GetBlurRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Vertical;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);
            dc.RawDraw(pass1Target.ColorBuffer, shadowVector, null, Color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            dc.End();

            // Draw the element on top of the shadow
            dc.Begin(SpriteSortMode.Immediate, null, Matrix.Identity);
            dc.RawDraw(target.ColorBuffer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            dc.End();
        }

        /// <inheritdoc/>
        protected internal override void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var state = dc.GetCurrentState();

            var position = (Vector2)element.View.Display.DipsToPixels(target.VisualBounds.Location);
            var positionRounded = new Vector2((Int32)position.X, (Int32)position.Y);

            dc.End();

            dc.Begin(SpriteSortMode.Immediate, null, Matrix.Identity);
            dc.RawDraw(target.Next.Next.ColorBuffer, positionRounded, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);            
            dc.End();

            dc.Begin(state);
        }

        /// <inheritdoc/>
        protected internal override void ModifyVisualBounds(ref RectangleD bounds)
        {
            var radius = ShadowDepth + BlurRadius;
            RectangleD.Inflate(ref bounds, radius, radius, out bounds);
        }

        /// <summary>
        /// Gets the shadow blur radius for the specified element, in pixels.
        /// </summary>
        private Single GetBlurRadiusInPixels(UIElement element)
        {
            var display = element.View.Display;
            return (Single)display.DipsToPixels(BlurRadius);
        }

        // The singleton instance of effect used to render the shadow.
        private static readonly UltravioletSingleton<Graphics.BlurEffect> effect =
            new UltravioletSingleton<Graphics.BlurEffect>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
                Graphics.BlurEffect.Create());
    }
}
