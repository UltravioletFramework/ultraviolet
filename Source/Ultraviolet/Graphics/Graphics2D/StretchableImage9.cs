using System;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a stretchable 2D image with nine segments.
    /// </summary>
    public sealed partial class StretchableImage9 : StretchableImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        private StretchableImage9()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, 0, 0, 0, 0, left, top, right, bottom);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            var img = new StretchableImage9();
            img.TextureID = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left = left;
            img.Top = top;
            img.Right = right;
            img.Bottom = bottom;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Rectangle textureRegion, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, top, right, bottom);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(Texture2D texture, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, 0, 0, 0, 0, left, top, right, bottom);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(Texture2D texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            var img = new StretchableImage9();
            img.Texture = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left = left;
            img.Top = top;
            img.Right = right;
            img.Bottom = bottom;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(Texture2D texture, Rectangle textureRegion, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, top, right, bottom);
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
        /// Gets the distance in pixels between the top edge of the image and the top edge of the image's center segment.
        /// </summary>
        public Int32 Top
        {
            get { return top; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                top = value;
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
        /// Gets the distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.
        /// </summary>
        public Int32 Bottom
        {
            get { return bottom; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                bottom = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
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

            effects |= SpriteEffects.OriginRelativeToDestination;

            var texture = this.Texture;

            var srcLeft = this.Left;
            var srcTop = this.Top;
            var srcRight = this.Right;
            var srcBottom = this.Bottom;

            var dstLeft = srcLeft;
            var dstTop = srcTop;
            var dstRight = srcRight;
            var dstBottom = srcBottom;

            if (width < MinimumRecommendedSize.Width)
            {
                var scale = width / MinimumRecommendedSize.Width;
                dstLeft = (Int32)Math.Floor(dstLeft * scale);
                dstRight = (Int32)Math.Ceiling(dstRight * scale);
            }
            if (height < MinimumRecommendedSize.Height)
            {
                var scale = height / MinimumRecommendedSize.Height;
                dstTop = (Int32)Math.Floor(dstTop * scale);
                dstBottom = (Int32)Math.Ceiling(dstBottom * scale);
            }

            var srcStretchableWidth = this.TextureRegion.Width - (srcLeft + srcRight);
            var srcStretchableHeight = this.TextureRegion.Height - (srcTop + srcBottom);

            var dstStretchableWidth = width - (dstLeft + dstRight);
            var dstStretchableHeight = height - (dstTop + dstBottom);

            // Center
            var centerSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Top + srcTop, srcStretchableWidth, srcStretchableHeight);
            var centerRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstStretchableHeight);
            var centerPosition = new Vector2(dstLeft, dstTop);
            if (this.TileCenter)
            {
                tilingData.Position = centerPosition;
                tilingData.DestinationRectangle = centerRegion;
                tilingData.SourceRectangle = centerSource;
                TileImageSegment(spriteBatch, TilingMode.Both, ref tilingData, data);
            }
            else
            {
                var centerOrigin = origin - centerPosition;
                spriteBatch.Draw(texture, centerRegion, centerSource, color, rotation, centerOrigin, effects, layerDepth, data);
            }

            // Edges
            var leftSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top + srcTop, srcLeft, srcStretchableHeight);
            var leftRegion = new RectangleF(position.X, position.Y, dstLeft, dstStretchableHeight);
            var leftPosition = new Vector2(0, dstTop);

            var rightSource = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Top + srcTop, srcRight, srcStretchableHeight);
            var rightRegion = new RectangleF(position.X, position.Y, dstRight, dstStretchableHeight);
            var rightPosition = new Vector2(width - dstRight, dstTop);

            var topSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Top, srcStretchableWidth, srcTop);
            var topRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstTop);
            var topPosition = new Vector2(dstLeft, 0);

            var bottomSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Bottom - srcBottom, srcStretchableWidth, srcBottom);
            var bottomRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstBottom);
            var bottomPosition = new Vector2(dstLeft, height - dstBottom);

            if (this.TileEdges)
            {
                tilingData.Position = leftPosition;
                tilingData.DestinationRectangle = leftRegion;
                tilingData.SourceRectangle = leftSource;
                TileImageSegment(spriteBatch, TilingMode.Both, ref tilingData, data);

                tilingData.Position = rightPosition;
                tilingData.DestinationRectangle = rightRegion;
                tilingData.SourceRectangle = rightSource;
                TileImageSegment(spriteBatch, TilingMode.Both, ref tilingData, data);

                tilingData.Position = topPosition;
                tilingData.DestinationRectangle = topRegion;
                tilingData.SourceRectangle = topSource;
                TileImageSegment(spriteBatch, TilingMode.Both, ref tilingData, data);

                tilingData.Position = bottomPosition;
                tilingData.DestinationRectangle = bottomRegion;
                tilingData.SourceRectangle = bottomSource;
                TileImageSegment(spriteBatch, TilingMode.Both, ref tilingData, data);
            }
            else
            {
                var leftOrigin = origin - leftPosition;
                spriteBatch.Draw(texture, leftRegion, leftSource, color, rotation, leftOrigin, effects, layerDepth, data);
                var rightOrigin = origin - rightPosition;
                spriteBatch.Draw(texture, rightRegion, rightSource, color, rotation, rightOrigin, effects, layerDepth, data);
                var topOrigin = origin - topPosition;
                spriteBatch.Draw(texture, topRegion, topSource, color, rotation, topOrigin, effects, layerDepth, data);
                var bottomOrigin = origin - bottomPosition;
                spriteBatch.Draw(texture, bottomRegion, bottomSource, color, rotation, bottomOrigin, effects, layerDepth, data);
            }

            // Corners
            var cornerTLRegion = new RectangleF(position.X, position.Y, dstLeft, dstTop);
            var cornerTLPosition = new Vector2(0, 0);
            var cornerTLOrigin = origin - cornerTLPosition;
            var cornerTLSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top, srcLeft, srcTop);
            spriteBatch.Draw(texture, cornerTLRegion, cornerTLSource, color, rotation, cornerTLOrigin, effects, layerDepth, data);

            var cornerTRRegion = new RectangleF(position.X, position.Y, dstRight, dstTop);
            var cornerTRPosition = new Vector2(width - dstRight, 0);
            var cornerTROrigin = origin - cornerTRPosition;
            var cornerTRSource = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Top, srcRight, srcTop);
            spriteBatch.Draw(texture, cornerTRRegion, cornerTRSource, color, rotation, cornerTROrigin, effects, layerDepth, data);

            var cornerBLRegion = new RectangleF(position.X, position.Y, dstLeft, dstBottom);
            var cornerBLPosition = new Vector2(0, height - dstBottom);
            var cornerBLOrigin = origin - cornerBLPosition;
            var cornerBLSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Bottom - srcBottom, srcLeft, srcBottom);
            spriteBatch.Draw(texture, cornerBLRegion, cornerBLSource, color, rotation, cornerBLOrigin, effects, layerDepth, data);

            var cornerBRRegion = new RectangleF(position.X, position.Y, dstRight, dstBottom);
            var cornerBRPosition = new Vector2(width - dstRight, height - dstBottom);
            var cornerBROrigin = origin - cornerBRPosition;
            var cornerBRSource = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Bottom - srcBottom, srcLeft, srcBottom);
            spriteBatch.Draw(texture, cornerBRRegion, cornerBRSource, color, rotation, cornerBROrigin, effects, layerDepth, data);
        }

        /// <summary>
        /// Updates the value of the <see cref="TextureImage.MinimumRecommendedSize"/> property.
        /// </summary>
        private void UpdateMinimumRecommendedSize()
        {
            MinimumRecommendedSize = new Size2(left + right, top + bottom);
        }

        // Property values.
        private Int32 left;
        private Int32 top;
        private Int32 right;
        private Int32 bottom;
    }
}