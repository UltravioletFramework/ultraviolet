using System;
using Ultraviolet.Graphics;

namespace Ultraviolet
{
    /// <summary>
    /// Initializes a new instance of the IUltravioletGraphics implementation.
    /// </summary>
    /// <param name="context">The Ultraviolet context.</param>
    /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
    public delegate IUltravioletGraphics UltravioletGraphicsFactory(UltravioletContext context, UltravioletConfiguration configuration);

    /// <summary>
    /// Represents the Ultraviolet Framework's graphics subsystem.
    /// </summary>
    public interface IUltravioletGraphics : IUltravioletSubsystem
    {
        /// <summary>
        /// Clears the back buffer to the specified color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to which to clear the color buffer.</param>
        void Clear(Color color);

        /// <summary>
        /// Clears the back buffer to the specified color, depth, and stencil values.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to which to clear the color buffer.</param>
        /// <param name="depth">The depth value to which to clear the depth buffer.</param>
        /// <param name="stencil">The stencil value to which to clear the stencil buffer.</param>
        void Clear(Color color, Double depth, Int32 stencil);

        /// <summary>
        /// Clears the back buffer to the specified color, depth, and stencil values.
        /// </summary>
        /// <param name="options">A set of <see cref="ClearOptions"/> flags specifying which buffers to clear.</param>
        /// <param name="color">The <see cref="Color"/> to which to clear the color buffer.</param>
        /// <param name="depth">The depth value to which to clear the depth buffer.</param>
        /// <param name="stencil">The stencil value to which to clear the stencil buffer.</param>
        void Clear(ClearOptions options, Color color, Double depth, Int32 stencil);

        /// <summary>
        /// Sets the render target.
        /// </summary>
        /// <param name="rt">The render target to set, or <see langword="null"/> to revert to
        /// the default render target for the current window's compositor.</param>
        void SetRenderTarget(RenderTarget2D rt);

        /// <summary>
        /// Sets the render target and clears it to the specified values (if it is 
        /// set to <see cref="RenderTargetUsage.DiscardContents"/>).
        /// </summary>
        /// <param name="rt">The render target to set, or <see langword="null"/> to revert to
        /// the default render target for the current window's compositor.</param>
        /// <param name="clearColor">The color to which to clear the render target.</param>
        void SetRenderTarget(RenderTarget2D rt, Color clearColor);

        /// <summary>
        /// Sets the render target and clears it to the specified values (if it is 
        /// set to <see cref="RenderTargetUsage.DiscardContents"/>).
        /// </summary>
        /// <param name="rt">The render target to set, or <see langword="null"/> to revert to
        /// the default render target for the current window's compositor.</param>
        /// <param name="clearColor">The color to which to clear the render target.</param>
        /// <param name="clearDepth">The depth to which to clear the render target.</param>
        /// <param name="clearStencil">The stencil value to which to clear the render target.</param>
        void SetRenderTarget(RenderTarget2D rt, Color clearColor, Double clearDepth, Int32 clearStencil);

        /// <summary>
        /// Sets the render target to the back buffer, bypassing any window compositors.
        /// </summary>
        void SetRenderTargetToBackBuffer();

        /// <summary>
        /// Sets the render target to the back buffer, bypassing any window compositors, and clears
        /// it to the specified values (if it is set to <see cref="RenderTargetUsage.DiscardContents"/>).
        /// </summary>
        /// <param name="clearColor">The color to which to clear the render target.</param>
        void SetRenderTargetToBackBuffer(Color clearColor);

        /// <summary>
        /// Sets the render target to the back buffer, bypassing any window compositors, and clears
        /// it to the specified values (if it is set to <see cref="RenderTargetUsage.DiscardContents"/>).
        /// </summary>
        /// <param name="clearColor">The color to which to clear the render target.</param>
        /// <param name="clearDepth">The depth to which to clear the render target.</param>
        /// <param name="clearStencil">The stencil value to which to clear the render target.</param>
        void SetRenderTargetToBackBuffer(Color clearColor, Double clearDepth, Int32 clearStencil);

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
        /// Unbinds the specified texture from the graphics device.
        /// </summary>
        /// <param name="texture">The texture to unbind.</param>
        void UnbindTexture(Texture texture);

