using System;
using System.Diagnostics;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a fullscreen container for user interface elements.
    /// </summary>
    public abstract class UIScreen : UIPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreen"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        protected UIScreen(String rootDirectory, String definitionAsset, ContentManager globalContent)
            : this(null, rootDirectory, definitionAsset, globalContent)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreen"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        protected UIScreen(UltravioletContext uv, String rootDirectory, String definitionAsset, ContentManager globalContent)
            : base(uv, rootDirectory, globalContent)
        {
            var definitionWrapper = LoadPanelDefinition(definitionAsset);
            if (definitionWrapper?.HasValue ?? false)
            {
                var definition = definitionWrapper.Value;
                DefaultOpenTransitionDuration = definition.DefaultOpenTransitionDuration;
                DefaultCloseTransitionDuration = definition.DefaultCloseTransitionDuration;

                PrepareView(definitionWrapper);
            }

            this.definitionAsset = definitionAsset;
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            FinishLoadingView();

            if (View != null)
            {
                var newDensityScale = Window?.Display?.DensityScale ?? 0f;
                if (newDensityScale != Scale)
                {
                    if (Scale > 0)
                    {
                        RecreateView(LoadPanelDefinition(definitionAsset));
                    }
                    Scale = newDensityScale;
                }
            }

            if (View != null && State != UIPanelState.Closed)
            {
                UpdateViewPosition();
                UpdateView(time);
            }
            UpdateTransition(time);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.EnsureNotDisposed(this, Disposed);

            if (!IsOnCurrentWindow)
                return;

            OnDrawingBackground(time, spriteBatch);

            Window.Compositor.BeginContext(CompositionContext.Interface);

            FinishLoadingView();

            if (View != null)
            {
                DrawView(time, spriteBatch);
                OnDrawingView(time, spriteBatch);
            }

            Window.Compositor.BeginContext(CompositionContext.Overlay);

            OnDrawingForeground(time, spriteBatch);
        }

        /// <inheritdoc/>
        public override Int32 X => 0;

        /// <inheritdoc/>
        public override Int32 Y => 0;

        /// <inheritdoc/>
        public override Size2 Size => WindowSize;

        /// <inheritdoc/>
        public override Int32 Width => WindowWidth;

        /// <inheritdoc/>
        public override Int32 Height => WindowHeight;

        /// <inheritdoc/>
        public override Boolean IsReadyForInput
        {
            get
            {
                if (!IsOpen)
                    return false;

                var screens = WindowScreens;
                if (screens == null)
                    return false;

                return screens.Peek() == this;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsReadyForBackgroundInput => IsOpen;

        /// <summary>
        /// Gets or sets a value indicating whether this screen is opaque.
        /// </summary>
        /// <remarks>Marking a screen as opaque is a performance optimization. If a screen is opaque, then Ultraviolet
        /// will not render any screens below it in the screen stack.</remarks>
        public Boolean IsOpaque { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this screen is the topmost screen on its current window.
        /// </summary>
        public Boolean IsTopmost => WindowScreens?.Peek() == this;

        /// <summary>
        /// Gets the scale at which the screen is currently being rendered.
        /// </summary>
        public Single Scale { get; private set; }

        /// <inheritdoc/>
        internal override void HandleOpening()
        {
            base.HandleOpening();

            UpdateViewPosition();

            if (View != null)
                View.OnOpening();
        }

        /// <inheritdoc/>
        internal override void HandleOpened()
        {
            base.HandleOpened();

            if (View != null)
                View.OnOpened();
        }

        /// <inheritdoc/>
        internal override void HandleClosing()
        {
            base.HandleClosing();

            if (View != null)
                View.OnClosing();
        }

        /// <inheritdoc/>
        internal override void HandleClosed()
        {
            base.HandleClosed();

            if (View != null)
                View.OnClosed();
        }

        /// <inheritdoc/>
        internal override void HandleViewLoaded()
        {
            if (View != null)
                View.SetContentManagers(GlobalContent, LocalContent);

            base.HandleViewLoaded();
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (Window != null && !Window.Disposed)
                {
                    Ultraviolet.GetUI().GetScreens(Window).Close(this, TimeSpan.Zero);
                }
            }
            
            SafeDispose.Dispose(pendingView);

            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Loads the screen's panel definition from the specified asset.
        /// </summary>
        /// <param name="asset">The name of the asset that contains the panel definition.</param>
        /// <returns>The panel definition that was loaded from the specified asset.</returns>
        protected virtual WatchedAsset<UIPanelDefinition> LoadPanelDefinition(String asset)
        {
            if (String.IsNullOrEmpty(asset))
                return null;

            var display = Window?.Display ?? Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var density = display.DensityBucket;
            Scale = display.DensityScale;

            var watch = Ultraviolet.GetUI().WatchingViewFilesForChanges;
            if (watch)
            {
                var definition = new WatchedAsset<UIPanelDefinition>(LocalContent, asset, density, OnValidatingUIPanelDefinition, OnReloadingUIPanelDefinition);
                return definition;
            }
            else
            {
                var definition = LocalContent.Load<UIPanelDefinition>(asset, density);
                return new WatchedAsset<UIPanelDefinition>(LocalContent, definition);
            }
        }

        /// <summary>
        /// Validates panel definitions that are being reloaded.
        /// </summary>
        private bool OnValidatingUIPanelDefinition(String path, UIPanelDefinition asset)
        {
            try
            {
                pendingView = CreateViewFromFromUIPanelDefinition(asset);

                return true;
            }
            catch (Exception e)
            {
                pendingView = null;

                Debug.WriteLine(UltravioletStrings.ExceptionDuringViewReloading);
                Debug.WriteLine(e);

                return false;
            }
        }

        /// <summary>
        /// Reloads panel definitions.
        /// </summary>
        private void OnReloadingUIPanelDefinition(String path, UIPanelDefinition asset, Boolean validated)
        {
            if (validated)
            {
                if (State == UIPanelState.Open)
                {
                    var currentViewModel = View?.ViewModel;
                    var currentView = View;
                    if (currentView != null)
                    {
                        currentView.OnClosing();
                        currentView.OnClosed();
                        UnloadView();
                    }

                    DefaultOpenTransitionDuration = asset.DefaultOpenTransitionDuration;
                    DefaultCloseTransitionDuration = asset.DefaultCloseTransitionDuration;

                    FinishLoadingView(pendingView);
                    pendingView = null;

                    var updatedView = View;
                    if (updatedView != null)
                    {
                        if (currentViewModel != null)
                            updatedView.SetViewModel(currentViewModel);

                        updatedView.OnOpening();
                        updatedView.OnOpened();
                    }
                }
            }
            else
            {
                pendingView?.Dispose();
                pendingView = null;
            }
        }

        // State values.
        private UIView pendingView;
        private String definitionAsset;
    }
}
