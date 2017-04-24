using System;
using System.Collections.Generic;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a comparer which uses a provided function to compare two objects.
    /// </summary>
    /// <typeparam name="T">The type of object being compared.</typeparam>
    public sealed class FunctorComparer<T> : Comparer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctorComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">The function used to compare objects.</param>
        public FunctorComparer(Func<T, T, Int32> comparer)
        {
            Contract.Require(comparer, nameof(comparer));

            this.comparer = comparer;
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A value indicating the relative order of the specified objects.</returns>
        public override Int32 Compare(T x, T y)
        {
            return comparer(x, y);
        }

        // The comparer function.
        private readonly Func<T, T, Int32> comparer;
    }
}
