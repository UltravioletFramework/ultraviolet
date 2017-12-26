using System;
using System.Collections.Generic;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Media
{
    partial class VisualCollection
    {
        /// <summary>
        /// Represents a <see cref="Comparer{T}"/> which is used by <see cref="VisualCollection"/> to order
        /// elements based on their z-index values.
        /// </summary>
        private class ZIndexComparer : Comparer<Int32>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ZIndexComparer"/> class.
            /// </summary>
            /// <param name="owner">The <see cref="VisualCollection"/> that owns this comparer instance.</param>
            public ZIndexComparer(VisualCollection owner)
            {
                this.owner = owner;
            }

            /// <inheritdoc/>
            public override Int32 Compare(Int32 xIndex, Int32 yIndex)
            {
                var x = owner.storage[xIndex];
                var y = owner.storage[yIndex];

                var xZIndex = x.GetValue<Int32>(Panel.ZIndexProperty);
                var yZIndex = y.GetValue<Int32>(Panel.ZIndexProperty);
                if (xZIndex == yZIndex)
                {
                    return xIndex.CompareTo(yIndex);
                }
                return xZIndex.CompareTo(yZIndex);
            }

            /// <summary>
            /// Gets the <see cref="VisualCollection"/> that owns this comparer instance.
            /// </summary>
            public VisualCollection Owner
            {
                get { return owner; }
            }

            // Property values.
            private readonly VisualCollection owner;
        }
    }
}
