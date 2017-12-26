using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a key which identifies a particular visual state transition.
    /// </summary>
    internal partial struct VisualStateTransitionKey : IEquatable<VisualStateTransitionKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateTransitionKey"/> structure.
        /// </summary>
        /// <param name="from">The visual state that is being transitioned from.</param>
        /// <param name="to">The visual state that is being transitioned to.</param>
        public VisualStateTransitionKey(VisualState from, VisualState to)
        {
            this.From = from;
            this.To = to;
        }
        
        /// <summary>
        /// Gets the visual state that is being transitioned from.
        /// </summary>
        public VisualState From { get; }

        /// <summary>
        /// Gets the visual state that is being transition to.
        /// </summary>
        public VisualState To { get; }
    }
}
