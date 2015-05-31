using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the core implementation of the Ultraviolet UI subsystem.
    /// </summary>
    public sealed class UltravioletUI : UltravioletResource, IUltravioletUI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletUI"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public UltravioletUI(UltravioletContext uv, UltravioletConfiguration configuration)
            : base(uv)
        {
            screenStacks = new UIScreenStackCollection(uv);
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var stack in screenStacks)
            {
                stack.Update(time);
            }

            OnUpdating(time);
        }

        /// <summary>
        /// Gets the screen stack associated with the primary window.
        /// </summary>
        /// <returns>The <see cref="UIScreenStack"/> associated with the primary window.</returns>
        public UIScreenStack GetScreens()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

            return screenStacks[primary];
        }

        /// <summary>
        /// Gets the screen stack associated with the specified window.
        /// </summary>
        /// <param name="window">The window for which to retrieve a screen stack, 
        /// or <c>null</c> to retrieve the screen stack for the primary window.</param>
        /// <returns>The <see cref="UIScreenStack"/> associated with the specified window.</returns>
        public UIScreenStack GetScreens(IUltravioletWindow window)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (window == null) ? GetScreens() : screenStacks[window];
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Disposed)
            {
                SafeDispose.Dispose(screenStacks);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        // The collection of screens associated with each window.
        private readonly UIScreenStackCollection screenStacks;
    }
}
