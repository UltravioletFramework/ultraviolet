using System;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    partial class Selector
    {
        /// <summary>
        /// Represents the metadata for a particular selected item.
        /// </summary>
        private struct SelectionMetadata
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SelectionMetadata"/> structure.
            /// </summary>
            /// <param name="container">The item container for the selected object.</param>
            /// <param name="index">The index of the selected object within the selection list.</param>
            public SelectionMetadata(DependencyObject container, Int32 index) 
            {
                this.container = container;
                this.index     = index;
            }

            /// <summary>
            /// Gets the item container for the selected object.
            /// </summary>
            public DependencyObject Container
            {
                get { return container; }
            }

            /// <summary>
            /// Gets the index of the selected object within the selection list.
            /// </summary>
            public Int32 Index
            {
                get { return index; }
            }

            // Property values.
            private readonly DependencyObject container;
            private readonly Int32 index;
        }
    }
}
