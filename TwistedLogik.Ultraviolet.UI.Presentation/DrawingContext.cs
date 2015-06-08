using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the context in which a layout is being drawn, and provides methods
    /// for pushing and popping various rendering states.
    /// </summary>
    public sealed partial class DrawingContext
    {
        /// <summary>
        /// Resets the drawing context by clearing its render state stacks.
        /// </summary>
        /// <param name="display">The display on which the drawing context is rendering.</param>
        public void Reset(IUltravioletDisplay display)
        {
            this.display = display;

            opacityStack.Clear();
            clipStack.Clear();
        }

        /// <summary>
        /// Pushes an opacity value onto the render state stack.
        /// </summary>
        /// <param name="opacity">The opacity value to push onto the stack.</param>
        public void PushOpacity(Single opacity)
        {
            if (opacity < 0)
                opacity = 0;

            if (opacity > 1)
                opacity = 1;

            var state = new OpacityState(opacity, opacity * Opacity);
            opacityStack.Push(state);
        }

        /// <summary>
        /// Pops an opacity value off of the render state stack.
        /// </summary>
        public void PopOpacity()
        {
            opacityStack.Pop();
        }

        /// <summary>
        /// Pushes a clipping rectangle onto the render state stack.
        /// </summary>
        /// <param name="clipRectangle">The clipping region to push onto the render state stack.</param>
        public void PushClipRectangle(RectangleD clipRectangle)
        {
            Contract.EnsureRange(clipRectangle.Width >= 0 && clipRectangle.Height >= 0, "clipRectangle");

            var cumulativeClipRectangle = clipRectangle;

            var currentClipRectangle = ClipRectangle;
            if (currentClipRectangle != null)
            {
                cumulativeClipRectangle = RectangleD.Intersect(currentClipRectangle.Value, clipRectangle);
            }

            var state = new ClipState(clipRectangle, cumulativeClipRectangle);
            clipStack.Push(state);

            FlushClipRectangle();
        }

        /// <summary>
        /// Pops a clipping rectangle off of the render state stack.
        /// </summary>
        public void PopClipRectangle()
        {
            clipStack.Pop();

            FlushClipRectangle();
        }

        /// <summary>
        /// Reapplies the drawing context's clipping rectangle to the graphics device.
        /// </summary>
        public void ReapplyClipRectangle()
        {
            FlushClipRectangle(false);
        }

        /// <summary>
        /// Gets the <see cref="SpriteBatch"/> with which the layout will be rendered.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            internal set { spriteBatch = value; }
        }

        /// <summary>
        /// Gets the current opacity.
        /// </summary>
        public Single Opacity
        {
            get { return opacityStack.Count > 0 ? opacityStack.Peek().CumulativeOpacity : 1f; }
        }

        /// <summary>
        /// Gets the current clip rectangle in device-independent pixels.
        /// </summary>
        public RectangleD? ClipRectangle
        {
            get { return clipStack.Count > 0 ? clipStack.Peek().CumulativeClipRectangle : (RectangleD?)null; }
        }

        /// <summary>
        /// Gets the amount by which the drawing context's clipping regions are translated along the x-axis.
        /// </summary>
        internal Single ClipTranslationX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the amount by which the drawing context's clipping regions are translated along the y-axis.
        /// </summary>
        internal Single ClipTranslationY
        {
            get;
            set;
        }

        /// <summary>
        /// Flushes the sprite batch and applies the current clip rectangle to the graphics device.
        /// </summary>
        /// <param name="flush">A value indicating whether to flush the sprite batch before applying the scissor rectangle.</param>
        private void FlushClipRectangle(Boolean flush = true)
        {
            if (spriteBatch == null)
                return;

            var uv       = SpriteBatch.Ultraviolet;
            var cliprect = (ClipRectangle == null || display == null) ? (Rectangle?)null : (Rectangle?)display.DipsToPixels(ClipRectangle.Value);

            if (cliprect.HasValue)
            {
                var cliprectValue = cliprect.Value;

                cliprect = new Rectangle(
                    cliprectValue.X + (Int32)ClipTranslationX,
                    cliprectValue.Y + (Int32)ClipTranslationY,
                    cliprectValue.Width,
                    cliprectValue.Height);
            }

            var current = SpriteBatch.Ultraviolet.GetGraphics().GetScissorRectangle();
            if (current == cliprect)
                return;

            if (flush)
            {
                SpriteBatch.Flush();
            }

            SpriteBatch.Ultraviolet.GetGraphics().SetScissorRectangle(cliprect);
        }

        // Property values.
        private SpriteBatch spriteBatch;

        // State values.
        private IUltravioletDisplay display;
        private readonly Stack<OpacityState> opacityStack = new Stack<OpacityState>(32);
        private readonly Stack<ClipState> clipStack = new Stack<ClipState>(32);
    }
}
