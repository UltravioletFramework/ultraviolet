using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssTriggerCollection : IEnumerable<UvssTrigger>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<UvssTrigger>.Enumerator GetEnumerator()
        {
            return triggers.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssTrigger> IEnumerable<UvssTrigger>.GetEnumerator()
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
