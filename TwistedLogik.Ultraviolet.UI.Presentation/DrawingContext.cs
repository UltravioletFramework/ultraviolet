using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
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
            this.opacityStack.Clear();
            this.clipStack.Clear();
            this.currentStencil = null;
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        public void Begin()
        {
            Begin(SpriteSortMode.Deferred, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        public void Begin(SpriteSortMode sortMode)
        {
            Begin(sortMode, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        /// <param name="sortMode">The sorting mode to use when rendering interface elements.</param>
        /// <param name="effect">The custom effect to apply to the rendered interface elements.</param>
        /// <param name="transform">The transform matrix to apply to the rendered interface elements.</param>
        public void Begin(SpriteSortMode sortMode, Effect effect, Matrix transform)
        {
            if (spriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            spriteBatch.Begin(sortMode, BlendState.AlphaBlend, SamplerState.LinearClamp, StencilReadDepthState, RasterizerState.CullCounterClockwise, null, transform);
        }

        /// <summary>
        /// Ends the current sprite batch.
        /// </summary>
        public void End()
        {
            if (spriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Flushes the current sprite batch.
        /// </summary>
        public void Flush()
        {
            if (spriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            spriteBatch.Flush();
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

            ApplyClipRectangleToDevice();
        }

        /// <summary>
        /// Pops a clipping rectangle off of the render state stack.
        /// </summary>
        public void PopClipRectangle()
        {
            clipStack.Pop();

            ApplyClipRectangleToDevice();
        }

        /// <summary>
        /// Applies the drawing context's clipping rectangle to the graphics device.
        /// </summary>
        public void ApplyClipRectangleToDevice()
        {
            if (spriteBatch == null)
                return;

            var cliprect = (ClipRectangle == null || display == null) ? (Rectangle?)null : (Rectangle?)display.DipsToPixels(ClipRectangle.Value);
            if (cliprect == currentStencil)
                return;

            var state = SpriteBatch.GetCurrentState();
            SpriteBatch.End();

            currentStencil = cliprect;
            if (currentStencil.HasValue)
            {
                SpriteBatch.Ultraviolet.GetGraphics().Clear(ClearOptions.Stencil, Color.White, 0.0, 1);

                SpriteBatch.Begin(SpriteSortMode.Immediate, StencilBlendState, SamplerState.LinearClamp,
                    StencilWriteDepthState, RasterizerState.CullCounterClockwise, null, state.TransformMatrix);
                SpriteBatch.Draw(StencilTexture, currentStencil.Value, Color.White);
                SpriteBatch.End();
            }
            else
            {
                SpriteBatch.Ultraviolet.GetGraphics().Clear(ClearOptions.Stencil, Color.White, 0.0, 0);
            }

            Begin(SpriteSortMode.Deferred, null, state.TransformMatrix);
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

        // Property values.
        private SpriteBatch spriteBatch;

        // State values.
        private IUltravioletDisplay display;
        private readonly Stack<OpacityState> opacityStack = new Stack<OpacityState>(32);
        private readonly Stack<ClipState> clipStack = new Stack<ClipState>(32);
        private Rectangle? currentStencil;
    }
}
