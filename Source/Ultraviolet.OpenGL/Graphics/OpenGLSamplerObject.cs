using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents an OpenGL Sampler Object.
    /// </summary>
    public sealed class OpenGLSamplerObject : UltravioletResource, IOpenGLResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLSamplerObject"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLSamplerObject(UltravioletContext uv) 
            : base(uv)
        {
            var sampler = 0u;

            uv.QueueWorkItem(state =>
            {
                sampler = GL.GenSampler();
                GL.ThrowIfError();
            }).Wait();

            this.sampler = sampler;
        }

        /// <summary>
        /// Applies the specified sampler state to this sampler.
        /// </summary>
        /// <param name="state">The sampler state to apply.</param>
        public void ApplySamplerState(SamplerState state)
        {
            Contract.Require(state, nameof(state));

            var textureWrapR = OpenGLSamplerState.GetTextureAddressModeGL(state.AddressW);
            if (textureWrapR != cachedTextureWrapR)
            {
                cachedTextureWrapR = textureWrapR;
                GL.SamplerParameteri(sampler, GL.GL_TEXTURE_WRAP_R, textureWrapR);
                GL.ThrowIfError();
            }

            var textureWrapS = OpenGLSamplerState.GetTextureAddressModeGL(state.AddressU);
            if (textureWrapS != cachedTextureWrapS)
            {
                cachedTextureWrapS = textureWrapS;
                GL.SamplerParameteri(sampler, GL.GL_TEXTURE_WRAP_S, textureWrapS);
                GL.ThrowIfError();
            }

            var textureWrapT = OpenGLSamplerState.GetTextureAddressModeGL(state.AddressV);
            if (textureWrapT != cachedTextureWrapT)
            {
                cachedTextureWrapT = textureWrapT;
                GL.SamplerParameteri(sampler, GL.GL_TEXTURE_WRAP_T, textureWrapT);
                GL.ThrowIfError();
            }

            if (state.MipMapLevelOfDetailBias != 0)
            {
                if (GL.IsMapMapLevelOfDetailBiasAvailable)
                {
                    if (cachedMipMapLODBias != state.MipMapLevelOfDetailBias)
                    {
                        cachedMipMapLODBias = state.MipMapLevelOfDetailBias;
                        GL.SamplerParameterf(sampler, GL.GL_TEXTURE_LOD_BIAS, state.MipMapLevelOfDetailBias);
                        GL.ThrowIfError();
                    }
                }
                else throw new NotSupportedException(OpenGLStrings.UnsupportedLODBiasGLES);
            }

            switch (state.Filter)
            {
                case TextureFilter.Point:
                    if (cachedMaxAnisotropy != 1f)
                    {
                        cachedMaxAnisotropy = 1f;
                        GL.SamplerParameterf(sampler, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                        GL.ThrowIfError();
                    }

                    if (cachedMinFilter != GL.GL_NEAREST)
                    {
                        cachedMinFilter = GL.GL_NEAREST;
                        GL.SamplerParameterf(sampler, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_NEAREST);
                        GL.ThrowIfError();
                    }

                    if (cachedMagFilter != GL.GL_NEAREST)
                    {
                        cachedMagFilter = GL.GL_NEAREST;
                        GL.SamplerParameterf(sampler, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_NEAREST);
                        GL.ThrowIfError();
                    }
                    break;

                case TextureFilter.Linear:
                    if (GL.IsAnisotropicFilteringAvailable)
                    {
                        if (cachedMaxAnisotropy != 1f)
                        {
                            cachedMaxAnisotropy = 1f;
                            GL.SamplerParameterf(sampler, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                            GL.ThrowIfError();
                        }
                    }

                    if (cachedMinFilter != GL.GL_LINEAR)
                    {
                        cachedMinFilter = GL.GL_LINEAR;
                        GL.SamplerParameteri(sampler, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                        GL.ThrowIfError();
                    }

                    if (cachedMagFilter != GL.GL_LINEAR)
                    {
                        cachedMagFilter = GL.GL_LINEAR;
                        GL.SamplerParameteri(sampler, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                        GL.ThrowIfError();
                    }
                    break;

                case TextureFilter.Anisotropic:
                    if (GL.IsAnisotropicFilteringAvailable)
                    {
                        var maxAnisotropy = Math.Min(1f, state.MaxAnisotropy);
                        if (maxAnisotropy != cachedMaxAnisotropy)
                        {
                            cachedMaxAnisotropy = maxAnisotropy;
                            GL.SamplerParameterf(sampler, GL.GL_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy);
                            GL.ThrowIfError();
                        }
                    }

                    if (cachedMinFilter != GL.GL_LINEAR)
                    {
                        cachedMinFilter = GL.GL_LINEAR;
                        GL.SamplerParameteri(sampler, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                        GL.ThrowIfError();
                    }

                    if (cachedMagFilter != GL.GL_LINEAR)
                    {
                        cachedMagFilter = GL.GL_LINEAR;
                        GL.SamplerParameteri(sampler, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                        GL.ThrowIfError();
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Binds the sampler object to the specified texture unit.
        /// </summary>
        /// <param name="unit">The texture unit to which to bind the sampler object.</param>
        public void Bind(UInt32 unit)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            GL.BindSampler(unit, sampler);
            GL.ThrowIfError();
        }

        /// <inheridoc/>
        public UInt32 OpenGLName => sampler;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                var glname = sampler;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        GL.DeleteSampler(glname);
                        GL.ThrowIfError();
                    }, null, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                sampler = 0;
            }

            base.Dispose(disposing);
        }

        // Property values.
        private UInt32 sampler;

        // Cached state values.
        private Int32 cachedTextureWrapR = (int)GL.GL_REPEAT;
        private Int32 cachedTextureWrapS = (int)GL.GL_REPEAT;
        private Int32 cachedTextureWrapT = (int)GL.GL_REPEAT;
        private Single cachedMipMapLODBias = 0.0f;
        private Single cachedMaxAnisotropy = 1.0f;
        private UInt32 cachedMinFilter = GL.GL_NEAREST_MIPMAP_LINEAR;
        private UInt32 cachedMagFilter = GL.GL_LINEAR;
    }
}
