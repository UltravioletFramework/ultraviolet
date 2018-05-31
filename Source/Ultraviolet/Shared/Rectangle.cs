using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a rectangle with integer components.
    /// </summary>
    [Serializable]
    public partial struct Rectangle : IEquatable<Rectangle>, IInterpolatable<Rectangle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="position">The rectangle's position.</param>
        /// <param name="size">The rectangle's size.</param>
        public Rectangle(Point2 position, Size2 size)
            : this(position.X, position.Y, size.Width, size.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The y-coordinate of the rectangle's top-right corner.</param>
        /// <param name="width">The rectangle's width.</param>
        /// <param name="height">The rectangle's height.</param>
        [JsonConstructor]
        public Rectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by adding the specified <see cref="Point2"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="Rectangle"/> that has been offset by the specified amount.</returns>
        public static Rectangle operator +(Rectangle rect, Point2 point)
        {
            Rectangle result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by subtracting the specified <see cref="Point2"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="Rectangle"/> that has been offset by the specified amount.</returns>
        public static Rectangle operator -(Rectangle rect, Point2 point)
        {
            Rectangle result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator +(Rectangle rect, Point2F point)
        {
            RectangleF result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        public static RectangleF operator -(Rectangle rect, Point2F point)
        {
            RectangleF result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator +(Rectangle rect, Point2D point)
        {
            RectangleD result;

            result.X = rect.X + point.X;
            result.Y = rect.Y + point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Rectangle"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator -(Rectangle rect, Point2D point)
        {
            RectangleD result;

            result.X = rect.X - point.X;
            result.Y = rect.Y - point.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }
        
        /// <summary>
        /// Implicitly converts a <see cref="Rectangle"/> structure to a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator RectangleF(Rectangle rect)
        {
            RectangleF result;

            result.X = rect.X;
            result.Y = rect.Y;
            result.Width = rect.Width;
            result.Height = rect.Height;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Rectangle"/> structure to a <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator RectangleD(Rectangle rect)
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
        /// <param name="rectangle">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <returns>The offset <see cref="Rectangle"/>.</returns>
        public static Rectangle Offset(Rectangle rectangle, Int32 offsetX, Int32 offsetY)
        {
            Rectangle result;

            result.X = rectangle.X + offsetX;
            result.Y = rectangle.Y + offsetY;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <param name="result">The offset <see cref="Rectangle"/>.</param>
        public static void Offset(ref Rectangle rectangle, Int32 offsetX, Int32 offsetY, out Rectangle result)
        {
            result.X = rectangle.X + offsetX;
            result.Y = rectangle.Y + offsetY;
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <returns>The offset <see cref="Rectangle"/>.</returns>
        public static Rectangle Offset(Rectangle rectangle, Vector2 offset)
        {
            Rectangle result;

            result.X = (Int32)(rectangle.X + offset.X);
            result.Y = (Int32)(rectangle.Y + offset.Y);
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;

            return result;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <param name="result">The offset <see cref="Rectangle"/>.</param>
        public static void Offset(ref Rectangle rectangle, ref Vector2 offset, out Rectangle result)
        {
            result.X = (Int32)(rectangle.X + offset.X);
            result.Y = (Int32)(rectangle.Y + offset.Y);
            result.Width = rectangle.Width;
            result.Height = rectangle.Height;
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <returns>The inflated <see cref="Rectangle"/>.</returns>
        public static Rectangle Inflate(Rectangle rectangle, Int32 horizontalAmount, Int32 verticalAmount)
        {
            Rectangle result;

            result.X = rectangle.X - horizontalAmount;
            result.Y = rectangle.Y - verticalAmount;
            result.Width = rectangle.Width + (2 * horizontalAmount);
            result.Height = rectangle.Height + (2 * verticalAmount);

            return result;
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <param name="result">The inflated <see cref="Rectangle"/>.</param>
        public static void Inflate(ref Rectangle rectangle, Int32 horizontalAmount, Int32 verticalAmount, out Rectangle result)
        {
            result.X = rectangle.X - horizontalAmount;
            result.Y = rectangle.Y - verticalAmount;
            result.Width = rectangle.Width + (2 * horizontalAmount);
            result.Height = rectangle.Height + (2 * verticalAmount);
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="Rectangle"/>.</param>
        /// <param name="rectangle2">The second <see cref="Rectangle"/>.</param>
        /// <returns>The <see cref="Rectangle"/> that was created.</returns>
        public static Rectangle Union(Rectangle rectangle1, Rectangle rectangle2)
        {
            var minLeft = rectangle1.Left < rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var minTop = rectangle1.Top < rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var maxRight = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var maxBottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            Rectangle result;

            result.X = minLeft;
            result.Y = minTop;
            result.Width = maxRight - minLeft;
            result.Height = maxBottom - minTop;

            return result;
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="Rectangle"/>.</param>
        /// <param name="rectangle2">The second <see cref="Rectangle"/>.</param>
        /// <param name="result">The <see cref="Rectangle"/> that was created.</param>
        public static void Union(ref Rectangle rectangle1, ref Rectangle rectangle2, out Rectangle result)
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
        /// <param name="rectangle1">The first <see cref="Rectangle"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="Rectangle"/> to intersect.</param>
        /// <returns>The <see cref="Rectangle"/> that was created.</returns>
        public static Rectangle Intersect(Rectangle rectangle1, Rectangle rectangle2)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);
            if (isEmpty)
                return Rectangle.Empty;

            Rectangle result;

            result.X = maxLeft;
            result.Y = maxTop;
            result.Width = minRight - maxLeft;
            result.Height = minBottom - maxTop;

            return result;
        }

        /// <summary>
        /// Creates a rectangle which represents the intersection between the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="Rectangle"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="Rectangle"/> to intersect.</param>
        /// <param name="result">The <see cref="Rectangle"/> that was created.</param>
        public static void Intersect(ref Rectangle rectangle1, ref Rectangle rectangle2, out Rectangle result)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);
            if (isEmpty)
            {
                result = Rectangle.Empty;
            }
            else
            {
                result.X = maxLeft;
                result.Y = maxTop;
                result.Width = minRight - maxLeft;
                result.Height = minBottom - maxTop;
            }
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y}: Width:{Width} Height{Height}}}";

        /// <summary>
        /// Gets a value indicating whether this rectangle intersects the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to evaluate.</param>
        /// <returns><see langword="true"/> if this rectangle intersects the specified rectangle; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(Rectangle rectangle)
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
        /// <param name="rectangle">The <see cref="Rectangle"/> to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this rectangle intersects the specified rectangle; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref Rectangle rectangle, out Boolean result)
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
        public Boolean Contains(Int32 x, Int32 y)
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
        public void Contains(ref Point2 point, out Boolean result)
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
        public Boolean Contains(Point2 point)
        {
            return
                point.X >= this.X && point.X < this.X + this.Width &&
                point.Y >= this.Y && point.Y < this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
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
        /// <param name="point">The point to evaluate.</param>
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
        /// <param name="rectangle">The rectangle to evaluate.</param>
        /// <param name="result"><see langword="true"/> if the rectangle completely contains the specified rectangle; otherwise, <see langword="false"/>.</param>
        public void Contains(ref Rectangle rectangle, out Boolean result)
        {
            result = 
                this.X <= rectangle.X && rectangle.X + rectangle.Width <= this.X + this.Width &&
                this.Y <= rectangle.Y && rectangle.Y + rectangle.Height <= this.Y + this.Height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle completely contains the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to evaluate.</param>
        /// <returns><see langword="true"/> if the rectangle completely contains the specified rectangle; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Rectangle rectangle)
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
        public Rectangle Interpolate(Rectangle target, Single t)
        {
            Rectangle result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Width = Tweening.Lerp(this.Width, target.Width, t);
            result.Height = Tweening.Lerp(this.Height, target.Height, t);

            return result;
        }

        /// <summary>
        /// Gets a rectangle at position (0, 0) with zero width and height.
        /// </summary>
        public static Rectangle Empty
        {
            get { return new Rectangle(0, 0, 0, 0); }
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
        public Int32 Top
        {
            get { return Y; }
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Int32 Left
        {
            get { return X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the bottom edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Int32 Bottom
        {
            get { return Y + Height; }
        }

        /// <summary>
        /// Gets the x-coordinate of the right edge of the rectangle.
        /// </summary>
        [JsonIgnore]
        public Int32 Right
        {
            get { return X + Width; }
        }

        /// <summary>
        /// Gets the position of the rectangle's top-left corner.
        /// </summary>
        [JsonIgnore]
        public Point2 Location
        {
            get { return new Point2(X, Y); }
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
        public Point2 Center
        {
            get { return new Point2(X + (Width / 2), Y + (Height / 2)); }
        }

        /// <summary>
        /// Gets the rectangle's size.
        /// </summary>
        [JsonIgnore]
        public Size2 Size
        {
            get { return new Size2(Width, Height); }
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
        public Int32 X;

        /// <summary>
        /// The y-coordinate of the rectangle's top-left corner.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Int32 Y;

        /// <summary>
        /// The rectangle's width.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Int32 Width;

        /// <summary>
        /// The rectangle's height.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Int32 Height;
    }
}
