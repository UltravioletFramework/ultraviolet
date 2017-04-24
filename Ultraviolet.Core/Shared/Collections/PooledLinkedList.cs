using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a linked list that draws its nodes from a pre-allocated pool.
    /// </summary>
    /// <remarks>The Base Class Library's built-in <see cref="LinkedList{T}"/> class will allocate a new instance of
    /// <see cref="LinkedListNode{T}"/> whenever it needs one, making it difficult to use in performance scenarios
    /// which are sensitive to garbage collection, like games. Ultraviolet <see cref="PooledLinkedList{T}"/> maintains 
    /// an internal pool of nodes which it uses instead of allocating.</remarks>
    /// <typeparam name="T">The type of item contained by the list.</typeparam>
    public class PooledLinkedList<T> : IEnumerable<T>
    {
        /// <summary>
        /// Initializes a new instance of the PooledLinkedList class.
        /// </summary>
        public PooledLinkedList()
            : this(10)
        {

        }

        /// <summary>
        /// Initializes a new instance of the PooledLinkedList class.
        /// </summary>
        /// <param name="capacity">The linked list's initial capacity.</param>
        public PooledLinkedList(Int32 capacity)
        {
            Contract.EnsureRange(capacity >= 1, nameof(capacity));

            NodePool = new List<LinkedListNode<T>>(capacity);

            for (int i = 0; i < capacity; i++)
                NodePool.Add(new LinkedListNode<T>(default(T)));
        }

        /// <summary>
        /// Finds the first node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the list.</param>
        /// <returns>The first node that contains the specified value, if one exists; otherwise, null.</returns>
        public LinkedListNode<T> Find(T value)
        {
            return List.Find(value);
        }

        /// <summary>
        /// Finds the last node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the list.</param>
        /// <returns>The last node that contains the specified value, if one exists; otherwise, null.</returns>
        public LinkedListNode<T> FindLast(T value)
        {
            return List.FindLast(value);
        }

        /// <summary>
        /// Clears the linked list's contents.
        /// </summary>
        public void Clear()
        {
            var node = List.First;
            var next = List.First;
            while (node != null)
            {
                next = node.Next;
                ReleaseNode(node);
                node = next;
            }
            List.Clear();
        }

        /// <summary>
        /// Adds the specified item to the beginning of the linked list.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void AddFirst(T item)
        {
            var node = RetrieveNode();
            node.Value = item;
            List.AddFirst(node);
        }

        /// <summary>
        /// Adds the specified item to the end of the linked list.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void AddLast(T item)
        {
            var node = RetrieveNode();
            node.Value = item;
            List.AddLast(node);
        }

        /// <summary>
        /// Adds the specified new node after the specified existing node.
        /// </summary>
        /// <param name="node">The node after which to insert a new value.</param>
        /// <param name="item">The value to insert.</param>
        public void AddAfter(LinkedListNode<T> node, T item)
        {
            Contract.Require(node, nameof(node));
            Contract.Ensure(node.List == List, CoreStrings.ListNodeDoesNotBelongToList);

            var value = RetrieveNode();
            value.Value = item;
            List.AddAfter(node, value);
        }

        /// <summary>
        /// Adds the specified new node before the specified existing node.
        /// </summary>
        /// <param name="node">The node before which to insert a new value.</param>
        /// <param name="item">The value to insert.</param>
        public void AddBefore(LinkedListNode<T> node, T item)
        {
            Contract.Require(node, nameof(node));
            Contract.Ensure(node.List == List, CoreStrings.ListNodeDoesNotBelongToList);

            var value = RetrieveNode();
            value.Value = item;
            List.AddBefore(node, value);
        }

        /// <summary>
        /// Removes the first occurrence of the specified item from the list.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        /// <returns><see langword="true"/> if the specified item was removed from the list; otherwise, <see langword="false"/>.</returns>
        public bool Remove(T item)
        {
            var node = List.Find(item);
            if (node != null)
            {
                List.Remove(node);
                ReleaseNode(node);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the specified node from the list.
        /// </summary>
        /// <param name="node">The node to remove from the list.</param>
        /// <returns><see langword="true"/> if the specified item was removed from the list; otherwise, <see langword="false"/>.</returns>
        public bool Remove(LinkedListNode<T> node)
        {
            Contract.Require(node, nameof(node));
            Contract.Ensure(node.List == List, CoreStrings.ListNodeDoesNotBelongToList);

            List.Remove(node);
            ReleaseNode(node);
            return true;
        }

        /// <summary>
        /// Removes the node at the beginning of the list.
        /// </summary>
        public void RemoveFirst()
        {
            var node = List.First;
            List.RemoveFirst();
            ReleaseNode(node);
        }

        /// <summary>
        /// Removes the node at the end of the list.
        /// </summary>
        public void RemoveLast()
        {
            var node = List.Last;
            List.RemoveLast();
            ReleaseNode(node);
        }

        /// <summary>
        /// Gets a value indicating whether the linked list contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the linked list.</param>
        /// <returns><see langword="true"/> if the linked list contains the specified value; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns>An enumerator for the list.</returns>
        public LinkedList<T>.Enumerator GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns>An enumerator for the list.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns>An enumerator for the list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public Int32 Count
        {
            get { return List.Count; }
        }

        /// <summary>
        /// Gets the first node of the linked list.
        /// </summary>
        public LinkedListNode<T> First
        {
            get { return List.First; }
        }

        /// <summary>
        /// Gets the last node of the linked list.
        /// </summary>
        public LinkedListNode<T> Last
        {
            get { return List.Last; }
        }

        /// <summary>
        /// Retrieves a node from the list's pool of nodes.
        /// </summary>
        /// <returns>The node that was retrieved.</returns>
        private LinkedListNode<T> RetrieveNode()
        {
            LinkedListNode<T> node;
            if (NodePool.Count == 0)
            {
                node = new LinkedListNode<T>(default(T));
                NodePool.Capacity++;
            }
            else
            {
                node = NodePool[NodePool.Count - 1];
                NodePool.RemoveAt(NodePool.Count - 1);
            }
            return node;
        }

        /// <summary>
        /// Releases a node back into the list's pool of nodes.
        /// </summary>
        /// <param name="node">The node to release.</param>
        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            NodePool.Add(node);
        }

        // The underlying linked list and its pool of nodes.
        private readonly LinkedList<T> List = new LinkedList<T>();
        private readonly List<LinkedListNode<T>> NodePool;
    }
}
