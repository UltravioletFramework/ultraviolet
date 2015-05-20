using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStylePrioritizer
    {
        /// <summary>
        /// Represents a prioritized trigger.
        /// </summary>
        private struct PrioritizedTrigger
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PrioritizedTrigger"/> structure.
            /// </summary>
            /// <param name="trigger">The trigger being applied.</param>
            /// <param name="selector">The selector which caused the trigger to be applied.</param>
            /// <param name="priority">The trigger's priority.</param>
            public PrioritizedTrigger(Trigger trigger, UvssSelector selector, Int32 priority)
            {
                this.Trigger  = trigger;
                this.Selector = selector;
                this.Priority = priority;
            }

            /// <summary>
            /// The trigger being applied.
            /// </summary>
            public readonly Trigger Trigger;

            /// <summary>
            /// The selector which caused this trigger to be applied.
            /// </summary>
            public readonly UvssSelector Selector;

            /// <summary>
            /// The trigger's priority.
            /// </summary>
            public readonly Int32 Priority;
        }
    }
}
