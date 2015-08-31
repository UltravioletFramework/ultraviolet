using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media.Effects
{
    /// <summary>
    /// Represents an effect that draws a drop shadow on the target element.
    /// </summary>
    [UvmlKnownType]
    public sealed class DropShadowEffect : Effect
    {
        /// <summary>
        /// Gets or sets the color of the drop shadow.
        /// </summary>
        public Color Color
        {
            get { return GetValue<Color>(ColorProperty); }
            set { SetValue(ColorProperty, value); }
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
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(DropShadowEffect),
            new PropertyMetadata<Color>(Color.Black, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ShadowDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register("ShadowDepth", typeof(Double), typeof(DropShadowEffect),
            new PropertyMetadata<Double>(5.0, PropertyMetadataOptions.None));
        
        /// <inheritdoc/>
        protected internal override void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            var cumulativeTransform = target.CumulativeTransform;

            var shadowVectorStart = new Vector2(0, 0);
            Vector2.Transform(ref shadowVectorStart, ref cumulativeTransform, out shadowVectorStart);

            var shadowVectorEnd = new Vector2(1, 1);
            Vector2.Transform(ref shadowVectorEnd, ref cumulativeTransform, out shadowVectorEnd);

            var display = element.View.Display;
            var shadowDepth = (Int32)display.DipsToPixels(ShadowDepth);
            var shadowVector = Vector2.Normalize(shadowVectorEnd - shadowVectorStart) * shadowDepth;

            var state = dc.SpriteBatch.GetCurrentState();
            
            var position = (Vector2)element.View.Display.DipsToPixels(target.VisualBounds.Location);
            var positionRounded = new Vector2((Int32)position.X, (Int32)position.Y);

            dc.End();
            dc.Begin(SpriteSortMode.Immediate, null, Matrix.Identity);

            dc.SpriteBatch.Draw(target.ColorBuffer, positionRounded + shadowVector, null, Color.Black, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            
            dc.End();
            dc.Begin(SpriteSortMode.Immediate, null, Matrix.Identity);

            dc.SpriteBatch.Draw(target.ColorBuffer, positionRounded, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            dc.End();
            dc.Begin(state.SortMode, state.Effect, state.TransformMatrix);
        }
    }
}
