using System;

namespace Ultraviolet.Presentation.Styles
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
            /// <param name="index">The index of the trigger's rule set within its style sheet.</param>
            public PrioritizedTrigger(UvssTrigger trigger, UvssSelector selector, Int32 priority, Int32 index)
            {
                this.Trigger = trigger;
                this.Selector = selector;
                this.Priority = priority;
                this.Index = index;
            }

            /// <summary>
            /// The trigger being applied.
            /// </summary>
            public readonly UvssTrigger Trigger;

            /// <summary>
            /// The selector which caused this trigger to be applied.
            /// </summary>
            public readonly UvssSelector Selector;

            /// <summary>
            /// The trigger's priority.
            /// </summary>
            public readonly Int32 Priority;

            /// <summary>
            /// The index of the trigger's rule set within its style sheet.
            /// </summary>
            public readonly Int32 Index;
        }
    }
}
