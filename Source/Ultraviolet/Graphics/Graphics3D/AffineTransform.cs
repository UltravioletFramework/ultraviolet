using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an 3D affine transformation consisting of scale, rotation, and translation.
    /// </summary>
    public class AffineTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransform"/> class.
        /// </summary>
        public AffineTransform()
        {
            UpdateFromIdentity();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransform"/> class.
        /// </summary>
        /// <param name="scale">The scaling component of the transformation.</param>
        /// <param name="rotation">The rotation component of the transformation.</param>
        /// <param name="translation">The translation component of the transformation.</param>
        public AffineTransform(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            this.scale = scale;
            this.rotation = rotation;
            this.translation = translation;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AffineTransform"/> class which is a clone of this instance.
        /// </summary>
        /// <returns>The <see cref="AffineTransform"/> instance which was created.</returns>
        public AffineTransform Clone() => new AffineTransform(this.scale, this.rotation, this.translation);

        /// <summary>
        /// Updates this <see cref="AffineTransform"/> instance to match the identity transform.
        /// </summary>
        public void UpdateFromIdentity()
        {
            this.rotation = Quaternion.Identity;
            this.scale = Vector3.One;
            this.translation = Vector3.Zero;
            this.matrix = Matrix.Identity;
        }

        /// <summary>
        /// Updates this <see cref="AffineTransform"/> instance to match the affine transformation
        /// described by the specified translation, rotation, and scale components.
        /// </summary>
        /// <param name="translation">The transform's translation component.</param>
        /// <param name="rotation">The transform's rotation component.</param>
        /// <param name="scale">The transform's scale component.</param>
        public void UpdateFromTranslationRotationScale(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            this.translation = translation;
            this.rotation = rotation;
            this.scale = scale;
            this.matrix = null;
        }

        /// <summary>
        /// Updates this <see cref="AffineTransform"/> instance to match the affine transformation
        /// described by the specified matrix.
        /// </summary>
        /// <param name="transform">An affine transformation matrix.</param>
        public void UpdateFromMatrix(in Matrix transform)
        {
            if (!transform.Decompose(out this.scale, out this.rotation, out this.translation))
                throw new ArgumentException(UltravioletStrings.NonAffineTransformationMatrix);

            this.matrix = transform;
        }
        
        /// <summary>
        /// Updates this <see cref="AffineTransform"/> instance to match the affine transformation
        /// described by the specified <see cref="AffineTransform"/> instance.
        /// </summary>
        /// <param name="transform">An affine transformation.</param>
        public void UpdateFromAffineTransform(AffineTransform transform)
        {
            Contract.Require(transform, nameof(transform));

            this.translation = transform.translation;
            this.rotation = transform.rotation;
            this.scale = transform.scale;
            this.matrix = transform.matrix;
        }

        /// <summary>
        /// Calculates the <see cref="Matrix"/> which represents this transformation.
        /// </summary>
        /// <param name="matrix">The matrix which represents this transformation.</param>
        public void AsMatrix(out Matrix matrix)
        {
            if (!this.matrix.HasValue)
            {
                Matrix.CreateScale(ref this.scale, out var matScale);
                Matrix.CreateFromQuaternion(ref this.rotation, out var matRotation);
                Matrix.Multiply(ref matScale, ref matRotation, out var matResult);
                matResult.Translation = Translation;
                this.matrix = matResult;
            }
            matrix = this.matrix.Value;
        }

        /// <summary>
        /// The rotation component of the transformation.
        /// </summary>
        public Quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                this.matrix = null;
            }
        }

        /// <summary>
        /// The scale component of the transformation.
        /// </summary>
        public Vector3 Scale
        {
            get => this.scale;
            set
            {
                this.scale = value;
                this.matrix = null;
            }
        }

        /// <summary>
        /// The translation component of the transformation.
        /// </summary>
        public Vector3 Translation
        {
            get => this.translation;
            set
            {
                this.translation = value;
                this.matrix = null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is an identity transformation.
        /// </summary>
        public Boolean IsIdentity
        {
            get
            {
                if (Rotation != Quaternion.Identity)
                    return false;

                if (Scale != Vector3.One)
                    return false;

                if (Translation != Vector3.Zero)
                    return false;

                return true;
            }
        }

        // Transformation values.
        private Quaternion rotation;
        private Vector3 scale;
        private Vector3 translation;
        private Matrix? matrix;
    }
}
