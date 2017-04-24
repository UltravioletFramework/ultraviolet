using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    partial class OutOfBandRenderer
    {
        /// <summary>
        /// A comparer which sorts instances of <see cref="UIElement"/> according to their layout depths.
        /// </summary>
        private class UIElementComparer : IComparer<WeakReference>
        {
            /// <inheritdoc/>
            public Int32 Compare(WeakReference x, WeakReference y)
            {
                var xElem = (UIElement)x.Target;
                var yElem = (UIElement)y.Target;

                if (ReferenceEquals(xElem, yElem))
                    return 0;

                var xLayoutDepth = (xElem == null) ? 0 : xElem.LayoutDepth;
                var yLayoutDepth = (yElem == null) ? 0 : yElem.LayoutDepth;

                var depthComparison = yLayoutDepth.CompareTo(xLayoutDepth);
                if (depthComparison == 0)
                {
                    var xHashCode = (xElem == null) ? 0 : xElem.GetHashCode();
                    var yHashCode = (yElem == null) ? 0 : yElem.GetHashCode();

                    var hashCompare = yHashCode.CompareTo(xHashCode);
                    if (hashCompare == 0)
                    {
                        var xTypeHandle = (xElem == null) ? 0L : xElem.GetType().TypeHandle.Value.ToInt64();
                        var yTypeHandle = (yElem == null) ? 0L : yElem.GetType().TypeHandle.Value.ToInt64();

                        return yTypeHandle.CompareTo(xTypeHandle);
                    }
                    return hashCompare;
                }
                return depthComparison;
            }
        }
    }
}