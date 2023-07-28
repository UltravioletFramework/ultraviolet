using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a collection of transformations.
    /// </summary>
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
        public void Clear()
        {
            transforms.Clear();
        }

        /// <inheritdoc/>
        public void Add(Transform item)
        {
            transforms.Add(item);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, Transform item)
        {
            transforms.Insert(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            transforms.RemoveAt(index);
        }

        /// <inheritdoc/>
        public Boolean Remove(Transform item)
        {
            return transforms.Remove(item);
        }

        /// <inheritdoc/>
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
