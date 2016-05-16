using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
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

            uv.QueueWorkItemAndWait(() =>
            {
                sampler = gl.GenSampler();
                gl.ThrowIfError();
            });

            this.sampler = sampler;
        }

        /// <summary>
        /// Applies the specified sampler state to this sampler.
        /// </summary>
        /// <param name="state">The sampler state to apply.</param>
        public void ApplySamplerState(SamplerState state)
        {
            Contract.Require(state, nameof(state));

            gl.SamplerParameteri(sampler, gl.GL_TEXTURE_WRAP_S, OpenGLSamplerState.GetTextureAddressModeGL(state.AddressU));
            gl.ThrowIfError();

            gl.SamplerParameteri(sampler, gl.GL_TEXTURE_WRAP_T, OpenGLSamplerState.GetTextureAddressModeGL(state.AddressV));
            gl.ThrowIfError();

            if (gl.IsGLES)
            {
                if (state.MipMapLevelOfDetailBias != 0)
                    throw new NotSupportedException(OpenGLStrings.UnsupportedLODBiasGLES);
            }
            else
            {
                gl.SamplerParameterf(sampler, gl.GL_TEXTURE_LOD_BIAS, state.MipMapLevelOfDetailBias);
                gl.ThrowIfError();
            }

            switch (state.Filter)
            {
                case TextureFilter.Point:
                    gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                    gl.ThrowIfError();

                    gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_NEAREST);
                    gl.ThrowIfError();

                    gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_NEAREST);
                    gl.ThrowIfError();
                    break;

                case TextureFilter.Linear:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, 1f);
                        gl.ThrowIfError();
                    }

                    gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();
                    break;

                case TextureFilter.Anisotropic:
                    if (gl.IsAnisotropicFilteringAvailable)
                    {
                        gl.SamplerParameterf(sampler, gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, Math.Min(1f, state.MaxAnisotropy));
                        gl.ThrowIfError();
                    }

                    gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.SamplerParameteri(sampler, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();
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
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return sampler;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteSampler(((OpenGLSamplerObject)state).sampler);
                        gl.ThrowIfError();
                    }, this);
                }
            }

            base.Dispose(disposing);
        }

        // Property values.
        private UInt32 sampler;
    }
}
