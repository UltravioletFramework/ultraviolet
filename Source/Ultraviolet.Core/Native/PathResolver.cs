using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Represents an algorithm which enumerates the list of possible load targets
    /// for a given native library name.
    /// </summary>
    public abstract class PathResolver
    {
        /// <summary>
        /// Returns a collection which contains the list of possible load targets
        /// for the specified native library name.
        /// </summary>
        /// <param name="name">The name of the native library for which to enumerate load targets.</param>
        /// <returns>A collection which contains the retrieved list of load targets.</returns>
        public abstract IEnumerable<String> EnumeratePossibleLibraryLoadTargets(String name);

        /// <summary>
        /// Gets the default path resolution algorithm.
        /// </summary>
        public static PathResolver Default { get; } = new DefaultPathResolver();
    }
}
