using System;
using System.Diagnostics;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a region of the screen in which rendering takes place.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Width:{Width} Height:{Height}\}")]
    public struct Viewport : IEquatable<Viewport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="width">The width of the viewport in pixels.</param>
        /// <param name="height">The height of the viewport in pixels.</param>
        public Viewport(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Compares two viewports for equality.
        /// </summary>
        /// <param name="v1">The first <see cref="Viewport"/> to compare.</param>
        /// <param name="v2">The second <see cref="Viewport"/> to compare.</param>
        /// <returns><c>true</c> if the specified viewports are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Viewport v1, Viewport v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two viewports for inequality.
        /// </summary>
        /// <param name="v1">The first <see cref="Viewport"/> to compare.</param>
        /// <param name="v2">The second <see cref="Viewport"/> to compare.</param>
        /// <returns><c>true</c> if the specified viewports are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Viewport v1, Viewport v2)
        {
            return !v1.Equals(v2);
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
            return String.Format(provider, "{0}, {1}, {2}, {3}", x, y, width, height);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Viewport))
                return false;
            return Equals((Viewport)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified viewport.
        /// </summary>
        /// <param name="other">The <see cref="Viewport"/> to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified viewport; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Viewport other)
        {
            return x == other.x && y == other.y && width == other.width && height == other.height;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 X { get { return x; } }

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 Y { get { return y; } }

        /// <summary>
        /// Gets the width of the viewport in pixels.
        /// </summary>
        public Int32 Width { get { return width; } }

        /// <summary>
        /// Gets the height of the viewport in pixels.
        /// </summary>
        public Int32 Height { get { return height; } }

        // Property values.
        private readonly Int32 x;
        private readonly Int32 y;
        private readonly Int32 width;
        private readonly Int32 height;
    }
}
