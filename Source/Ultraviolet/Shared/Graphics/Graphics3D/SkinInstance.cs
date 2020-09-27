using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an animated instance of a <see cref="Skin"/>. Each instance represents a particular
    /// skinned animation at a particular point in time.
    /// </summary>
    public class SkinInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinInstance"/> class.
        /// </summary>
        /// <param name="template">The <see cref="Skin"/> which serves as this instance's template.</param>
        /// <param name="parentModelInstance">The <see cref="SkinnedModelInstance"/> which represents this skin's parent model.</param>
        public SkinInstance(Skin template, SkinnedModelInstance parentModelInstance)
        {
            Contract.Require(template, nameof(template));
            Contract.Require(parentModelInstance, nameof(parentModelInstance));

            this.Template = template;
            this.ParentModelInstance = parentModelInstance;

            this.boneTransforms = new Matrix[template.Joints.Count];
            Reset();
        }

        /// <summary>
        /// Resets the skin instance to its default state.
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < boneTransforms.Length; i++)
                boneTransforms[i] = Matrix.Identity;
        }

        /// <summary>
        /// Updates the skin instance's bone transforms.
        /// </summary>
        public void Update()
        {
            for (var i = 0; i < boneTransforms.Length; i++)
            {
                var joint = Template.Joints[i];
                var jointInverseBindMatrix = joint.InverseBindMatrix;

                var node = joint.Node;
                var nodeInstance = ParentModelInstance.GetNodeInstanceByLogicalIndex(node.LogicalIndex);
                
                nodeInstance.GetWorldMatrix(out var worldMatrix);
                Matrix.Multiply(ref jointInverseBindMatrix, ref worldMatrix, out worldMatrix);

                boneTransforms[i] = worldMatrix;
            }
        }

        /// <summary>
        /// Gets the skin instance's bone transforms by copying them into the specified array.
        /// </summary>
        /// <param name="destination">The destination array into which the bone transforms will be copied.</param>
        public void GetBoneTransforms(Matrix[] destination)
        {
            Contract.Require(destination, nameof(destination));

            for (var i = 0; i < destination.Length && i < boneTransforms.Length; i++)
                destination[i] = boneTransforms[i];
        }

        /// <summary>
        /// Gets the skin instance's bone transforms by returning the instance's internal array.
        /// </summary>
        /// <returns>An array containing the skin instance's bone transforms.</returns>
        public Matrix[] GetBoneTransforms() => boneTransforms;

        /// <summary>
        /// Gets the <see cref="Skin"/> which serves as this instance's template.
        /// </summary>
        public Skin Template { get; }

        /// <summary>
        /// Gets the <see cref="SkinnedModelInstance"/> which represents this skin's parent model.
        /// </summary>
        public SkinnedModelInstance ParentModelInstance { get; }

        /// <summary>
        /// Gets the number of bones in this skin.
        /// </summary>
        public Int32 BoneCount => boneTransforms.Length;

        // State values.
        private readonly Matrix[] boneTransforms;
    }
}
