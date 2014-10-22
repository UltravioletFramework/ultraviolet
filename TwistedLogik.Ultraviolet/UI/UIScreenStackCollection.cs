using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a collection of screen stacks organized by window.
    /// </summary>
    public sealed class UIScreenStackCollection : UltravioletCollection<UIScreenStack>, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreenStackCollection"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UIScreenStackCollection(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            var windows = uv.GetPlatform().Windows;

            foreach (var window in windows)
            {
                CreateScreenStack(window);
            }

            windows.WindowCreated   += WindowInfo_WindowCreated;
            windows.WindowDestroyed += WindowInfo_WindowDestroyed;

            this.spriteBatch = SpriteBatch.Create();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the screen stack associated with the specified window.
        /// </summary>
        /// <param name="window">The window for which to retrieve a screen stack.</param>
        /// <returns>The <see cref="UIScreenStack"/> associated with the specified window.</returns>
        public UIScreenStack this[IUltravioletWindow window]
        {
            get
            {
                Contract.Require(window, "window");

                UIScreenStack stack;
                if (!screenStacks.TryGetValue(window, out stack))
                    throw new ArgumentException(UltravioletStrings.InvalidWindow);

                return stack;
            }
        }

        /// <summary>
        /// Handles a window's DrawingUI event.
        /// </summary>
        /// <param name="window">The window being drawn.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void Window_DrawingUI(IUltravioletWindow window, UltravioletTime time)
        {
            screenStacks[window].Draw(time, spriteBatch);
        }

        /// <summary>
        /// Handles the window manager's WindowCreated event.
        /// </summary>
        /// <param name="window">The window that was created.</param>
        private void WindowInfo_WindowCreated(IUltravioletWindow window)
        {
            CreateScreenStack(window);
        }

        /// <summary>
        /// Handles the window manager's WindowDestroyed event.
        /// </summary>
        /// <param name="window">The window that was destroyed.</param>
        private void WindowInfo_WindowDestroyed(IUltravioletWindow window)
        {
            DestroyScreenStack(window);
        }

        /// <summary>
        /// Creates the specified window's screen stack.
        /// </summary>
        /// <param name="window">The window being created.</param>
        private void CreateScreenStack(IUltravioletWindow window)
        {
            var stack = new UIScreenStack(window);
            AddInternal(stack);
            screenStacks.Add(window, stack);
            window.DrawingUI += Window_DrawingUI;
        }

        /// <summary>
        /// Destroys the specified window's screen stack.
        /// </summary>
        /// <param name="window">The window being destroyed.</param>
        private void DestroyScreenStack(IUltravioletWindow window)
        {
            window.DrawingUI -= Window_DrawingUI;
            var stack = screenStacks[window];
            screenStacks.Remove(window);
            RemoveInternal(stack);
        }

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposing && !disposed)
            {
                SafeDispose.Dispose(spriteBatch);
            }
            disposed = true;
        }

        // The sprite batch with which screens are drawn.
        private readonly SpriteBatch spriteBatch;
        private Boolean disposed;

        // The registry of screen stacks for each window.
        private readonly Dictionary<IUltravioletWindow, UIScreenStack> screenStacks = 
            new Dictionary<IUltravioletWindow, UIScreenStack>();
    }
}
