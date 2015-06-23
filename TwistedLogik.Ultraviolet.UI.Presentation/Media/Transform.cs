using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a two-dimensional transformation.
    /// </summary>
    [UvmlKnownType]
    public abstract class Transform : GeneralTransform
    {
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
        public static Transform Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// Gets the <see cref="Matrix"/> that represents this transformation.
        /// </summary>
        /// <returns>A <see cref="Matrix"/> that represents the transformation.</returns>
        public abstract Matrix Value
        {
            get;
        }

        /// <summary>
        /// Gets the inverse of this transform, if it exists.
        /// </summary>
        public abstract Matrix? Inverse
        {
            get;
        }

        /// Property values.
        private static readonly Transform identity = new IdentityTransform();
    }
}
