using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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
            /// <param name="style">The style being applied.</param>
            /// <param name="selector">The selector which caused the style to be applied.</param>
            /// <param name="priority">The style's priority.</param>
            public PrioritizedStyleData(UvssStyle style, UvssSelector selector, Int32 priority)
            {
                this.Style    = style;
                this.Selector = selector;
                this.Priority = priority;
            }

            /// <summary>
            /// The style being applied.
            /// </summary>
            public readonly UvssStyle Style;

            /// <summary>
            /// The selector which caused this style to be applied.
            /// </summary>
            public readonly UvssSelector Selector;

            /// <summary>
            /// The style's priority.
            /// </summary>
            public readonly Int32 Priority;
        }
    }
}
