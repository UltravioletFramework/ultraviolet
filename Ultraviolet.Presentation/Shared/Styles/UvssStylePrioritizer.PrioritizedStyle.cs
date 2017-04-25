using System;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssStylePrioritizer
    {
        /// <summary>
        /// Represents a prioritized style.
        /// </summary>
        private struct PrioritizedStyle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PrioritizedStyle"/> structure.
            /// </summary>
            /// <param name="style">The style being applied.</param>
            /// <param name="selector">The selector which caused the style to be applied.</param>
            /// <param name="priority">The style's priority.</param>
            /// <param name="index">The index of the style's rule set within its style sheet.</param>
            public PrioritizedStyle(UvssRule style, UvssSelector selector, Int32 priority, Int32 index)
            {
                this.Style = style;
                this.Selector = selector;
                this.Priority = priority;
                this.Index = index;
            }

            /// <summary>
            /// The style being applied.
            /// </summary>
            public readonly UvssRule Style;

            /// <summary>
            /// The selector which caused this style to be applied.
            /// </summary>
            public readonly UvssSelector Selector;

            /// <summary>
            /// The style's priority.
            /// </summary>
            public readonly Int32 Priority;

            /// <summary>
            /// The index of the style's rule set within its style sheet.
            /// </summary>
            public readonly Int32 Index;
        }
    }
}
