using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a rectangle with single-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Width:{Width} Height:{Height}\}")]
    public struct RectangleF : IEquatable<RectangleF>, IInterpolatable<RectangleF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> class.
        /// </summary>
        /// <param name="position">The rectangle's position.</param>
        /// <param name="size">The rectangle's size.</param>
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
        public static RectangleF operator +(RectangleF rect, Point2 point)
        {
            return new RectangleF(rect.X + point.X, rect.Y + point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static RectangleF operator -(RectangleF rect, Point2 point)
        {
            return new RectangleF(rect.X - point.X, rect.Y - point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static RectangleF operator +(RectangleF rect, Point2F point)
        {
            return new RectangleF(rect.X + point.X, rect.Y + point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static RectangleF operator -(RectangleF rect, Point2F point)
        {
            return new RectangleF(rect.X - point.X, rect.Y - point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static RectangleD operator +(RectangleF rect, Point2D point)
        {
            return new RectangleD(rect.X + point.X, rect.Y + point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="RectangleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static RectangleD operator -(RectangleF rect, Point2D point)
        {
            return new RectangleD(rect.X - point.X, rect.Y - point.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Compares two rectangles for equality.
        /// </summary>
        /// <param name="r1">The first <see cref="RectangleF"/> to compare.</param>
        /// <param name="r2">The second <see cref="RectangleF"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rectangles are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(RectangleF r1, RectangleF r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Compares two rectangles for inequality.
        /// </summary>
        /// <param name="r1">The first <see cref="RectangleF"/> to compare.</param>
        /// <param name="r2">The second <see cref="RectangleF"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rectangles are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(RectangleF r1, RectangleF r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// Explicitly converts a <see cref="RectangleF"/> structure to a <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Rectangle(RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        /// <summary>
        /// Implicitly converts a <see cref="RectangleF"/> structure to a <see cref="RectangleD"/> structure.
        /// </summary>
        /// <param name="rect">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator RectangleD(RectangleF rect)
        {
            return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleF"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out RectangleF rect)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out rect);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <returns>A instance of the <see cref="RectangleF"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static RectangleF Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleF"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="rect">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out RectangleF rect)
        {
            rect = default(RectangleF);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;

            Single x, y, width, height;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out width))
                return false;
            if (!Single.TryParse(components[3], style, provider, out height))
                return false;

            rect = new RectangleF(x, y, width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a rectangle into an instance of the <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="s">A string containing a rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="RectangleF"/> structure equivalent to the rectangle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static RectangleF Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            RectangleF rect;
            if (!TryParse(s, style, provider, out rect))
                throw new FormatException();
            return rect;
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
            return new RectangleF(rectangle.X + offsetX, rectangle.Y + offsetY, rectangle.Width, rectangle.Height);
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
            result = new RectangleF(rectangle.X + offsetX, rectangle.Y + offsetY, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <returns>The offset <see cref="RectangleF"/>.</returns>
        public static RectangleF Offset(RectangleF rectangle, Vector2 offset)
        {
            return new RectangleF(rectangle.X + offset.X, rectangle.Y + offset.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Offsets the specified rectangle by the specified amount.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="offset">The amount by which to offset the rectangle.</param>
        /// <param name="result">The offset <see cref="RectangleF"/>.</param>
        public static void Offset(ref RectangleF rectangle, ref Vector2 offset, out RectangleF result)
        {
            result = new RectangleF(rectangle.X + offset.X, rectangle.Y + offset.Y, rectangle.Width, rectangle.Height);
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
            return new RectangleF(
                rectangle.X - horizontalAmount,
                rectangle.Y - verticalAmount,
                rectangle.Width + (2f * horizontalAmount),
                rectangle.Height + (2f * verticalAmount)
            );
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
            result = new RectangleF(
                rectangle.X - horizontalAmount,
                rectangle.Y - verticalAmount,
                rectangle.Width + (2f * horizontalAmount),
                rectangle.Height + (2f * verticalAmount)
            );
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

            return new RectangleF(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
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

            result = new RectangleF(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
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

            return isEmpty ? RectangleF.Empty : new RectangleF(maxLeft, maxTop, minRight - maxLeft, minBottom - maxTop);
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

            result = isEmpty ? RectangleF.Empty : new RectangleF(maxLeft, maxTop, minRight - maxLeft, minBottom - maxTop);
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

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
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

            result = new RectangleF(minX, minY, maxX - minX, maxY - minY);
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
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is RectangleF))
                return false;
            return Equals((RectangleF)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(RectangleF other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

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
        [Preserve]
        public RectangleF Interpolate(RectangleF target, Single t)
        {
            var x      = Tweening.Lerp(this.X, target.X, t);
            var y      = Tweening.Lerp(this.Y, target.Y, t);
            var width  = Tweening.Lerp(this.Width, target.Width, t);
            var height = Tweening.Lerp(this.Height, target.Height, t);
            return new RectangleF(x, y, width, height);
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
        }

        /// <summary>
        /// The x-coordinate of the rectangle's top-left corner.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Single X;

        /// <summary>
        /// The y-coordinate of the rectangle's top-left corner.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Single Y;

        /// <summary>
        /// The rectangle's width.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public Single Width;

        /// <summary>
        /// The rectangle's height.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public Single Height;
    }
}
