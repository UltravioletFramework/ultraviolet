using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a rectangle with integer components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Width:{Width} Height:{Height}\}")]
    public struct Rectangle : IEquatable<Rectangle>, IInterpolatable<Rectangle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="position">The rectangle's position.</param>
        /// <param name="size">The rectangle's size.</param>
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        /// Compares two rectangles for equality.
        /// </summary>
        /// <param name="r1">The first <see cref="Rectangle"/> to compare.</param>
        /// <param name="r2">The second <see cref="Rectangle"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rectangles are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Rectangle r1, Rectangle r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Compares two rectangles for inequality.
        /// </summary>
        /// <param name="r1">The first <see cref="Rectangle"/> to compare.</param>
        /// <param name="r2">The second <see cref="Rectangle"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rectangles are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Rectangle r1, Rectangle r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Rectangle"/> structure to a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
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
        [Preserve]
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
        /// Converts the string representation of a rectangle into an instance of the <see cref="Rectangle"/>  structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Rectangle rect)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out rect);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <returns>A instance of the <see cref="Rectangle"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Rectangle Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="Rectangle"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Rectangle rect)
        {
            rect = default(Rectangle);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;

            Int32 x, y, width, height;
            if (!Int32.TryParse(components[0], style, provider, out x))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out y))
                return false;
            if (!Int32.TryParse(components[2], style, provider, out width))
                return false;
            if (!Int32.TryParse(components[3], style, provider, out height))
                return false;

            rect = new Rectangle(x, y, width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Rectangle"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Rectangle Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Rectangle rect;
            if (!TryParse(s, style, provider, out rect))
            {
                throw new FormatException();
            }
            return rect;
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

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format(provider, "{0} {1} {2} {3}", X, Y, Width, Height);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Rectangle))
                return false;
            return Equals((Rectangle)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

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
        [Preserve]
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
        }
        
        /// <summary>
        /// The x-coordinate of the rectangle's top-left corner.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Int32 X;

        /// <summary>
        /// The y-coordinate of the rectangle's top-left corner.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Int32 Y;

        /// <summary>
        /// The rectangle's width.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public Int32 Width;

        /// <summary>
        /// The rectangle's height.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public Int32 Height;
    }
}
