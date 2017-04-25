using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a collection of <see cref="CommandBinding"/> objects.
    /// </summary>
    public class CommandBindingCollection : UltravioletCollection<CommandBinding>, IList<CommandBinding>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBindingCollection"/> class.
        /// </summary>
        public CommandBindingCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBindingCollection"/> class.
        /// </summary>
        /// <param name="commandBindings">The collection whose elements should be added to the end of the list.</param>
        public CommandBindingCollection(IEnumerable<CommandBinding> commandBindings)
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
        public void CopyTo(CommandBinding[] array, Int32 arrayIndex)
        {
            Storage.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ClearInternal();
        }

        /// <inheritdoc/>
        public void Add(CommandBinding item)
        {
            Contract.Require(item, nameof(item));

            AddInternal(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the list.</param>
        public void AddRange(IEnumerable<CommandBinding> collection)
        {
            Contract.Require(collection, nameof(collection));

            AddRangeInternal(collection);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, CommandBinding item)
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
        public Boolean Remove(CommandBinding item)
        {
            Contract.Require(item, nameof(item));

            return RemoveInternal(item);
        }

        /// <inheritdoc/>
        public Boolean Contains(CommandBinding item)
        {
            Contract.Require(item, nameof(item));

            return ContainsInternal(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(CommandBinding item)
        {
            Contract.Require(item, nameof(item));

            return IndexOfInternal(item);
        }

        /// <inheritdoc/>
        public new CommandBinding this[Int32 index]
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
