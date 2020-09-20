using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents one of a <see cref="SkinnedModel"/> instance's skins.
    /// </summary>
    public class Skin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Skin"/> class.
        /// </summary>
        /// <param name="name">The skin's name.</param>
        /// <param name="joints">The skin's joints.</param>
        /// <param name="nodes">The skin's list of skinned nodes.</param>
        public Skin(String name, IEnumerable<SkinJoint> joints, IEnumerable<ModelNode> nodes)
        {
            this.Name = name;
            this.Joints = new SkinJointCollection(joints);
            this.Nodes = new SkinNodeCollection(nodes);
        }

        /// <summary>
        /// Gets the skin's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the skin's collection of joints.
        /// </summary>
        public SkinJointCollection Joints { get; }

        /// <summary>
        /// Gets the skin's collection of nodes.
        /// </summary>
        public SkinNodeCollection Nodes { get; }
    }
}
