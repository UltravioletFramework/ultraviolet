using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the SamplerState class.
    /// </summary>
    public class OpenGLSamplerState : SamplerState
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLSamplerState class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLSamplerState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates the PointClamp sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreatePointClamp(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Point;
            state.AddressU = TextureAddressMode.Clamp;
            state.AddressV = TextureAddressMode.Clamp;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the PointWrap sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreatePointWrap(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Point;
            state.AddressU = TextureAddressMode.Wrap;
            state.AddressV = TextureAddressMode.Wrap;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the LinearClamp sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreateLinearClamp(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Linear;
            state.AddressU = TextureAddressMode.Clamp;
            state.AddressV = TextureAddressMode.Clamp;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the LinearWrap sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreateLinearWrap(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Linear;
            state.AddressU = TextureAddressMode.Wrap;
            state.AddressV = TextureAddressMode.Wrap;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the AnisotropicClamp sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreateAnisotropicClamp(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Anisotropic;
            state.AddressU = TextureAddressMode.Clamp;
            state.AddressV = TextureAddressMode.Clamp;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Creates the AnisotropicWrap sampler state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The sampler state that was created.</returns>
        public static OpenGLSamplerState CreateAnisotropicWrap(UltravioletContext uv)
        {
            var state = new OpenGLSamplerState(uv);
            state.Filter = TextureFilter.Anisotropic;
            state.AddressU = TextureAddressMode.Wrap;
            state.AddressV = TextureAddressMode.Wrap;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Applies the sampler state to the device.
        /// </summary>
        /// <param name="sampler">The sampler index on which to set the state.</param>
        internal void Apply(Int32 sampler)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            gl.ActiveTexture((uint)(gl.GL_TEXTURE0 + sampler));
            gl.ThrowIfError();

            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_S, GetTextureAddressModeGL(AddressU));
            gl.ThrowIfError();

            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_T, GetTextureAddressModeGL(AddressV));
            gl.ThrowIfError();

            if (MipMapLevelOfDetailBias != 0)
            {
                gl.ThrowIfGLES(OpenGLStrings.UnsupportedLODBiasGLES);

                gl.TexParameterf(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_LOD_BIAS, MipMapLevelOfDetailBias);
                gl.ThrowIfError();
            }

            switch (Filter)
            {
                case TextureFilter.Point:
                    gl.TexParameterf(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                    gl.ThrowIfError();

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_NEAREST);
                    gl.ThrowIfError();

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_NEAREST);
                    gl.ThrowIfError();
                    break;

                case TextureFilter.Linear:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        gl.TexParameterf(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                        gl.ThrowIfError();
                    }

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();
                    break;

                case TextureFilter.Anisotropic:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        gl.TexParameterf(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, Math.Min(1f, MaxAnisotropy));
                        gl.ThrowIfError();
                    }

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Converts the specified TextureAddressMode value to the equivalent OpenGL value.
        /// </summary>
        /// <param name="mode">The TextureAddressMode value to convert.</param>
        /// <returns>The converted value.</returns>
        private static Int32 GetTextureAddressModeGL(TextureAddressMode mode)
        {
            switch (mode)
            {
                case TextureAddressMode.Clamp:
                    return (int)gl.GL_CLAMP_TO_EDGE;
                case TextureAddressMode.Wrap:
                    return (int)gl.GL_REPEAT;
                case TextureAddressMode.Mirror:
                    return (int)gl.GL_MIRRORED_REPEAT;
            }
            throw new NotSupportedException();
        }
    }
}
