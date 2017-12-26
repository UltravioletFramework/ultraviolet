using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class TriggerActionCollection : IEnumerable<TriggerAction>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<TriggerAction>.Enumerator GetEnumerator()
        {
            return actions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<TriggerAction> IEnumerable<TriggerAction>.GetEnumerator()
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
