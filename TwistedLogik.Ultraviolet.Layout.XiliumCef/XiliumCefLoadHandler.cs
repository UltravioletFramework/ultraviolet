using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// represents the method that is called when a load handler raises an event.
    /// </summary>
    /// <param name="handler"></param>
    internal delegate void XiliumCefLoadHandlerEvent(XiliumCefLoadHandler handler);

    /// <summary>
    /// Represents the load handler for the CEF client.
    /// </summary>
    internal sealed class XiliumCefLoadHandler : CefLoadHandler
    {
        /// <summary>
        /// Initializes a new instance of the XiliumCefLoadHandler class.
        /// </summary>
        public XiliumCefLoadHandler()
        {

        }

        /// <summary>
        /// Occurs when the layout's main frame starts loading.
        /// </summary>
        public event XiliumCefLoadHandlerEvent MainFrameLoadStart;

        /// <summary>
        /// Occurs when the layout's main frame finishes loading.
        /// </summary>
        public event XiliumCefLoadHandlerEvent MainFrameLoadEnd;

        /// <summary>
        /// Called when the browser starts loading a frame.
        /// </summary>
        protected override void OnLoadStart(CefBrowser browser, CefFrame frame)
        {
            if (frame.IsMain)
            {
                var temp = MainFrameLoadStart;
                if (temp != null)
                {
                    temp(this);
                }
            }
            base.OnLoadStart(browser, frame);
        }

        /// <summary>
        /// Called when the browser finishes loading a frame.
        /// </summary>
        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            if (frame.IsMain)
            {
                var temp = MainFrameLoadEnd;
                if (temp != null)
                {
                    temp(this);
                }
            }
            base.OnLoadEnd(browser, frame, httpStatusCode);
        }
    }
}
