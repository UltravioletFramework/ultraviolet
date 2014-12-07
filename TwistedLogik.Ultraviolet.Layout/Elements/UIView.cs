using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents the top-level container for UI elements.
    /// </summary>
    public sealed class UIView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="viewModelType">The view's associated model type.</param>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="screenArea">The view's initial area on the screen.</param>
        public UIView(Type viewModelType, UltravioletContext uv)
        {
            this.viewModelType = viewModelType;

            this.canvas = new Canvas(uv, null);
            this.canvas.UpdateView(this);
        }

        /// <summary>
        /// Loads an instance of <see cref="UIView"/> from an XML document.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XDocument"/> from which to load the view.</param>
        /// <returns>The <see cref="UIView"/> that was loaded from the specified XML document.</returns>
        public static UIView Load(UltravioletContext uv, XDocument xml)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UIViewLoader.Load(uv, xml.Root);
        }

        /// <summary>
        /// Loads an instance of the <see cref="UIView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the view.</param>
        /// <returns>The <see cref="UIView"/> that was loaded from the specified XML element.</returns>
        public static UIView Load(UltravioletContext uv, XElement xml)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UIViewLoader.Load(uv, xml);
        }

        /// <summary>
        /// Draws the view and all of its contained elements.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            Canvas.Draw(time, spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Updates the view's state and the state of its contained elements.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Canvas.Update(time);
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

            Canvas.ReloadContent();
        }

        /// <summary>
        /// Sets the view's stylesheet.
        /// </summary>
        /// <param name="stylesheet">The view's stylesheet.</param>
        public void SetStylesheet(UvssDocument stylesheet)
        {
            this.stylesheet = stylesheet;

            if (stylesheet != null)
            {
                stylesheet.ApplyStylesRecursively(Canvas);
            }
            else
            {
                Canvas.ClearStyledValuesRecursive();
            }
        }

        /// <summary>
        /// Sets the view's associated view model.
        /// </summary>
        /// <param name="viewModel">The view's associated view model.</param>
        public void SetViewModel(Object viewModel)
        {
            if (viewModel != null && viewModel.GetType() != viewModelType)
            {
                throw new InvalidOperationException("TODO");
            }

            this.viewModel = viewModel;
            Canvas.UpdateViewModel(viewModel);
        }

        /// <summary>
        /// Sets the view's area on the screen.
        /// </summary>
        /// <param name="area">The area on the screen that is occupied by the view.</param>
        public void SetViewArea(Rectangle area)
        {
            var newPosition = false;
            var newSize = false;

            if (this.viewArea.X != area.X || this.viewArea.Y != area.Y)
                newPosition = true;

            if (this.viewArea.Width != area.Width || this.viewArea.Height != area.Height)
                newSize = true;

            this.viewArea = area;

            if (newSize)
            {
                Canvas.PerformLayout();
            }

            if (newSize || newPosition)
            {

            }
        }

        /// <summary>
        /// Requests that a layout be performed during the next call to <see cref="UIElement.Update(UltravioletTime)"/>.
        /// </summary>
        public void RequestLayout()
        {
            Canvas.RequestLayout();
        }

        /// <summary>
        /// Immediately recalculates the layout of the container and all of its children.
        /// </summary>
        public void PerformLayout()
        {
            Canvas.PerformLayout();
        }

        /// <summary>
        /// Loads the specified sourced asset.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadContent<TOutput>(SourcedAssetID asset)
        {
            switch (asset.AssetSource)
            {
                case AssetSource.Global:
                    return (globalContent == null) ? default(TOutput) : globalContent.Load<TOutput>(asset.AssetID);
                
                case AssetSource.Local:
                    return (localContent == null) ? default(TOutput) : localContent.Load<TOutput>(asset.AssetID);

                default:
                    throw new NotSupportedException();
            }
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
        /// Gets the stylesheet that is currently applied to this view.
        /// </summary>
        public UvssDocument Stylesheet
        {
            get { return stylesheet; }
        }

        /// <summary>
        /// Gets the view's view model.
        /// </summary>
        public Object ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Gets or sets the area on the screen that the UI view occupies.
        /// </summary>
        public Rectangle Area
        {
            get { return viewArea; }
            set
            {
                if (!viewArea.Equals(value))
                {
                    viewArea = value;
                    Canvas.ContainerRelativeLayout = new Rectangle(0, 0, value.Width, value.Height);
                    Canvas.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the view's top left corner.
        /// </summary>
        public Int32 X
        {
            get { return viewArea.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the view's top left corner.
        /// </summary>
        public Int32 Y
        {
            get { return viewArea.Y; }
        }

        /// <summary>
        /// Gets the view's width on the screen.
        /// </summary>
        public Int32 Width
        {
            get { return viewArea.Width; }
        }

        /// <summary>
        /// Gets the view's height on the screen.
        /// </summary>
        public Int32 Height
        {
            get { return viewArea.Height; }
        }

        /// <summary>
        /// Gets the <see cref="Canvas"/> that contains all of the UI's elements.
        /// </summary>
        public Canvas Canvas
        {
            get { return canvas; }
        }

        // Property values.
        private ContentManager globalContent;
        private ContentManager localContent;
        private UvssDocument stylesheet;
        private Object viewModel;
        private Rectangle viewArea;
        private Canvas canvas;

        private readonly Type viewModelType;
    }
}
