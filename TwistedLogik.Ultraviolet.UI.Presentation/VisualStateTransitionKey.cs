using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a key which identifies a particular visual state transition.
    /// </summary>
    internal struct VisualStateTransitionKey : IEquatable<VisualStateTransitionKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateTransitionKey"/> structure.
        /// </summary>
        /// <param name="from">The visual state that is being transitioned from.</param>
        /// <param name="to">The visual state that is being transitioned to.</param>
        public VisualStateTransitionKey(VisualState from, VisualState to)
        {
            this.from = from;
            this.to   = to;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified transition keys are equal.
        /// </summary>
        /// <param name="vstk1">The first <see cref="VisualStateTransitionKey"/> to compare.</param>
        /// <param name="vstk2">The second <see cref="VisualStateTransitionKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified transition keys are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(VisualStateTransitionKey vstk1, VisualStateTransitionKey vstk2)
        {
            return vstk1.Equals(vstk2);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified transition keys are not equal.
        /// </summary>
        /// <param name="vstk1">The first <see cref="VisualStateTransitionKey"/> to compare.</param>
        /// <param name="vstk2">The second <see cref="VisualStateTransitionKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified transition keys are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(VisualStateTransitionKey vstk1, VisualStateTransitionKey vstk2)
        {
            return !vstk1.Equals(vstk2);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 29 + (from == null ? 0 : from.GetHashCode());
                hash = hash * 20 + (to == null ? 0 : to.GetHashCode());
                return hash;
            }
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (obj is VisualStateTransitionKey)
            {
                return Equals((VisualStateTransitionKey)obj);
            }
            return false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(VisualStateTransitionKey obj)
        {
            return this.from == obj.from && this.to == obj.to;
        }

        /// <summary>
        /// Gets the visual state that is being transitioned from.
        /// </summary>
        public VisualState From
        {
            get { return from; }
        }

        /// <summary>
        /// Gets the visual state that is being transition to.
        /// </summary>
        public VisualState To
        {
            get { return to; }
        }

        // Property values.
        private readonly VisualState from;
        private readonly VisualState to;
    }
}
