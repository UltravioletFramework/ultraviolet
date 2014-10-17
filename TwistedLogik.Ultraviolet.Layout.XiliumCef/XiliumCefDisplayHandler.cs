using System;
using TwistedLogik.Nucleus;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents the display handler for the CEF client.
    /// </summary>
    internal sealed class XiliumCefDisplayHandler : CefDisplayHandler
    {
        /// <summary>
        /// Initializes a new instance of the XiliumCefDisplayHandler class.
        /// </summary>
        /// <param name="layout">The layout that owns the handler.</param>
        public XiliumCefDisplayHandler(XiliumCefUILayout layout)
        {
            Contract.Require(layout, "layout");

            this.layout = layout;
        }

        /// <summary>
        /// Handles console messages from the browser.
        /// </summary>
        protected override Boolean OnConsoleMessage(CefBrowser browser, String message, String source, Int32 line)
        {
            layout.HandleConsoleMessage(message, source, line);
            return true;
        }

        // The layout that owns the handler.
        private readonly XiliumCefUILayout layout;
    }
}
