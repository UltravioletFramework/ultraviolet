using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssPropertyTriggerConditionCollection : IEnumerable<UvssPropertyTriggerCondition>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>The enumerator for the collection.</returns>
        public List<UvssPropertyTriggerCondition>.Enumerator GetEnumerator()
        {
            return conditions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssPropertyTriggerCondition> IEnumerable<UvssPropertyTriggerCondition>.GetEnumerator()
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
