using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents the top-level container for UI elements.
    /// </summary>
    public sealed class UIViewport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIViewport"/> class.
        /// </summary>
        /// <param name="modelType">The viewport's associated model type.</param>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="screenArea">The viewport's initial area on the screen.</param>
        public UIViewport(Type modelType, UltravioletContext uv, Rectangle screenArea)
        {
            this.viewModelType = modelType;

            this.canvas = new Canvas(uv, null);
            this.canvas.UpdateViewport(this);

            this.ScreenArea = screenArea;
        }

        /// <summary>
        /// Loads an instance of <see cref="UIViewport"/> from an XML document.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XDocument"/> from which to load the viewport.</param>
        /// <param name="screenArea">The viewport's initial area on the screen.</param>
        /// <returns>The <see cref="UIViewport"/> that was loaded from the specified XML document.</returns>
        public static UIViewport Load(UltravioletContext uv, XDocument xml, Rectangle screenArea)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UIViewportLoader.Load(uv, xml.Root, screenArea);
        }

        /// <summary>
        /// Loads an instance of the <see cref="UIViewport"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the viewport.</param>
        /// <param name="screenArea">The viewport's initial area on the screen.</param>
        /// <returns>The <see cref="UIViewport"/> that was loaded from the specified XML element.</returns>
        public static UIViewport Load(UltravioletContext uv, XElement xml, Rectangle screenArea)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UIViewportLoader.Load(uv, xml, screenArea);
        }

        /// <summary>
        /// Updates the viewport's state and the state of its contained elements.
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
        /// Sets the viewport's stylesheet.
        /// </summary>
        /// <param name="stylesheet">The viewport's stylesheet.</param>
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
        /// Sets the viewport's associated view model.
        /// </summary>
        /// <param name="viewModel">The viewport's associated view model.</param>
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
        /// Gets the stylesheet that is currently applied to this viewport.
        /// </summary>
        public UvssDocument Stylesheet
        {
            get { return stylesheet; }
        }

        /// <summary>
        /// Gets the viewport's view model.
        /// </summary>
        public Object ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Gets or sets the area on the screen that the UI viewport occupies.
        /// </summary>
        public Rectangle ScreenArea
        {
            get { return screenArea; }
            set
            {
                if (!screenArea.Equals(value))
                {
                    screenArea = value;
                    Canvas.ContainerRelativeLayout = new Rectangle(0, 0, value.Width, value.Height);
                    Canvas.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the viewport's top left corner.
        /// </summary>
        public Int32 X
        {
            get { return screenArea.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the viewport's top left corner.
        /// </summary>
        public Int32 Y
        {
            get { return screenArea.Y; }
        }

        /// <summary>
        /// Gets the viewport's width on the screen.
        /// </summary>
        public Int32 Width
        {
            get { return screenArea.Width; }
        }

        /// <summary>
        /// Gets the viewport's height on the screen.
        /// </summary>
        public Int32 Height
        {
            get { return screenArea.Height; }
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
        private Rectangle screenArea;
        private Canvas canvas;

        private readonly Type viewModelType;
    }
}
