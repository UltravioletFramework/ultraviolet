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
            /// <param name="container">The name of the style's container, if it represents an attached property.</param>
            /// <param name="name">The name of the style.</param>
            /// <param name="value">The style's value.</param>
            /// <param name="priority">The style's priority.</param>
            public PrioritizedStyleData(String container, String name, String value, Int32 priority)
            {
                this.Value     = value;
                this.Container = container;
                this.Name      = name;
                this.Priority  = priority;
            }

            /// <summary>
            /// The name of the style's container, if it represents an attached property.
            /// </summary>
            public readonly String Container;

            /// <summary>
            /// The name of the style.
            /// </summary>
            public readonly String Name;

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
