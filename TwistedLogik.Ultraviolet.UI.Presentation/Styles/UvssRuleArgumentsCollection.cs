using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of arguments belonging to a styling rule.
    /// </summary>
    public sealed partial class UvssRuleArgumentsCollection : IEnumerable<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleArgumentsCollection"/> class.
        /// </summary>
        /// <param name="args">The styling rule's collection of arguments.</param>
        internal UvssRuleArgumentsCollection(IEnumerable<String> args)
        {
            if (args != null)
            {
                this.arguments.AddRange(args);
            }
        }

        /// <inheritdoc/>
        public override String ToString() =>
            String.Join(", ", arguments.Select(x => String.Format("\"{0}\"", x)));

        /// <summary>
        /// Gets the argument at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the argument to retrieve.</param>
        /// <returns>The argument at the specified index within the collection.</returns>
        public String this[Int32 ix]
        {
            get { return arguments[ix]; }
        }

        /// <summary>
        /// Gets the number of arguments in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return arguments.Count; }
        }

        // State values.
        private readonly List<String> arguments = 
            new List<String>();
    }
}
