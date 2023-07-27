using System;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics.Caching;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the DepthStencilState class.
    /// </summary>
    public class OpenGLDepthStencilState : DepthStencilState
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLDepthStencilState class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLDepthStencilState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates the Default depth/stencil state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The depth/stencil state that was created.</returns>
        public static OpenGLDepthStencilState CreateDefault(UltravioletContext uv)
        {
            var state = new OpenGLDepthStencilState(uv);
            state.DepthBufferEnable = true;
            state.DepthBufferWriteEnable = true;
            state.MakeImmutable();
            return state;
        }
        
        /// <summary>
        /// Creates the DepthRead depth/stencil state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The depth/stencil state that was created.</returns>
        public static OpenGLDepthStencilState CreateDepthRead(UltravioletContext uv)
        {
            var state = new OpenGLDepthStencilState(uv);
            state.DepthBufferEnable = true;
            state.DepthBufferWriteEnable = false;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the None depth/stencil state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The depth/stencil state that was created.</returns>
        public static OpenGLDepthStencilState CreateNone(UltravioletContext uv)
        {
            var state = new OpenGLDepthStencilState(uv);
            state.DepthBufferEnable = false;
            state.DepthBufferWriteEnable = false;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Applies the depth/stencil state to the device.
        /// </summary>
        internal void Apply()
        {
            OpenGLState.DepthTestEnabled = DepthBufferEnable;
            OpenGLState.DepthMask = DepthBufferWriteEnable;
            OpenGLState.DepthFunc = GetCompareFunctionGL(DepthBufferFunction);

            OpenGLState.StencilTestEnabled = StencilEnable;
            OpenGLState.StencilMask = (UInt32)StencilWriteMask;

            if (TwoSidedStencilMode)
            {
                OpenGLState.StencilFuncFront = new CachedStencilFunc(GetCompareFunctionGL(StencilFunction), ReferenceStencil, StencilMask);
                OpenGLState.StencilFuncBack = new CachedStencilFunc(GetCompareFunctionGL(CounterClockwiseStencilFunction), ReferenceStencil, StencilMask);
                OpenGLState.StencilOpFront = new CachedStencilOp(
                    GetStencilOpGL(StencilFail), 
                    GetStencilOpGL(StencilDepthBufferFail),
                    GetStencilOpGL(StencilPass));
                OpenGLState.StencilOpBack = new CachedStencilOp( 
                    GetStencilOpGL(CounterClockwiseStencilFail),
                    GetStencilOpGL(CounterClockwiseStencilDepthBufferFail),
                    GetStencilOpGL(CounterClockwiseStencilPass));
            }
            else
            {
                OpenGLState.StencilFuncCombined = new CachedStencilFunc(GetCompareFunctionGL(StencilFunction), ReferenceStencil, StencilMask);
                OpenGLState.StencilOpCombined = new CachedStencilOp(
                    GetStencilOpGL(StencilFail),
                    GetStencilOpGL(StencilDepthBufferFail),
                    GetStencilOpGL(StencilPass));
            }
        }

        /// <summary>
        /// Gets the OpenGL enum value that corresponds to the specified CompareFunction value.
        /// </summary>
        /// <param name="func">The CompareFunction value to convert.</param>
        /// <returns>The converted value.</returns>
        private static UInt32 GetCompareFunctionGL(CompareFunction func)
        {
            switch (func)
            {
                case CompareFunction.Always:
                    return GL.GL_ALWAYS;
                case CompareFunction.Never:
                    return GL.GL_NEVER;
                case CompareFunction.Equal:
                    return GL.GL_EQUAL;
                case CompareFunction.NotEqual:
                    return GL.GL_NOTEQUAL;
                case CompareFunction.Greater:
                    return GL.GL_GREATER;
                case CompareFunction.GreaterEqual:
                    return GL.GL_GEQUAL;
                case CompareFunction.Less:
                    return GL.GL_LESS;
                case CompareFunction.LessEqual:
                    return GL.GL_LEQUAL;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the OpenGL enum value that corresponds to the specified StencilOperation value.
        /// </summary>
        /// <param name="op">The StencilOperation value to convert.</param>
        /// <returns>The converted value.</returns>
        private static UInt32 GetStencilOpGL(StencilOperation op)
        {
            switch (op)
            {
                case StencilOperation.Decrement:
                    return GL.GL_DECR_WRAP;
                case StencilOperation.DecrementSaturation:
                    return GL.GL_DECR;
                case StencilOperation.Increment:
                    return GL.GL_INCR_WRAP;
                case StencilOperation.IncrementSaturation:
                    return GL.GL_INCR;
                case StencilOperation.Invert:
                    return GL.GL_INVERT;
                case StencilOperation.Keep:
                    return GL.GL_KEEP;
                case StencilOperation.Replace:
                    return GL.GL_REPLACE;
                case StencilOperation.Zero:
                    return GL.GL_ZERO;
            }
            throw new NotSupportedException();
        }
    }
}
