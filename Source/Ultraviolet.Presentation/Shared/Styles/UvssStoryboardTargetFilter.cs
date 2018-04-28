using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents the type filter which is applied to a particular storyboard target.
    /// </summary>
    public sealed partial class UvssStoryboardTargetFilter : IEnumerable<String>
    {
        /// <summary>
        /// Adds a type filter to the collection.
        /// </summary>
        /// <param name="type">The type filter to add to the collection.</param>
        internal void Add(String type)
        {
            Contract.Require(type, nameof(type));

            this.types.Add(type);
        }

        /// <summary>
        /// Gets the number of types in the filter.
        /// </summary>
        public Int32 Count
        {
            get { return types.Count; }
        }

        // State values.
        private readonly List<String> types = 
            new List<String>();
    }
}
