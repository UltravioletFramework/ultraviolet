using System;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a two-dimensional transformation.
    /// </summary>
    [UvmlKnownType]
    public abstract class Transform : GeneralTransform
    {
        /// <summary>
        /// Gets a value indicating whether the specified transformation is an identity transformation.
        /// </summary>
        /// <param name="transform">The transform to evaluate.</param>
        /// <returns><see langword="true"/> if the specified transform is an identity transform; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsIdentityTransform(Transform transform)
        {
            return (transform == null || transform.IsIdentity);
        }

        /// <inheritdoc/>
        public override Boolean TryTransform(Vector2 vector, out Vector2 result)
        {
            result = Vector2.Transform(vector, Value);
            return true;
        }

        /// <inheritdoc/>
        public override Boolean TryTransform(Point2D point, out Point2D result)
        {
            result = Point2D.Transform(point, Value);
            return true;
        }

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        /// <value>A <see cref="Transform"/> that represents an identity transformation.</value>
        public static Transform Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// Gets the <see cref="Matrix"/> that represents this transformation.
        /// </summary>
        /// <value>A <see cref="Matrix"/> that represents the transformation applied by this instance.</value>
        public abstract Matrix Value
        {
            get;
        }

        /// <summary>
        /// Gets the inverse of this transform, if it exists.
        /// </summary>
        /// <value>A <see cref="Matrix"/> that represents the inverse of the transformation applied by
        /// this instance, or <see langword="null"/> if the transform has no inverse.</value>
        public abstract Matrix? Inverse
        {
            get;
        }

        // Property values.
        private static readonly Transform identity = new IdentityTransform();
    }
}
