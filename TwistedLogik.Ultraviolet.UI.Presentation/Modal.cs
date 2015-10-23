using System;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for displaying modal dialogs.
    /// </summary>
    public abstract class Modal : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modal"/> class.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> that the modal represents.</param>
        public Modal()
        {
            this.onClosedHandler = OnClosed;
        }

        /// <summary>
        /// Opens the specified modal dialog box in the primary window.
        /// </summary>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <c>null</c> to use the default transition time.</param>
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
        /// box's state, or <c>null</c> to use the default transition time.</param>
        public static void ShowDialog(IUltravioletWindow window, Modal modal, TimeSpan? duration = null)
        {
            Contract.Require(modal, "modal");

            modal.Show(window, duration);
        }

        /// <summary>
        /// Opens the specified modal dialog box in the primary window and returns a <see cref="Task"/> that completes
        /// when the modal dialog box is closed.
        /// </summary>
        /// <param name="modal">The modal dialog box to open.</param>
        /// <param name="duration">The amount of time over which to transition the modal dialog 
        /// box's state, or <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> that completes when the modal dialog box is closed.</returns>
        public static Task<Boolean?> ShowDialogAsync(Modal modal, TimeSpan? duration = null)
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
        /// box's state, or <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> that completes when the modal dialog box is closed.</returns>
        public static Task<Boolean?> ShowDialogAsync(IUltravioletWindow window, Modal modal, TimeSpan? duration = null)
        {
            Contract.Require(modal, "modal");

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
        public void Close()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var screen = Screen;
            if (screen != null)
            {
                var uv = screen.Ultraviolet;
                uv.GetUI().GetScreens(screen.Window).Close(screen);
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
        /// Gets or sets the dialog's result value.
        /// </summary>
        public Boolean? DialogResult
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return dialogResult;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (dialogResult != value)
                {
                    dialogResult = value;
                    Close();
                }
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the modal is currently open.
        /// </summary>
        public Boolean IsOpen
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return open;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the modal is currently closed.
        /// </summary>
        public Boolean IsClosed
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return !open;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
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
        /// <param name="duration">The amount of time over which to transition the screen's state, or <c>null</c> to use the default transition time.</param>
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
        /// <param name="duration">The amount of time over which to transition the screen's state, or <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which completes when the modal is closed.</returns>
        private Task<Boolean?> ShowAsync(IUltravioletWindow window, TimeSpan? duration = null)
        {
            var screen = Screen;
            if (screen == null)
                return new Task<Boolean?>(() => true);

            var wasOpen = open;

            open = true;
            dialogResult = null;

            this.taskCompletionSource = new TaskCompletionSource<Boolean?>();

            if (wasOpen)
                return this.taskCompletionSource.Task;

            OnOpening();
            Open(window, screen, duration);

            return this.taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows the modal and returns a <see cref="Task"/> which completes when
        /// the modal is closed.
        /// </summary>
        /// <param name="window">The window in which to show the modal.</param>
        /// <param name="screen">The screen on which the modal will be opened.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <c>null</c> to use the default transition time.</param>
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
                this.taskCompletionSource.SetResult(DialogResult);

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
