using System;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a CEF client.
    /// </summary>
    internal sealed partial class XiliumCefClient : IDisposable
    {
        /// <summary>
        /// The implementation of CefClient.
        /// </summary>
        private sealed class CefClientImpl : CefClient
        {
            /// <summary>
            /// Initializes a new instance of the CefClientImpl class.
            /// </summary>
            /// <param name="wrapper">The client's wrapper object.</param>
            /// <param name="width">The browser's initial width in pixels.</param>
            /// <param name="height">The browser's initial height in pixels.</param>
            public CefClientImpl(XiliumCefClient wrapper, Int32 width, Int32 height)
            {
                this.wrapper = wrapper;

                var cefWindowInfo = CefWindowInfo.Create();
                cefWindowInfo.Width = width;
                cefWindowInfo.Height = height;
                cefWindowInfo.SetAsWindowless(IntPtr.Zero, true);

                var cefBrowserSettings = new CefBrowserSettings();
                cefBrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
                cefBrowserSettings.WebGL = CefState.Disabled;

                CefBrowserHost.CreateBrowser(cefWindowInfo, this, cefBrowserSettings);
            }

            /// <summary>
            /// Gets the client's life span handler.
            /// </summary>
            protected override CefLifeSpanHandler GetLifeSpanHandler()
            {
                return wrapper.cefLifeSpanHandler;
            }

            /// <summary>
            /// Gets the client's load handler.
            /// </summary>
            protected override CefLoadHandler GetLoadHandler()
            {
                return wrapper.cefLoadHandler;
            }

            /// <summary>
            /// Gets the client's render handler.
            /// </summary>
            protected override CefRenderHandler GetRenderHandler()
            {
                return wrapper.cefRenderHandler;
            }

            /// <summary>
            /// Gets the client's display handler.
            /// </summary>
            protected override CefDisplayHandler GetDisplayHandler()
            {
                return wrapper.cefDisplayHandler;
            }

            // The client's wrapper object.
            private readonly XiliumCefClient wrapper;
        }
    }
}
