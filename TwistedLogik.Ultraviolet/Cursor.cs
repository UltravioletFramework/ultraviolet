using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Cursor"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="surface">The surface that contains the cursor image.</param>
    /// <param name="hx">The x-coordinate of the cursor's hotspot.</param>
    /// <param name="hy">The y-coordinate of the cursor's hotspot.</param>
    /// <returns>The instance of <see cref="Cursor"/> that was created.</returns>
    public delegate Cursor CursorFactory(UltravioletContext uv, Surface2D surface, Int32 hx, Int32 hy);

    /// <summary>
    /// Represents a mouse cursor.
    /// </summary>
    public abstract class Cursor : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Cursor(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="surface">The <see cref="Surface2D"/> that contains the cursor image.</param>
        /// <returns>The instance of <see cref="Cursor"/> that was created.</returns>
        public static Cursor Create(Surface2D surface)
        {
            Contract.Require(surface, nameof(surface));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<CursorFactory>()(uv, surface, 0, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="surface">The <see cref="Surface2D"/> that contains the cursor image.</param>
        /// <param name="hx">The x-coordinate of the cursor's hotspot.</param>
        /// <param name="hy">The y-coordinate of the cursor's hotspot.</param>
        /// <returns>The instance of <see cref="Cursor"/> that was created.</returns>
        public static Cursor Create(Surface2D surface, Int32 hx, Int32 hy)
        {
            Contract.Require(surface, nameof(surface));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<CursorFactory>()(uv, surface, hx, hy);
        }

        /// <summary>
        /// Gets the width of the cursor in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the height of the cursor in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets the x-coordinate of the cursor's hotspot.
        /// </summary>
        public abstract Int32 HotspotX
        {
            get;
        }

        /// <summary>
        /// Gets the y-coordinate of the cursor's hotspot.
        /// </summary>
        public abstract Int32 HotspotY
        {
            get;
        }
    }
}
