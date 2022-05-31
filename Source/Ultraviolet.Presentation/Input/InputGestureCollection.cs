using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a collection of <see cref="InputGesture"/> objects.
    /// </summary>
    public class InputGestureCollection : UltravioletCollection<InputGesture>, IList<InputGesture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputGestureCollection"/> class.
        /// </summary>
        public InputGestureCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputGestureCollection"/> class.
        /// </summary>
        /// <param name="inputGestures">The collection whose elements should be added to the end of the list.</param>
        public InputGestureCollection(IEnumerable<InputGesture> inputGestures)
        {
            if (inputGestures != null)
            {
                AddRange(inputGestures);
            }
        }

        /// <summary>
        /// Copies the collection to an array.
        /// </summary>
        /// <param name="array">The array into which the collection should be copied.</param>
        /// <param name="arrayIndex">The zero-based index within <paramref name="array"/> at which to begin copying.</param>
        public void CopyTo(InputGesture[] array, Int32 arrayIndex)
        {
            Storage.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Marks the collection as read-only.
        /// </summary>
        public void Seal()
        {
            isReadOnly = true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            ClearInternal();
        }

        /// <inheritdoc/>
        public void Add(InputGesture item)
        {
            Contract.Require(item, nameof(item));

            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            AddInternal(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the list.</param>
        public void AddRange(IEnumerable<InputGesture> collection)
        {
            Contract.Require(collection, nameof(collection));

            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            AddRangeInternal(collection);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, InputGesture item)
        {
            Contract.Require(item, nameof(item));

            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            InsertInternal(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            RemoveAtInternal(index);
        }

        /// <inheritdoc/>
        public Boolean Remove(InputGesture item)
        {
            Contract.Require(item, nameof(item));

            if (isReadOnly)
                throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

            return RemoveInternal(item);
        }

        /// <inheritdoc/>
        public Boolean Contains(InputGesture item)
        {
            Contract.Require(item, nameof(item));

            return ContainsInternal(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(InputGesture item)
        {
            Contract.Require(item, nameof(item));

            return IndexOfInternal(item);
        }

        /// <inheritdoc/>
        public new InputGesture this[Int32 index]
        {
            get { return base[index]; }
            set
            {
                Contract.Require(value, nameof(value));

                if (isReadOnly)
                    throw new NotSupportedException(CoreStrings.CannotModifyReadOnlyCollection);

                base[index] = value;
            }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly => isReadOnly;

        // Property values.
        private Boolean isReadOnly;
    }
}
