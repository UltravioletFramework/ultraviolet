using System;
using System.Security;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains methods for drawing batches of 2D sprites.
    /// </summary>
    /// <typeparam name="VertexType">The type of vertex used to render the batch's sprites.</typeparam>
    /// <typeparam name="SpriteData">The type of data object associated with each of the batch's sprite instances.</typeparam>
    [SecuritySafeCritical]
    public abstract unsafe partial class SpriteBatchBase<VertexType, SpriteData> : UltravioletResource
        where VertexType : struct, IVertexType
        where SpriteData : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchBase{VertexType, SpriteData}"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="batchSize">The maximum number of sprites that can be drawn in a single batch.</param>
        protected SpriteBatchBase(UltravioletContext uv, Int32 batchSize = 2048)
            : base(uv)
        {
            Contract.EnsureRange(batchSize > 0, "batchSize");

            this.batchSize = batchSize;
            this.batchInfo = new SpriteBatchInfo<SpriteData>(batchSize);

            this.vertices = new VertexType[batchSize * 4];
            this.vertexBufferPosition = 0;

            this.spriteEffect = (Effect)SpriteBatchEffect.Create();

            uv.QueueWorkItemAndWait(CreateVertexAndIndexBuffers);
        }

        /// <summary>
        /// Flushes the current batch.
        /// </summary>
        public void Flush()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeEnd);

            var sortMode          = this.sortMode;
            var blendState        = this.blendState;
            var samplerState      = this.samplerState;
            var depthStencilState = this.depthStencilState;
            var rasterizerState   = this.rasterizerState;
            var effect            = this.customEffect;
            var transformMatrix   = this.transformMatrix;

            End();
            Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        /// <summary>
        /// Begins a sprite batch operation using deferred sort and default state objects.
        /// </summary>
        public void Begin()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(begun, UltravioletStrings.BeginCannotBeCalledAgain);

            BeginInternal(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch operation using the specified sort and blend state objects.
        /// </summary>
        /// <param name="sortMode">The batch's sprite drawing order.</param>
        /// <param name="blendState">The batch's blend state.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(begun, UltravioletStrings.BeginCannotBeCalledAgain);

            BeginInternal(sortMode, blendState, null, null, null, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch operation using the specified sort and blend state objects.
        /// </summary>
        /// <param name="sortMode">The batch's sprite drawing order.</param>
        /// <param name="blendState">The batch's blend state.</param>
        /// <param name="samplerState">The batch's sampler state.</param>
        /// <param name="depthStencilState">The batch's depth/stencil state.</param>
        /// <param name="rasterizerState">The batch's rasterizer state.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(begun, UltravioletStrings.BeginCannotBeCalledAgain);

            BeginInternal(sortMode, blendState, samplerState, depthStencilState, rasterizerState, null, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch operation using the specified sort and blend state objects, as well as a custom effect.
        /// </summary>
        /// <param name="sortMode">The batch's sprite drawing order.</param>
        /// <param name="blendState">The batch's blend state.</param>
        /// <param name="samplerState">The batch's sampler state.</param>
        /// <param name="depthStencilState">The batch's depth/stencil state.</param>
        /// <param name="rasterizerState">The batch's rasterizer state.</param>
        /// <param name="effect">The batch's custom effect.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(begun, UltravioletStrings.BeginCannotBeCalledAgain);

            BeginInternal(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch operation using the specified sort and blend state objects, as well as a custom effect and transformation matrix.
        /// </summary>
        /// <param name="sortMode">The batch's sprite drawing order.</param>
        /// <param name="blendState">The batch's blend state.</param>
        /// <param name="samplerState">The batch's sampler state.</param>
        /// <param name="depthStencilState">The batch's depth/stencil state.</param>
        /// <param name="rasterizerState">The batch's rasterizer state.</param>
        /// <param name="effect">The batch's custom effect.</param>
        /// <param name="transformMatrix">The batch's transformation matrix.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(begun, UltravioletStrings.BeginCannotBeCalledAgain);

            BeginInternal(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        /// <summary>
        /// Finishes a sprite batch operation.
        /// </summary>
        public void End()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeEnd);

            EndInternal();
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color)
        {
            Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, default(SpriteData));
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
            Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, default(SpriteData));
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
            Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="color">The sprite's tint color.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            Draw(texture, position, null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, default(SpriteData));
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
            Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, default(SpriteData));
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
            Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, default(SpriteData));
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
            Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, data);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, data);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, position, null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, data);
        }

        /// <summary>
        /// Adds a sprite to the batch.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="sourceRectangle">The sprite's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, position, sourceRectangle, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(texture, "texture");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawInternal(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, data);
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position)
        {
            DrawSprite(animation, position, null, null, Color.White, 0f, SpriteEffects.None, 0f, default(SpriteData));
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="width">The width in pixels of the destination rectangle, or <c>null</c> to use the width of the sprite.</param>
        /// <param name="height">The height in pixels of the destination rectangle, or <c>null</c> to use the height of the sprite.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height)
        {
            DrawSprite(animation, position, width, height, Color.White, 0f, SpriteEffects.None, 0f, default(SpriteData));
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
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height, Color color, Single rotation)
        {
            DrawSprite(animation, position, width, height, color, rotation, SpriteEffects.None, 0f, default(SpriteData));
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
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height, Color color, Single rotation, SpriteEffects effects, Single layerDepth)
        {
            DrawSprite(animation, position, width, height, color, rotation, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, SpriteData data)
        {
            DrawSprite(animation, position, null, null, Color.White, 0f, SpriteEffects.None, 0f, data);
        }

        /// <summary>
        /// Draws a sprite animation.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="width">The width in pixels of the destination rectangle, or <c>null</c> to use the width of the sprite.</param>
        /// <param name="height">The height in pixels of the destination rectangle, or <c>null</c> to use the height of the sprite.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height, SpriteData data)
        {
            DrawSprite(animation, position, width, height, Color.White, 0f, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height, Color color, Single rotation, SpriteData data)
        {
            DrawSprite(animation, position, width, height, color, rotation, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void DrawSprite(SpriteAnimationController animation, Vector2 position, Int32? width, Int32? height, Color color, Single rotation, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(animation, "animation");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            // Retrieve the current frame.
            var frame = animation.GetFrame();
            if (frame == null || frame.TextureResource == null)
                return;

            // Draw the sprite.
            var scaleX = (width ?? frame.Width) / (float)frame.Width;
            var scaleY = (height ?? frame.Height) / (float)frame.Height;
            var scale = new Vector2(scaleX, scaleY);
            var source = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            var origin = new Vector2(frame.OriginX, frame.OriginY);
            DrawInternal(frame.TextureResource, position, source, color, rotation, origin, scale, effects, layerDepth, data);
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale)
        {
            DrawScaledSprite(animation, position, scale, Color.White, 0f, SpriteEffects.None, 0f, default(SpriteData));
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
            DrawScaledSprite(animation, position, scale, color, rotation, SpriteEffects.None, 0f, default(SpriteData));
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
            DrawScaledSprite(animation, position, scale, color, rotation, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale, SpriteData data)
        {
            DrawScaledSprite(animation, position, scale, Color.White, 0f, SpriteEffects.None, 0f, data);
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale, Color color, Single rotation, SpriteData data)
        {
            DrawScaledSprite(animation, position, scale, color, rotation, SpriteEffects.None, 0f, data);
        }

        /// <summary>
        /// Draws a sprite animation with the specified scaling factor.
        /// </summary>
        /// <param name="animation">A <see cref="SpriteAnimationController"/> representing the sprite animation to draw.</param>
        /// <param name="position">The sprite's position in screen coordinates.</param>
        /// <param name="scale">The sprite's scale factor.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="effects">The sprite's rendering effects.</param>
        /// <param name="layerDepth">The sprite's layer depth.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawScaledSprite(SpriteAnimationController animation, Vector2 position, Vector2 scale, Color color, Single rotation, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(animation, "animation");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            var frame = animation.GetFrame();
            if (frame == null || frame.TextureResource == null)
                return;

            var sourceRect = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            var origin = new Vector2(frame.OriginX, frame.OriginY);
            DrawInternal(frame.TextureResource, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, 0f, data);
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
            DrawFrame(frame, destinationRectangle, color, rotation, SpriteEffects.None, 0f, default(SpriteData));
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
            DrawFrame(frame, destinationRectangle, color, rotation, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a single animation frame.
        /// </summary>
        /// <param name="frame">The <see cref="SpriteFrame"/> to draw.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the sprite will be drawn.</param>
        /// <param name="color">The sprite's tint color.</param>
        /// <param name="rotation">The sprite's rotation in radians.</param>
        /// <param name="data">The sprite's custom data.</param>
        public void DrawFrame(SpriteFrame frame, Rectangle destinationRectangle, Color color, Single rotation, SpriteData data)
        {
            DrawFrame(frame, destinationRectangle, color, rotation, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The sprite's custom data.</param>
        public void DrawFrame(SpriteFrame frame, Rectangle destinationRectangle, Color color, Single rotation, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Draw(frame.TextureResource, destinationRectangle, frame.Area, color, rotation, frame.Origin, SpriteEffects.None, 0f, data);
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, String text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, data);
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, data);
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, default(SpriteData));
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
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The <see cref="SpriteFontFace"/> with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, data);
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
        /// <param name="data">The text's custom data.</param>
        public void DrawString(SpriteFontFace fontFace, StringSegment text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            DrawStringInternal(fontFace, new StringSource(text), position, color, rotation, origin, scale, effects, layerDepth, data);
        }
        
        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="image">An <see cref="TextureImage"/> that represents the image to draw.</param>
        /// <param name="position">The position at which to draw the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="color">The image's color.</param>
        public void DrawImage(TextureImage image, Vector2 position, Int32 width, Int32 height, Color color)
        {
            DrawImage(image, position, width, height, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, default(SpriteData));
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
        public void DrawImage(TextureImage image, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            DrawImage(image, position, width, height, color, rotation, origin, effects, layerDepth, default(SpriteData));
        }

        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="image">An <see cref="TextureImage"/> that represents the image to draw.</param>
        /// <param name="position">The position at which to draw the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="color">The image's color.</param>
        /// <param name="data">The image's custom data.</param>
        public void DrawImage(TextureImage image, Vector2 position, Int32 width, Int32 height, Color color, SpriteData data)
        {
            DrawImage(image, position, width, height, color, 0f, Vector2.Zero, SpriteEffects.None, 0f, data);
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
        /// <param name="data">The image's custom data.</param>
        public void DrawImage(TextureImage image, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(image, "image");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            if (!image.IsLoaded)
                throw new ArgumentException(UltravioletStrings.StretchableImageNotLoaded.Format("image"));

            image.Draw<VertexType, SpriteData>(this, position, width, height, color, rotation, origin, effects, layerDepth, data);
        }

        /// <summary>
        /// Gets the maximum number of sprites that can drawn in a single batch by this <see cref="SpriteBatch"/>.
        /// </summary>
        public Int32 BatchSize
        {
            get { return batchSize; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                SafeDispose.DisposeRef(ref vertexBuffer);
                SafeDispose.DisposeRef(ref indexBuffer);
                SafeDispose.DisposeRef(ref geometryStream);
                SafeDispose.DisposeRef(ref spriteEffect);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Calculates and caches the UV factors for the specified texture.
        /// </summary>
        /// <param name="texture">The texture for which to calculate UV factors.</param>
        protected void CalculateUV(Texture2D texture)
        {
            cachedU = 1f / (float)texture.Width;
            cachedV = 1f / (float)texture.Height;
        }

        /// <summary>
        /// Calculates and caches the sine and cosine of the specified rotation.
        /// </summary>
        /// <param name="rotation">The rotation for which to calculate sine and cosine values.</param>
        protected void CalculateSinAndCos(Single rotation)
        {
            cachedCos = 1f;
            cachedSin = 0f;
            if (rotation != 0f)
            {
                cachedCos = (float)Math.Cos(rotation);
                cachedSin = (float)Math.Sin(rotation);
            }
        }

        /// <summary>
        /// Calculates the relative origin of the specified sprite.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        [CLSCompliant(false)]
        protected void CalculateRelativeOrigin(SpriteHeader* metadata)
        {
            if ((metadata->Effects & SpriteEffects.OriginRelativeToDestination) == SpriteEffects.OriginRelativeToDestination)
            {
                cachedOriginX = (metadata->DestinationWidth == 0) ? 0 : metadata->OriginX / (float)metadata->DestinationWidth;
                cachedOriginY = (metadata->DestinationHeight == 0) ? 0 : metadata->OriginY / (float)metadata->DestinationHeight;
            }
            else
            {
                cachedOriginX = (metadata->SourceWidth == 0) ? 0 : metadata->OriginX / (float)metadata->SourceWidth;
                cachedOriginY = (metadata->SourceHeight == 0) ? 0 : metadata->OriginY / (float)metadata->SourceHeight;
            }
        }

        /// <summary>
        /// Calculates the relative origin of the specified sprite.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        protected void CalculateRelativeOrigin(IntPtr metadata)
        {
            CalculateRelativeOrigin((SpriteHeader*)metadata);
        }

        /// <summary>
        /// Calculates the position of a sprite vertex.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="position">The position of the current vertex represented as a <see cref="MutableVector3"/>.</param>
        [CLSCompliant(false)]
        protected void CalculatePosition(SpriteHeader* metadata, Int32 ix, MutableVector3* position)
        {
            var x = xCoords[ix];
            var y = yCoords[ix];

            var scaledX = (x - cachedOriginX) * metadata->DestinationWidth;
            var scaledY = (y - cachedOriginY) * metadata->DestinationHeight;

            position->X = metadata->DestinationX + scaledX * cachedCos - scaledY * cachedSin;
            position->Y = metadata->DestinationY + scaledX * cachedSin + scaledY * cachedCos;
            position->Z = 0;
        }

        /// <summary>
        /// Calculates the position of a sprite vertex.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="position">The position of the current vertex.</param>
        [CLSCompliant(false)]
        protected void CalculatePosition(SpriteHeader* metadata, Int32 ix, ref Vector3 position)
        {
            fixed (Vector3* pPosition = &position)
            {
                CalculatePosition(metadata, ix, (MutableVector3*)pPosition);
            }
        }

        /// <summary>
        /// Calculates the position of a sprite vertex.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="position">The position of the current vertex.</param>
        protected void CalculatePosition(IntPtr metadata, Int32 ix, ref Vector3 position)
        {
            fixed (Vector3* pPosition = &position)
            {
                CalculatePosition((SpriteHeader*)metadata, ix, (MutableVector3*)pPosition);
            }
        }

        /// <summary>
        /// Calculates the texture coordinates of a sprite vertex.
        /// </summary>
        /// <param name="metadata">The sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="textureCoordinates">The texture coordinates of the current vertex.</param>
        [CLSCompliant(false)]
        protected void CalculateTextureCoordinates(SpriteHeader* metadata, Int32 ix, MutableVector2* textureCoordinates)
        {
            var x = xCoords[ix];
            var y = yCoords[ix];

            if ((metadata->Effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                x = 1f - x;

            if ((metadata->Effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
                y = 1f - y;

            textureCoordinates->X = (metadata->SourceX + x * metadata->SourceWidth) * cachedU;
            textureCoordinates->Y = 1f - ((metadata->SourceY + y * metadata->SourceHeight) * cachedV);
        }

        /// <summary>
        /// Calculates the texture coordinates of a sprite vertex.
        /// </summary>
        /// <param name="metadata">The sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="textureCoordinates">The texture coordinates of the current vertex.</param>
        [CLSCompliant(false)]
        protected void CalculateTextureCoordinates(SpriteHeader* metadata, Int32 ix, ref Vector2 textureCoordinates)
        {
            fixed (Vector2* pTextureCoordinates = &textureCoordinates)
            {
                CalculateTextureCoordinates(metadata, ix, (MutableVector2*)pTextureCoordinates);
            }
        }

        /// <summary>
        /// Calculates the texture coordinates of a sprite vertex.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="textureCoordinates">The texture coordinates of the current vertex.</param>
        protected void CalculateTextureCoordinates(IntPtr metadata, Int32 ix, ref Vector2 textureCoordinates)
        {
            fixed (Vector2* pTextureCoordinates = &textureCoordinates)
            {
                CalculateTextureCoordinates((SpriteHeader*)metadata, ix, (MutableVector2*)pTextureCoordinates);
            }
        }

        /// <summary>
        /// Calculates the position and texture coordinates of a sprite vertex.
        /// </summary>
        /// <param name="metadata">A pointer to the sprite metadata.</param>
        /// <param name="ix">The index of the current vertex.</param>
        /// <param name="position">The position of the current vertex represented as a <see cref="MutableVector3"/>.</param>
        /// <param name="textureCoordinates">The texture coordinates of the current vertex.</param>
        [CLSCompliant(false)]
        protected void CalculatePositionAndTextureCoordinates(SpriteHeader* metadata, Int32 ix, MutableVector3* position, MutableVector2* textureCoordinates)
        {
            var x = xCoords[ix];
            var y = yCoords[ix];

            var scaledX = (x - cachedOriginX) * metadata->DestinationWidth;
            var scaledY = (y - cachedOriginY) * metadata->DestinationHeight;

            position->X = metadata->DestinationX + scaledX * cachedCos - scaledY * cachedSin;
            position->Y = metadata->DestinationY + scaledX * cachedSin + scaledY * cachedCos;
            position->Z = 0;

            if ((metadata->Effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                x = 1f - x;

            if ((metadata->Effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
                y = 1f - y;

            textureCoordinates->X = (metadata->SourceX + x * metadata->SourceWidth) * cachedU;
            textureCoordinates->Y = 1f - ((metadata->SourceY + y * metadata->SourceHeight) * cachedV);
        }

        /// <summary>
        /// Generates vertices for a group of sprites.
        /// </summary>
        /// <param name="texture">The batch's texture.</param>
        /// <param name="sprites">The batch's sprite metadata array.</param>
        /// <param name="vertices">The batch's vertex data array.</param>
        /// <param name="data">The batch's custom data array.</param>
        /// <param name="offset">The offset of the first sprite being drawn.</param>
        /// <param name="count">The number of sprites being drawn.</param>
        /// <returns>The vertex stride.</returns>
        protected abstract void GenerateVertices(Texture2D texture, SpriteHeader[] sprites,
            VertexType[] vertices, SpriteData[] data, Int32 offset, Int32 count);

        /// <summary>
        /// Begins a sprite batch operation using the specified sort and blend state objects, as well as a custom effect and transformation matrix.
        /// </summary>
        /// <param name="sortMode">The batch's sprite drawing order.</param>
        /// <param name="blendState">The batch's blend state.</param>
        /// <param name="samplerState">The batch's sampler state.</param>
        /// <param name="depthStencilState">The batch's depth/stencil state.</param>
        /// <param name="rasterizerState">The batch's rasterizer state.</param>
        /// <param name="effect">The batch's custom effect.</param>
        /// <param name="transformMatrix">The batch's transformation matrix.</param>
        private void BeginInternal(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            if (sortMode == SpriteSortMode.Immediate)
            {
                SpriteBatchCoordinator.DemandImmediate();
            }
            else 
            { 
                SpriteBatchCoordinator.DemandDeferred(); 
            }

            this.sortMode = sortMode;
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.depthStencilState = depthStencilState;
            this.rasterizerState = rasterizerState;
            this.customEffect = effect ?? spriteEffect;
            this.transformMatrix = transformMatrix;

            Ultraviolet.ValidateResource(this);
            Ultraviolet.ValidateResource(this.blendState);
            Ultraviolet.ValidateResource(this.samplerState);
            Ultraviolet.ValidateResource(this.depthStencilState);
            Ultraviolet.ValidateResource(this.rasterizerState);
            Ultraviolet.ValidateResource(this.customEffect);

            begun = true;

            if (sortMode == SpriteSortMode.Immediate)
            {
                ApplyState();
            }
        }

        /// <summary>
        /// Finishes a sprite batch operation.
        /// </summary>
        private void EndInternal()
        {
            if (sortMode == SpriteSortMode.Immediate)
            {
                SpriteBatchCoordinator.RelinquishImmediate();
            }
            else
            {
                ApplyState();
                FlushBatch();

                SpriteBatchCoordinator.RelinquishDeferred();
            }
            begun = false;
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
        /// <param name="data">The sprite's custom data.</param>
        private void DrawInternal(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Ultraviolet.ValidateResource(texture);

            var ix = batchInfo.Reserve(texture, ref data);

            fixed (SpriteHeader* pSprite = &batchInfo.GetHeaders()[ix])
            {
                if (sourceRectangle.HasValue)
                {
                    var sourceRectangleValue = sourceRectangle.GetValueOrDefault();
                    pSprite->SourceX = sourceRectangleValue.X;
                    pSprite->SourceY = sourceRectangleValue.Y;
                    pSprite->SourceWidth = sourceRectangleValue.Width;
                    pSprite->SourceHeight = sourceRectangleValue.Height;
                }
                else
                {
                    pSprite->SourceX = 0;
                    pSprite->SourceY = 0;
                    pSprite->SourceWidth = texture.Width;
                    pSprite->SourceHeight = texture.Height;
                }
                pSprite->DestinationX = destinationRectangle.X;
                pSprite->DestinationY = destinationRectangle.Y;
                pSprite->DestinationWidth = destinationRectangle.Width;
                pSprite->DestinationHeight = destinationRectangle.Height;
                pSprite->OriginX = origin.X;
                pSprite->OriginY = origin.Y;
                pSprite->Rotation = rotation;
                pSprite->Depth = layerDepth;
                pSprite->Color = color;
                pSprite->Effects = effects;
            }

            if (this.sortMode == SpriteSortMode.Immediate)
            {
                FlushSprites(texture, 0, 1);
                this.batchInfo.Clear();
            }
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
        /// <param name="data">The sprite's custom data.</param>
        private void DrawInternal(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Ultraviolet.ValidateResource(texture);

            var ix = batchInfo.Reserve(texture, ref data);

            fixed (SpriteHeader* pSprite = &this.batchInfo.GetHeaders()[ix])
            {
                float width, height;
                if (sourceRectangle.HasValue)
                {
                    var sourceRectangleValue = sourceRectangle.GetValueOrDefault();
                    pSprite->SourceX = sourceRectangleValue.X;
                    pSprite->SourceY = sourceRectangleValue.Y;
                    pSprite->SourceWidth = sourceRectangleValue.Width;
                    pSprite->SourceHeight = sourceRectangleValue.Height;
                    width = sourceRectangleValue.Width * scale.X;
                    height = sourceRectangleValue.Height * scale.Y;
                }
                else
                {
                    pSprite->SourceX = 0;
                    pSprite->SourceY = 0;
                    pSprite->SourceWidth = texture.Width;
                    pSprite->SourceHeight = texture.Height;
                    width = texture.Width * scale.X;
                    height = texture.Height * scale.Y;
                }
                pSprite->DestinationX = position.X;
                pSprite->DestinationY = position.Y;
                pSprite->DestinationWidth = width;
                pSprite->DestinationHeight = height;
                pSprite->OriginX = origin.X;
                pSprite->OriginY = origin.Y;
                pSprite->Rotation = rotation;
                pSprite->Depth = layerDepth;
                pSprite->Color = color;
                pSprite->Effects = effects;
            }

            if (this.sortMode == SpriteSortMode.Immediate)
            {
                FlushSprites(texture, 0, 1);
                this.batchInfo.Clear();
            }
        }

        /// <summary>
        /// Draws a string of text.
        /// </summary>
        /// <param name="fontFace">The font face with which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The text's position.</param>
        /// <param name="color">The text's color.</param>
        /// <param name="rotation">The text's rotation in radians.</param>
        /// <param name="origin">The text's point of origin relative to its top-left corner.</param>
        /// <param name="scale">The text's scale factor.</param>
        /// <param name="effects">The text's rendering effects.</param>
        /// <param name="layerDepth">The text's layer depth.</param>
        /// <param name="data">The text's custom data.</param>
        private void DrawStringInternal(SpriteFontFace fontFace, StringSource text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            Contract.Require(fontFace, "fontFace");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(begun, UltravioletStrings.BeginMustBeCalledBeforeDraw);

            // Determine the direction in which to move the render position
            // according to the specified sprite effects.
            var flipHorizontal = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
            var flipVertical = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
            var dirX = flipHorizontal ? -1 : 1;
            var dirY = flipVertical ? -1 : 1;

            // Retrieve the font's glyph data.
            if (fontFace.Texture == null)
            {
                throw new InvalidOperationException(UltravioletStrings.InvalidFontTexture);
            }

            // Calculate the size of the area in which the text will be rendered.
            var measure = fontFace.MeasureString(ref text);
            var areaTL = new Vector2((int)position.X - origin.X, (int)position.Y - origin.Y);
            var areaBR = new Vector2((int)position.X - origin.X + measure.Width, (int)position.Y - origin.Y + measure.Height);

            // Calculate the transformation matrix.
            var transformRotation = Matrix.CreateRotationZ(rotation);
            var transformScale =  Matrix.CreateScale(scale.X, scale.Y, 1f);
            Matrix transform;
            Matrix.Multiply(ref transformRotation, ref transformScale, out transform);

            // Transform the text area.            
            Vector2 transformedTL, transformedBR;
            Vector2.Transform(ref areaTL, ref transform, out transformedTL);
            Vector2.Transform(ref areaBR, ref transform, out transformedBR);

            // Add the text's glyphs to the sprite batch.
            var glyphPosRaw = Vector2.Zero;
            var glyphPosTransformed = Vector2.Zero;
            var cx = flipHorizontal ? areaBR.X : areaTL.X;
            var cy = flipVertical ? areaBR.Y : areaTL.Y;
            for (int i = 0; i < text.Length; i++)
            {
                // Handle special characters.
                var character = text[i];
                switch (character)
                {
                    case '\t':
                        cx = cx + (fontFace.TabWidth * dirX);
                        continue;

                    case '\r':
                        continue;

                    case '\n':
                        cx = flipHorizontal ? areaBR.X : areaTL.X;
                        cy = cy + (fontFace.LineSpacing * dirY);
                        continue;
                }

                // Calculate the glyph's position on the screen.
                var source = fontFace[character];
                source = new Rectangle(source.X, source.Y, source.Width, source.Height);
                var glyphX = flipHorizontal ? cx - source.Width : cx;
                var glyphY = flipVertical ? cy - source.Height : cy;
                glyphPosRaw = new Vector2(glyphX, glyphY);

                // Add the glyph to the batch.
                Vector2.Transform(ref glyphPosRaw, ref transform, out glyphPosTransformed);
                DrawInternal(fontFace.Texture, glyphPosTransformed,
                    source, color, rotation, Vector2.Zero, scale, effects, layerDepth, data);

                cx += fontFace.MeasureGlyph(ref text, i).Width * dirX;
            }
        }

        /// <summary>
        /// Creates the batch's vertex and index buffers.
        /// </summary>
        private void CreateVertexAndIndexBuffers()
        {
            var geometryStreamInvalidated = false;

            if (vertexBuffer == null || vertexBuffer.Disposed)
            {
                vertexBuffer = DynamicVertexBuffer.Create<VertexType>(batchSize * 4);
                vertexBuffer.ContentLost += (sender, e) => { vertexBufferPosition = 0; };
                vertexBufferPosition = 0;

                geometryStreamInvalidated = true;
            }
            if (indexBuffer == null || indexBuffer.Disposed)
            {
                indexBuffer = DynamicIndexBuffer.Create(IndexBufferElementType.Int16, batchSize * 6);
                indexBuffer.ContentLost += (sender, e) => { PopulateIndexBuffer(); };
                PopulateIndexBuffer();

                geometryStreamInvalidated = true;
            }

            if (geometryStream == null || geometryStreamInvalidated)
            {
                SafeDispose.Dispose(geometryStream);
                geometryStream = GeometryStream.Create();
                geometryStream.Attach(vertexBuffer);
                geometryStream.Attach(indexBuffer);
            }
        }

        /// <summary>
        /// Destroys the batch's vertex and index buffers.
        /// </summary>
        private void DestroyVertexAndIndexBuffers()
        {
            SafeDispose.DisposeRef(ref vertexBuffer);
            SafeDispose.DisposeRef(ref indexBuffer);
        }

        /// <summary>
        /// Populates the batch's index buffer.
        /// </summary>
        private void PopulateIndexBuffer()
        {
            var indices = new short[batchSize * 6];
            for (int i = 0; i < batchSize; i++)
            {
                indices[i * 6 + 0] = (short)(i * 4 + 0);
                indices[i * 6 + 1] = (short)(i * 4 + 1);
                indices[i * 6 + 2] = (short)(i * 4 + 2);
                indices[i * 6 + 3] = (short)(i * 4 + 2);
                indices[i * 6 + 4] = (short)(i * 4 + 3);
                indices[i * 6 + 5] = (short)(i * 4 + 0);
            }
            indexBuffer.SetData<short>(indices);
        }

        /// <summary>
        /// Applies the batch's state to the graphics device.
        /// </summary>
        private void ApplyState()
        {
            var graphics = Ultraviolet.GetGraphics();

            graphics.SetGeometryStream(geometryStream);
            graphics.SetBlendState(blendState ?? BlendState.AlphaBlend);
            graphics.SetSamplerState(0, samplerState ?? SamplerState.LinearClamp);
            graphics.SetDepthStencilState(depthStencilState ?? DepthStencilState.None);
            graphics.SetRasterizerState(rasterizerState ?? RasterizerState.CullCounterClockwise);

            var viewport = Ultraviolet.GetGraphics().GetViewport();
            var projection = Matrix.CreateSpriteBatchProjection(viewport.Width, viewport.Height);
            customEffect.Parameters["MatrixTransform"].SetValue(transformMatrix * projection);
            customEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Flushes a batch of sprites to the graphics device.
        /// </summary>
        private void FlushBatch()
        {
            if (sortMode != SpriteSortMode.Deferred)
                batchInfo.Sort(sortMode);

            var offset = 0;
            var texture = default(Texture2D);
            var textureCurrent = default(Texture2D);
            for (int i = 0; i < batchInfo.Count; i++)
            {
                texture = batchInfo.GetTexture(i);
                if (texture != textureCurrent)
                {
                    if (i > offset)
                    {
                        FlushSprites(textureCurrent, offset, i - offset);
                    }
                    offset = i;
                    textureCurrent = texture;
                }
            }
            FlushSprites(textureCurrent, offset, batchInfo.Count - offset);
            batchInfo.Clear();
        }

        /// <summary>
        /// Flushes a set of sprites within the batch to the device.
        /// </summary>
        /// <param name="texture">The texture with which to render the sprites.</param>
        /// <param name="offset">The offset of the first sprite in the set to flush.</param>
        /// <param name="count">The number of sprites in the set to flush.</param>
        private void FlushSprites(Texture2D texture, Int32 offset, Int32 count)
        {
            // Set the sprite texture.
            var graphics = Ultraviolet.GetGraphics();
            graphics.SetTexture(0, texture);

            // Draw the sprites in this batch.
            var options = SetDataOptions.NoOverwrite;
            while (count > 0)
            {
                // Limit the number of sprites being drawn to our maximum batch size and
                // determine whether we need to reset to the beginning of the vertex buffer.
                var drawn = count;
                if (vertexBufferPosition >= batchSize)
                {
                    options = SetDataOptions.Discard;
                    drawn = (count > batchSize) ? batchSize : count;
                    vertexBufferPosition = 0;
                }
                else
                {
                    options = SetDataOptions.NoOverwrite;
                    if (vertexBufferPosition + drawn > batchSize)
                    {
                        drawn = batchSize - vertexBufferPosition;
                    }
                }

                // Generate vertices for the current set of sprites and dispatch them to the graphics device.
                var spriteMetadata = batchInfo.GetHeaders();
                var spriteCustomData = batchInfo.GetData();
                GenerateVertices(texture, spriteMetadata, vertices, spriteCustomData, offset, drawn);
                vertexBuffer.SetData<VertexType>(vertices, vertexBufferPosition * 4, drawn * 4, options);

                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, vertexBufferPosition * 6, drawn * 2);

                // Advance the batch position.
                vertexBufferPosition += drawn;
                offset += drawn;
                count -= drawn;
            }
        }

        // Property values.
        private readonly Int32 batchSize;

        // Batch state.
        private readonly SpriteBatchInfo<SpriteData> batchInfo;
        private SpriteSortMode sortMode;
        private BlendState blendState;
        private SamplerState samplerState;
        private DepthStencilState depthStencilState;
        private RasterizerState rasterizerState;
        private Effect spriteEffect;
        private Effect customEffect;
        private Matrix transformMatrix;
        private Boolean begun;

        // Vertex data  
        private readonly VertexType[] vertices;
        private GeometryStream geometryStream;
        private DynamicVertexBuffer vertexBuffer;
        private DynamicIndexBuffer indexBuffer;
        private Int32 vertexBufferPosition;

        // Cached values used during standard vertex generation.
        private Single cachedU;
        private Single cachedV;
        private Single cachedSin;
        private Single cachedCos;
        private Single cachedOriginX;
        private Single cachedOriginY;

        // Coordinates used to construct sprite vertices.
        private static readonly Single[] xCoords = new[] { 0f, 1f, 1f, 0f };
        private static readonly Single[] yCoords = new[] { 0f, 0f, 1f, 1f };
    }
}
