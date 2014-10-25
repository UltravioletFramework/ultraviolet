using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate Texture2D Texture2DFactory(UltravioletContext uv, Int32 width, Int32 height);

    /// <summary>
    /// Represents a two-dimensional texture.
    /// </summary>
    public abstract class Texture2D : UltravioletResource, IComparable<Texture2D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Texture2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D Create(Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture2DFactory>()(uv, width, height);
        }
        
        /// <summary>
        /// Compares the texture with another texture and returns a value indicating whether the current
        /// instance comes before, after, or in the same position as the specified texture.
        /// </summary>
        /// <param name="other">The <see cref="Texture2D"/> to compare to this instance.</param>
        /// <returns>A value indicating the relative order of the objects being compared.</returns>
        public abstract Int32 CompareTo(Texture2D other);

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array being set as the texture's data.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        public abstract void SetData<T>(T[] data) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array being set as the texture's data.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        public abstract void SetData<T>(T[] data, Int32 offset, Int32 count) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the texture's data.</typeparam>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set, or <c>null</c> to set the entire texture.</param>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data, or zero to use the width of <paramref name="rect"/>.</param>
        public abstract void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, Int32 stride = 0) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set, or <c>null</c> to set the entire texture.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data, or zero to use the width of <paramref name="rect"/>.</param>
        /// <param name="format">The format of the data being set.</param>
        public abstract void SetData(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format);

        /// <summary>
        /// Gets the texture's width in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the texture's height in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

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
    }
}
