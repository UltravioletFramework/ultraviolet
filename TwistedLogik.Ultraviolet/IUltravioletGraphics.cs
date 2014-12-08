using System;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's graphics subsystem.
    /// </summary>
    public interface IUltravioletGraphics : IUltravioletSubsystem
    {
        /// <summary>
        /// Clears the back buffer to the specified color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to which to clear the buffer.</param>
        void Clear(Color color);
        
        /// <summary>
        /// Clears the back buffer to the specified color, depth, and stencil values.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to which to clear the buffer.</param>
        /// <param name="depth">The depth value to which to clear the buffer.</param>
        /// <param name="stencil">The stencil value to which to clear the buffer.</param>
        void Clear(Color color, Double depth, Int32 stencil);

        /// <summary>
        /// Sets the render target.
        /// </summary>
        /// <param name="rt">The render target to set, or <c>null</c> to revert to the default render target.</param>
        void SetRenderTarget(RenderTarget2D rt);

        /// <summary>
        /// Gets the device's current render target.
        /// </summary>
        /// <returns>The device's current render target.</returns>
        RenderTarget2D GetRenderTarget();

        /// <summary>
        /// Sets the viewport.
        /// </summary>
        /// <param name="viewport">The viewport to set.</param>
        void SetViewport(Viewport viewport);

        /// <summary>
        /// Gets the device's viewport.
        /// </summary>
        /// <returns>The device's viewport.</returns>
        Viewport GetViewport();

        /// <summary>
        /// Binds a texture to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <param name="texture">The texture to bind to the specified texture stage.</param>
        void SetTexture(Int32 sampler, Texture2D texture);

        /// <summary>
        /// Gets the texture that is bound to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <returns>The texture that is bound to the specified sampler.</returns>
        Texture2D GetTexture(Int32 sampler);

        /// <summary>
        /// Binds a geometry stream to the graphics device.
        /// </summary>
        /// <param name="stream">The geometry stream to bind to the graphics device.</param>
        void SetGeometryStream(GeometryStream stream);

        /// <summary>
        /// Gets the geometry stream that is bound to the graphics device.
        /// </summary>
        /// <returns>The geometry stream that is bound to the graphics device.</returns>
        GeometryStream GetGeometryStream();

        /// <summary>
        /// Binds a blend state to the graphics device.
        /// </summary>
        /// <param name="state">The blend state to bind to the graphics device.</param>
        void SetBlendState(BlendState state);

        /// <summary>
        /// Gets the blend state that is bound to the device.
        /// </summary>
        /// <returns>The blend state that is bound to the device.</returns>
        BlendState GetBlendState();

        /// <summary>
        /// Binds a depth/stencil state to the graphics device.
        /// </summary>
        /// <param name="state">The depth/stencil state to bind to the graphics device.</param>
        void SetDepthStencilState(DepthStencilState state);

        /// <summary>
        /// Gets the depth/stencil state that is bound to the device.
        /// </summary>
        /// <returns>The depth/stencil state that is bound to the device.</returns>
        DepthStencilState GetDepthStencilState();

        /// <summary>
        /// Binds a rasterizer state to the graphics device.
        /// </summary>
        /// <param name="state">The rasterizer state to bind to the graphics device.</param>
        void SetRasterizerState(RasterizerState state);

        /// <summary>
        /// Gets the rasterizer state that is bound to the device.
        /// </summary>
        /// <returns>The rasterizer state that is bound to the device.</returns>
        RasterizerState GetRasterizerState();

        /// <summary>
        /// Binds a sampler state to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <param name="state">The sampler state to bind to the sampler.</param>
        void SetSamplerState(Int32 sampler, SamplerState state);

        /// <summary>
        /// Gets the sampler state that is bound to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <returns>The sampler state that is bound to the specified sampler.</returns>
        SamplerState GetSamplerState(Int32 sampler);

        /// <summary>
        /// Sets the device's scissor rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the scissor rectangle.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the scissor rectangle.</param>
        /// <param name="width">The width of the scissor rectangle.</param>
        /// <param name="height">The height of the scissor rectangle.</param>
        void SetScissorRectangle(Int32 x, Int32 y, Int32 width, Int32 height);

        /// <summary>
        /// Sets the device's scissor rectangle.
        /// </summary>
        /// <param name="rect">The scissor rectangle, or <c>null</c> to disable the scissor test.</param>
        void SetScissorRectangle(Rectangle? rect);

        /// <summary>
        /// Gets the device's scissor rectangle.
        /// </summary>
        /// <returns>The device's scissor rectangle, or <c>null</c> if the scissor test is disabled.</returns>
        Rectangle? GetScissorRectangle();

        /// <summary>
        /// Draws a collection of non-indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawPrimitives(PrimitiveType type, Int32 start, Int32 count);
        
        /// <summary>
        /// Draws a collection of indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawIndexedPrimitives(PrimitiveType type, Int32 start, Int32 count);

        /// <summary>
        /// Gets the current frame rate.
        /// </summary>
        Single FrameRate
        {
            get;
        }
    }
}
