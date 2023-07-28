using System;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation.Media.Effects
{
    /// <summary>
    /// Represents an effect that blurs target element.
    /// </summary>
    [UvmlKnownType]
    public sealed class BlurEffect : Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlurEffect"/> class.
        /// </summary>
        public BlurEffect()
        {
            var effectValue = effect.Value;
            if (effectValue != null)
            {
                effectValue.Mix = 0f;
            }
        }

        /// <inheritdoc/>
        public override Int32 AdditionalRenderTargetsRequired
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets or sets the radius of the blur which is applied to the element.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the radius of the blur. 
        /// The default value is 5.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="RadiusProperty"/></dpropField>
        ///		<dpropStylingName>radius</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Radius
        {
            get { return GetValue<Double>(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Radius"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Radius"/> dependency property.</value>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(Double), typeof(BlurEffect),
            new PropertyMetadata<Double>(5.0, PropertyMetadataOptions.None));        

        /// <inheritdoc/>
        protected internal override void DrawRenderTargets(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var blurTarget = target.Next.RenderTarget;

            var gfx = dc.Ultraviolet.GetGraphics();
            gfx.SetRenderTarget(blurTarget, Color.Transparent);

            effect.Value.Radius = GetRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Horizontal;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);
            dc.RawDraw(target.ColorBuffer, Vector2.Zero, Color.White);
            dc.End();
        }

        /// <inheritdoc/>
        protected internal override void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var state = dc.GetCurrentState();
            
            var position = (Vector2)element.View.Display.DipsToPixels(target.VisualBounds.Location);
            var positionRounded = new Vector2((Int32)position.X, (Int32)position.Y);

            dc.End();

            effect.Value.Radius = GetRadiusInPixels(element);
            effect.Value.Direction = BlurDirection.Vertical;

            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);

            var shadowTexture = target.Next.ColorBuffer;
            dc.RawDraw(shadowTexture, positionRounded, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            
            dc.End();
            dc.Begin(state);
        }

        /// <inheritdoc/>
        protected internal override void ModifyVisualBounds(ref RectangleD bounds)
        {
            var radius = Radius;
            RectangleD.Inflate(ref bounds, radius, radius, out bounds);
        }

        /// <summary>
        /// Gets the blur radius for the specified element, in pixels.
        /// </summary>
        private Single GetRadiusInPixels(UIElement element)
        {
            var display = element.View.Display;
            return (Single)display.DipsToPixels(Radius);
        }

        // The singleton instance of effect used to render the shadow.
        private static readonly UltravioletSingleton<Graphics.BlurEffect> effect = 
            new UltravioletSingleton<Graphics.BlurEffect>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy, uv =>
                Graphics.BlurEffect.Create());
    }
}
