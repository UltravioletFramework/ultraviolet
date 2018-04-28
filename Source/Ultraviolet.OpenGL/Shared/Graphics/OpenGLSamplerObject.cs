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
                sampler = gl.GenSampler();
                gl.ThrowIfError();
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
                gl.SamplerParameteri(sampler, gl.GL_TEXTURE_WRAP_R, textureWrapR);
                gl.ThrowIfError();
            }

            var textureWrapS = OpenGLSamplerState.GetTextureAddressModeGL(state.AddressU);
            if (textureWrapS != cachedTextureWrapS)
            {
                cachedTextureWrapS = textureWrapS;
                gl.SamplerParameteri(sampler, gl.GL_TEXTURE_WRAP_S, textureWrapS);
                gl.ThrowIfError();
            }

            var textureWrapT = OpenGLSamplerState.GetTextureAddressModeGL(state.AddressV);
            if (textureWrapT != cachedTextureWrapT)
            {
                cachedTextureWrapT = textureWrapT;
                gl.SamplerParameteri(sampler, gl.GL_TEXTURE_WRAP_T, textureWrapT);
                gl.ThrowIfError();
            }

            if (state.MipMapLevelOfDetailBias != 0)
            {
                if (gl.IsMapMapLevelOfDetailBiasAvailable)
                {
                    if (cachedMipMapLODBias != state.MipMapLevelOfDetailBias)
                    {
                        cachedMipMapLODBias = state.MipMapLevelOfDetailBias;
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_LOD_BIAS, state.MipMapLevelOfDetailBias);
                        gl.ThrowIfError();
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
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                        gl.ThrowIfError();
                    }

                    if (cachedMinFilter != gl.GL_NEAREST)
                    {
                        cachedMinFilter = gl.GL_NEAREST;
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_NEAREST);
                        gl.ThrowIfError();
                    }

                    if (cachedMagFilter != gl.GL_NEAREST)
                    {
                        cachedMagFilter = gl.GL_NEAREST;
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_NEAREST);
                        gl.ThrowIfError();
                    }
                    break;

                case TextureFilter.Linear:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        if (cachedMaxAnisotropy != 1f)
                        {
                            cachedMaxAnisotropy = 1f;
                            gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                            gl.ThrowIfError();
                        }
                    }

                    if (cachedMinFilter != gl.GL_LINEAR)
                    {
                        cachedMinFilter = gl.GL_LINEAR;
                        gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                        gl.ThrowIfError();
                    }

                    if (cachedMagFilter != gl.GL_LINEAR)
                    {
                        cachedMagFilter = gl.GL_LINEAR;
                        gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                        gl.ThrowIfError();
                    }
                    break;

                case TextureFilter.Anisotropic:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        var maxAnisotropy = Math.Min(1f, state.MaxAnisotropy);
                        if (maxAnisotropy != cachedMaxAnisotropy)
                        {
                            cachedMaxAnisotropy = maxAnisotropy;
                            gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy);
                            gl.ThrowIfError();
                        }
                    }

                    if (cachedMinFilter != gl.GL_LINEAR)
                    {
                        cachedMinFilter = gl.GL_LINEAR;
                        gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                        gl.ThrowIfError();
                    }

                    if (cachedMagFilter != gl.GL_LINEAR)
                    {
                        cachedMagFilter = gl.GL_LINEAR;
                        gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                        gl.ThrowIfError();
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

            gl.BindSampler(unit, sampler);
            gl.ThrowIfError();
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
                        gl.DeleteSampler(glname);
                        gl.ThrowIfError();
                    }, null, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                sampler = 0;
            }

            base.Dispose(disposing);
        }

        // Property values.
        private UInt32 sampler;

        // Cached state values.
        private Int32 cachedTextureWrapR = (int)gl.GL_REPEAT;
        private Int32 cachedTextureWrapS = (int)gl.GL_REPEAT;
        private Int32 cachedTextureWrapT = (int)gl.GL_REPEAT;
        private Single cachedMipMapLODBias = 0.0f;
        private Single cachedMaxAnisotropy = 1.0f;
        private UInt32 cachedMinFilter = gl.GL_NEAREST_MIPMAP_LINEAR;
        private UInt32 cachedMagFilter = gl.GL_LINEAR;
    }
}
