using System;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Produces instances of the ApiSchemeHandler class.
    /// </summary>
    internal sealed class ApiSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        /// <summary>
        /// Produces an instance of the ApiSchemeHandler class that exposes the specified browser's
        /// registered API calls via the http://api/ domain.
        /// </summary>
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, String schemeName, CefRequest request)
        {
            var registry = ApiMethodRegistryManager.GetByBrowserID(browser.Identifier);
            return new ApiSchemeHandler(registry);
        }
    }
}
