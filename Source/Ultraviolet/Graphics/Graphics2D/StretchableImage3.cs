using System;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a stretchable 2D image with three segments.
    /// </summary>
    public sealed partial class StretchableImage3 : StretchableImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        private StretchableImage3()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Int32 left, Int32 right)
        {
            return Create(texture, 0, 0, 0, 0, left, right);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 right)
        {
            var img = new StretchableImage3();
            img.TextureID = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left = left;
            img.Right = right;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Rectangle textureRegion, Int32 left, Int32 right)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, right);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(Texture2D texture, Int32 left, Int32 right)
        {
            return Create(texture, 0, 0, 0, 0, left, right);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(Texture2D texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 right)
        {
            var img = new StretchableImage3();
            img.Texture = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left = left;
            img.Right = right;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(Texture2D texture, Rectangle textureRegion, Int32 left, Int32 right)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, right);
        }

        /// <summary>
        /// Gets the distance in pixels between the left edge of the image and the left edge of the image's center segment.
        /// </summary>
        public Int32 Left
        {
            get { return left; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                left = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <summary>
        /// Gets the distance in pixels between the right edge of the image and the right edge of the image's center segment.
        /// </summary>
        public Int32 Right
        {
            get { return right; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                right = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this image is rendered vertically.
        /// </summary>
        public Boolean Vertical
        {
            get { return vertical; }
            set
            {
                vertical = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            effects |= SpriteEffects.OriginRelativeToDestination;

            if (vertical)
            {
                DrawVertical<VertexType, SpriteData>(spriteBatch, position, width, height, color, rotation, origin, effects, layerDepth, data);
            }
            else
            {
                DrawHorizontal<VertexType, SpriteData>(spriteBatch, position, width, height, color, rotation, origin, effects, layerDepth, data);
            }
        }

        /// <summary>
        /// Draws vertical images.
        /// </summary>
        private void DrawVertical<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
            where VertexType : struct, IVertexType
            where SpriteData : struct
        {
            var tilingData = new TilingData()
            {
                Texture = this.Texture,
                Color = color,
                Rotation = rotation,
                Origin = origin,
                Effects = effects,
                LayerDepth = layerDepth
            };

            var srcTop = this.Left;
            var srcBottom = this.Right;
            var dstTop = srcTop;
            var dstBottom = srcBottom;

            if (height < MinimumRecommendedSize.Height)
            {
                var scale = height / MinimumRecommendedSize.Height;
                dstTop = (Int32)Math.Floor(dstTop * scale);
                dstBottom = (Int32)Math.Ceiling(dstBottom * scale);
            }

            var srcStretchableWidth = this.TextureRegion.Width;
            var srcStretchableHeight = this.TextureRegion.Height - (srcTop + srcBottom);

            var dstStretchableWidth = width;
            var dstStretchableHeight = height - (dstTop + dstBottom);

            // Center
            var centerSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top + srcTop, srcStretchableWidth, srcStretchableHeight);
            var centerRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstStretchableHeight);
            var centerPosition = new Vector2(0, dstTop);
            if (this.TileCenter)
            {
                tilingData.Position = centerPosition;
                tilingData.DestinationRectangle = centerRegion;
                tilingData.SourceRectangle = centerSource;
                TileImageSegment(spriteBatch, TilingMode.Vertical, ref tilingData, data);
            }
            else
            {
                var centerOrigin = origin - centerPosition;
                spriteBatch.Draw(this.Texture, centerRegion, centerSource, color, rotation, centerOrigin, effects, layerDepth, data);
            }

            // Edges
            var topSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top, srcStretchableWidth, srcTop);
            var topRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstTop);
            var topPosition = new Vector2(0, 0);

            var bottomSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Bottom - srcBottom, srcStretchableWidth, srcBottom);
            var bottomRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstBottom);
            var bottomPosition = new Vector2(0, height - dstBottom);

            if (this.TileEdges)
            {
                tilingData.Position = topPosition;
                tilingData.DestinationRectangle = topRegion;
                tilingData.SourceRectangle = topSource;
                TileImageSegment(spriteBatch, TilingMode.Vertical, ref tilingData, data);

                tilingData.Position = centerPosition;
                tilingData.DestinationRectangle = centerRegion;
                tilingData.SourceRectangle = centerSource;
                TileImageSegment(spriteBatch, TilingMode.Vertical, ref tilingData, data);
            }
            else
            {
                var topOrigin = origin - topPosition;
                spriteBatch.Draw(this.Texture, topRegion, topSource, color, rotation, topOrigin, effects, layerDepth, data);
                var bottomOrigin = origin - bottomPosition;
                spriteBatch.Draw(this.Texture, bottomRegion, bottomSource, color, rotation, bottomOrigin, effects, layerDepth, data);
            }
        }

        /// <summary>
        /// Draws horizontal images.
        /// </summary>
        private void DrawHorizontal<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
            where VertexType : struct, IVertexType
            where SpriteData : struct
        {
            var tilingData = new TilingData()
            {
                Texture = this.Texture,
                Color = color,
                Rotation = rotation,
                Origin = origin,
                Effects = effects,
                LayerDepth = layerDepth
            };

            var texture = this.Texture;

            var srcLeft = this.Left;
            var srcRight = this.Right;
            var dstLeft = srcLeft;
            var dstRight = srcRight;

            if (width < MinimumRecommendedSize.Width)
            {
                var scale = width / MinimumRecommendedSize.Width;
                dstLeft = (Int32)Math.Floor(dstLeft * scale);
                dstRight = (Int32)Math.Ceiling(dstRight * scale);
            }

            var srcStretchableWidth = this.TextureRegion.Width - (srcLeft + srcRight);
            var srcStretchableHeight = this.TextureRegion.Height;

            var dstStretchableWidth = width - (dstLeft + dstRight);
            var dstStretchableHeight = height;

            // Center
            var centerSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Top, srcStretchableWidth, srcStretchableHeight);
            var centerRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstStretchableHeight);
            var centerPosition = new Vector2(dstLeft, 0);
            if (this.TileCenter)
            {
                tilingData.Position = centerPosition;
                tilingData.DestinationRectangle = centerRegion;
                tilingData.SourceRectangle = centerSource;
                TileImageSegment(spriteBatch, TilingMode.Horizontal, ref tilingData, data);
            }
            else
            {
                var centerOrigin = origin - centerPosition;
                spriteBatch.Draw(texture, centerRegion, centerSource, color, rotation, centerOrigin, effects, layerDepth, data);
            }

            // Edges
            var leftSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top, srcLeft, srcStretchableHeight);
            var leftRegion = new RectangleF(position.X, position.Y, dstLeft, dstStretchableHeight);
            var leftPosition = new Vector2(0, 0);

            var rightSource = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Top, srcRight, srcStretchableHeight);
            var rightRegion = new RectangleF(position.X, position.Y, dstRight, dstStretchableHeight);
            var rightPosition = new Vector2(width - dstRight, 0);

            if (this.TileEdges)
            {
                tilingData.Position = leftPosition;
                tilingData.DestinationRectangle = leftRegion;
                tilingData.SourceRectangle = leftSource;
                TileImageSegment(spriteBatch, TilingMode.Horizontal, ref tilingData, data);

                tilingData.Position = rightPosition;
                tilingData.DestinationRectangle = rightRegion;
                tilingData.SourceRectangle = rightSource;
                TileImageSegment(spriteBatch, TilingMode.Horizontal, ref tilingData, data);
            }
            else
            {
                var leftOrigin = origin - leftPosition;
                spriteBatch.Draw(texture, leftRegion, leftSource, color, rotation, leftOrigin, effects, layerDepth, data);
                var rightOrigin = origin - rightPosition;
                spriteBatch.Draw(texture, rightRegion, rightSource, color, rotation, rightOrigin, effects, layerDepth, data);
            }
        }

        /// <summary>
        /// Parses a tiling parameter included in a string which represents a stretchable image.
        /// </summary>
        /// <param name="parameter">The parameter string to parse.</param>
        /// <param name="tileCenter">A value indicating whether the image is set to tile its center piece.</param>
        /// <param name="tileEdges">A value indicating whether the image is set to tile its edges.</param>
        /// <param name="vertical">A value indicating whether the image is rendered veritcally.</param>
        /// <returns><see langword="true"/> if the parameter was parsed successfully; otherwise, <see langword="false"/>.</returns>
        private static Boolean ParseTilingParameter3(String parameter, ref Boolean tileCenter, ref Boolean tileEdges, ref Boolean vertical)
        {
            if (String.Equals(parameter, "vertical", StringComparison.OrdinalIgnoreCase))
            {
                if (vertical)
                {
                    return false;
                }
                vertical = true;
                return true;
            }
            return ParseTilingParameter(parameter, ref tileCenter, ref tileEdges);
        }

        /// <summary>
        /// Updates the value of the <see cref="TextureImage.MinimumRecommendedSize"/> property.
        /// </summary>
        private void UpdateMinimumRecommendedSize()
        {
            if (vertical)
            {
                MinimumRecommendedSize = new Size2(0, left + right);
            }
            else
            {
                MinimumRecommendedSize = new Size2(left + right, 0);
            }
        }

        // Property values.
        private Int32 left;
        private Int32 right;
        private Boolean vertical;
    }
}