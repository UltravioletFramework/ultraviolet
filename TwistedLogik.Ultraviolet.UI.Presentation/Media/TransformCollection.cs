using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a collection of transformations.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public sealed partial class TransformCollection : IList<Transform>
    {
        /// <inheritdoc/>
        public Int32 IndexOf(Transform item)
        {
            return transforms.IndexOf(item);
        }

        /// <inheritdoc/>
        public void CopyTo(Transform[] array, Int32 arrayIndex)
        {
            transforms.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        [Preserve]
        public void Clear()
        {
            transforms.Clear();
        }

        /// <inheritdoc/>
        [Preserve]
        public void Add(Transform item)
        {
            transforms.Add(item);
        }

        /// <inheritdoc/>
        [Preserve]
        public void Insert(Int32 index, Transform item)
        {
            transforms.Insert(index, item);
        }

        /// <inheritdoc/>
        [Preserve]
        public void RemoveAt(Int32 index)
        {
            transforms.RemoveAt(index);
        }

        /// <inheritdoc/>
        [Preserve]
        public Boolean Remove(Transform item)
        {
            return transforms.Remove(item);
        }

        /// <inheritdoc/>
        [Preserve]
        public Boolean Contains(Transform item)
        {
            return transforms.Contains(item);
        }

        /// <inheritdoc/>
        public Transform this[Int32 index]
        {
            get { return transforms[index]; }
            set { transforms[index] = value; }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return transforms.Count; }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        // The collection's underlying storage.
        private readonly List<Transform> transforms = 
            new List<Transform>();
    }
}
