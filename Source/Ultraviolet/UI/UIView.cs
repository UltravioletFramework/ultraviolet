using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Platform;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a method which constructs view model instances for the specified view.
    /// </summary>
    /// <param name="view">The view for which to create a view model.</param>
    /// <returns>The view model that was created.</returns>
    public delegate Object UIViewModelFactory(UIView view);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UIView"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the view.</param>
    /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
    /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <see langword="null"/> to skip view model creation.</param>
    /// <returns>The instance of <see cref="UIView"/> that was created.</returns>
    public delegate UIView UIViewFactory(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory);
    
    /// <summary>
    /// Represents the graphical user interface of a <see cref="UIPanel"/> instance, which can optionally be bound to a view model.
    /// </summary>
    public abstract class UIView : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="panel">The panel that owns the view.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        public UIView(UltravioletContext uv, UIPanel panel, Type viewModelType)
            : base(uv)
        {
            Contract.Require(panel, nameof(panel));

            this.panel = panel;
            this.viewModelType = viewModelType;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="uiPanel">The <see cref="UIPanel"/> which is creating the view.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
        /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <see langword="null"/> to skip view model creation.</param>
        /// <returns>The instance of <see cref="UIView"/> that was created.</returns>
        public static UIView Create(UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory)
        {
            Contract.Require(uiPanel, nameof(uiPanel));
            Contract.Require(uiPanelDefinition, nameof(uiPanelDefinition));

            var uv = UltravioletContext.DemandCurrent();
            var factory = uv.TryGetFactoryMethod<UIViewFactory>();
            if (factory != null)
            {
                return factory(uv, uiPanel, uiPanelDefinition, vmfactory);
            }

            return null;
        }
        
        /// <summary>
        /// Draws the view.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the view.</param>
        /// <param name="opacity">The view's overall opacity.</param>
        public abstract void Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity = 1f);

        /// <summary>
        /// Updates the view's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Sets the content managers used to load UI assets.
        /// </summary>
        /// <param name="global">The content manager used to load globally-sourced assets.</param>
        /// <param name="local">The content manager used to load locally-sourced assets.</param>
        public virtual void SetContentManagers(ContentManager global, ContentManager local)
        {
            this.globalContent = global;
            this.localContent = local;

            OnContentManagersChanged();
        }

        /// <summary>
        /// Sets the view's associated view model.
        /// </summary>
        /// <param name="viewModel">The view's associated view model.</param>
        public virtual void SetViewModel(Object viewModel)
        {
            if (viewModel != null && viewModel.GetType() != viewModelType)
                throw new ArgumentException(UltravioletStrings.IncompatibleViewModel.Format(viewModelType));

            this.viewModel = viewModel;

            OnViewModelChanged();
        }

        /// <summary>
        /// Positions the view on the specified window.
        /// </summary>
        /// <param name="window">The window on which to position the view.</param>
        /// <param name="area">The area on the window in which to position the view.</param>
        public virtual void SetViewPosition(IUltravioletWindow window, Rectangle area)
        {
            var oldWindow = this.window;

            var viewWindowChanged = (this.window != window);
            var viewPositionChanged = (this.area.X != area.X || this.area.Y != area.Y);
            var viewSizeChanged = (this.area.Width != area.Width || this.area.Height != area.Height);

            this.window = window;
            this.area = area;

            if (viewWindowChanged)
                OnViewWindowChanged(oldWindow, window);

            if (viewPositionChanged)
                OnViewPositionChanged();

            if (viewSizeChanged)
                OnViewSizeChanged();
        }
        
        /// <summary>
        /// Loads the specified asset from the global content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadGlobalContent<TOutput>(AssetID asset)
        {
            if (!asset.IsValid)
                return default(TOutput);

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return (globalContent == null) ? default(TOutput) : globalContent.Load<TOutput>(asset, density);
        }

        /// <summary>
        /// Loads the specified asset from the local content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadLocalContent<TOutput>(AssetID asset)
        {
            if (!asset.IsValid)
                return default(TOutput);

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return (localContent == null) ? default(TOutput) : localContent.Load<TOutput>(asset, density);
        }

        /// <summary>
        /// Loads the specified animation from the global content manager.
        /// </summary>
        /// <param name="animation">The identifier of the animation to load.</param>
        /// <returns>The animation that was loaded.</returns>
        public SpriteAnimation LoadGlobalContent(SpriteAnimationID animation)
        {
            if (!animation.IsValid || globalContent == null)
                return null;

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return globalContent.Load(animation, density);
        }

        /// <summary>
        /// Loads the specified animation from the local content manager.
        /// </summary>
        /// <param name="animation">The identifier of the animation to load.</param>
        /// <returns>The animation that was loaded.</returns>
        public SpriteAnimation LoadLocalContent(SpriteAnimationID animation)
        {
            if (!animation.IsValid || localContent == null)
                return null;

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return localContent.Load(animation, density);
        }

        /// <summary>
        /// Loads the specified image from the global content manager.
        /// </summary>
        /// <typeparam name="TImage">The type of image to load.</typeparam>
        /// <param name="image">The image to load.</param>
        public void LoadGlobalImage<TImage>(TImage image) where TImage : TextureImage
        {
            if (image == null || globalContent == null)
                return;

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            image.Load(globalContent, density);
        }

        /// <summary>
        /// Loads the specified image from the local content manager.
        /// </summary>
        /// <typeparam name="TImage">The type of image to load.</typeparam>
        /// <param name="image">The image to load.</param>
        public void LoadLocalImage<TImage>(TImage image) where TImage : TextureImage
        {
            if (image == null || localContent == null)
                return;

            var density = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;

            image.Load(localContent, density);
        }

        /// <summary>
        /// Converts a position in screen space to a position in view space.
        /// </summary>
        /// <param name="x">The x-coordinate of the screen space position to convert.</param>
        /// <param name="y">The y-coordinate of the screen space position to convert.</param>
        /// <returns>The converted view space position.</returns>
        public Point2 ScreenPositionToViewPosition(Int32 x, Int32 y)
        {
            return new Point2(x - Area.X, y - Area.Y);
        }

        /// <summary>
        /// Converts a position in screen space to a position in view space.
        /// </summary>
        /// <param name="position">The screen space position to convert.</param>
        /// <returns>The converted view space position.</returns>
        public Point2 ScreenPositionToViewPosition(Point2 position)
        {
            return ScreenPositionToViewPosition(position.X, position.Y);
        }

        /// <summary>
        /// Converts a position in view space to a position in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate of the view space position to convert.</param>
        /// <param name="y">The y-coordinate of the view space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Point2 ViewPositionToScreenPosition(Int32 x, Int32 y)
        {
            return new Point2(x + Area.X, y + Area.Y);
        }

        /// <summary>
        /// Converts a position in view space to a position in screen space.
        /// </summary>
        /// <param name="position">The view space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Point2 ViewPositionToScreenPosition(Point2 position)
        {
            return ViewPositionToScreenPosition(position.X, position.Y);
        }

        /// <summary>
        /// Gets the view's view model object.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to retrieve.</typeparam>
        /// <returns>The view's view model object.</returns>
        public virtual TViewModel GetViewModel<TViewModel>() where TViewModel : class
        {
            return ViewModel as TViewModel;
        }

        /// <summary>
        /// Gets the panel that owns the view.
        /// </summary>
        public UIPanel Panel => panel;

        /// <summary>
        /// Gets the content manager used to load globally-sourced assets.
        /// </summary>
        public ContentManager GlobalContent => globalContent;

        /// <summary>
        /// Gets the content manager used to load locally-sourced assets.
        /// </summary>
        public ContentManager LocalContent => localContent;

        /// <summary>
        /// Gets the type of view model expected by this view.
        /// </summary>
        public Type ViewModelType => viewModelType;

        /// <summary>
        /// Gets the actual type of the view's current view model.
        /// </summary>
        public Type ViewModelActualType => ViewModel?.GetType();

        /// <summary>
        /// Gets the view's view model.
        /// </summary>
        public Object ViewModel => viewModel;

        /// <summary>
        /// Gets the area on the screen that the UI view occupies.
        /// </summary>
        public Rectangle Area => area;

        /// <summary>
        /// Gets the x-coordinate of the view's top left corner.
        /// </summary>
        public Int32 X => area.X;

        /// <summary>
        /// Gets the y-coordinate of the view's top left corner.
        /// </summary>
        public Int32 Y => area.Y;

        /// <summary>
        /// Gets the view's width on the screen.
        /// </summary>
        public Int32 Width => area.Width;

        /// <summary>
        /// Gets the view's height on the screen.
        /// </summary>
        public Int32 Height => area.Height;

        /// <summary>
        /// Gets or sets a value indicating whether input is enabled for this view.
        /// If <see langword="false"/>, then the view will not receive any input events.
        /// </summary>
        public Boolean IsInputEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether input is allowed for this view.
        /// If <see langword="false"/>, then the view will not receive any input events.
        /// </summary>
        /// <remarks>While <see cref="IsInputEnabled"/> may be changed at any time, the value of <see cref="IsInputAllowed"/> is managed
        /// by Ultraviolet itself and corresponds to the <see cref="UIPanel.IsReadyForInput"/> property on the panel that owns the view.</remarks>
        public Boolean IsInputAllowed => panel.IsReadyForInput;

        /// <summary>
        /// Gets a value indicating whether input is currently both enabled and allowed on this view.
        /// </summary>
        public Boolean IsInputEnabledAndAllowed => IsInputEnabled && IsInputAllowed;

        /// <summary>
        /// Gets the window in which the view is being rendered.
        /// </summary>
        public IUltravioletWindow Window => window;

        /// <summary>
        /// Gets the display on which the view is being rendered.
        /// </summary>
        public IUltravioletDisplay Display => window?.Display;

        /// <summary>
        /// Occurs when the view is about to be opened.
        /// </summary>
        protected internal abstract void OnOpening();

        /// <summary>
        /// Occurs when the view has been opened.
        /// </summary>
        protected internal abstract void OnOpened();

        /// <summary>
        /// Occurs when the view is about to be closed.
        /// </summary>
        protected internal abstract void OnClosing();

        /// <summary>
        /// Occurs when the view has been closed.
        /// </summary>
        protected internal abstract void OnClosed();
        
        /// <summary>
        /// Called when the view's content managers are changed.
        /// </summary>
        protected virtual void OnContentManagersChanged()
        {

        }

        /// <summary>
        /// Called when the view is moved to a new window.
        /// </summary>
        /// <param name="oldWindow">The window that previously contained the view.</param>
        /// <param name="newWindow">The window that currently contains the view.</param>
        protected virtual void OnViewWindowChanged(IUltravioletWindow oldWindow, IUltravioletWindow newWindow)
        {

        }

        /// <summary>
        /// Called when the view is repositioned.
        /// </summary>
        protected virtual void OnViewPositionChanged()
        {

        }

        /// <summary>
        /// Called when the view's size changes.
        /// </summary>
        protected virtual void OnViewSizeChanged()
        {

        }

        /// <summary>
        /// Called when the view's view model is changed.
        /// </summary>
        protected virtual void OnViewModelChanged()
        {

        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                var vm = GetViewModel<IDisposable>();
                if (vm != null)
                {
                    vm.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Property values.
        private readonly UIPanel panel;
        private readonly Type viewModelType;
        private ContentManager globalContent;
        private ContentManager localContent;
        private Object viewModel;
        private Rectangle area;
        private IUltravioletWindow window;
    }
}
