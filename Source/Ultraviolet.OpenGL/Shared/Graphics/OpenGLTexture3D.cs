using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="Texture3D"/> class.
    /// </summary>
    public unsafe sealed class OpenGLTexture3D : Texture3D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="layers">A pointer to the raw pixel data for each of the texture's layers.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
        public OpenGLTexture3D(UltravioletContext uv, IList<IntPtr> layers, Int32 width, Int32 height, Int32 bytesPerPixel)
            : base(uv)
        {
            Contract.Require(layers, nameof(layers));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(bytesPerPixel == 4, nameof(bytesPerPixel));

            this.width = width;
            this.height = height;
            this.depth = layers.Count;
        }

        /// <inheritdoc/>
        public void BindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite == 0, OpenGLStrings.ResourceCannotBeReadWhileWriting);

            boundRead++;
        }

        /// <inheritdoc/>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead == 0, OpenGLStrings.ResourceCannotBeWrittenWhileReading);

            boundWrite++;
        }

        /// <inheritdoc/>
        public void UnbindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead > 0, OpenGLStrings.ResourceNotBound);

            boundRead--;
        }

        /// <inheritdoc/>
        public void UnbindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite > 0, OpenGLStrings.ResourceNotBound);

            boundWrite--;
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return height;
            }
        }
        
        /// <inheritdoc/>
        public override Int32 Depth
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return depth;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForReading
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundRead > 0;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForWriting
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundWrite > 0;
            }
        }

        /// <inheritdoc/>
        public override Boolean ImmutableStorage
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return immutable;
            }
        }

        /// <inheritdoc/>
        public override Boolean WillNotBeSampled
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return false;
            }
        }

        /// <inheritdoc/>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return texture;
            }
        }
        
        /// <summary>
        /// Creates the underlying native OpenGL texture with the specified format and data.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texel format.</param>
        /// <param name="type">The texel data type.</param>
        /// <param name="pixels">A pointer to the beginning of the texture's pixel data.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        private void CreateNativeTexture(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, void* pixels, Boolean immutable)
        {
            if (uv.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);

            this.width = width;
            this.height = height;
            this.internalformat = internalformat;
            this.format = format;
            this.type = type;
            this.immutable = immutable;

            this.texture = uv.QueueWorkItemAndWait(() =>
            {
                uint glname;

                using (OpenGLState.ScopedCreateTexture3D(out glname))
                {
                    if (!gl.IsGLES2)
                    {
                        gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MAX_LEVEL, 0);
                        gl.ThrowIfError();
                    }

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_WRAP_S, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_WRAP_T, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    if (immutable)
                    {
                        if (gl.IsTextureStorageAvailable)
                        {
                            gl.TextureStorage3D(glname, gl.GL_TEXTURE_3D, 1, internalformat, width, height, depth);
                            gl.ThrowIfError();

                            if (pixels != null)
                            {
                                gl.TextureSubImage3D(glname, gl.GL_TEXTURE_3D, 0, 0, 0, 0, width, height, depth, format, type, pixels);
                                gl.ThrowIfError();
                            }
                        }
                        else
                        {
                            gl.TextureImage3D(glname, gl.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                            gl.ThrowIfError();
                        }
                    }
                }

                if (!immutable)
                {
                    using (OpenGLState.ScopedBindTexture3D(glname, true))
                    {
                        gl.TexImage3D(gl.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                        gl.ThrowIfError();
                    }
                }

                return glname;
            });
        }

        // Property values.
        private UInt32 texture;
        private Int32 width;
        private Int32 height;
        private Int32 depth;
        private UInt32 internalformat;
        private UInt32 format;
        private UInt32 type;
        private Boolean immutable;
        private Int32 boundRead;
        private Int32 boundWrite;
    }
}
