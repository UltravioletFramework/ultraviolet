using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the data from a <see cref="SkinnedAnimation"/> which is applied to a particular <see cref="ModelNode"/> instance.
    /// </summary>
    public class SkinnedModelNodeAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelNodeAnimation"/> class.
        /// </summary>
        /// <param name="node">The <see cref="ModelNode"/> to which this animation data is applied.</param>
        /// <param name="scale">The curve which animates the node's scale.</param>
        /// <param name="translation">The curve which animates the node's translation.</param>
        /// <param name="rotation">The curve which animates the node's rotation.</param>
        /// <param name="morphWeights">The curve which animates the node's morph weights.</param>
        public SkinnedModelNodeAnimation(ModelNode node, Curve<Vector3> scale, Curve<Vector3> translation, Curve<Quaternion> rotation, Curve<ArraySegment<Single>> morphWeights)
        {
            this.Node = node;
            this.Scale = scale;
            this.Translation = translation;
            this.Rotation = rotation;
            this.MorphWeights = morphWeights;
        }

        /// <summary>
        /// Gets the <see cref="ModelNode"/> to which this animation data is applied.
        /// </summary>
        public ModelNode Node { get; }

        /// <summary>
        /// Gets the curve which animates the node's scale.
        /// </summary>
        public Curve<Vector3> Scale { get; }

        /// <summary>
        /// Gets the curve which animates the node's translation.
        /// </summary>
        public Curve<Vector3> Translation { get; }

        /// <summary>
        /// Gets the curve which animates the node's rotation.
        /// </summary>
        public Curve<Quaternion> Rotation { get; }
        
        /// <summary>
        /// Gets the curve which animates the node's morph weights.
        /// </summary>
        public Curve<ArraySegment<Single>> MorphWeights { get; }
    }
}
