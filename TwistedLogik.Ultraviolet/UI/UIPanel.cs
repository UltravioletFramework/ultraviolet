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

            this.localContent  = ContentManager.Create(rootDirectory);
            this.globalContent = globalContent;
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
        /// Grants input focus to the panel.
        /// </summary>
        public void Focus()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            isFocused = true;

            if (view != null)
            {
                view.Focus();
            }
        }

        /// <summary>
        /// Removes input focus from the panel.
        /// </summary>
        public void Blur()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            isFocused = false;

            if (view != null)
            {
                view.Blur();
            }
        }

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
            get { return view; }
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
        /// Gets a value indicating whether the panel has input focus.
        /// </summary>
        public Boolean IsFocused
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isFocused; 
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
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">The Ultraviolet time.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Raises the <see cref="DrawingBackground"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingBackground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            var temp = DrawingBackground;
            if (temp != null)
            {
                temp(this, time, spriteBatch);
            }
        }

        /// <summary>
        /// Raises the <see cref="DrawingLayout"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingLayout(UltravioletTime time, SpriteBatch spriteBatch)
        {
            var temp = DrawingLayout;
            if (temp != null)
            {
                temp(this, time, spriteBatch);
            }
        }

        /// <summary>
        /// Raises the <see cref="DrawingForeground"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected virtual void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            var temp = DrawingForeground;
            if (temp != null)
            {
                temp(this, time, spriteBatch);
            }
        }

        /// <summary>
        /// Raises the <see cref="Opening"/> event.
        /// </summary>
        protected virtual void OnOpening()
        {
            var temp = Opening;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Opened"/> event.
        /// </summary>
        protected virtual void OnOpened()
        {
            var temp = Opened;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing"/> event.
        /// </summary>
        protected virtual void OnClosing()
        {
            var temp = Closing;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClosed()
        {
            var temp = Closed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the panel's view is loaded.
        /// </summary>
        protected virtual void OnViewLoaded()
        {

        }

        /// <summary>
        /// Loads the view from the specified panel definition.
        /// </summary>
        /// <param name="definition">The panel definition from which to load the view.</param>
        protected void LoadView(UIPanelDefinition definition)
        {
            var view = UIView.Create(definition);
            if (view != null)
            {
                if (window != null)
                {
                    var area = new Rectangle(X, Y, Width, Height);
                    view.SetViewPosition(window, area);
                }

                if (IsFocused)
                    view.Focus();
            }

            this.view = view;

            HandleViewLoaded();
        }

        /// <summary>
        /// Draws the panel's view.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which the panel is being drawn.</param>
        protected void DrawView(UltravioletTime time, SpriteBatch spriteBatch)
        {
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
            if (view != null && State == UIPanelState.Open)
            {
                var area = new Rectangle(X, Y, Width, Height);
                view.SetViewPosition(Window, area);
                view.Update(time);
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
        /// Gets a value indicating whether the panel is on the primary window.
        /// </summary>
        protected Boolean IsOnPrimaryWindow
        {
            get
            {
                return window != null && window == Ultraviolet.GetPlatform().Windows.GetPrimary();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the panel is on the current window.
        /// </summary>
        protected Boolean IsOnCurrentWindow
        {
            get
            {
                return window != null && window == Ultraviolet.GetPlatform().Windows.GetCurrent();
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
        /// Gets the size of the panel's current window.
        /// </summary>
        protected Size2 WindowSize
        {
            get
            {
                if (window == null)
                    return Size2.Zero;

                return window.ClientSize;
            }
        }

        /// <summary>
        /// Gets the width of the panel's current window.
        /// </summary>
        protected Int32 WindowWidth
        {
            get
            {
                if (window == null)
                    return 0;

                return window.ClientSize.Width;
            }
        }

        /// <summary>
        /// Gets the height of the panel's current window.
        /// </summary>
        protected Int32 WindowHeight
        {
            get
            {
                if (window == null)
                    return 0;

                return window.ClientSize.Height;
            }
        }

        /// <summary>
        /// Gets the screen stack for the panel's current window.
        /// </summary>
        protected UIScreenStack WindowScreens
        {
            get
            {
                if (window == null)
                    return null;

                return Ultraviolet.GetUI().GetScreens(window);
            }
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
            OnViewLoaded();
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
            CreateTaskCompletionSourceIfNeeded(async, ref tcsOpened);

            var remaining = 1.0 - TransitionPosition;

            this.state = UIPanelState.Opening;
            this.transitionDuration = remaining * duration.TotalMilliseconds;
            this.transitionDirection = 1;
            OnOpening();

            if (this.transitionDuration == 0)
            {
                this.state = UIPanelState.Open;
                this.transitionPosition = 1;
                HandleOpened();
            }

            return async ? tcsOpened.Task : null;
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
            CreateTaskCompletionSourceIfNeeded(async, ref tcsClosed);

            var remaining = TransitionPosition;

            this.state = UIPanelState.Closing;
            this.transitionDuration = remaining * duration.TotalMilliseconds;
            this.transitionDirection = -1;
            this.Blur();

            OnClosing();

            if (this.transitionDuration == 0)
            {
                this.state = UIPanelState.Closed;
                this.transitionPosition = 0;
                HandleClosed();
            }

            return async ? tcsClosed.Task : null;
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
        private Boolean isFocused;
        private Double transitionPosition = 0f;
        private Double transitionDuration = 0f;
        private Double transitionDirection = 0f;
        private TimeSpan defaultOpenTransitionDuration = TimeSpan.Zero;
        private TimeSpan defaultCloseTransitionDuration = TimeSpan.Zero;
        private IUltravioletWindow window;

        // Task completion sources which are triggered when the panel is opened or closed.
        private TaskCompletionSource<UIPanel> tcsOpened;
        private TaskCompletionSource<UIPanel> tcsClosed;
    }
}
