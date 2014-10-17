using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents the method that is called when a CEF life span handler creates a browser instance.
    /// </summary>
    /// <param name="handler">The life span handler that raised the event.</param>
    /// <param name="browser">The browser instance that was created.</param>
    internal delegate void XiliumCefLifeSpanHandlerBrowserCreatedEventHandler(XiliumCefLifeSpanHandler handler, CefBrowser browser);

    /// <summary>
    /// Represents the life span handler for the CEF client.
    /// </summary>
    internal sealed class XiliumCefLifeSpanHandler : CefLifeSpanHandler
    {
        /// <summary>
        /// Occurs when the life span handler creates a browser instance.
        /// </summary>
        public event XiliumCefLifeSpanHandlerBrowserCreatedEventHandler BrowserCreated;

        /// <summary>
        /// Called after the browser is created.
        /// </summary>
        /// <param name="browser">The browser that was created.</param>
        protected override void OnAfterCreated(CefBrowser browser)
        {
            browser.GetHost().SetMouseCursorChangeDisabled(true);

            var temp = BrowserCreated;
            if (temp != null)
            {
                temp(this, browser);
            }
            base.OnAfterCreated(browser);
        }

        /// <summary>
        /// Called before the browser is destroyed.
        /// </summary>
        /// <param name="browser">The browser that is being destroyed.</param>
        protected override void OnBeforeClose(CefBrowser browser)
        {
            base.OnBeforeClose(browser);
        }
    }
}
