using System;
using System.Collections.Generic;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.UI;
using UvDebug.UI.Screens;

namespace UvDebug.UI
{
    /// <summary>
    /// Represents a service which provides instances of UI screen types upon request.
    /// </summary>
    public sealed class UIScreenService : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the UIScreenService type.
        /// </summary>
        public UIScreenService(ContentManager globalContent)
            : base(globalContent?.Ultraviolet)
        {
            Contract.Require(globalContent, nameof(globalContent));

            Register(new GameMenuScreen(globalContent, this));
            Register(new GamePlayScreen(globalContent, this));
#if IMGUI
            Register(new ImGuiScreen(globalContent, this));
#endif
        }

        /// <summary>
        /// Gets an instance of the specified screen type.
        /// </summary>
        /// <returns>An instance of the specified screen type.</returns>
        public T Get<T>() where T : UIScreen
        {
            screens.TryGetValue(typeof(T), out var screen);
            return (T)screen;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                foreach (var kvp in screens)
                    kvp.Value.Dispose();

                screens.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Registers the screen instance that is returned for the specified type.
        /// </summary>
        /// <param name="instance">The instance to return for the specified type.</param>
        private void Register<T>(T instance) where T : UIScreen
        {
            screens[typeof(T)] = instance;
        }

        // State values.
        private readonly Dictionary<Type, UIScreen> screens = 
            new Dictionary<Type, UIScreen>();
    }
}
