using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the thickness of a frame around a rectangle.
    /// </summary>
    [Serializable]
    public partial struct Thickness : IEquatable<Thickness>, IInterpolatable<Thickness>
    {
        /// <summary>
        /// Initializes the <see cref="Thickness"/> type.
        /// </summary>
        static Thickness()
        {
            Tweening.Interpolators.RegisterDefault<Thickness>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="value">The size, in device independent pixels, of the bounding rectangles' four edges.</param>
        public Thickness(Double value)
            : this(value, value, value, value)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="left">The width, in device independent pixels (1/96 of an inch), of the left side of the bounding rectangle.</param>
        /// <param name="top">The width, in device independent pixels (1/96 of an inch), of the top side of the bounding rectangle.</param>
        /// <param name="right">The width, in device independent pixels (1/96 of an inch), of the right side of the bounding rectangle.</param>
        /// <param name="bottom">The width, in device independent pixels (1/96 of an inch), of the bottom side of the bounding rectangle.</param>
        public Thickness(Double left, Double top, Double right, Double bottom)
        {
            this.left   = left;
            this.top    = top;
            this.right  = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Adds two <see cref="Thickness"/> values together.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to add.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to add.</param>
        /// <returns>A <see cref="Thickness"/> that is the sum of the specified <see cref="Thickness"/> values.</returns>
        public static Thickness operator +(Thickness t1, Thickness t2)
        {
            return new Thickness(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> value from another <see cref="Thickness"/> value.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to subtract.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to subtract.</param>
        /// <returns>A <see cref="Thickness"/> that is the difference of the specified <see cref="Thickness"/> values.</returns>
        public static Thickness operator -(Thickness t1, Thickness t2)
        {
            return new Thickness(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size2D"/> to which to add the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="Size2D"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the sum of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator +(Size2D size, Thickness thickness)
        {
            var width  = Math.Max(0, size.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, size.Height + thickness.Top + thickness.Bottom);

            return new Size2D(width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="Size2D"/>.</param>
        /// <param name="size">The <see cref="Size2D"/> to which to add the <see cref="Thickness"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the sum of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator +(Thickness thickness, Size2D size)
        {
            var width  = Math.Max(0, size.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, size.Height + thickness.Top + thickness.Bottom);

            return new Size2D(width, height);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> from a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size2D"/> from which to subtract the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to subtract from the <see cref="Size2D"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the difference of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator -(Size2D size, Thickness thickness)
        {
            var width  = Math.Max(0, size.Width - (thickness.Left + thickness.Right));
            var height = Math.Max(0, size.Height - (thickness.Top + thickness.Bottom));

            return new Size2D(width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to which to add the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="RectangleD"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the sum of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator +(RectangleD rectangle, Thickness thickness)
        {
            var width = Math.Max(0, rectangle.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, rectangle.Height + thickness.Top + thickness.Bottom);

            return new RectangleD(rectangle.X - thickness.Left, rectangle.Y - thickness.Top, width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="RectangleD"/>.</param>
        /// <param name="rectangle">The <see cref="RectangleD"/> to which to add the <see cref="Thickness"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the sum of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator +(Thickness thickness, RectangleD rectangle)
        {
            var width = Math.Max(0, rectangle.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, rectangle.Height + thickness.Top + thickness.Bottom);

            return new RectangleD(rectangle.X - thickness.Left, rectangle.Y - thickness.Top, width, height);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> from a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> from which to subtract the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to subtract from the <see cref="RectangleD"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the difference of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator -(RectangleD rectangle, Thickness thickness)
        {
            var width  = Math.Max(0, rectangle.Width - (thickness.Left + thickness.Right));
            var height = Math.Max(0, rectangle.Height - (thickness.Top + thickness.Bottom));

            return new RectangleD(rectangle.X + thickness.Left, rectangle.Y + thickness.Top, width, height);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Left:{Left} Right:{Right} Top:{Top} Bottom:{Bottom}}}";

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Thickness Interpolate(Thickness target, Single t)
        {
            var left   = Tweening.Lerp(this.left, target.left, t);
            var top    = Tweening.Lerp(this.top, target.top, t);
            var right  = Tweening.Lerp(this.right, target.right, t);
            var bottom = Tweening.Lerp(this.bottom, target.bottom, t);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Gets an instance representing a thickness of zero on all edges.
        /// </summary>
        public static Thickness Zero
        {
            get { return new Thickness(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets an instance representing a thickness of one on all edges.
        /// </summary>
        public static Thickness One
        {
            get { return new Thickness(1, 1, 1, 1); }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the left side of the bounding rectangle.
        /// </summary>
        public Double Left
        {
            get { return left; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the top side of the bounding rectangle.
        /// </summary>
        public Double Top
        {
            get { return top; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the right side of the bounding rectangle.
        /// </summary>
        public Double Right
        {
            get { return right; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the bottom side of the bounding rectangle.
        /// </summary>
        public Double Bottom
        {
            get { return bottom; }
        }

        // Property values.
        private readonly Double left;
        private readonly Double top;
        private readonly Double right;
        private readonly Double bottom;
    }
}
