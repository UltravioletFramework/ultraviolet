using System;
using System.Threading.Tasks;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for displaying modal dialogs.
    /// </summary>
    public abstract class Modal : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modal"/> class.
        /// </summary>
        public Modal()
        {
            this.onClosedHandler = OnClosed;
        }

        /// <summary>
        /// Opens the specified modal dialog box in the primary window.
        /// </summary>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <see langword="null"/> to use the default transition time.</param>
        public static void ShowDialog(Modal modal, TimeSpan? duration = null)
        {
            ShowDialog(null, modal, duration);
        }

        /// <summary>
        /// Opens the specified modal dialog box.
        /// </summary>
        /// <param name="window">The window in which to open the modal dialog box.</param>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <see langword="null"/> to use the default transition time.</param>
        public static void ShowDialog(IUltravioletWindow window, Modal modal, TimeSpan? duration = null)
        {
            Contract.Require(modal, nameof(modal));

            modal.Show(window, duration);
        }

        /// <summary>
        /// Opens the specified modal dialog box in the primary window and returns a <see cref="Task"/> that completes
        /// when the modal dialog box is closed.
        /// </summary>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <see langword="null"/> to use the default transition time.</param>
        /// <returns>A <see cref="ModalTask{T}"/> that completes when the modal dialog box is closed.</returns>
        public static ModalTask<Boolean?> ShowDialogAsync(Modal modal, TimeSpan? duration = null)
        {
            return ShowDialogAsync(null, modal, duration);
        }

        /// <summary>
        /// Opens the specified modal dialog box and returns a <see cref="Task"/> that completes
        /// when the modal dialog box is closed.
        /// </summary>
        /// <param name="window">The window in which to open the modal dialog box.</param>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <see langword="null"/> to use the default transition time.</param>
        /// <returns>A <see cref="ModalTask{T}"/> that completes when the modal dialog box is closed.</returns>
        public static ModalTask<Boolean?> ShowDialogAsync(IUltravioletWindow window, Modal modal, TimeSpan? duration = null)
        {
            Contract.Require(modal, nameof(modal));

            return modal.ShowAsync(window, duration);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Closes the modal.
        /// </summary>
        /// <param name="duration">The amount of time over which to transition the modal's state,
        /// or <see langword="null"/> to use the default time.</param>
        public void Close(TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var screen = Screen;
            if (screen != null)
            {
                var uv = screen.Ultraviolet;
                uv.GetUI().GetScreens(screen.Window).Close(screen, duration);
            }
        }

        /// <summary>
        /// Closes the modal with the specified result value.
        /// </summary>
        /// <param name="dialogResult">The dialog's result value.</param>
        /// <param name="duration">The amount of time over which to transition the modal's state,
        /// or <see langword="null"/> to use the default time.</param>
        public void Close(Boolean? dialogResult, TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.dialogResult = dialogResult;

            var screen = Screen;
            if (screen != null)
            {
                var uv = screen.Ultraviolet;
                uv.GetUI().GetScreens(screen.Window).Close(screen, duration);
            }
        }

        /// <summary>
        /// Gets the screen that this modal represents.
        /// </summary>
        public abstract UIScreen Screen
        {
            get;
        }

        /// <summary>
        /// Gets the dialog's result value.
        /// </summary>
        public Boolean? DialogResult => dialogResult;

        /// <summary>
        /// Gets a value indicating whether the modal is currently open.
        /// </summary>
        public Boolean IsOpen => open;

        /// <summary>
        /// Gets a value indicating whether the modal is currently closed.
        /// </summary>
        public Boolean IsClosed => !open;

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed => disposed;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (Screen != null)
                {
                    Screen.Closed -= onClosedHandler;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Called when the modal is being opened.
        /// </summary>
        protected virtual void OnOpening()
        {

        }

        /// <summary>
        /// Called when the modal is being closed.
        /// </summary>
        protected virtual void OnClosing()
        {

        }

        /// <summary>
        /// Shows the modal.
        /// </summary>
        /// <param name="window">The window in which to show the modal.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <see langword="null"/> to use the default transition time.</param>
        private void Show(IUltravioletWindow window, TimeSpan? duration = null)
        {
            var screen = Screen;
            if (screen == null || open)
                return;

            open = true;
            dialogResult = null;

            this.taskCompletionSource = null;

            OnOpening();
            Open(window, screen, duration);
        }

        /// <summary>
        /// Shows the modal and returns a <see cref="Task"/> which completes when
        /// the modal is closed.
        /// </summary>
        /// <param name="window">The window in which to show the modal.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <see langword="null"/> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which completes when the modal is closed.</returns>
        private ModalTask<Boolean?> ShowAsync(IUltravioletWindow window, TimeSpan? duration = null)
        {
            var screen = Screen;
            if (screen == null)
                return new ModalTask<Boolean?>(new Task<Boolean?>(() => true));

            var wasOpen = open;

            open = true;
            dialogResult = null;

            this.taskCompletionSource = new TaskCompletionSource<Boolean?>();

            if (wasOpen)
                return new ModalTask<Boolean?>(this.taskCompletionSource.Task);

            OnOpening();
            Open(window, screen, duration);

            return new ModalTask<Boolean?>(this.taskCompletionSource.Task);
        }

        /// <summary>
        /// Shows the modal and returns a <see cref="Task"/> which completes when
        /// the modal is closed.
        /// </summary>
        /// <param name="window">The window in which to show the modal.</param>
        /// <param name="screen">The screen on which the modal will be opened.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <see langword="null"/> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which completes when the modal is closed.</returns>
        private void Open(IUltravioletWindow window, UIScreen screen, TimeSpan? duration = null)
        {
            var screenStack = screen.Ultraviolet.GetUI().GetScreens(window);

            if (screen.State != UIPanelState.Closed)
                screenStack.Close(screen, TimeSpan.Zero);

            if (!hooked)
            {
                screen.Closed += onClosedHandler;
                hooked = true;
            }

            screenStack.Open(screen, duration);
        }

        /// <summary>
        /// Called when the modal is closed.
        /// </summary>
        /// <param name="panel">The panel that was closed.</param>
        private void OnClosed(UIPanel panel)
        {
            if (!open)
                return;

            OnClosing();

            if (this.taskCompletionSource != null)
            {
                var id = System.Threading.Thread.CurrentThread.ManagedThreadId;
                
                this.taskCompletionSource.SetResult(DialogResult);
            }

            this.taskCompletionSource = null;

            open = false;
        }

        // Property values.
        private Boolean? dialogResult;
        private Boolean open;
        private Boolean disposed;

        // State values.
        private TaskCompletionSource<Boolean?> taskCompletionSource;
        private UIPanelEventHandler onClosedHandler;
        private Boolean hooked;
    }
}
