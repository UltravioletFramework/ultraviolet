using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents the method that is called when a CEF client raises an event.
    /// </summary>
    /// <param name="client">The client that raised the event.</param>
    internal delegate void XiliumCefClientEventHandler(XiliumCefClient client);

    /// <summary>
    /// Represents a CEF client.
    /// </summary>
    internal sealed partial class XiliumCefClient : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the XiliumCefClient class.
        /// </summary>
        /// <param name="layout">The layout that owns this client.</param>
        /// <param name="x">The x-coordinate of the browser's initial position on the screen.</param>
        /// <param name="y">The y-coordinate of the browser's initial position on the screen.</param>
        /// <param name="width">The browser's initial width in pixels.</param>
        /// <param name="height">The browser's initial height in pixels.</param>
        public XiliumCefClient(XiliumCefUILayout layout, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.cefLifeSpanHandler = new XiliumCefLifeSpanHandler();
            this.cefLifeSpanHandler.BrowserCreated += (sender, browser) => 
            {
                ApiMethodRegistryManager.Create(browser.Identifier);
                this.browser = browser;
                this.browserHost = browser.GetHost();
                this.browserHost.SendFocusEvent(layout.Focused);
                this.OnInitialized(); 
            };
            
            this.cefLoadHandler = new XiliumCefLoadHandler();
            this.cefLoadHandler.MainFrameLoadStart += (sender) => { this.OnLoadStart(); };
            this.cefLoadHandler.MainFrameLoadEnd   += (sender) => 
            {
                this.OnLoadEnd(); 
                this.browserHost.SendFocusEvent(layout.Focused); 
            };

            this.cefRenderHandler = new XiliumCefRenderHandler(x, y, width, height);

            this.cefDisplayHandler = new XiliumCefDisplayHandler(layout);
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            if (browser != null)
            {
                var id = browser.Identifier;

                var host = browser.GetHost();
                if (host != null)
                {
                    host.CloseBrowser();
                    host.Dispose();
                    host = null;
                }
                browser.Dispose();
                browser = null;

                ApiMethodRegistryManager.Destroy(id);
            }
            SafeDispose.Dispose(cefRenderHandler);

            GC.SuppressFinalize(this);
            Disposed = true;
        }

        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A content manager with which to load layout resources.</param>
        /// <param name="layout">The asset path of the layout to load.</param>
        public void LoadLayout(ContentManager content, String layout)
        {
            Contract.Require(content, "content");
            Contract.RequireNotEmpty(layout, "layout");
            Contract.EnsureNotDisposed(this, Disposed);

            if (layout.StartsWith("http://") || layout.StartsWith("https://"))
            {
                Browser.GetMainFrame().LoadUrl(layout);
            }
            else
            {
                var path = content.ResolveAssetFilePath(layout);
                var uri = "file://" + path;
                Browser.GetMainFrame().LoadUrl(uri);
            }
        }

        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A content manager with which to load layout resources.</param>
        /// <param name="definition">A UI panel definition containing the layout information to load.</param>
        public void LoadLayout(ContentManager content, UIPanelDefinition definition)
        {
            Contract.Require(content, "content");
            Contract.Require(definition, "definition");
            Contract.EnsureNotDisposed(this, Disposed);

            if (String.IsNullOrWhiteSpace(definition.LayoutSource))
                return;

            var asset = Path.Combine(definition.LayoutRootDirectory, definition.LayoutSource);
            LoadLayout(content, asset);
        }

        /// <summary>
        /// Draws the layout.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the layout.</param>
        /// <param name="color">The color with which to tint the layout.</param>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.EnsureNotDisposed(this, Disposed);

            cefRenderHandler.Draw(spriteBatch, color);
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the client's position on the screen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.Position;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cefRenderHandler.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the browser's position on the screen.
        /// </summary>
        public Int32 X
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.X;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cefRenderHandler.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the browser's positoin on the screen.
        /// </summary>
        public Int32 Y
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.Y;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cefRenderHandler.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the browser's size in pixels.
        /// </summary>
        public Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.Size;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (cefRenderHandler.Size != value)
                {
                    cefRenderHandler.Size = value;
                    BrowserHost.WasResized();
                }
            }
        }

        /// <summary>
        /// Gets or sets the browser's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.Width;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (cefRenderHandler.Width != value)
                {
                    cefRenderHandler.Width = value;
                    BrowserHost.WasResized();
                }
            }
        }

        /// <summary>
        /// Gets or sets the browser's height in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cefRenderHandler.Height;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (cefRenderHandler.Height != value)
                {
                    cefRenderHandler.Height = value;
                    BrowserHost.WasResized();
                }
            }
        }

        /// <summary>
        /// Gets the CEF browser instance.
        /// </summary>
        public CefBrowser Browser
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return browser;
            }
        }

        /// <summary>
        /// Gets the CEF browser host.
        /// </summary>
        public CefBrowserHost BrowserHost
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return browserHost;
            }
        }

        /// <summary>
        /// Occurs when the client is initialized.
        /// </summary>
        public event XiliumCefClientEventHandler Initialized;

        /// <summary>
        /// Occurs when the client starts loading a layout.
        /// </summary>
        public event XiliumCefClientEventHandler LoadStart;

        /// <summary>
        /// Occurs when the layout finishes loading.
        /// </summary>
        public event XiliumCefClientEventHandler LoadEnd;

        /// <summary>
        /// Raises the Initialized event.
        /// </summary>
        private void OnInitialized()
        {
            var temp = Initialized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the LoadStart event.
        /// </summary>
        private void OnLoadStart()
        {
            var temp = LoadStart;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the LoadEnd event.
        /// </summary>
        private void OnLoadEnd()
        {
            var temp = LoadEnd;
            if (temp != null)
            {
                temp(this);
            }
        }

        // CEF handlers.
        private readonly XiliumCefLifeSpanHandler cefLifeSpanHandler;
        private readonly XiliumCefLoadHandler cefLoadHandler;
        private readonly XiliumCefRenderHandler cefRenderHandler;
        private readonly XiliumCefDisplayHandler cefDisplayHandler;
        
        // CEF browser object
        private CefBrowser browser;
        private CefBrowserHost browserHost;
    }
}
