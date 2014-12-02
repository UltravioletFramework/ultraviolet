using System;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    partial class UvssDocument
    {
        /// <summary>
        /// Represents a prioritized style.
        /// </summary>
        private struct PrioritizedStyleData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PrioritizedStyleData"/> structure.
            /// </summary>
            /// <param name="value">The style's value.</param>
            /// <param name="priority">The style's priority.</param>
            public PrioritizedStyleData(String value, Int32 priority)
            {
                this.Value    = value;
                this.Priority = priority;
            }

            /// <summary>
            /// The style's value.
            /// </summary>
            public readonly String Value;

            /// <summary>
            /// The style's priority.
            /// </summary>
            public readonly Int32 Priority;
        }
    }
}
