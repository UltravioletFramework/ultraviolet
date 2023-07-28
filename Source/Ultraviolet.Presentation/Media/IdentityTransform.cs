using System;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents an identity transformation.
    /// </summary>
    [UvmlKnownType]
    public sealed class IdentityTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix Value
        {
            get { return Matrix.Identity; }
        }

        /// <inheritdoc/>
        public override Matrix? Inverse
        {
            get { return Matrix.Identity; }
        }

        /// <inheritdoc/>
        public override Boolean IsIdentity
        {
            get { return true; }
        }
    }
}
