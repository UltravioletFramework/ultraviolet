using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class OutOfBandRenderer
    {
        /// <summary>
        /// A comparer which sorts instances of <see cref="UIElement"/> according to their layout depths.
        /// </summary>
        private class UIElementComparer : IComparer<UIElement>
        {
            /// <inheritdoc/>
            public Int32 Compare(UIElement x, UIElement y)
            {
                var depthComparison = y.LayoutDepth.CompareTo(x.LayoutDepth);
                if (depthComparison == 0)
                {
                    if (Object.ReferenceEquals(x, y))
                        return 0;

                    var hashCompare = y.GetHashCode().CompareTo(x.GetHashCode());
                    if (hashCompare == 0)
                    {
                        return y.GetType().TypeHandle.Value.ToInt64().CompareTo(x.GetType().TypeHandle.Value.ToInt64());
                    }
                    return hashCompare;
                }
                return depthComparison;
            }
        }
    }
}
