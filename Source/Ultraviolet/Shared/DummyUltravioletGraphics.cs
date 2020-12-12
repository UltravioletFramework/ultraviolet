using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletGraphics"/>.
    /// </summary>
    public sealed class DummyUltravioletGraphics : UltravioletResource, IUltravioletGraphics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyUltravioletGraphics"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public DummyUltravioletGraphics(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Updating?.Invoke(this, time);
        }

        /// <inheritdoc/>
        public void Clear(Color color)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void Clear(Color color, Double depth, Int32 stencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void Clear(ClearOptions options, Color color, Double depth, Int32 stencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt, Color clearColor)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt, Color clearColor, Double clearDepth, Int32 clearStencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer(Color clearColor)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer(Color clearColor, Double clearDepth, Int32 clearStencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public RenderTarget2D GetRenderTarget()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(RenderTarget2D);
        }

        /// <inheritdoc/>
        public void SetViewport(Viewport viewport)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public Viewport GetViewport()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(Viewport);
        }

        /// <inheritdoc/>
        public void UnbindTexture(Texture texture)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void UnbindTextures(Object state, Func<Texture, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void UnbindTextures(Object state, Func<Texture2D, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void UnbindTextures(Object state, Func<Texture3D, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void UnbindAllTextures()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetTexture(Int32 sampler, Texture texture)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public Texture GetTexture(Int32 sampler)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(Texture);
        }

        /// <inheritdoc/>
        public void SetGeometryStream(GeometryStream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public GeometryStream GetGeometryStream()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(GeometryStream);
        }

        /// <inheritdoc/>
        public void SetBlendState(BlendState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public BlendState GetBlendState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(BlendState);
        }

        /// <inheritdoc/>
        public void SetDepthStencilState(DepthStencilState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public DepthStencilState GetDepthStencilState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(DepthStencilState);
        }

        /// <inheritdoc/>
        public void SetRasterizerState(RasterizerState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public RasterizerState GetRasterizerState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(RasterizerState);
        }

        /// <inheritdoc/>
        public void SetSamplerState(Int32 sampler, SamplerState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public SamplerState GetSamplerState(Int32 sampler)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(SamplerState);
        }

        /// <inheritdoc/>
        public void SetScissorRectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void SetScissorRectangle(Rectangle? rect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public Rectangle? GetScissorRectangle()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return default(Rectangle?);
        }

        /// <inheritdoc/>
        public void DrawPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void DrawPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void DrawIndexedPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void DrawIndexedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances, Int32 baseInstance)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count, Int32 instances, Int32 baseInstance)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public GraphicsCapabilities Capabilities { get; } = new DummyGraphicsCapabilities();

        /// <inheritdoc/>
        public Single FrameRate => 60f;

        /// <inheritdoc/>
        public Single FrameTimeInMilliseconds => 16.67f;

        /// <inheritdoc/>
        public Boolean CurrentRenderTargetIsSrgbEncoded => false;

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;
    }
}
