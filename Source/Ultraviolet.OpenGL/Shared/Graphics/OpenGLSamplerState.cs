using System;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the SamplerState class.
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
            state.AddressW = TextureAddressMode.Clamp;
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
            state.AddressW = TextureAddressMode.Wrap;
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
            state.AddressW = TextureAddressMode.Clamp;
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
            state.AddressW = TextureAddressMode.Wrap;
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
            state.AddressW = TextureAddressMode.Clamp;
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
            state.AddressW = TextureAddressMode.Wrap;
            state.MakeImmutable();
            return state;
        }

        /// <summary>
        /// Applies the sampler state to the device.
        /// </summary>
        /// <param name="sampler">The sampler index on which to set the state.</param>
        /// <param name="target">GL_TEXTURE_2D or GL_TEXTURE_3D, as appropriate.</param>
        internal void Apply(Int32 sampler, UInt32 target)
        {
            if (Ultraviolet.GetGraphics().Capabilities.SupportsIndependentSamplerState)
                throw new InvalidOperationException(UltravioletStrings.GenericError);

            OpenGLState.ActiveTexture((uint)(GL.GL_TEXTURE0 + sampler));

            if (Ultraviolet.GetGraphics().Capabilities.Supports3DTextures)
            {
                GL.TexParameteri(target, GL.GL_TEXTURE_WRAP_R, GetTextureAddressModeGL(AddressW));
                GL.ThrowIfError();
            }

            GL.TexParameteri(target, GL.GL_TEXTURE_WRAP_S, GetTextureAddressModeGL(AddressU));
            GL.ThrowIfError();

            GL.TexParameteri(target, GL.GL_TEXTURE_WRAP_T, GetTextureAddressModeGL(AddressV));
            GL.ThrowIfError();

            if (MipMapLevelOfDetailBias != 0)
            {
                GL.ThrowIfGLES(OpenGLStrings.UnsupportedLODBiasGLES);

                GL.TexParameterf(target, GL.GL_TEXTURE_LOD_BIAS, MipMapLevelOfDetailBias);
                GL.ThrowIfError();
            }

            switch (Filter)
            {
                case TextureFilter.Point:
                    GL.TexParameterf(target, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                    GL.ThrowIfError();

                    GL.TexParameteri(target, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_NEAREST);
                    GL.ThrowIfError();

                    GL.TexParameteri(target, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_NEAREST);
                    GL.ThrowIfError();
                    break;

                case TextureFilter.Linear:
                    if (GL.IsAnisotropicFilteringAvailable)
                    {
                        GL.TexParameterf(target, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                        GL.ThrowIfError();
                    }

                    GL.TexParameteri(target, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TexParameteri(target, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();
                    break;

                case TextureFilter.Anisotropic:
                    if (GL.IsAnisotropicFilteringAvailable)
                    {
                        GL.TexParameterf(target, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, Math.Min(1f, MaxAnisotropy));
                        GL.ThrowIfError();
                    }

                    GL.TexParameteri(target, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TexParameteri(target, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();
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
        internal static Int32 GetTextureAddressModeGL(TextureAddressMode mode)
        {
            switch (mode)
            {
                case TextureAddressMode.Clamp:
                    return (int)GL.GL_CLAMP_TO_EDGE;
                case TextureAddressMode.Wrap:
                    return (int)GL.GL_REPEAT;
                case TextureAddressMode.Mirror:
                    return (int)GL.GL_MIRRORED_REPEAT;
            }
            throw new NotSupportedException();
        }
    }
}
