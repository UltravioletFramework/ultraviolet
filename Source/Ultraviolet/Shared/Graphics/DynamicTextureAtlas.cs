using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="DynamicTextureAtlas"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The width of the texture atlas in pixels.</param>
    /// <param name="height">The height of the texture atlas in pixels.</param>
    /// <param name="spacing">The number of pixels between cells on the texture atlas.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <returns>The instance of <see cref="DynamicTextureAtlas"/> that was created.</returns>
    public delegate DynamicTextureAtlas DynamicTextureAtlasFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 spacing, TextureOptions options);

    /// <summary>
    /// Represents a texture atlas which can be built dynamically at runtime.
    /// </summary>
    public sealed partial class DynamicTextureAtlas : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTextureAtlas"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The width of the texture atlas in pixels.</param>
        /// <param name="height">The height of the texture atlas in pixels.</param>
        /// <param name="spacing">The number of pixels between cells on the texture atlas.</param>
        /// <param name="options">The texture's configuration options.</param>
        private DynamicTextureAtlas(UltravioletContext uv, Int32 width, Int32 height, Int32 spacing, TextureOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(spacing >= 0, nameof(spacing));

            var isSrgb = (options & TextureOptions.SrgbColor) == TextureOptions.SrgbColor;
            var isLinear = (options & TextureOptions.LinearColor) == TextureOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.TextureCannotHaveMultipleEncodings);

            var caps = uv.GetGraphics().Capabilities;
            var srgbEncoded = (isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForTexture2D)) && caps.SrgbEncodingEnabled;
            var surfOptions = (srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor);

            this.IsFlipped = Ultraviolet.GetGraphics().Capabilities.FlippedTextures;

            this.Width = width;
            this.Height = height;
            this.Spacing = spacing;
            this.Surface = Surface2D.Create(width, height, surfOptions);
            this.Texture = Texture2D.CreateDynamicTexture(width, height, options, this, (dt2d, state) =>
            {
                ((DynamicTextureAtlas)state).Flush();
            });

            Clear(true);
            Invalidate();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DynamicTextureAtlas"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="spacing">The number of pixels between cells on the texture atlas.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of <see cref="DynamicTextureAtlas"/> that was created.</returns>
        public static DynamicTextureAtlas Create(Int32 width, Int32 height, Int32 spacing, TextureOptions options = TextureOptions.Default)
        {
            var uv = UltravioletContext.DemandCurrent();
            return new DynamicTextureAtlas(uv, width, height, spacing, options);
        }

        /// <summary>
        /// Implicitly converts a <see cref="DynamicTextureAtlas"/> to its underlying <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="atlas">The <see cref="DynamicTextureAtlas"/> to convert.</param>
        /// <returns>The underlying <see cref="Texture2D"/> represented by <paramref name="atlas"/>.</returns>
        public static implicit operator Texture2D(DynamicTextureAtlas atlas) => atlas?.Texture;

        /// <summary>
        /// Invalidates the texture atlas, forcing its contents to be re-uploaded to the GPU.
        /// </summary>
        public void Invalidate()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.IsDirty = true;
        }

        /// <summary>
        /// Flushes the contents of the texture atlas to GPU memory.
        /// </summary>
        public void Flush()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (this.IsDirty)
            {
                this.IsDirty = false;
                this.Texture.SetData(this.Surface);
            }
        }

        /// <summary>
        /// Clears the atlas to its default state.
        /// </summary>
        /// <param name="erase">A value indicating whether the atlas texture should be completely erased.</param>
        public void Clear(Boolean erase = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            rowHeight = 0;

            x = Spacing;
            y = Spacing;

            this.pendingErasure = erase;
            this.IsDirty = true;
        }

        /// <summary>
        /// Attempts to reserve a space on the texture atlas for a new cell with the specified dimensions.
        /// </summary>
        /// <param name="width">The width in pixels of the cell to reserve.</param>
        /// <param name="height">The height in pixels of the cell to reserve.</param>
        /// <param name="result">A <see cref="Reservation"/> which represents the cell.</param>
        /// <returns><see langword="true"/> if the cell was reserved successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean TryReserveCell(Int32 width, Int32 height, out Reservation result)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var paddedWidth = width + Spacing;

            if (x + paddedWidth > Width)
            {
                x = Spacing;
                y = y + rowHeight + Spacing;
            }

            var paddedHeight = height + Spacing;

            if (y + paddedHeight > Height)
            {
                result = default(Reservation);
                return false;
            }

            var resX = x;
            var resY = IsFlipped ? Height - (y + height) : y;
            result = new Reservation(this, resX, resY, width, height);

            x = x + width + Spacing;
            rowHeight = (rowHeight > height) ? rowHeight : height;

            Invalidate();

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the atlas' surface data is flipped vertically.
        /// </summary>
        public Boolean IsFlipped { get; }

        /// <summary>
        /// Gets a value indicating whether the atlas contains data which has not been uploaded to the GPU.
        /// </summary>
        public Boolean IsDirty { get; private set; }

        /// <summary>
        /// Gets the width of the texture atlas in pixels.
        /// </summary>
        public Int32 Width { get; }

        /// <summary>
        /// Gets the height of the texture atlas in pixels.
        /// </summary>
        public Int32 Height { get; }

        /// <summary>
        /// Gets the number of pixels between cells on the texture atlas.
        /// </summary>
        public Int32 Spacing { get; }

        /// <summary>
        /// Gets the <see cref="Surface2D"/> which represents the atlas' pixel data.
        /// </summary>
        public Surface2D Surface { get; }

        /// <summary>
        /// Gets the <see cref="Texture2D"/> which represents the atlas.
        /// </summary>
        public Texture2D Texture { get; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Surface.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Erases the atlas' surface, if necessary.
        /// </summary>
        private void Erase()
        {
            if (pendingErasure)
            {
                pendingErasure = false;
                Surface.Clear(Color.Transparent);
            }
        }

        // Positioning values.
        private Int32 x;
        private Int32 y;
        private Int32 rowHeight;
        private Boolean pendingErasure;
    }
}
