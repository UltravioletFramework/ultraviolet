using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which renders skinned models.
    /// </summary>
    public interface IEffectSkin
    {
        /// <summary>
        /// Sets the array of bone transform matrices for this effect.
        /// </summary>
        /// <param name="boneTransforms">An array of bone transformation matrices.</param>
        void SetBoneTransforms(Matrix[] boneTransforms);

        /// <summary>
        /// Gets the array of bone transforms matrices for this effect.
        /// </summary>
        /// <param name="boneTransforms">The destination array to populate with bone transforms.</param>
        void GetBoneTransforms(Matrix[] boneTransforms);

        /// <summary>
        /// Gets the array of bone transforms matrices for this effect.
        /// </summary>
        /// <param name="boneTransforms">The destination array to populate with bone transforms.</param>
        /// <param name="count">The maximum number of bone transforms to copy into <paramref name="boneTransforms"/>.</param>
        void GetBoneTransforms(Matrix[] boneTransforms, Int32 count);

        /// <summary>
        /// Gets or sets the number of bone weights which are applied to each vertex.
        /// </summary>
        Int32 WeightsPerVertex { get; set; }

        /// <summary>
        /// Gets the maximum number of bones which are supported.
        /// </summary>
        Int32 MaxBoneCount { get; }
    }
}
