using System;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the method that is called when a <see cref="UIPanel"/> raises an event.
    /// </summary>
    /// <param name="panel">The <see cref="UIPanel"/> that raised the event.</param>
    public delegate void UIPanelEventHandler(UIPanel panel);

    /// <summary>
    /// Represents the method that is called when a <see cref="UIPanel"/> is updated.
    /// </summary>
    /// <param name="panel">The <see cref="UIPanel"/> that raised the event.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
    public delegate void UIPanelUpdateEventHandler(UIPanel panel, UltravioletTime time);

    /// <summary>
    /// Represents the method that is called when a <see cref="UIPanel"/> is being drawn.
    /// </summary>
    /// <param name="panel">The <see cref="UIPanel"/> that raised the event.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    /// <param name="spriteBatch">The sprite batch with which the panel is being drawn.</param>
    public delegate void UIPanelDrawEventHandler(UIPanel panel, UltravioletTime time, SpriteBatch spriteBatch);

    /// <summary>
    /// Represents a user interface panel.
    /// </summary>
    public abstract class UIPanel : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanel"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        internal UIPanel(String rootDirectory, ContentManager globalContent)
            : this(null, rootDirectory, globalContent)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        internal UIPanel(UltravioletContext uv, String rootDirectory, ContentManager globalContent)
            : base(uv ?? UltravioletContext.DemandCurrent())
        {
            Contract.RequireNotEmpty(rootDirectory, "rootDirectory");
            Contract.Require(globalContent, "globalContent");

            this.vmfactory = new UIViewModelFactory(CreateViewModel);

            this.localContent = ContentManager.Create(rootDirectory);
            this.globalContent = globalContent;
        }

        /// <summary>
        /// Forces the panel to immediately finish loading its view, if it has
        /// a view and the view hasn't already been loaded.
        /// </summary>
        public void FinishLoadingView()
        {
            if (IsViewLoaded)
                return;

            this.isViewLoaded = true;

            if (definition != null)
            {
                var view = UIView.Create(this, definition, vmfactory);
                if (view != null)
                {
                    if (window != null)
                    {
                        var area = new Rectangle(X, Y, Width, Height);
                        view.SetViewPosition(window, area);
                    }
                }
                this.view = view;

                HandleViewLoaded();
            }
        }

        /// <summary>
        /// Updates the panel's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Draws the panel using the specified sprite batch.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the panel.</param>
        public abstract void Draw(UltravioletTime time, SpriteBatch spriteBatch);

        /// <summary>
        /// Gets the content manager which is used to load globally-available assets.
        /// </summary>
        public ContentManager GlobalContent
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return globalContent;
            }
        }

        /// <summary>
        /// Gets the content manager which is used to load assets which are local to this screen.
        /// </summary>
        public ContentManager LocalContent
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return localContent;
            }
        }

        /// <summary>
        /// Gets the screen's view, if it has one.
        /// </summary>
        public UIView View
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                FinishLoadingView();

                return view;
            }
        }

        /// <summary>
        /// Gets the panel's x-coordinate on the screen.
        /// </summary>
        public abstract Int32 X
        {
            get;
        }

        /// <summary>
        /// Gets the panel's y-coordinate on the screen.
        /// </summary>
        public abstract Int32 Y
        {
            get;
        }

        /// <summary>
        /// Gets the panel's size in pixels.
        /// </summary>
        public abstract Size2 Size
        {
            get;
        }

        /// <summary>
        /// Gets the panel's width in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the panel's height in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this panel is ready for input.
        /// </summary>
        public abstract Boolean IsReadyForInput
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this panel is ready for input which does
        /// not require the panel to be foremost on the window.
        /// </summary>
        public abstract Boolean IsReadyForBackgroundInput
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="UIPanelState"/> value that represents the panel's current transition state.
        /// </summary>
        public UIPanelState State
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return state;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the screen is in a transition state.
        /// </summary>
        public Boolean IsTransitioning
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return state != UIPanelState.Open && state != UIPanelState.Closed;
            }
        }

        /// <summary>
        /// Gets the panel's position within its current transition, if it is transitioning.
        /// A value of 0.0 indicates that the panel is closed, while a value of 1.0 indicates
        /// that the panel is open.
        /// </summary>
        public Single TransitionPosition
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (Single)transitionPosition;
            }
        }

        /// <summary>
        /// Gets the duration in milliseconds of the panel's current transition, if it is transitioning.
        /// </summary>
        public Single TransitionDuration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (Single)transitionDuration;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time over which the panel will transition to
        /// its open state if no time is explicitly specified.
        /// </summary>
        public TimeSpan DefaultOpenTransitionDuration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return defaultOpenTransitionDuration;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.Ensure<ArgumentException>(value.TotalMilliseconds >= 0, "value");

                defaultOpenTransitionDuration = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time over which the panel will transition to
        /// its closed state if no time is explicitly specified.
        /// </summary>
        public TimeSpan DefaultCloseTransitionDuration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return defaultCloseTransitionDuration;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.Ensure<ArgumentException>(value.TotalMilliseconds >= 0, "value");

                defaultCloseTransitionDuration = value;
            }
        }

        /// <summary>
        /// Gets the window to which the screen is drawn.
        /// </summary>
        public IUltravioletWindow Window
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.window;
            }
            internal set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.window = value;
            }
        }

        /// <summary>
        /// The screen stack for the panel's current window.
        /// </summary>
        public UIScreenStack Screens
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.window == null ? null : Ultraviolet.GetUI().GetScreens(this.window);
            }
        }

        /// <summary>
        /// Occurs when the panel is being updated.
        /// </summary>
        public event UIPanelUpdateEventHandler Updating;

        /// <summary>
        /// Occurs when the panel is drawing its background.
        /// </summary>
        public event UIPanelDrawEventHandler DrawingBackground;

        /// <summary>
        /// Occurs when the panel is drawing its layout.
        /// </summary>
        public event UIPanelDrawEventHandler DrawingLayout;

        /// <summary>
        /// Occurs when the panel is drawing its foreground.
        /// </summary>
        public event UIPanelDrawEventHandler DrawingForeground;

        /// <summary>
        /// Occurs when the panel begins opening.
        /// </summary>
        public event UIPanelEventHandler Opening;

        /// <summary>
        /// Occurs after the panel has opened.
        /// </summary>
        public event UIPanelEventHandler Opened;

        /// <summary>
        /// Occurs when the panel begins closing.
        /// </summary>
        public event UIPanelEventHandler Closing;

        /// <summary>
        /// Occurs after the panel has closed.
        /// </summary>
        public event UIPanelEventHandler Closed;

        /// <summary>
        /// Opens the panel.
        /// </summary>
        /// <param name="duration">The amount of time over which to transition the panel's state, or
        /// <c>null</c> to use the default transition time.</param>
        internal void Open(TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OpenInternal(duration ?? DefaultOpenTransitionDuration, false);
        }

        /// <summary>
        /// Asynchronously opens the panel.
        /// </summary>
        /// <param name="duration">The amount of time over which to transition the panel's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        internal Task OpenAsync(TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return OpenInternal(duration ?? DefaultOpenTransitionDuration, true);
        }

        /// <summary>
        /// Closes the panel.
        /// </summary>
        /// <param name="duration">The amount of time over which to transition the panel's state, or
        /// <c>null</c> to use the default transition time.</param>
        internal void Close(TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            CloseInternal(duration ?? DefaultCloseTransitionDuration, false);
        }

        /// <summary>
        /// Asynchronously closes the panel.
        /// </summary>
        /// <param name="duration">The amount of time over which to transition the panel's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        internal Task CloseAsync(TimeSpan? duration = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return CloseInternal(duration ?? DefaultCloseTransitionDuration, true);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(this.view);
                SafeDispose.Dispose(this.localContent);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a view model for the specified view.
        /// </summary>
        /// <param name="view">The view for which to create a view model.</param>
        /// <returns>The view model for the specified view, or <c>null</c> if the view has no view model.</returns>
        protected virtual Object CreateViewModel(UIView view)
        {
            return null;
        }

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">The Ultraviolet time.</param>
        protected virtual void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

        /// <summary>
        /// Raises the <see cref="DrawingBackground"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingBackground(UltravioletTime time, SpriteBatch spriteBatch) =>
            DrawingBackground?.Invoke(this, time, spriteBatch);

        /// <summary>
        /// Raises the <see cref="DrawingLayout"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingLayout(UltravioletTime time, SpriteBatch spriteBatch) =>
            DrawingLayout?.Invoke(this, time, spriteBatch);

        /// <summary>
        /// Raises the <see cref="DrawingForeground"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch) =>
            DrawingForeground?.Invoke(this, time, spriteBatch);

        /// <summary>
        /// Raises the <see cref="Opening"/> event.
        /// </summary>
        protected virtual void OnOpening() =>
            Opening?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Opened"/> event.
        /// </summary>
        protected virtual void OnOpened() =>
            Opened?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Closing"/> event.
        /// </summary>
        protected virtual void OnClosing() =>
            Closing?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClosed() =>
            Closed?.Invoke(this);

        /// <summary>
        /// Prepares the panel to load the view from the specified panel definition.
        /// </summary>
        /// <param name="definition">The panel definition from which to load the view.</param>
        protected void PrepareView(UIPanelDefinition definition)
        {
            Contract.Require(definition, nameof(definition));
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsViewLoaded)
                throw new InvalidOperationException(UltravioletStrings.ViewAlreadyLoaded);

            this.definition = definition;
        }

        /// <summary>
        /// Draws the panel's view.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected void DrawView(UltravioletTime time, SpriteBatch spriteBatch)
        {
            FinishLoadingView();

            if (view != null)
            {
                view.Draw(time, spriteBatch);
            }
        }

        /// <summary>
        /// Updates the panel's view.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected void UpdateView(UltravioletTime time)
        {
            FinishLoadingView();

            if (view != null && State != UIPanelState.Closed)
            {
                UpdateViewPosition();
                view.Update(time);
            }
        }

        /// <summary>
        /// Updates the position of the panel's view.
        /// </summary>
        protected void UpdateViewPosition()
        {
            FinishLoadingView();

            if (view != null && State != UIPanelState.Closed)
            {
                var area = new Rectangle(X, Y, Width, Height);
                view.SetViewPosition(Window, area);
            }
        }

        /// <summary>
        /// Updates the panel's transition state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected void UpdateTransition(UltravioletTime time)
        {
            if (State == UIPanelState.Open || State == UIPanelState.Closed)
                return;

            var delta = transitionDirection * (time.ElapsedTime.TotalMilliseconds / transitionDuration);
            transitionPosition += delta;

            if (transitionPosition < 0)
            {
                this.transitionPosition = 0;
                this.transitionDuration = 0;
                this.state = UIPanelState.Closed;
                HandleClosed();
            }
            if (transitionPosition > 1)
            {
                this.transitionPosition = 1;
                this.transitionDuration = 0;
                this.state = UIPanelState.Open;
                HandleOpened();
            }
        }

        /// <summary>
        /// Creates a new view model for the panel's view and sets it on the view.
        /// </summary>
        protected void ResetViewModel()
        {
            FinishLoadingView();

            if (View == null)
                return;

            var vm = vmfactory(View);
            if (vm != View.GetViewModel<Object>())
            {
                View.SetViewModel(vm);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is on the primary window.
        /// </summary>
        protected Boolean IsOnPrimaryWindow
        {
            get
            {
                return !(window?.Disposed ?? true) && window == Ultraviolet.GetPlatform().Windows.GetPrimary();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is on the current window.
        /// </summary>
        protected Boolean IsOnCurrentWindow
        {
            get
            {
                return !(window?.Disposed ?? true) && window == Ultraviolet.GetPlatform().Windows.GetCurrent();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is in the <see cref="UIPanelState.Open"/> state.
        /// </summary>
        protected Boolean IsOpen
        {
            get { return state == UIPanelState.Open; }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is in the <see cref="UIPanelState.Closed"/> state.
        /// </summary>
        protected Boolean IsClosed
        {
            get { return state == UIPanelState.Closed; }
        }

        /// <summary>
        /// Gets a value indicating whether the panel has loaded its view.
        /// </summary>
        protected Boolean IsViewLoaded
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isViewLoaded;
            }
        }

        /// <summary>
        /// Gets the size of the panel's current window.
        /// </summary>
        protected Size2 WindowSize
        {
            get
            {
                if (window?.Disposed ?? true)
                    return Size2.Zero;

                return window.Compositor.Size;
            }
        }

        /// <summary>
        /// Gets the width of the panel's current window.
        /// </summary>
        protected Int32 WindowWidth
        {
            get
            {
                if (window?.Disposed ?? true)
                    return 0;

                return window.Compositor.Width;
            }
        }

        /// <summary>
        /// Gets the height of the panel's current window.
        /// </summary>
        protected Int32 WindowHeight
        {
            get
            {
                if (window?.Disposed ?? true)
                    return 0;

                return window.Compositor.Height;
            }
        }

        /// <summary>
        /// Gets the screen stack for the panel's current window.
        /// </summary>
        protected UIScreenStack WindowScreens
        {
            get
            {
                if (window?.Disposed ?? true)
                    return null;

                return Ultraviolet.GetUI().GetScreens(window);
            }
        }

        /// <summary>
        /// Raises the <see cref="Opening"/> event.
        /// </summary>
        internal virtual void HandleOpening()
        {
            OnOpening();
        }

        /// <summary>
        /// Raises the <see cref="Opened"/> event.
        /// </summary>
        internal virtual void HandleOpened()
        {
            OnOpened();
            if (tcsOpened != null)
            {
                tcsOpened.SetResult(this);
                tcsOpened = null;
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing"/> event.
        /// </summary>
        internal virtual void HandleClosing()
        {
            OnClosing();
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        internal virtual void HandleClosed()
        {
            OnClosed();
            if (tcsClosed != null)
            {
                tcsClosed.SetResult(this);
                tcsClosed = null;
            }
        }

        /// <summary>
        /// Occurs when the panel's view is loaded.
        /// </summary>
        internal virtual void HandleViewLoaded()
        {

        }

        /// <summary>
        /// Cancels any pending tasks associated with the panel's state.
        /// </summary>
        private void CancelPendingTasks()
        {
            if (tcsOpened != null)
            {
                tcsOpened.SetCanceled();
                tcsOpened = null;
            }
            if (tcsClosed != null)
            {
                tcsClosed.SetCanceled();
                tcsClosed = null;
            }
        }

        /// <summary>
        /// Opens the panel.
        /// </summary>
        private Task OpenInternal(TimeSpan duration, Boolean async)
        {
            if (State != UIPanelState.Closed && State != UIPanelState.Closing)
            {
                return CreateTaskCompletionSourceIfNeeded(async, ref tcsOpened);
            }

            CancelPendingTasks();
            var task = CreateTaskCompletionSourceIfNeeded(async, ref tcsOpened);

            var remaining = 1.0 - TransitionPosition;

            this.state = UIPanelState.Opening;
            this.transitionDuration = remaining * duration.TotalMilliseconds;
            this.transitionDirection = 1;
            HandleOpening();

            if (this.transitionDuration == 0)
            {
                this.state = UIPanelState.Open;
                this.transitionPosition = 1;
                HandleOpened();
            }

            return async ? task : null;
        }

        /// <summary>
        /// Closes the panel.
        /// </summary>
        private Task CloseInternal(TimeSpan duration, Boolean async)
        {
            if (State != UIPanelState.Open && State != UIPanelState.Opening)
            {
                return CreateTaskCompletionSourceIfNeeded(async, ref tcsClosed);
            }

            CancelPendingTasks();
            var task = CreateTaskCompletionSourceIfNeeded(async, ref tcsClosed);

            var remaining = TransitionPosition;

            this.state = UIPanelState.Closing;
            this.transitionDuration = remaining * duration.TotalMilliseconds;
            this.transitionDirection = -1;
            HandleClosing();

            if (this.transitionDuration == 0)
            {
                this.state = UIPanelState.Closed;
                this.transitionPosition = 0;
                HandleClosed();
            }

            return async ? task : null;
        }

        /// <summary>
        /// Creates the specified task completion source if it is requested and does not already exist.
        /// </summary>
        private Task CreateTaskCompletionSourceIfNeeded(Boolean async, ref TaskCompletionSource<UIPanel> tcs)
        {
            if (async)
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource<UIPanel>(this);
                }
                return tcs.Task;
            }
            return null;
        }

        // Property values.
        private readonly ContentManager globalContent;
        private readonly ContentManager localContent;
        private UIView view;
        private UIPanelState state = UIPanelState.Closed;
        private Double transitionPosition = 0f;
        private Double transitionDuration = 0f;
        private Double transitionDirection = 0f;
        private TimeSpan defaultOpenTransitionDuration = TimeSpan.Zero;
        private TimeSpan defaultCloseTransitionDuration = TimeSpan.Zero;
        private IUltravioletWindow window;

        // View lazy loading parameters.
        private UIPanelDefinition definition;
        private Boolean isViewLoaded;

        // Task completion sources which are triggered when the panel is opened or closed.
        private TaskCompletionSource<UIPanel> tcsOpened;
        private TaskCompletionSource<UIPanel> tcsClosed;

        // View model factory for the panel's view.
        private readonly UIViewModelFactory vmfactory;
    }
}
