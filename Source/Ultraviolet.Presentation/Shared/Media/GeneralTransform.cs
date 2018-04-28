using System;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a coordinate transformation.
    /// </summary>
    [UvmlKnownType]
    public abstract class GeneralTransform : DependencyObject
    {
        /// <summary>
        /// Transforms the specified vector.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        public Vector2 Transform(Vector2 vector)
        {
            Vector2 result;

            if (!TryTransform(vector, out result))
            {
                throw new InvalidOperationException(PresentationStrings.InvalidTransformation);
            }

            return result;
        }

        /// <summary>
        /// Transforms the specified point.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point.</returns>
        public Point2D Transform(Point2D point)
        {
            Point2D result;

            if (!TryTransform(point, out result))
            {
                throw new InvalidOperationException(PresentationStrings.InvalidTransformation);
            }

            return result;
        }

        /// <summary>
        /// Attempts to transform the specified vector.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="result">The transformed vector.</param>
        /// <returns><see langword="true"/> if the transformation was successful; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean TryTransform(Vector2 vector, out Vector2 result);

        /// <summary>
        /// Attempts to transform the specified point.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="result">The transformed point.</param>
        /// <returns><see langword="true"/> if the transformation was successful; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean TryTransform(Point2D point, out Point2D result);

        /// <summary>
        /// Gets a value indicating whether this is an identity transform.
        /// </summary>
        /// <value><see langword="true"/> if this is an identity transform; otherwise,
        /// <see langword="false"/>.</value>
        public abstract Boolean IsIdentity
        {
            get;
        }
    }
}
