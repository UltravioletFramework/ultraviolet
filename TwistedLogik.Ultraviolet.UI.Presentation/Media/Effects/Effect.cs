using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using GraphicsEffect = Ultraviolet.Graphics.Effect;

namespace Ultraviolet.Presentation.Media.Effects
{
    /// <summary>
    /// Represents a custom effect which can be applied to UI elements.
    /// </summary>
    public abstract class Effect : DependencyObject
    {
        /// <summary>
        /// Draws the specified render target at its desired visual bounds.
        /// </summary>
        /// <param name="dc">The current drawing context.</param>
        /// <param name="element">The element being drawn.</param>
        /// <param name="target">The render target that contains the element's graphics.</param>
        /// <param name="effect">The shader effect to apply to the render target, if any.</param>
        public static void DrawRenderTargetAtVisualBounds(DrawingContext dc, UIElement element, OutOfBandRenderTarget target, GraphicsEffect effect = null)
        {
            Contract.Require(dc, nameof(dc));
            Contract.Require(element, nameof(element));
            Contract.Require(target, nameof(target));

            if (element.View == null)
                return;

            var state = dc.GetCurrentState();

            dc.End();
            dc.Begin(SpriteSortMode.Immediate, effect, Matrix.Identity);

            var position = (Vector2)element.View.Display.DipsToPixels(target.VisualBounds.Location);
            var positionRounded = new Vector2((Int32)position.X, (Int32)position.Y);
            dc.RawDraw(target.ColorBuffer, positionRounded, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            dc.End();
            dc.Begin(state);
        }

        /// <summary>
        /// Gets the number of additional render targets which are required by this effect.
        /// </summary>
        /// <value>An <see cref="Int32"/> value that represents the number of additional render 
        /// targets (all effects get at least one) which should be allocated to this effect.</value>
        /// <remarks>This property is only examined when the effect is first applied to an element. 
        /// Changing it after that point will do nothing.</remarks>
        public virtual Int32 AdditionalRenderTargetsRequired
        {
            get { return 0; }
        }

        /// <summary>
        /// Draws the effect's additional render targets, if it has any.
        /// </summary>
        /// <param name="dc">The current drawing context.</param>
        /// <param name="element">The element being drawn.</param>
        /// <param name="target">The render target that contains the element's graphics.</param>
        protected internal virtual void DrawRenderTargets(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {

        }

        /// <summary>
        /// Draws the specified element using an out-of-band render target.
        /// </summary>
        /// <param name="dc">The current drawing context.</param>
        /// <param name="element">The element being drawn.</param>
        /// <param name="target">The render target that contains the element's graphics.</param>
        protected internal virtual void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {

        }

        /// <summary>
        /// Reloads the effect's resources.
        /// </summary>
        /// <param name="global">The global content manager.</param>
        /// <param name="local">The local content manager.</param>
        protected internal virtual void ReloadResources(ContentManager global, ContentManager local)
        {

        }

        /// <summary>
        /// Allows the <see cref="Effect"/> to modify the visual bounds of the element to which it is applied.
        /// </summary>
        /// <param name="bounds">The visual bounds of the effect's element.</param>
        protected internal virtual void ModifyVisualBounds(ref RectangleD bounds)
        {

        }
    }
}
