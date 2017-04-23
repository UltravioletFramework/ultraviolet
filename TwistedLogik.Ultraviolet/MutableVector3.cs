using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// The <see cref="MutableVector3"/> structure is a mutable version of the <see cref="Vector3"/> structure used 
    /// primarily for performance micro-optimizations within the Ultraviolet Framework.
    /// </summary>
    [Preserve(AllMembers = true)]
    public struct MutableVector3
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

        /// <summary>
        /// The vector's z-coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "z", Required = Required.Always)]
        public Single Z;
    }
}
