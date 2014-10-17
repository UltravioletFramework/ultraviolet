using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a layout provider using XiliumCef.
    /// </summary>
    public sealed class XiliumCefUILayoutProvider : UltravioletResource, IUILayoutProvider
    {
        /// <summary>
        /// Initializes the XiliumCefUILayoutProvider type.
        /// </summary>
        static XiliumCefUILayoutProvider()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new NotSupportedException();
            }
            LibraryLoader.Load("libcef");
        }

        /// <summary>
        /// Initializes a new instance of the XiliumCefUILayoutProvider class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public XiliumCefUILayoutProvider(UltravioletContext uv)
            : base(uv)
        {
            CefRuntime.Load();
            
            cefSettings = new CefSettings();
            cefSettings.MultiThreadedMessageLoop = false;
            cefSettings.WindowlessRenderingEnabled = true;
            cefSettings.BrowserSubprocessPath = "TwistedLogik.Ultraviolet.Layout.XiliumCef.exe";

            if (CefRuntime.ExecuteProcess(cefArgs, null, IntPtr.Zero) != -1)
                throw new LayoutEngineException(XiliumStrings.CefRuntimeError);

            CefRuntime.Initialize(cefArgs, cefSettings, null, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory("http", "api", new ApiSchemeHandlerFactory());
            CefRuntime.RegisterSchemeHandlerFactory("http", "rvalue", new ScriptResultSchemeHandlerFactory());
        }

        /// <summary>
        /// Updates the provider's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!cefSettings.MultiThreadedMessageLoop)
            {
                CefRuntime.DoMessageLoopWork();
            }
        }

        /// <summary>
        /// Creates a new layout object.
        /// </summary>
        /// <param name="x">The x-coordinate of the layout's initial position.</param>
        /// <param name="y">The y-coordinate of the layout's initial position.</param>
        /// <param name="width">The layout's initial width in pixels.</param>
        /// <param name="height">The layout's initial height in pixels.</param>
        /// <returns>The layout object that was created.</returns>
        public IUILayout CreateLayout(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return new XiliumCefUILayout(Ultraviolet, x, y, width, height);
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (!Disposed)
            {
                CefRuntime.Shutdown();
            }
            base.Dispose(disposing);
        }

        // Arguments and settings for Chromium Embedded Framework.
        private readonly CefMainArgs cefArgs = new CefMainArgs(new String[0]);
        private readonly CefSettings cefSettings;
    }
}
