using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="bytesPerPixel">The number of bytes used to represent a pixel on the surface.</param>
        /// <param name="options">The surface's configuration options.</param>
        public SDL2Surface3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel, SurfaceOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));
            Contract.EnsureRange(bytesPerPixel == 3 || bytesPerPixel == 4, nameof(bytesPerPixel));

            var isSrgb = (options & SurfaceOptions.SrgbColor) == SurfaceOptions.SrgbColor;
            var isLinear = (options & SurfaceOptions.LinearColor) == SurfaceOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.SurfaceCannotHaveMultipleEncodings);

            this.width = width;
            this.height = height;
            this.bytesPerPixel = bytesPerPixel;

            this.layers = new Surface2D[depth];
            this.layerOwnership = new Boolean[depth];

            this.SrgbEncoded = isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForSurface3D);
        }

        /// <inheritdoc/>
        public override Surface2D GetLayer(Int32 layer)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(layer >= 0, nameof(layer));

            return this.layers[layer];
        }

        /// <inheritdoc/>
        public override void SetLayer(Int32 layer, Surface2D surface, Boolean transferOwnership = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
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
            this.layerOwnership[layer] = transferOwnership;
            this.isComplete = !this.layers.Contains(null);
        }

        /// <inheritdoc/>
        public override void SetLayers(IEnumerable<Surface2D> surfaces, Boolean transferOwnership = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(surfaces, nameof(surfaces));

            SetLayers(surfaces, 0, transferOwnership);
        }

        /// <inheritdoc/>
        public override void SetLayers(IEnumerable<Surface2D> surfaces, Int32 offset, Boolean transferOwnership = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(surfaces, nameof(surfaces));

            var layers = surfaces.Skip(offset).Take(Depth).ToArray();
            if (layers.Length < Depth)
                throw new ArgumentException(CoreStrings.NotEnoughData.Format(nameof(surfaces)));

            for (int i = 0; i < Depth; i++)
                SetLayer(i, layers[i], transferOwnership);
        }

        /// <inheritdoc/>
        public override Surface3D CreateSurface()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var resultOpts = SrgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            var result = new SDL2Surface3D(Ultraviolet, Width, Height, Depth, BytesPerPixel, resultOpts);
            for (int i = 0; i < Depth; i++)
            {
                var layerCopy = this.GetLayer(i).CreateSurface();
                result.SetLayer(i, layerCopy, true);
            }
            return result;
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));
            Contract.Ensure(IsComplete, UltravioletStrings.SurfaceIsNotComplete);

            EnsureAllLayersMatchSrgbEncoding();

            var outputSurfaceOptions = SrgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            using (var outputSurface = Surface2D.Create(Width * Depth, Height, outputSurfaceOptions))
            {
                var position = 0;
                for (int i = 0; i < Depth; i++)
                {
                    GetLayer(i).Blit(outputSurface, new Rectangle(position, 0, Width, Height));
                    position += Width;
                }

                var surfaceSaver = SurfaceSaver.Create();
                surfaceSaver.SaveAsJpeg(outputSurface, stream);
            }
        }

        /// <inheritdoc/>
        public override void SaveAsPng(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));
            Contract.Ensure(IsComplete, UltravioletStrings.SurfaceIsNotComplete);

            EnsureAllLayersMatchSrgbEncoding();

            var outputSurfaceOptions = SrgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            using (var outputSurface = Surface2D.Create(Width * Depth, Height, outputSurfaceOptions))
            {
                var position = 0;
                for (int i = 0; i < Depth; i++)
                {
                    GetLayer(i).Blit(outputSurface, new Rectangle(position, 0, Width, Height));
                    position += Width;
                }

                var surfaceSaver = SurfaceSaver.Create();
                surfaceSaver.SaveAsPng(outputSurface, stream);
            }
        }

        /// <inheritdoc/>
        public override Boolean SrgbEncoded { get; set; }

        /// <inheritdoc/>
        public override Int32 Width => width;

        /// <inheritdoc/>
        public override Int32 Height => height;

        /// <inheritdoc/>
        public override Int32 Depth => layers.Length;

        /// <inheritdoc/>
        public override Int32 BytesPerPixel => bytesPerPixel;

        /// <inheritdoc/>
        public override Boolean IsComplete => isComplete;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < layers.Length; i++)
                {
                    if (layerOwnership[i] && layers[i] != null)
                    {
                        layers[i].Dispose();
                        layers[i] = null;
                        layerOwnership[i] = false;
                    }
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Throws an exception if any of the surface's layers don't match the current value 
        /// of the surface's <see cref="SrgbEncoded"/> property.
        /// </summary>
        private void EnsureAllLayersMatchSrgbEncoding()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].SrgbEncoded != SrgbEncoded)
                    throw new InvalidOperationException(UltravioletStrings.SurfaceLayerEncodingMismatch);
            }
        }

        // State values.
        private readonly Surface2D[] layers;
        private readonly Boolean[] layerOwnership;

        // Property values.
        private Int32 width;
        private Int32 height;
        private Int32 bytesPerPixel;
        private Boolean isComplete;
    }
}
