using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a property trigger's collection of conditions.
    /// </summary>
    public sealed partial class PropertyTriggerConditionCollection
    {
        /// <summary>
        /// Gets a value indicating whether the specified dependency object satisfies all of the conditions in the collection.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object to evaluate.</param>
        /// <returns><c>true</c> if the specified object satisfies all of the collection's conditions; otherwise, <c>false</c>.</returns>
        internal Boolean Evaluate(UltravioletContext uv, DependencyObject dobj)
        {
            Contract.Require(uv, "uv");
            Contract.Require(dobj, "dobj");

            foreach (var condition in conditions)
            {
                if (!condition.Evaluate(uv, dobj))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        internal void Clear()
        {
            conditions.Clear();
        }

        /// <summary>
        /// Adds the specified item to the collection.
        /// </summary>
        /// <param name="condition">The item to add to the collection.</param>
        internal void Add(PropertyTriggerCondition condition)
        {
            Contract.Require(condition, "condition");

            conditions.Add(condition);
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="condition">The item to remove from the collection.</param>
        internal Boolean Remove(PropertyTriggerCondition condition)
        {
            Contract.Require(condition, "condition");

            return conditions.Remove(condition);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="condition">The item to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
        internal Boolean Contains(PropertyTriggerCondition condition)
        {
            Contract.Require(condition, "condition");

            return conditions.Contains(condition);
        }

        /// <summary>
        /// Gets the number of conditions in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return conditions.Count; }
        }

        // The collection's underlying list of conditions.
        private readonly List<PropertyTriggerCondition> conditions = 
            new List<PropertyTriggerCondition>();
    }
}
