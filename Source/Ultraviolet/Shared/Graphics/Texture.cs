using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the base class for texture resources.
    /// </summary>
    public abstract class Texture : UltravioletResource, IComparable<Texture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Texture(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Compares the texture with another texture and returns a value indicating whether the current
        /// instance comes before, after, or in the same position as the specified texture.
        /// </summary>
        /// <param name="other">The <see cref="Texture"/> to compare to this instance.</param>
        /// <returns>A value indicating the relative order of the objects being compared.</returns>
        public abstract Int32 CompareTo(Texture other);

        /// <summary>
        /// Gets a value indicating whether the texture is bound to the device for reading.
        /// </summary>
        public abstract Boolean BoundForReading
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the texture is bound to the device for writing.
        /// </summary>
        public abstract Boolean BoundForWriting
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the texture is using immutable storage.
        /// </summary>
        public abstract Boolean ImmutableStorage
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the texture is optimized with the assumption that it will not be sampled. Textures
        /// which are thus optimized cannot be bound to a sampler or have their data set via the SetData() method.
        /// </summary>
        public abstract Boolean WillNotBeSampled
        {
            get;
        }
    }
}
