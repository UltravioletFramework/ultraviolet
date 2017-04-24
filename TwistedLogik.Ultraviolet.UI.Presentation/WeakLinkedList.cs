using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a linked list which tracks its items with weak references.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the linked list.</typeparam>
    internal sealed class WeakLinkedList<T> where T : class
    {
        /// <summary>
        /// Performs the specified action on every item in the list.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public void ForEach(Func<T, Boolean> action)
        {
            Contract.Require(action, nameof(action));

            temp.Clear();
            try
            {
                lock (storage)
                {
                    for (var current = storage.First; current != null; current = current.Next)
                    {
                        var target = current.Value.Target;
                        if (target == null)
                        {
                            storage.Remove(current);
                        }
                        else
                        {
                            temp.Add((T)target);
                        }
                    }
                }

                foreach (var item in temp)
                {
                    if (!action(item))
                        break;
                }
            }
            finally
            {
                temp.Clear();
            }
        }

        /// <summary>
        /// Adds a value to the beginning of the list.
        /// </summary>
        /// <param name="value">The value to add to the list.</param>
        public void AddFirst(T value)
        {
            Contract.Require(value, nameof(value));

            var weakref = new WeakReference(value);
            lock (storage)
            {
                storage.AddFirst(weakref);
            }
        }

        /// <summary>
        /// Adds a value to the end of the list.
        /// </summary>
        /// <param name="value">The value to add to the list.</param>
        public void AddLast(T value)
        {
            Contract.Require(value, nameof(value));

            var weakref = new WeakReference(value);
            lock (storage)
            {
                storage.AddLast(weakref);
            }
        }

        /// <summary>
        /// Removes a value from the list.
        /// </summary>
        /// <param name="value">The value to remove from the list.</param>
        public void Remove(T value)
        {
            Contract.Require(value, nameof(value));

            lock (storage)
            {
                for (var current = storage.First; current != null; current = current.Next)
                {
                    var target = current.Value.Target;
                    if (target == value)
                    {
                        storage.Remove(current);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        // State values.
        private readonly LinkedList<WeakReference> storage = new LinkedList<WeakReference>();
        private readonly List<T> temp = new List<T>();
    }
}
