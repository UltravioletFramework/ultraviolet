using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class PropertyTriggerConditionCollection : IEnumerable<PropertyTriggerCondition>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>The enumerator for the collection.</returns>
        public List<PropertyTriggerCondition>.Enumerator GetEnumerator()
        {
            return conditions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<PropertyTriggerCondition> IEnumerable<PropertyTriggerCondition>.GetEnumerator()
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
