using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the RasterizerState class.
    /// </summary>
    public class OpenGLRasterizerState : RasterizerState
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLRasterizerState class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLRasterizerState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates the CullClockwise rasterizer state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The rasterizer state that was created.</returns>
        public static OpenGLRasterizerState CreateCullClockwise(UltravioletContext uv)
        {
            var state = new OpenGLRasterizerState(uv);
            state.CullMode = CullMode.CullClockwiseFace;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the CullCounterClockwise rasterizer state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The rasterizer state that was created.</returns>
        public static OpenGLRasterizerState CreateCullCounterClockwise(UltravioletContext uv)
        {
            var state = new OpenGLRasterizerState(uv);
            state.CullMode = CullMode.CullCounterClockwiseFace;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the CullNone rasterizer state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The rasterizer state that was created.</returns>
        public static OpenGLRasterizerState CreateCullNone(UltravioletContext uv)
        {
            var state = new OpenGLRasterizerState(uv);
            state.CullMode = CullMode.None;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Applies the rasterizer state to the device.
        /// </summary>
        internal void Apply()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            gl.Enable(gl.GL_CULL_FACE, CullMode != CullMode.None);
            gl.ThrowIfError();

            gl.FrontFace(GetFrontFaceGL(CullMode));
            gl.ThrowIfError();

            gl.CullFace(gl.GL_BACK);
            gl.ThrowIfError();

            if (FillMode != FillMode.Solid)
            {
                gl.ThrowIfGLES(OpenGLStrings.UnsupportedFillModeGLES);

                gl.PolygonMode(gl.GL_FRONT_AND_BACK, GetFillModeGL(FillMode));
                gl.ThrowIfError();
            }

            gl.Enable(gl.GL_SCISSOR_TEST, ScissorTestEnable);
            gl.ThrowIfError();

            if (DepthBias != 0f && SlopeScaleDepthBias != 0f)
            {
                gl.Enable(gl.GL_POLYGON_OFFSET_FILL);
                gl.ThrowIfError();

                gl.PolygonOffset(SlopeScaleDepthBias, DepthBias);
                gl.ThrowIfError();
            }
            else
            {
                gl.Disable(gl.GL_POLYGON_OFFSET_FILL);
                gl.ThrowIfError();
            }
        }

        /// <summary>
        /// Gets the OpenGL face mode that corresponds to the specified CullMode value.
        /// </summary>
        /// <param name="mode">The CullMode value to convert.</param>
        /// <returns>The converted value.</returns>
        private static UInt32 GetFrontFaceGL(CullMode mode)
        {
            switch (mode)
            {
                case CullMode.None:
                case CullMode.CullCounterClockwiseFace:
                    // Cull back faces with counterclockwise vertices, i.e. front is clockwise
                    return gl.GL_CW;

                case CullMode.CullClockwiseFace:
                    // Cull back faces with clockwise vertices, i.e. front is counterclockwise
                    return gl.GL_CCW;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the OpenGL fill mode value that corresponds to the specified FillMode value.
        /// </summary>
        /// <param name="mode">The FillMode value to convert.</param>
        /// <returns>The converted value.</returns>
        private static UInt32 GetFillModeGL(FillMode mode)
        {
            switch (mode)
            {
                case FillMode.Solid:
                    return gl.GL_FILL;
                case FillMode.Wireframe:
                    return gl.GL_LINE;
            }
            throw new NotSupportedException();
        }
    }
}
