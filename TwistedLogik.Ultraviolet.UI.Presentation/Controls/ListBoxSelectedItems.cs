using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the selected items in a list box.
    /// </summary>
    public sealed partial class ListBoxSelectedItems : IEnumerable<Object>
    {
        public Boolean Contains(Object item)
        {
            return storage.Contains(item);
        }

        public Int32 IndexOf(Object item)
        {
            return storage.IndexOf(item);
        }

        public void CopyTo(Object[] array)
        {
            storage.CopyTo(array);
        }

        public void CopyTo(Object[] array, Int32 arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        public Object this[Int32 index]
        {
            get { return storage[index]; }
        }

        public Int32 Count
        {
            get { return storage.Count; }
        }

        internal void Add(Object item)
        {
            storage.Add(item);
        }

        internal void Remove(Object item)
        {
            storage.Remove(item);
        }

        internal void Clear()
        {
            storage.Clear();
        }

        private readonly List<Object> storage = new List<Object>(8);

        public List<Object>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator<Object> IEnumerable<Object>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
