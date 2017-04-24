using System;
using Ultraviolet.Core;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace UltravioletSample.Sample13_UPFAdvanced.UI.Screens
{
    /// <summary>
    /// Represents the view model for <see cref="GameMenuScreen"/>.
    /// </summary>
    public sealed class GameMenuViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="GameMenuScreen"/> that owns this view model.</param>
        public GameMenuViewModel(GameMenuScreen owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }

        /// <summary>
        /// Handles the <see cref="View.OpenedEvent"/> attached event.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        public void HandleViewOpened(DependencyObject dobj, RoutedEventData data)
        {
            if (container != null)
                container.Focus();
        }

        /// <summary>
        /// Handles the <see cref="Keyboard.KeyDownEvent"/> attached event for the view's topmost <see cref="Grid"/> instance.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        public void HandleKeyDown(DependencyObject dobj, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            switch (key)
            {
                case Key.AppControlBack:
                    {
                        owner.Ultraviolet.Host.Exit();
                        data.Handled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event for the "Start" button.
        /// </summary>
        public void Click_Start(DependencyObject element, RoutedEventData data)
        {
            var playScreen = owner.UIScreenService.Get<GamePlayScreen>();
            owner.Ultraviolet.GetUI().GetScreens().CloseThenOpen(owner, playScreen);
        }

        /// <summary>
        /// Handles the Click event for the "Exit" button.
        /// </summary>
        public void Click_Exit(DependencyObject element, RoutedEventData data)
        {
            owner.Ultraviolet.Host.Exit();
        }
        
        /// <summary>
        /// Gets the <see cref="GameMenuScreen"/> that owns the view model.
        /// </summary>
        public GameMenuScreen Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// Gets a value indicating whether the screen's exit button is visible.
        /// </summary>
        public Boolean IsExitButtonVisible
        {
            get { return owner.Ultraviolet.Platform != UltravioletPlatform.iOS; }
        }

        // Property values.
        private readonly GameMenuScreen owner;

        // Component references.
        private readonly Grid container = null;
    }
}
