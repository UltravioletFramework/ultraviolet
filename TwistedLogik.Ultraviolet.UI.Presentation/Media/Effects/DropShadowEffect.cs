using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media.Effects
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
            effect.Value.Mix = 1f;
        }

        /// <inheritdoc/>
        public override Int32 AdditionalRenderTargetsRequired
        {
            get { return 1; }
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
            var shadowTarget = target.Next.RenderTarget;

            var gfx = dc.SpriteBatch.Ultraviolet.GetGraphics();
            gfx.SetRenderTarget(shadowTarget);
            gfx.Clear(Color.Transparent);

            effect.Value.Radius = GetBlurRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Vertical;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);

            effect.Value.Direction = BlurDirection.Horizontal;

            dc.SpriteBatch.Draw(target.ColorBuffer, Vector2.Zero, Color);
            dc.End();
        }

        /// <inheritdoc/>
        protected internal override void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var cumulativeTransform = target.CumulativeTransform;
            
            var shadowVectorStart = new Vector2(0, 0);
            Vector2.Transform(ref shadowVectorStart, ref cumulativeTransform, out shadowVectorStart);

            var shadowVectorEnd = Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(Radians.FromDegrees(-Direction)));
            Vector2.Transform(ref shadowVectorEnd, ref cumulativeTransform, out shadowVectorEnd);

            var display = element.View.Display;
            var shadowDepth = (Int32)display.DipsToPixels(ShadowDepth);
            var shadowVector = Vector2.Normalize(shadowVectorEnd - shadowVectorStart) * shadowDepth;

            var state = dc.SpriteBatch.GetCurrentState();

            var position = (Vector2)element.View.Display.DipsToPixels(target.RelativeVisualBounds.Location);
            var positionRounded = new Vector2((Int32)position.X, (Int32)position.Y);

            dc.End();

            effect.Value.Radius = GetBlurRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Vertical;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);

            var shadowTexture = target.Next.ColorBuffer;
            dc.SpriteBatch.Draw(shadowTexture, positionRounded + shadowVector, null, Color * Opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            
            dc.End();

            dc.Begin(SpriteSortMode.Immediate, null, Matrix.Identity);
            dc.SpriteBatch.Draw(target.ColorBuffer, positionRounded, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            dc.End();

            dc.Begin(state.SortMode, state.Effect, state.TransformMatrix);
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
        private static readonly UltravioletSingleton<Graphics.BlurEffect> effect = new UltravioletSingleton<Graphics.BlurEffect>((uv) =>
        {
            return Graphics.BlurEffect.Create();
        });
    }
}
