using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    partial class ItemCollection : IEnumerable<Object>
    {
        /// <inheritdoc/>
        public GenericEnumerator<Object> GetEnumerator()
        {
            if (IsBoundToItemsSource)
            {
                return new GenericEnumerator<Object>(this, (Object state, Int32 index, out Object result) =>
                {
                    throw new NotImplementedException();
                });
            }
            else
            {
                return new GenericEnumerator<Object>(this, (Object state, Int32 index, out Object result) =>
                {
                    throw new NotImplementedException();
                });
            }
        }

        /// <inheritdoc/>
        IEnumerator<Object> IEnumerable<Object>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