        /// <summary>
        /// Unbinds any textures which satisfy the specified predicate from the graphics device.
        /// </summary>
        /// <param name="state">A state object to pass to the predicat.e</param>
        /// <param name="predicate">A predicate which indicates which textures should be unbound.</param>
        void UnbindTextures(Object state, Func<Texture, Object, Boolean> predicate);

        /// <summary>
        /// Unbinds any textures which satisfy the specified predicate from the graphics device.
        /// </summary>
        /// <param name="state">A state object to pass to the predicat.e</param>
        /// <param name="predicate">A predicate which indicates which textures should be unbound.</param>
        void UnbindTextures(Object state, Func<Texture2D, Object, Boolean> predicate);

        /// <summary>
        /// Unbinds any textures which satisfy the specified predicate from the graphics device.
        /// </summary>
        /// <param name="state">A state object to pass to the predicat.e</param>
        /// <param name="predicate">A predicate which indicates which textures should be unbound.</param>
        void UnbindTextures(Object state, Func<Texture3D, Object, Boolean> predicate);

        /// <summary>
        /// Unbinds all textures from the graphics device.
        /// </summary>
        void UnbindAllTextures();

        /// <summary>
        /// Binds a texture to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <param name="texture">The texture to bind to the specified texture stage.</param>
        void SetTexture(Int32 sampler, Texture texture);

        /// <summary>
        /// Gets the texture that is bound to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <returns>The texture that is bound to the specified sampler.</returns>
        Texture GetTexture(Int32 sampler);

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
        /// <param name="rect">The scissor rectangle, or <see langword="null"/> to disable the scissor test.</param>
        void SetScissorRectangle(Rectangle? rect);

        /// <summary>
        /// Gets the device's scissor rectangle.
        /// </summary>
        /// <returns>The device's scissor rectangle, or <see langword="null"/> if the scissor test is disabled.</returns>
        Rectangle? GetScissorRectangle();

        /// <summary>
        /// Draws a collection of non-indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawPrimitives(PrimitiveType type, Int32 start, Int32 count);

        /// <summary>
        /// Draws a collection of non-indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="offset">The offset from the beginning of the vertex buffer, in bytes, at which to begin reading vertex data.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count);

        /// <summary>
        /// Draws a collection of indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawIndexedPrimitives(PrimitiveType type, Int32 start, Int32 count);

        /// <summary>
        /// Draws a collection of indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="offset">The offset from the beginning of the vertex buffer, in bytes, at which to begin reading vertex data.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        void DrawIndexedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count);

        /// <summary>
        /// Draws a collection of instanced geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        /// <param name="instances">The number of instances to render.</param>
        void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances);

        /// <summary>
        /// Draws a collection of instanced geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        /// <param name="instances">The number of instances to render.</param>
        /// <param name="baseInstance">The index of the first instance to render.</param>
        void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances, Int32 baseInstance);

        /// <summary>
        /// Draws a collection of instanced geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">A <see cref="PrimitiveType"/> value representing the type of primitive to render.</param>
        /// <param name="offset">The offset from the beginning of the vertex buffer, in bytes, at which to begin reading vertex data.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        /// <param name="instances">The number of instances to render.</param>
        /// <param name="baseInstance">The index of the first instance to render.</param>
        void DrawInstancedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count, Int32 instances, Int32 baseInstance);

        /// <summary>
        /// Gets a <see cref="GraphicsCapabilities"/> object which exposes the capabilities of the current graphics device.
        /// </summary>
        GraphicsCapabilities Capabilities { get; }

        /// <summary>
        /// Gets the current frame rate.
        /// </summary>
        Single FrameRate { get; }
        
        /// <summary>
        /// Gets the previous frame time in milliseconds.
        /// </summary>
        Single FrameTimeInMilliseconds { get; }

        /// <summary>
        /// Gets a value indicating whether the current render target uses sRGB encoded color.
        /// </summary>
        Boolean CurrentRenderTargetIsSrgbEncoded { get; }
    }
}
