using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a rectangle with single-precision floating point components.
    /// </summary>
    [Serializable]
    public partial struct RectangleF : IEquatable<RectangleF>, IInterpolatable<RectangleF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> class.
        /// </summary>
        /// <param name="position">The rectangle's position.</param>
        /// <param name="size">The rectangle's size.</param>
        public RectangleF(Point2F position, Size2F size)
            : this(position.X, position.Y, size.Width, size.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The y-coordinate of the rectangle's top-right corner.</param>
        /// <param name="width">The rectangle's width.</param>
        /// <param name="height">The rectangle's height.</param>
        [JsonConstructor]
        public RectangleF(Single x, Single y, Single width, Single height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="position">The position of the rectangle's top-left corner.</param>
        /// <param name="size">The area of the rectangle.</param>
        public RectangleF(Vector2 position, Size2F size)
            : this(position.X, position.Y, size.Width, size.Height)
        {

        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator +(RectangleF rect, Point2 point)
        {
            RectangleF result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator -(RectangleF rect, Point2 point)
        {
            RectangleF result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator +(RectangleF rect, Point2F point)
        {
            RectangleF result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator -(RectangleF rect, Point2F point)
        {
            RectangleF result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator +(RectangleF rect, Point2D point)
        {
            RectangleD result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator -(RectangleF rect, Point2D point)
        {
            RectangleD result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }
        
        /// <summary>
        /// Explicitly converts a <see cref="RectangleF"/> structure to a <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Rectangle(RectangleF rect)
        {
            Rectangle result;

            result.X = (Int32)rect.X;
            result.Y = (Int32)rect.Y;
            result.Width = (Int32)rect.Width;
            result.Height = (Int32)rect.Height;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="RectangleF"/> structure to a <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator RectangleD(RectangleF rect)
        {
            RectangleD result;

            result.X = rect.X;
            result.Y = rect.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }
        
        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <returns>The offset <see cref="RectangleF"/>.</returns>
        public static RectangleF Offset(RectangleF rectangle, Single offsetX, Single offsetY)
        {
            RectangleF result;

            result.X = rectangle.X + offsetX;
            result.Y = rectangle.Y + offsetY;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <param name="result">The offset <see cref="RectangleF"/>.</param>
        public static void Offset(ref RectangleF rectangle, Single offsetX, Single offsetY, out RectangleF result)
        {
            result.X = rectangle.X + offsetX;
            result.Y = rectangle.Y + offsetY;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <returns>The offset <see cref="RectangleF"/>.</returns>
        public static RectangleF Offset(RectangleF rectangle, Vector2 offset)
        {
            RectangleF result;

            result.X = rectangle.X + offset.X;
            result.Y = rectangle.Y + offset.Y;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <param name="result">The offset <see cref="RectangleF"/>.</param>
        public static void Offset(ref RectangleF rectangle, ref Vector2 offset, out RectangleF result)
        {
            result.X = rectangle.X + offset.X;
            result.Y = rectangle.Y + offset.Y;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <returns>The inflated <see cref="RectangleF"/>.</returns>
        public static RectangleF Inflate(RectangleF rectangle, Single horizontalAmount, Single verticalAmount)
        {
            RectangleF result;

            result.X = rectangle.X - horizontalAmount;
            result.Y = rectangle.Y - verticalAmount;
            result.Width = rectangle.Width + (2 * horizontalAmount);
            result.Height = rectangle.Height + (2 * verticalAmount);

            return result;
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <param name="result">The inflated <see cref="RectangleF"/>.</param>
        public static void Inflate(ref RectangleF rectangle, Single horizontalAmount, Single verticalAmount, out RectangleF result)
        {
            result.X = rectangle.X - horizontalAmount;
            result.Y = rectangle.Y - verticalAmount;
            result.Width = rectangle.Width + (2 * horizontalAmount);
            result.Height = rectangle.Height + (2 * verticalAmount);
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleF"/>.</param>
        /// <param name="rectangle2">The second <see cref="RectangleF"/>.</param>
        /// <returns>The <see cref="RectangleF"/> that was created.</returns>
        public static RectangleF Union(RectangleF rectangle1, RectangleF rectangle2)
        {
            var minLeft = rectangle1.Left < rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var minTop = rectangle1.Top < rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var maxRight = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var maxBottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            RectangleF result;

            result.X = minLeft;
            result.Y = minTop;
            result.Width = maxRight - minLeft;
            result.Height = maxBottom - minTop;

            return result;
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleF"/>.</param>
        /// <param name="rectangle2">The second <see cref="RectangleF"/>.</param>
        /// <param name="result">The <see cref="RectangleF"/> that was created.</param>
        public static void Union(ref RectangleF rectangle1, ref RectangleF rectangle2, out RectangleF result)
        {
            var minLeft = rectangle1.Left < rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var minTop = rectangle1.Top < rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var maxRight = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var maxBottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            result.X = minLeft;
            result.Y = minTop;
            result.Width = maxRight - minLeft;
            result.Height = maxBottom - minTop;
        }

        /// <summary>
        /// Creates a rectangle which represents the intersection between the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleF"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="RectangleF"/> to intersect.</param>
        /// <returns>The <see cref="RectangleF"/> that was created.</returns>
        public static RectangleF Intersect(RectangleF rectangle1, RectangleF rectangle2)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);
            if (isEmpty)
                return RectangleF.Empty;
            
            RectangleF result;

            result.X = maxLeft;
            result.Y = maxTop;
            result.Width = minRight - maxLeft;
            result.Height = minBottom - maxTop;

            return result;
        }

        /// <summary>
        /// Creates a rectangle which represents the intersection between the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleF"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="RectangleF"/> to intersect.</param>
        /// <param name="result">The <see cref="RectangleF"/> that was created.</param>
        public static void Intersect(ref RectangleF rectangle1, ref RectangleF rectangle2, out RectangleF result)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);
            if (isEmpty)
            {
                result = RectangleF.Empty;
            }
            else
            {
                result.X = maxLeft;
                result.Y = maxTop;
                result.Width = minRight - maxLeft;
                result.Height = minBottom - maxTop;
            }
        }

        /// <summary>
        /// Transforms the specified rectangle and retrieves the axis-aligned bounding box of the result.
        /// </summary>
        /// <param name="rectangle">The rectangle to transform.</param>
        /// <param name="transform">The transform matrix.</param>
        /// <returns>The axis-aligned bounding box of <paramref name="rectangle"/> after it has been transformed.</returns>
        public static RectangleF TransformAxisAligned(RectangleF rectangle, Matrix transform)
        {
            var tl = new Vector2(rectangle.Left, rectangle.Top);
            var tr = new Vector2(rectangle.Right, rectangle.Top);
            var bl = new Vector2(rectangle.Left, rectangle.Bottom);
            var br = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref tl, ref transform, out tl);
            Vector2.Transform(ref tr, ref transform, out tr);
            Vector2.Transform(ref bl, ref transform, out bl);
            Vector2.Transform(ref br, ref transform, out br);

            var minX = Math.Min(Math.Min(tl.X, tr.X), Math.Min(bl.X, br.X));
            var maxX = Math.Max(Math.Max(tl.X, tr.X), Math.Max(bl.X, br.X));
            var minY = Math.Min(Math.Min(tl.Y, tr.Y), Math.Min(bl.Y, br.Y));
            var maxY = Math.Max(Math.Max(tl.Y, tr.Y), Math.Max(bl.Y, br.Y));

            RectangleF result;

            result.X = minX;
            result.Y = minY;
            result.Width = maxX - minX;
            result.Height = maxY - minY;

            return result;
        }

        /// <summary>
        /// Transforms the specified rectangle and retrieves the axis-aligned bounding box of the result.
        /// </summary>
        /// <param name="rectangle">The rectangle to transform.</param>
        /// <param name="transform">The transform matrix.</param>
        /// <param name="result">The axis-aligned bounding box of <paramref name="rectangle"/> after it has been transformed.</param>
        public static void TransformAxisAligned(ref RectangleF rectangle, ref Matrix transform, out RectangleF result)
        {
            var tl = new Vector2(rectangle.Left, rectangle.Top);
            var tr = new Vector2(rectangle.Right, rectangle.Top);
            var bl = new Vector2(rectangle.Left, rectangle.Bottom);
            var br = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref tl, ref transform, out tl);
            Vector2.Transform(ref tr, ref transform, out tr);
            Vector2.Transform(ref bl, ref transform, out bl);
            Vector2.Transform(ref br, ref transform, out br);

            var minX = Math.Min(Math.Min(tl.X, tr.X), Math.Min(bl.X, br.X));
            var maxX = Math.Max(Math.Max(tl.X, tr.X), Math.Max(bl.X, br.X));
            var minY = Math.Min(Math.Min(tl.Y, tr.Y), Math.Min(bl.Y, br.Y));
            var maxY = Math.Max(Math.Max(tl.Y, tr.Y), Math.Max(bl.Y, br.Y));

            result.X = minX;
            result.Y = minY;
            result.Width = maxX - minX;
            result.Height = maxY - minY;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y}: Width:{Width} Height{Height}}}";

        /// <summary>
        /// Gets a value indicating whether this rectangle intersects the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to evaluate.</param>
        /// <returns><see langword="true"/> if this rectangle intersects the specified rectangle; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(RectangleF rectangle)
        {
            return
                rectangle.Left < this.Right && 
                this.Left < rectangle.Right && 
                rectangle.Top < this.Bottom && 
                this.Top < rectangle.Bottom;
        }

        /// <summary>
        /// Gets a value indicating whether this rectangle intersects the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this rectangle intersects the specified rectangle; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref RectangleF rectangle, out Boolean result)
        {
            result =
                rectangle.Left < this.Right && 
                this.Left < rectangle.Right && 
                rectangle.Top < this.Bottom && 
                this.Top < rectangle.Bottom;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to evaluate.</param>
        /// <param name="y">The y-coordinate of the point to evaluate.</param>
        /// <returns><see langword="true"/> if the rectangle contains the specified point; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Single x, Single y)
        {
            return
                x >= this.X && x < this.X + this.Width &&
                y >= this.Y && y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <param name="result">A value indicating whether the rectangle contains the specified point.</param>
        public void Contains(ref Point2F point, out Boolean result)
        {
            result =
                point.X >= this.X && point.X < this.X + this.Width &&
                point.Y >= this.Y && point.Y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns><see langword="true"/> if the rectangle contains the specified point; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Point2F point)
        {
            return
                point.X >= this.X && point.X < this.X + this.Width &&
                point.Y >= this.Y && point.Y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> representing the point to evaluate.</param>
        /// <param name="result">A value indicating whether the rectangle contains the specified point.</param>
        public void Contains(ref Vector2 point, out Boolean result)
        {
            result = 
                point.X >= this.X && point.X < this.X + this.Width &&
                point.Y >= this.Y && point.Y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> representing the point to evaluate.</param>
        /// <returns><see langword="true"/> if the rectangle contains the specified point; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Vector2 point)
        {
            return
                point.X >= this.X && point.X < this.X + this.Width &&
                point.Y >= this.Y && point.Y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle completely contains the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to evaluate.</param>
        /// <param name="result"><see langword="true"/> if the rectangle completely contains the specified rectangle; otherwise, <see langword="false"/>.</param>
        public void Contains(ref RectangleF rectangle, out Boolean result)
        {
            result = 
                this.X <= rectangle.X && rectangle.X + rectangle.Width <= this.X + this.Width &&
                this.Y <= rectangle.Y && rectangle.Y + rectangle.Height <= this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle completely contains the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to evaluate.</param>
        /// <returns><see langword="true"/> if the rectangle completely contains the specified rectangle; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(RectangleF rectangle)
        {
            return
                this.X <= rectangle.X && rectangle.X + rectangle.Width <= this.X + this.Width &&
                this.Y <= rectangle.Y && rectangle.Y + rectangle.Height <= this.Y + this.Height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public RectangleF Interpolate(RectangleF target, Single t)
        {
            RectangleF result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Width = Tweening.Lerp(this.Width, target.Width, t);
            result.Height = Tweening.Lerp(this.Height, target.Height, t);

            return result;
        }

        /// <summary>
        /// Gets an empty rectangle.
        /// </summary>
        public static RectangleF Empty
        {
            get { return new RectangleF(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle is empty.
        /// </summary>
        [JsonIgnore]
        public Boolean IsEmpty
        {
            get { return Width == 0 || Height == 0; }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Single Top
        {
            get { return Y; }
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Single Left
        {
            get { return X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the bottom edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Single Bottom
        {
            get { return Y + Height; }
        }

        /// <summary>
        /// Gets the x-coordinate of the right edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Single Right
        {
            get { return X + Width; }
        }

        /// <summary>
        /// Gets the position of the rectangle's top-left corner.
        /// </summary>
        [JsonIgnore]
        public Point2F Location
        {
            get { return new Point2F(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets the position of the rectangle's center.
        /// </summary>
        [JsonIgnore]
        public Point2F Center
        {
            get { return new Point2F(X + (Width / 2.0f), Y + (Height / 2.0f)); }
        }

        /// <summary>
        /// Gets the rectangle's size.
        /// </summary>
        [JsonIgnore]
        public Size2F Size
        {
            get { return new Size2F(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// The x-coordinate of the rectangle's top-left corner.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single X;

        /// <summary>
        /// The y-coordinate of the rectangle's top-left corner.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Y;

        /// <summary>
        /// The rectangle's width.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Width;

        /// <summary>
        /// The rectangle's height.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Height;
    }
}
