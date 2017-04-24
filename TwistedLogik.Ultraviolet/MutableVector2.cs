using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// The <see cref="MutableVector2"/> structure is a mutable version of the <see cref="Vector2"/> structure used 
    /// primarily for performance micro-optimizations within the Ultraviolet Framework.
    /// </summary>
    [Preserve(AllMembers = true)]
    public struct MutableVector2
    {
        /// <summary>
        /// The vector's x-coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Single X;

        /// <summary>
        /// The vector's y-coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Single Y;
    }
}
