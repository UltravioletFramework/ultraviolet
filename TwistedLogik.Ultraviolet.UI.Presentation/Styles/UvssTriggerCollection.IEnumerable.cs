using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssTriggerCollection : IEnumerable<Trigger>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<Trigger>.Enumerator GetEnumerator()
        {
            return triggers.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Trigger> IEnumerable<Trigger>.GetEnumerator()
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
