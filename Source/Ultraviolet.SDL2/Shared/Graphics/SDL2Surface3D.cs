using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="Surface3D"/> class.
    /// </summary>
    public unsafe sealed class SDL2Surface3D : Surface3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The surface's width in pixels.</param>
        /// <param name="height">The surface's height in pixels.</param>
        /// <param name="depth">The surface's depth in pixels.</param>
        /// <param name="bytesPerPixel">The number of bytes used to represent a pixel on this surface.</param>
        public SDL2Surface3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));
            Contract.EnsureRange(bytesPerPixel == 4, nameof(bytesPerPixel));

            this.width = width;
            this.height = height;
            this.bytesPerPixel = bytesPerPixel;

            this.layers = new Surface2D[depth];
        }

        /// <inheritdoc/>
        public override Surface2D GetLayer(Int32 layer)
        {
            Contract.EnsureRange(layer >= 0, nameof(layer));

            return this.layers[layer];
        }

        /// <inheritdoc/>
        public override void SetLayer(Int32 layer, Surface2D surface)
        {
            Contract.EnsureRange(layer >= 0, nameof(layer));

            if (surface != null)
            {
                if (surface.Width != Width)
                    throw new ArgumentException(UltravioletStrings.IncompatibleSurfaceLayer);
                if (surface.Height != Height)
                    throw new ArgumentException(UltravioletStrings.IncompatibleSurfaceLayer);
                if (surface.BytesPerPixel != BytesPerPixel)
                    throw new ArgumentException(UltravioletStrings.IncompatibleSurfaceLayer);
            }

            this.layers[layer] = surface;
            this.isComplete = !this.layers.Contains(null);
            this.isReadyForTextureExport = this.isComplete && this.layers.All(x => x.IsReadyForTextureExport);
        }

        /// <inheritdoc/>
        public override void SetLayers(IEnumerable<Surface2D> surfaces)
        {
            Contract.Require(surfaces, nameof(surfaces));

            SetLayers(surfaces, 0);
        }

        /// <inheritdoc/>
        public override void SetLayers(IEnumerable<Surface2D> surfaces, Int32 offset)
        {
            Contract.Require(surfaces, nameof(surfaces));

            var layers = surfaces.Skip(offset).Take(Depth).ToArray();
            if (layers.Length < Depth)
                throw new ArgumentException(CoreStrings.NotEnoughData.Format(nameof(surfaces)));

            for (int i = 0; i < Depth; i++)
                SetLayer(i, layers[i]);
        }

        /// <inheritdoc/>
        public override void PrepareForTextureExport(Boolean premultiply, Boolean flip)
        {
            foreach (var layer in layers)
            {
                if (!layer.IsReadyForTextureExport)
                    layer.PrepareForTextureExport(premultiply, flip);
            }

            this.isReadyForTextureExport = this.isComplete;
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

                return layers.Length;
            }
        }

        /// <inheritdoc/>
        public override Int32 BytesPerPixel
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return bytesPerPixel;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsComplete
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isComplete;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsReadyForTextureExport
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isReadyForTextureExport;
            }
        }

        // State values.
        private readonly Surface2D[] layers;

        // Property values.
        private Int32 width;
        private Int32 height;
        private Int32 bytesPerPixel;
        private Boolean isComplete;
        private Boolean isReadyForTextureExport;
    }
}
