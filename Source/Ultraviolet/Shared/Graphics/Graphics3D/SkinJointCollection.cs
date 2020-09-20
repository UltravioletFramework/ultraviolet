using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of joints associated with a particular <see cref="Skin"/> instance.
    /// </summary>
    public class SkinJointCollection : IEnumerable<SkinJoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinJointCollection"/> class.
        /// </summary>
        /// <param name="joints">The joints to add to the collection.</param>
        public SkinJointCollection(IEnumerable<SkinJoint> joints)
        {
            this.joints = joints?.ToArray() ?? new SkinJoint[0];
        }

        /// <summary>
        /// Gets the joint at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the joint to retrieve.</param>
        /// <returns>The joint at the specified index within the collection.</returns>
        public SkinJoint this[Int32 index] => joints[index];

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> for the collection.
        /// </summary>
        /// <returns>An <see cref="ArrayEnumerator{T}"/> which will enumerate through the collection.</returns>
        ArrayEnumerator<SkinJoint> GetEnumerator() => new ArrayEnumerator<SkinJoint>(joints);

        /// <inheritdoc/>
        IEnumerator<SkinJoint> IEnumerable<SkinJoint>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of animations in the collection.
        /// </summary>
        public Int32 Count => joints.Length;

        // Animation collections.
        private readonly SkinJoint[] joints;
    }
}
