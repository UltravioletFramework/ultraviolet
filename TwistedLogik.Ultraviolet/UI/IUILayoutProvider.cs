using System;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the service which provides layouts to user interfaces.
    /// </summary>
    public interface IUILayoutProvider : IDisposable
    {
        /// <summary>
        /// Updates the provider's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        void Update(UltravioletTime time);

        /// <summary>
        /// Creates a new layout object.
        /// </summary>
        /// <param name="x">The x-coordinate of the layout's initial position.</param>
        /// <param name="y">The y-coordinate of the layout's initial position.</param>
        /// <param name="width">The layout's initial width in pixels.</param>
        /// <param name="height">The layout's initial height in pixels.</param>
        /// <returns>The <see cref="IUILayout"/> that was created.</returns>
        IUILayout CreateLayout(Int32 x, Int32 y, Int32 width, Int32 height);
    }
}
