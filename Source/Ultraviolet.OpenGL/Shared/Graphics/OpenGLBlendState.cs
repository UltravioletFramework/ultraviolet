using System;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics.Caching;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the BlendState class.
    /// </summary>
    public class OpenGLBlendState : BlendState
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLBlendState class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLBlendState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates an Opaque blend state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The blend state that was created.</returns>
        public static OpenGLBlendState CreateOpaque(UltravioletContext uv)
        {
            var state = new OpenGLBlendState(uv);
            state.AlphaSourceBlend = Blend.One;
            state.AlphaDestinationBlend = Blend.Zero;
            state.ColorSourceBlend = Blend.One;
            state.ColorDestinationBlend = Blend.Zero;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates an AlphaBlend blend state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The blend state that was created.</returns>
        public static OpenGLBlendState CreateAlphaBlend(UltravioletContext uv)
        {
            var state = new OpenGLBlendState(uv);
            state.AlphaSourceBlend = Blend.One;
            state.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            state.ColorSourceBlend = Blend.One;
            state.ColorDestinationBlend = Blend.InverseSourceAlpha;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates an Additive blend state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The blend state that was created.</returns>
        public static OpenGLBlendState CreateAdditive(UltravioletContext uv)
        {
            var state = new OpenGLBlendState(uv);
            state.AlphaSourceBlend = Blend.SourceAlpha;
            state.AlphaDestinationBlend = Blend.One;
            state.ColorSourceBlend = Blend.SourceAlpha;
            state.ColorDestinationBlend = Blend.One;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates a NonPremultiplied blend state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The blend state that was created.</returns>
        public static OpenGLBlendState CreateNonPremultiplied(UltravioletContext uv)
        {
            var state = new OpenGLBlendState(uv);
            state.AlphaSourceBlend = Blend.SourceAlpha;
            state.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            state.ColorSourceBlend = Blend.SourceAlpha;
            state.ColorDestinationBlend = Blend.InverseSourceAlpha;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Gets a value indicating whether this state uses separate alpha and color blending.
        /// </summary>
        public Boolean IsSeparateBlend
        {
            get
            {
                return
                    AlphaBlendFunction != ColorBlendFunction ||
                    AlphaSourceBlend != ColorSourceBlend ||
                    AlphaDestinationBlend != ColorDestinationBlend;
            }
        }

        /// <summary>
        /// Applies the blend state to the device.
        /// </summary>
        internal void Apply()
        {
            OpenGLState.BlendEnabled = true;
            OpenGLState.BlendColor = BlendFactor;
            OpenGLState.BlendEquation = new CachedBlendEquation(
                GetBlendFunctionGL(ColorBlendFunction),
                GetBlendFunctionGL(AlphaBlendFunction));
            OpenGLState.BlendFunction = new CachedBlendFunction(
                GetBlendGL(ColorSourceBlend, false),
                GetBlendGL(AlphaSourceBlend, true),
                GetBlendGL(ColorDestinationBlend, false),
                GetBlendGL(AlphaDestinationBlend, true));

            OpenGLState.ColorMask = ColorWriteChannels;
        }

        /// <summary>
        /// Converts an Ultraviolet BlendFunction value to the equivalent OpenGL value.
        /// </summary>
        /// <param name="fn">The Ultraviolet BlendFunction value to convert.</param>
        /// <returns>The converted OpenGL value.</returns>
        private static UInt32 GetBlendFunctionGL(BlendFunction fn)
        {
            switch (fn)
            {
                case BlendFunction.Add:
                    return gl.GL_FUNC_ADD;
                case BlendFunction.Min:
                    return gl.GL_MIN;
                case BlendFunction.Max:
                    return gl.GL_MAX;
                case BlendFunction.ReverseSubtract:
                    return gl.GL_FUNC_REVERSE_SUBTRACT;
                case BlendFunction.Subtract:
                    return gl.GL_FUNC_SUBTRACT;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Converts an Ultraviolet Blend value to the equivalent OpenGL value.
        /// </summary>
        /// <param name="blend">The Ultraviolet Blend value to convert.</param>
        /// <param name="alpha">A value indicating whether alpha blending is enabled.</param>
        /// <returns>The converted OpenGL value.</returns>
        private static UInt32 GetBlendGL(Blend blend, Boolean alpha)
        {
            switch (blend)
            {
                case Blend.Zero:
                    return gl.GL_ZERO;
                case Blend.One:
                    return gl.GL_ONE;
                case Blend.SourceColor:
                    return gl.GL_SRC_COLOR;
                case Blend.InverseSourceColor:
                    return gl.GL_ONE_MINUS_SRC_COLOR;
                case Blend.SourceAlpha:
                    return gl.GL_SRC_ALPHA;
                case Blend.InverseSourceAlpha:
                    return gl.GL_ONE_MINUS_SRC_ALPHA;
                case Blend.DestinationAlpha:
                    return gl.GL_DST_ALPHA;
                case Blend.InverseDestinationAlpha:
                    return gl.GL_ONE_MINUS_DST_ALPHA;
                case Blend.DestinationColor:
                    return gl.GL_DST_COLOR;
                case Blend.InverseDestinationColor:
                    return gl.GL_ONE_MINUS_DST_COLOR;
                case Blend.SourceAlphaSaturation:
                    return gl.GL_SRC_ALPHA_SATURATE;
                case Blend.BlendFactor:
                    return alpha ? gl.GL_CONSTANT_ALPHA : gl.GL_CONSTANT_COLOR;
                case Blend.InverseBlendFactor:
                    return alpha ? gl.GL_ONE_MINUS_CONSTANT_ALPHA : gl.GL_ONE_MINUS_CONSTANT_COLOR;
            }
            throw new NotSupportedException();
        }
    }
}
