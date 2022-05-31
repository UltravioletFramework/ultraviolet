using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a property trigger's collection of conditions.
    /// </summary>
    public sealed partial class UvssPropertyTriggerConditionCollection
    {
        /// <summary>
        /// Gets a value indicating whether the specified dependency object satisfies all of the conditions in the collection.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object to evaluate.</param>
        /// <returns><see langword="true"/> if the specified object satisfies all of the collection's conditions; otherwise, <see langword="false"/>.</returns>
        internal Boolean Evaluate(UltravioletContext uv, DependencyObject dobj)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));

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
        internal void Add(UvssPropertyTriggerCondition condition)
        {
            Contract.Require(condition, nameof(condition));

            conditions.Add(condition);
            UpdateCanonicalName();
        }

        /// <summary>
        /// Adds a set of conditions to this collection.
        /// </summary>
        /// <param name="conditions">The set of conditions to add to this collection.</param>
        internal void AddRange(IEnumerable<UvssPropertyTriggerCondition> conditions)
        {
            Contract.Require(conditions, nameof(conditions));

            this.conditions.AddRange(conditions);
            UpdateCanonicalName();
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="condition">The item to remove from the collection.</param>
        internal Boolean Remove(UvssPropertyTriggerCondition condition)
        {
            Contract.Require(condition, nameof(condition));

            return conditions.Remove(condition);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="condition">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified item; otherwise, <see langword="false"/>.</returns>
        internal Boolean Contains(UvssPropertyTriggerCondition condition)
        {
            Contract.Require(condition, nameof(condition));

            return conditions.Contains(condition);
        }

        /// <summary>
        /// Gets the number of conditions in the collection.
        /// </summary>
        internal Int32 Count
        {
            get { return conditions.Count; }
        }

        /// <summary>
        /// Gets the canonical name that uniquely identifies the set of conditions included in this collection.
        /// </summary>
        internal String CanonicalName
        {
            get { return canonicalName; }
        }

        /// <summary>
        /// Updates the collection's canonical name.
        /// </summary>
        private void UpdateCanonicalName()
        {
            var conditionStrings = conditions.OrderBy(x => x.PropertyName.QualifiedName).Select(x => String.Format("{0} {1} {{ {2} }}", 
                x.PropertyName.QualifiedName, 
                x.ComparisonOperation.ConvertToSymbol(), 
                x.PropertyValue));

            canonicalName = String.Join(", ", conditionStrings);
        }

        // The collection's underlying list of conditions.
        private String canonicalName;
        private readonly List<UvssPropertyTriggerCondition> conditions = 
            new List<UvssPropertyTriggerCondition>();
    }
}
