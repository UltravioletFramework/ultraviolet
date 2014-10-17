using System;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Produces instances of the ScriptResultSchemeHandler class.
    /// </summary>
    internal sealed class ScriptResultSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        /// <summary>
        /// Produces new instances of the ScriptResultSchemeHandler clas which allows layout 
        /// scripts to asynchronously return values to the layout engine.
        /// </summary>
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, String schemeName, CefRequest request)
        {
            return new ScriptResultSchemeHandler();
        }
    }
}
