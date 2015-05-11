using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class PropertyTriggerConditionCollection : IEnumerable<TriggerCondition>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>The enumerator for the collection.</returns>
        public List<TriggerCondition>.Enumerator GetEnumerator()
        {
            return conditions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<TriggerCondition> IEnumerable<TriggerCondition>.GetEnumerator()
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
