using System;
using System.Runtime.CompilerServices;

namespace Ultraviolet
{
    /// <summary>
    /// Contains helper methods for working with array segments.
    /// </summary>
    internal static class ArraySegmentExtensions
    {
        /// <summary>
        /// Gets a reference to the item at the specified index within the array segment.
        /// </summary>
        /// <typeparam name="T">The type of element contained within the array segment.</typeparam>
        /// <param name="this">The array segment.</param>
        /// <param name="index">The index to retrieve.</param>
        /// <returns>The item at the specified index within the array segment.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetItemRef<T>(this ArraySegment<T> @this, Int32 index) => ref @this.Array[@this.Offset + index];

#if !NETSTANDARD21
        /// <summary>
        /// Copies the contents of an array segment into another array segment.
        /// </summary>
        /// <typeparam name="T">The type of element contained within the array segments.</typeparam>
        /// <param name="this">The source array segment.</param>
        /// <param name="destination">The destination array segment.</param>
        public static void CopyTo<T>(this ArraySegment<T> @this, ArraySegment<T> destination)
        {
            Array.Copy(@this.Array, @this.Offset, destination.Array, destination.Offset, @this.Count);
        }
#endif
    }
}
