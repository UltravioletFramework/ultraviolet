using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents one of a <see cref="Skin"/> instance's skeletal joints.
    /// </summary>
    public class SkinJoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinJoint"/> class.
        /// </summary>
        /// <param name="node">The joint's node.</param>
        /// <param name="inverseBindMatrix">The joint's inverse bind matrix.</param>
        public SkinJoint(ModelNode node, Matrix inverseBindMatrix)
        {
            Contract.Require(node, nameof(node));

            this.Node = node;
            this.InverseBindMatrix = inverseBindMatrix;
        }

        /// <summary>
        /// Gets the joint's node.
        /// </summary>
        public ModelNode Node { get; }

        /// <summary>
        /// Gets the joint's inverse bind matrix.
        /// </summary>
        public Matrix InverseBindMatrix { get; }
    }
}
