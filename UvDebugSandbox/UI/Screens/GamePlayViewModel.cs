using System;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using UvDebugSandbox.UI.Dialogs;

namespace UvDebugSandbox.UI.Screens
{
    /// <summary>
    /// Represents the view model for <see cref="GamePlayScreen"/>.
    /// </summary>
    public sealed class GamePlayViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayViewModel"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="GameMenuScreen"/> that owns this view model.</param>
        /// <param name="escMenuDialog">The escape menu dialog box used by this screen.</param>
        public GamePlayViewModel(GamePlayScreen owner, EscMenuDialog escMenuDialog)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
            this.escMenuDialog = escMenuDialog;
        }

        /// <summary>
        /// Handles the <see cref="View.OpenedEvent"/> attached event.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        public void HandleViewOpened(DependencyObject dobj, ref RoutedEventData data)
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
        public void HandleKeyDown(DependencyObject dobj, KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            if (key == Key.Escape)
            {
                // NOTE: We need to execute the continuation synchronously because the Ultraviolet context is
                // funamentally not threadsafe. Doing otherwise can cause crashes.
                Modal.ShowDialogAsync(escMenuDialog).ContinueWith(HandleEscMenuDialogResult, 
                    TaskContinuationOptions.ExecuteSynchronously);

                data.Handled = true;
            }
        }
        
        /// <summary>
        /// Gets the <see cref="GameMenuScreen"/> that owns the view model.
        /// </summary>
        public GamePlayScreen Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the displayed triangle is spinning.
        /// </summary>
        public Boolean IsTriangleSpinning
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation of the displayed triangle.
        /// </summary>
        public Single TriangleRotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zoom of the displayed triangle.
        /// </summary>
        public Single TriangleZoom
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the result of the escape menu.
        /// </summary>
        private void HandleEscMenuDialogResult(Task<Boolean?> task)
        {
            if (task.Result ?? false)
            {
                ReturnToMainMenu();
            }
        }

        /// <summary>
        /// Moves the game back to the main menu screen.
        /// </summary>
        private void ReturnToMainMenu()
        {
            var screenClosing = owner;
            var screenOpening = owner.UIScreenService.Get<GameMenuScreen>();
            owner.Ultraviolet.GetUI().GetScreens().CloseThenOpen(screenClosing, screenOpening);
        }
        
        // Property values.
        private readonly GamePlayScreen owner;
        
        // Component references.
        private readonly Grid container = null;
        private EscMenuDialog escMenuDialog;
    }
}
