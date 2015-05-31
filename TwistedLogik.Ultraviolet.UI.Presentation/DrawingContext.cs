using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the context in which a layout is being drawn, and provides methods
    /// for pushing and popping various rendering states.
    /// </summary>
    public sealed partial class DrawingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingContext"/> class.
        /// </summary>
        internal DrawingContext(PresentationFoundationView view)
        {
            Contract.Require(view, "view");

            this.view = view;
        }

        /// <summary>
        /// Resets the drawing context by clearing its render state stacks.
        /// </summary>
        public void Reset()
        {
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
        /// Flushes the sprite batch and applies the current clip rectangle to the graphics device.
        /// </summary>
        private void FlushClipRectangle()
        {
            if (spriteBatch == null)
                return;

            var uv       = SpriteBatch.Ultraviolet;
            var cliprect = (ClipRectangle == null) ? (Rectangle?)null : (Rectangle?)view.Display.DipsToPixels(ClipRectangle.Value);

            var current = SpriteBatch.Ultraviolet.GetGraphics().GetScissorRectangle();
            if (current == cliprect)
                return;

            SpriteBatch.Flush();
            SpriteBatch.Ultraviolet.GetGraphics().SetScissorRectangle(cliprect);
        }

        // Property values.
        private SpriteBatch spriteBatch;

        // State values.
        private readonly PresentationFoundationView view;
        private readonly Stack<OpacityState> opacityStack = new Stack<OpacityState>(32);
        private readonly Stack<ClipState> clipStack = new Stack<ClipState>(32);
    }
}
