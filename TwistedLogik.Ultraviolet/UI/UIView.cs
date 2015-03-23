using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UIView"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
    /// <returns>The instance of <see cref="UIView"/> that was created.</returns>
    public delegate UIView UIViewFactory(UltravioletContext uv, UIPanelDefinition uiPanelDefinition);

    /// <summary>
    /// Represents a 
    /// </summary>
    public abstract class UIView : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        public UIView(UltravioletContext uv, Type viewModelType)
            : base(uv)
        {
            this.viewModelType = viewModelType;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
        /// <returns>The instance of <see cref="UIView"/> that was created.</returns>
        public static UIView Create(UIPanelDefinition uiPanelDefinition)
        {
            Contract.Require(uiPanelDefinition, "uiPanelDefinition");

            var uv       = UltravioletContext.DemandCurrent();
            var factory  = uv.TryGetFactoryMethod<UIViewFactory>();
            if (factory != null)
            {
                return factory(uv, uiPanelDefinition);
            }

            return null;
        }

        /// <summary>
        /// Cleans up the view and its elements.
        /// </summary>
        /// <remarks>The <see cref="Cleanup()"/> method releases any internal resources which the view
        /// and its elements may be holding, allowing them to be reused by the Presentation Foundation.
        /// The view remains usable after a call to this method, but certain state values (such as
        /// animations, dependency property values, etc.) may be reset.</remarks>
        public abstract void Cleanup();

        /// <summary>
        /// Draws the view.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the view.</param>
        public abstract void Draw(UltravioletTime time, SpriteBatch spriteBatch);

        /// <summary>
        /// Updates the view's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Grants focus to the view.
        /// </summary>
        public void Focus()
        {
            if (focused)
                return;

            focused = true;

            OnFocused();
        }

        /// <summary>
        /// Removes focus from the view.
        /// </summary>
        public void Blur()
        {
            if (!focused)
                return;

            focused = false;

            OnBlurred();
        }

        /// <summary>
        /// Sets the content managers used to load UI assets.
        /// </summary>
        /// <param name="global">The content manager used to load globally-sourced assets.</param>
        /// <param name="local">The content manager used to load locally-sourced assets.</param>
        public void SetContentManagers(ContentManager global, ContentManager local)
        {
            this.globalContent = global;
            this.localContent  = local;

            OnContentManagersChanged();
        }

        /// <summary>
        /// Sets the view's associated view model.
        /// </summary>
        /// <param name="viewModel">The view's associated view model.</param>
        public void SetViewModel(Object viewModel)
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
        public void SetViewPosition(IUltravioletWindow window, Rectangle area)
        {
            Contract.Require(window, "window");

            var viewPositionChanged = (this.area.X != area.X || this.area.Y != area.Y);
            var viewSizeChanged     = (this.area.Width != area.Width || this.area.Height != area.Height);

            this.window = window;
            this.area   = area;

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

            return (globalContent == null) ? default(TOutput) : globalContent.Load<TOutput>(asset);
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

            return (localContent == null) ? default(TOutput) : localContent.Load<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified image from the global content manager.
        /// </summary>
        /// <param name="image">The image to load.</param>
        public void LoadGlobalImage<T>(T image) where T : TextureImage
        {
            if (image == null || globalContent == null)
                return;

            image.Load(globalContent);
        }

        /// <summary>
        /// Loads the specified image from the local content manager.
        /// </summary>
        /// <param name="image">The image to load.</param>
        public void LoadLocalImage<T>(T image) where T : TextureImage
        {
            if (image == null || localContent == null)
                return;

            image.Load(localContent);
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
        /// Gets the content manager used to load globally-sourced assets.
        /// </summary>
        public ContentManager GlobalContent
        {
            get { return globalContent; }
        }

        /// <summary>
        /// Gets the content manager used to load locally-sourced assets.
        /// </summary>
        public ContentManager LocalContent
        {
            get { return localContent; }
        }

        /// <summary>
        /// Gets the type of view model expected by this view.
        /// </summary>
        public Type ViewModelType
        {
            get { return viewModelType; }
        }

        /// <summary>
        /// Gets the actual type of the view's current view model.
        /// </summary>
        public Type ViewModelActualType
        {
            get { return (ViewModel == null) ? null : ViewModel.GetType(); }
        }

        /// <summary>
        /// Gets the view's view model.
        /// </summary>
        public Object ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Gets the area on the screen that the UI view occupies.
        /// </summary>
        public Rectangle Area
        {
            get { return area; }
        }

        /// <summary>
        /// Gets the x-coordinate of the view's top left corner.
        /// </summary>
        public Int32 X
        {
            get { return area.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the view's top left corner.
        /// </summary>
        public Int32 Y
        {
            get { return area.Y; }
        }

        /// <summary>
        /// Gets the view's width on the screen.
        /// </summary>
        public Int32 Width
        {
            get { return area.Width; }
        }

        /// <summary>
        /// Gets the view's height on the screen.
        /// </summary>
        public Int32 Height
        {
            get { return area.Height; }
        }

        /// <summary>
        /// Gets a value indicating whether the view has input focus.
        /// </summary>
        public Boolean Focused
        {
            get { return focused; }
        }

        /// <summary>
        /// Gets the window in which the view is being rendered.
        /// </summary>
        public IUltravioletWindow Window
        {
            get { return window; }
        }

        /// <summary>
        /// Gets the display on which the view is being rendered.
        /// </summary>
        public IUltravioletDisplay Display
        {
            get { return Ultraviolet.GetPlatform().Displays.PrimaryDisplay; }
        }

        /// <summary>
        /// Called when the view is focused.
        /// </summary>
        protected virtual void OnFocused()
        {

        }

        /// <summary>
        /// Called when the view is blurred.
        /// </summary>
        protected virtual void OnBlurred()
        {

        }

        /// <summary>
        /// Called when the view's content managers are changed.
        /// </summary>
        protected virtual void OnContentManagersChanged()
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

        // Property values.
        private readonly Type viewModelType;
        private ContentManager globalContent;
        private ContentManager localContent;
        private Object viewModel;
        private Rectangle area;
        private Boolean focused;
        private IUltravioletWindow window;
    }
}
