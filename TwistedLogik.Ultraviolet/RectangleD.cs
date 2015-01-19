using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a rectangle with double-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Width:{Width} Height:{Height}\}")]
    public struct RectangleD : IEquatable<RectangleD>, IInterpolatable<RectangleD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleD"/> class.
        /// </summary>
        /// <param name="position">The rectangle's position.</param>
        /// <param name="size">The rectangle's size.</param>
        public RectangleD(Point2D position, Size2D size)
            : this(position.X, position.Y, size.Width, size.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The y-coordinate of the rectangle's top-right corner.</param>
        /// <param name="width">The rectangle's width.</param>
        /// <param name="height">The rectangle's height.</param>
        public RectangleD(Double x, Double y, Double width, Double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleD"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator +(RectangleD rect, Point2D point)
        {
            return new RectangleD(rect.X + point.X, rect.Y + point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleD"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        public static RectangleD operator -(RectangleD rect, Point2D point)
        {
            return new RectangleD(rect.X - point.X, rect.Y - point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Compares two rectangles for equality.
        /// </summary>
        /// <param name="r1">The first <see cref="RectangleD"/> to compare.</param>
        /// <param name="r2">The second <see cref="RectangleD"/> to compare.</param>
        /// <returns><c>true</c> if the specified rectangles are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(RectangleD r1, RectangleD r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Compares two rectangles for inequality.
        /// </summary>
        /// <param name="r1">The first <see cref="RectangleD"/> to compare.</param>
        /// <param name="r2">The second <see cref="RectangleD"/> to compare.</param>
        /// <returns><c>true</c> if the specified rectangles are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(RectangleD r1, RectangleD r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// Explicitly converts a <see cref="RectangleD"/> structure to a <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Rectangle(RectangleD rect)
        {
            return new Rectangle((Int32)rect.x, (Int32)rect.y, (Int32)rect.width, (Int32)rect.height);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Rectangle"/> structure to a <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator RectangleD(Rectangle rect)
        {
            return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="RectangleD"/> structure to a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator RectangleF(RectangleD rect)
        {
            return new RectangleF((Single)rect.x, (Single)rect.y, (Single)rect.width, (Single)rect.height);
        }

        /// <summary>
        /// Implicitly converts a <see cref="RectangleF"/> structure to a <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator RectangleD(RectangleF rect)
        {
            return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleD"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out RectangleD rect)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out rect);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <returns>A instance of the <see cref="RectangleD"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        public static RectangleD Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleD"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out RectangleD rect)
        {
            rect = default(RectangleD);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 4)
                return false;

            Double x, y, width, height;
            if (!Double.TryParse(components[0], style, provider, out x))
                return false;
            if (!Double.TryParse(components[1], style, provider, out y))
                return false;
            if (!Double.TryParse(components[2], style, provider, out width))
                return false;
            if (!Double.TryParse(components[3], style, provider, out height))
                return false;

            rect = new RectangleD(x, y, width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="RectangleD"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        public static RectangleD Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            RectangleD rect;
            if (!TryParse(s, style, provider, out rect))
                throw new FormatException();
            return rect;
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <returns>The offset <see cref="RectangleD"/>.</returns>
        public static RectangleD Offset(RectangleD rectangle, Int32 offsetX, Int32 offsetY)
        {
            return new RectangleD(rectangle.X + offsetX, rectangle.Y + offsetY, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="offsetX">The amount by which to offset the rectangle along the x-axis.</param>
        /// <param name="offsetY">The amount by which to offset the rectangle along the y-axis.</param>
        /// <param name="result">The offset <see cref="RectangleD"/>.</param>
        public static void Offset(ref RectangleD rectangle, Int32 offsetX, Int32 offsetY, out RectangleD result)
        {
            result = new RectangleD(rectangle.X + offsetX, rectangle.Y + offsetY, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <returns>The offset <see cref="RectangleD"/>.</returns>
        public static RectangleD Offset(RectangleD rectangle, Vector2 offset)
        {
            return new RectangleD(rectangle.X + offset.X, rectangle.Y + offset.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <param name="result">The offset <see cref="RectangleD"/>.</param>
        public static void Offset(ref RectangleD rectangle, ref Vector2 offset, out RectangleD result)
        {
            result = new RectangleD(rectangle.X + offset.X, rectangle.Y + offset.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <returns>The inflated <see cref="RectangleD"/>.</returns>
        public static RectangleD Inflate(RectangleD rectangle, Double horizontalAmount, Double verticalAmount)
        {
            return new RectangleD(
                rectangle.X - horizontalAmount,
                rectangle.Y - verticalAmount,
                rectangle.Width + horizontalAmount,
                rectangle.Height + verticalAmount
            );
        }

        /// <summary>
        /// Inflates the specified rectangle by the specified horizontal and vertical values.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to inflate.</param>
        /// <param name="horizontalAmount">The amount by which to inflate the rectangle horizontally.</param>
        /// <param name="verticalAmount">The amount by which to inflate the rectangle vertically.</param>
        /// <param name="result">The inflated <see cref="RectangleD"/>.</param>
        public static void Inflate(ref RectangleD rectangle, Double horizontalAmount, Double verticalAmount, out RectangleD result)
        {
            result = new RectangleD(
                rectangle.X - horizontalAmount,
                rectangle.Y - verticalAmount,
                rectangle.Width + horizontalAmount,
                rectangle.Height + verticalAmount
            );
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleD"/>.</param>
        /// <param name="rectangle2">The second <see cref="RectangleD"/>.</param>
        /// <returns>The <see cref="RectangleD"/> that was created.</returns>
        public static RectangleD Union(RectangleD rectangle1, RectangleD rectangle2)
        {
            var minLeft = rectangle1.Left < rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var minTop = rectangle1.Top < rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var maxRight = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var maxBottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            return new RectangleD(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        /// <summary>
        /// Creates a rectangle which is the union of the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleD"/>.</param>
        /// <param name="rectangle2">The second <see cref="RectangleD"/>.</param>
        /// <param name="result">The <see cref="RectangleD"/> that was created.</param>
        public static void Union(ref RectangleD rectangle1, ref RectangleD rectangle2, out RectangleD result)
        {
            var minLeft = rectangle1.Left < rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var minTop = rectangle1.Top < rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var maxRight = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var maxBottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            result = new RectangleD(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        /// <summary>
        /// Creates a rectangle which represents the intersection between the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleD"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="RectangleD"/> to intersect.</param>
        /// <returns>The <see cref="RectangleD"/> that was created.</returns>
        public static RectangleD Intersect(RectangleD rectangle1, RectangleD rectangle2)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);

            return isEmpty ? RectangleD.Empty : new RectangleD(maxLeft, maxTop, minRight - maxLeft, minBottom - maxTop);
        }

        /// <summary>
        /// Creates a rectangle which represents the intersection between the specified rectangles.
        /// </summary>
        /// <param name="rectangle1">The first <see cref="RectangleD"/> to intersect.</param>
        /// <param name="rectangle2">The second <see cref="RectangleD"/> to intersect.</param>
        /// <param name="result">The <see cref="RectangleD"/> that was created.</param>
        public static void Intersect(ref RectangleD rectangle1, ref RectangleD rectangle2, out RectangleD result)
        {
            var maxLeft = rectangle1.Left > rectangle2.Left ? rectangle1.Left : rectangle2.Left;
            var maxTop = rectangle1.Top > rectangle2.Top ? rectangle1.Top : rectangle2.Top;
            var minRight = rectangle1.Right < rectangle2.Right ? rectangle1.Right : rectangle2.Right;
            var minBottom = rectangle1.Bottom < rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

            var isEmpty = (minRight <= maxLeft || minBottom <= maxTop);

            result = isEmpty ? RectangleD.Empty : new RectangleD(maxLeft, maxTop, minRight - maxLeft, minBottom - maxTop);
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
            return String.Format(provider, "{0} {1} {2} {3}", x, y, width, height);
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
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                hash = hash * 23 + width.GetHashCode();
                hash = hash * 23 + height.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is RectangleD))
                return false;
            return Equals((RectangleD)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(RectangleD other)
        {
            return x == other.x && y == other.y && width == other.width && height == other.height;
        }

        /// <summary>
        /// Gets a value indicating whether this rectangle intersects the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to evaluate.</param>
        /// <returns><c>true</c> if this rectangle intersects the specified rectangle; otherwise, <c>false</c>.</returns>
        public Boolean Intersects(RectangleD rectangle)
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
        /// <param name="rectangle">The <see cref="RectangleD"/> to evaluate.</param>
        /// <param name="result"><c>true</c> if this rectangle intersects the specified rectangle; otherwise, <c>false</c>.</param>
        public void Intersects(ref RectangleD rectangle, out Boolean result)
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
        /// <returns><c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Double x, Double y)
        {
            return
                x >= this.x && x < this.x + this.width &&
                y >= this.y && y < this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <param name="result">A value indicating whether the rectangle contains the specified point.</param>
        public void Contains(ref Point2D point, out Boolean result)
        {
            result =
                point.X >= this.x && point.X < this.x + this.width &&
                point.Y >= this.y && point.Y < this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns><c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Point2D point)
        {
            return
                point.X >= this.x && point.X < this.x + this.width &&
                point.Y >= this.y && point.Y < this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> representing the point to evaluate.</param>
        /// <param name="result">A value indicating whether the rectangle contains the specified point.</param>
        public void Contains(ref Vector2 point, out Boolean result)
        {
            result = 
                point.X >= this.x && point.X < this.x + this.width &&
                point.Y >= this.y && point.Y < this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> representing the point to evaluate.</param>
        /// <returns><c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Vector2 point)
        {
            return
                point.X >= this.x && point.X < this.x + this.width &&
                point.Y >= this.y && point.Y < this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle completely contains the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to evaluate.</param>
        /// <param name="result"><c>true</c> if the rectangle completely contains the specified rectangle; otherwise, <c>false</c>.</param>
        public void Contains(ref RectangleD rectangle, out Boolean result)
        {
            result = 
                this.x <= rectangle.x && rectangle.x + rectangle.width <= this.x + this.width &&
                this.y <= rectangle.y && rectangle.y + rectangle.height <= this.y + this.height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle completely contains the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to evaluate.</param>
        /// <returns><c>true</c> if the rectangle completely contains the specified rectangle; otherwise, <c>false</c>.</returns>
        public Boolean Contains(RectangleD rectangle)
        {
            return
                this.x <= rectangle.x && rectangle.x + rectangle.width <= this.x + this.width &&
                this.y <= rectangle.y && rectangle.y + rectangle.height <= this.y + this.height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public RectangleD Interpolate(RectangleD target, Single t)
        {
            var x      = Tweening.Lerp(this.x, target.x, t);
            var y      = Tweening.Lerp(this.y, target.y, t);
            var width  = Tweening.Lerp(this.width, target.width, t);
            var height = Tweening.Lerp(this.height, target.height, t);
            return new RectangleD(x, y, width, height);
        }

        /// <summary>
        /// Gets an empty rectangle.
        /// </summary>
        public static RectangleD Empty
        {
            get { return new RectangleD(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return width == 0 || height == 0; }
        }

        /// <summary>
        /// Gets the x-coordinate of the rectangle's top-left corner.
        /// </summary>
        public Double X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the rectangle's top-left corner.
        /// </summary>
        public Double Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the rectangle's width.
        /// </summary>
        public Double Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the rectangle's height.
        /// </summary>
        public Double Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge of the rectangle.
        /// </summary>
        public Double Top
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge of the rectangle.
        /// </summary>
        public Double Left
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the bottom edge of the rectangle.
        /// </summary>
        public Double Bottom
        {
            get { return y + height; }
        }

        /// <summary>
        /// Gets the x-coordinate of the right edge of the rectangle.
        /// </summary>
        public Double Right
        {
            get { return x + width; }
        }

        /// <summary>
        /// Gets the position of the rectangle's top-left corner.
        /// </summary>
        public Point2D Location
        {
            get { return new Point2D(x, y); }
        }

        /// <summary>
        /// Gets the position of the rectangle's center.
        /// </summary>
        public Point2D Center
        {
            get { return new Point2D(x + (width / 2.0), y + (height / 2.0)); }
        }

        /// <summary>
        /// Gets the rectangle's size.
        /// </summary>
        public Size2D Size
        {
            get { return new Size2D(width, height); }
        }

        // Property values.
        private readonly Double x;
        private readonly Double y;
        private readonly Double width;
        private readonly Double height;
    }
}
