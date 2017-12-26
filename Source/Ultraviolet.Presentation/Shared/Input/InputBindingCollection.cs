using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a collection of <see cref="InputBinding"/> objects.
    /// </summary>
    public class InputBindingCollection : UltravioletCollection<InputBinding>, IList<InputBinding>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputBindingCollection"/> class.
        /// </summary>
        public InputBindingCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBindingCollection"/> class.
        /// </summary>
        /// <param name="commandBindings">The collection whose elements should be added to the end of the list.</param>
        public InputBindingCollection(IEnumerable<InputBinding> commandBindings)
        {
            if (commandBindings != null)
            {
                AddRange(commandBindings);
            }
        }

        /// <summary>
        /// Copies the collection to an array.
        /// </summary>
        /// <param name="array">The array into which the collection should be copied.</param>
        /// <param name="arrayIndex">The zero-based index within <paramref name="array"/> at which to begin copying.</param>
        public void CopyTo(InputBinding[] array, Int32 arrayIndex)
        {
            Storage.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ClearInternal();
        }

        /// <inheritdoc/>
        public void Add(InputBinding item)
        {
            Contract.Require(item, nameof(item));

            AddInternal(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the list.</param>
        public void AddRange(IEnumerable<InputBinding> collection)
        {
            Contract.Require(collection, nameof(collection));

            AddRangeInternal(collection);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, InputBinding item)
        {
            Contract.Require(item, nameof(item));

            InsertInternal(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            RemoveAtInternal(index);
        }

        /// <inheritdoc/>
        public Boolean Remove(InputBinding item)
        {
            Contract.Require(item, nameof(item));

            return RemoveInternal(item);
        }

        /// <inheritdoc/>
        public Boolean Contains(InputBinding item)
        {
            Contract.Require(item, nameof(item));

            return ContainsInternal(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(InputBinding item)
        {
            Contract.Require(item, nameof(item));

            return IndexOfInternal(item);
        }

        /// <inheritdoc/>
        public new InputBinding this[Int32 index]
        {
            get { return base[index]; }
            set
            {
                Contract.Require(value, nameof(value));

                base[index] = value;
            }   
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly => false; 
    }
}
