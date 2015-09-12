using System;
using System.Collections.Generic;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
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
        /// Explicitly converts an instance of <see cref="DrawingContext"/> to its underlying <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="dc">The <see cref="DrawingContext"/> instance to convert.</param>
        /// <returns>The drawing context's underlying <see cref="SpriteBatch"/> instance.</returns>
        public static explicit operator SpriteBatch(DrawingContext dc)
        {
            return dc.SpriteBatch;
        }

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
            this.transforms = 0;
            this.outOfBand = 0;
            this.localTransform = Matrix.Identity;
            this.globalTransform = Matrix.Identity;
            this.combinedTransform = Matrix.Identity;
            this.IsOutOfBandRenderingSuppressed = false;
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        public void Begin()
        {
            Begin(SpriteSortMode.Deferred, null, null, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        public void Begin(SpriteSortMode sortMode)
        {
            Begin(sortMode, null, null, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        /// <param name="sortMode">The sorting mode to use when rendering interface elements.</param>
        /// <param name="effect">The custom effect to apply to the rendered interface elements.</param>
        /// <param name="localTransform">The transform matrix to apply to the rendered interface elements.</param>
        public void Begin(SpriteSortMode sortMode, Effect effect, Matrix localTransform)
        {
            Begin(sortMode, null, null, effect, localTransform);
        }

        /// <summary>
        /// Begins a new sprite batch using the appropriate settings for rendering UPF.
        /// </summary>
        /// <param name="sortMode">The sorting mode to use when rendering interface elements.</param>
        /// <param name="blendState">The blend state to apply to the rendered elements.</param>
        /// <param name="samplerState">The sampler state to apply to the rendered interface elements.</param>
        /// <param name="effect">The custom effect to apply to the rendered interface elements.</param>
        /// <param name="localTransform">The transform matrix to apply to the rendered interface elements.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, Effect effect, Matrix localTransform)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            this.localTransform = localTransform;
            this.combinedTransform = Matrix.Identity;
            Matrix.Concat(ref localTransform, ref globalTransform, out combinedTransform);

            SpriteBatch.Begin(sortMode, 
                blendState ?? BlendState.AlphaBlend,
                samplerState ?? SamplerState.LinearClamp, 
                StencilReadDepthState, RasterizerState.CullCounterClockwise, effect, combinedTransform);
        }

        /// <summary>
        /// Begins a new sprite batch using the specified state values.
        /// </summary>
        /// <param name="state">A <see cref="DrawingContextState"/> specifying the state values to use for drawing.</param>
        public void Begin(DrawingContextState state)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            GlobalTransform = state.GlobalTransform;
            Begin(state.SortMode, state.BlendState, state.SamplerState, state.Effect, state.LocalTransform);
        }

        /// <summary>
        /// Ends the current sprite batch.
        /// </summary>
        public void End()
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.End();
        }

        /// <summary>
        /// Flushes the current sprite batch.
        /// </summary>
        public void Flush()
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Flush();
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, destinationRectangle, color);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="origin">The sprite's origin point.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, position, color);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, position, sourceRectangle, color);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="origin">The sprite's origin point.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="origin">The sprite's origin point.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawSprite(animation, position);
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="width">The width in pixels of the destination rectangle, or <c>null</c> to use the width of the sprite.</param>
        /// <param name="height">The height in pixels of the destination rectangle, or <c>null</c> to use the height of the sprite.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Single? width, Single? height)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawSprite(animation, position, width, height);
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="width">The width in pixels of the destination rectangle, or <c>null</c> to use the width of the sprite.</param>
        /// <param name="height">The height in pixels of the destination rectangle, or <c>null</c> to use the height of the sprite.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Single? width, Single? height, Color color, Single rotation)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawSprite(animation, position, width, height, color, rotation);
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="width">The width in pixels of the destination rectangle, or <c>null</c> to use the width of the sprite.</param>
        /// <param name="height">The height in pixels of the destination rectangle, or <c>null</c> to use the height of the sprite.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Single? width, Single? height, Color color, Single rotation, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawSprite(animation, position, width, height, color, rotation, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawScaledSprite(animation, position, scale);
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale, Color color, Single rotation)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawScaledSprite(animation, position, scale, color, rotation);
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="color">The sprite's color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale, Color color, Single rotation, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawScaledSprite(animation, position, scale, color, rotation, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a single animation frame.
        /// </summary>
        /// <param name="frame">The <see cref="SpriteFrame"/> to draw.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        public void DrawFrame(SpriteFrame frame, Rectangle destinationRectangle, Color color, Single rotation)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawFrame(frame, destinationRectangle, color, rotation);
        }

        /// <summary>
        /// Draws a single animation frame.
        /// </summary>
        /// <param name="frame">The <see cref="SpriteFrame"/> to draw.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        public void DrawFrame(SpriteFrame frame, Rectangle destinationRectangle, Color color, Single rotation, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawFrame(frame, destinationRectangle, color, rotation, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawString(fontFace, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        
        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="image">An <see cref="TextureImage"/> that represents the image to draw.</param>
        /// <param name="position">The position at which to draw the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="color">The image's color.</param>
        public void DrawImage(TextureImage image, Vector2 position, Single width, Single height, Color color)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawImage(image, position, width, height, color);
        }

        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="image">An <see cref="TextureImage"/> that represents the image to draw.</param>
        /// <param name="position">The position at which to draw the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="color">The image's color.</param>
        /// <param name="rotation">The image's rotation in radians.</param>
        /// <param name="origin">The image's point of origin.</param>
        /// <param name="effects">The image's rendering effects.</param>
        /// <param name="layerDepth">The image's layer depth.</param>
        public void DrawImage(TextureImage image, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

            SpriteBatch.DrawImage(image, position, width, height, color, rotation, origin, effects, layerDepth);
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
            if (SpriteBatch == null)
                return;

            var cliprect = (ClipRectangle == null || display == null) ? (Rectangle?)null : (Rectangle?)display.DipsToPixels(ClipRectangle.Value);
            if (cliprect == currentStencil)
                return;

            var state = GetCurrentState();
            End();

            currentStencil = cliprect;
            if (currentStencil.HasValue)
            {
                SpriteBatch.Ultraviolet.GetGraphics().Clear(ClearOptions.Stencil, Color.White, 0.0, 1);
                
                SpriteBatch.Begin(SpriteSortMode.Immediate, StencilBlendState, SamplerState.LinearClamp,
                    StencilWriteDepthState, RasterizerState.CullCounterClockwise, null, state.CombinedTransform);
                SpriteBatch.Draw(StencilTexture, currentStencil.Value, Color.White);
                SpriteBatch.End();
            }
            else
            {
                SpriteBatch.Ultraviolet.GetGraphics().Clear(ClearOptions.Stencil, Color.White, 0.0, 0);
            }

            Begin(state);
        }

        /// <summary>
        /// Gets the drawing context's current state values.
        /// </summary>
        /// <returns>A <see cref="DrawingContextState"/> that describes the drawing context's current state.</returns>
        public DrawingContextState GetCurrentState()
        {
            var state = SpriteBatch.GetCurrentState();
            return new DrawingContextState(state.SortMode, state.BlendState, state.SamplerState, state.Effect, ref localTransform, ref globalTransform, ref combinedTransform);
        }

        /// <summary>
        /// Gets the Ultraviolet context currently associated with the drawing context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get
            {
                if (SpriteBatch == null)
                    throw new InvalidOperationException(PresentationStrings.DrawingContextDoesNotHaveSpriteBatch);

                return SpriteBatch.Ultraviolet;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the drawing context is currently under the effect of a transform.
        /// </summary>
        public Boolean IsTransformed
        {
            get { return transforms > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the drawing context is inside of an element that was drawn out of band.
        /// </summary>
        public Boolean IsInsideOutOfBandElement
        {
            get { return outOfBand > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether out-of-band rendering is currently being suppressed.
        /// </summary>
        public Boolean IsOutOfBandRenderingSuppressed
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current opacity.
        /// </summary>
        public Single Opacity
        {
            get { return opacityStack.Count > 0 ? opacityStack.Peek().CumulativeOpacity : 1f; }
        }

        /// <summary>
        /// Gets the local transform which has been applied to this drawing context.
        /// </summary>
        public Matrix LocalTransform
        {
            get { return localTransform; }
        }

        /// <summary>
        /// Gets the global transform which has been applied to this drawing context.
        /// </summary>
        public Matrix GlobalTransform
        {
            get { return globalTransform; }
            internal set { globalTransform = value; }
        }

        /// <summary>
        /// Gets the combined transform which has been applied to this drawing context.
        /// </summary>
        public Matrix CombinedTransform
        {
            get { return combinedTransform; }
        }

        /// <summary>
        /// Gets the current clip rectangle in device-independent pixels.
        /// </summary>
        public RectangleD? ClipRectangle
        {
            get { return clipStack.Count > 0 ? clipStack.Peek().CumulativeClipRectangle : (RectangleD?)null; }
        }

        /// <summary>
        /// Pushes a transform onto the context.
        /// </summary>
        internal void PushTransform()
        {
            transforms++;
        }

        /// <summary>
        /// Pops a transform off of the context.
        /// </summary>
        internal void PopTransform()
        {
            if (transforms == 0)
                throw new InvalidOperationException();

            transforms--;
        }

        /// <summary>
        /// Pushes a flag onto the context indicating that it is inside of an element which was drawn out-of-band.
        /// </summary>
        internal void PushDrawingOutOfBand()
        {
            outOfBand++;
        }

        /// <summary>
        /// Pops a flag off of the context indicating that it is inside of an element which was drawn out-of-band.
        /// </summary>
        internal void PopDrawingOutOfBand()
        {
            if (outOfBand == 0)
                throw new InvalidOperationException();

            outOfBand--;
        }

        /// <summary>
        /// Gets the <see cref="SpriteBatch"/> with which the layout will be rendered.
        /// </summary>
        internal SpriteBatch SpriteBatch
        {
            private get;
            set;
        }

        // State values.
        private IUltravioletDisplay display;
        private readonly Stack<OpacityState> opacityStack = new Stack<OpacityState>(32);
        private readonly Stack<ClipState> clipStack = new Stack<ClipState>(32);
        private Rectangle? currentStencil;
        private Int32 transforms;
        private Int32 outOfBand;
        private Matrix localTransform = Matrix.Identity;
        private Matrix globalTransform = Matrix.Identity;
        private Matrix combinedTransform = Matrix.Identity;
    }
}
