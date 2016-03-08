using System;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's configuration settings.
    /// </summary>
    public abstract class UltravioletConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletConfiguration"/> class.
        /// </summary>
        public UltravioletConfiguration()
        {
            WindowIsVisible = true;
            WindowIsResizable = true;
            InitialWindowPosition = new Rectangle(100, 100, 640, 480);
        }

        /// <summary>
        /// Gets or sets the <see cref="RenderTargetUsage"/> value which is used by the back buffer.
        /// </summary>
        public RenderTargetUsage BackBufferRenderTargetUsage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the assembly which is responsible for
        /// creating and managing instances of the <see cref="TwistedLogik.Ultraviolet.UI.UIView"/> class.
        /// </summary>
        public String ViewProviderAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an object which provides configuration values for the view provider.
        /// </summary>
        public Object ViewProviderConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to create a debug context.
        /// </summary>
        public Boolean Debug
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the levels of debug output which are enabled.
        /// </summary>
        public DebugLevels DebugLevels
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the delegate that is invoked when a debug message is logged.
        /// </summary>
        public DebugCallback DebugCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Ultraviolet context is headless.
        /// A headless context will not create a default window upon initialization.
        /// </summary>
        public Boolean Headless
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether service mode is enabled. 
        /// </summary>
        /// <remarks>
        /// <para>In service mode, the graphics subsystem is never intialized and no windows are ever created. 
        /// This is different from headless mode, where the context's primary window is invisible but
        /// still exists.</para>
        /// <para>This mode is primarily useful in circumstances where an Ultraviolet-based application has to run in the context of a
        /// Windows service, for example on a build server. The Windows security model prevents services from accessing the graphics device,
        /// so we need to avoid doing so or else we'll run into errors.</para>
        /// </remarks>
        public Boolean EnableServiceMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Ultraviolet context's default window is visible at startup.
        /// If the context is headless, this setting has no effect.
        /// </summary>
        public Boolean WindowIsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Ultraviolet context's default window can be resized.
        /// If the context is headless, this setting has no effect.
        /// </summary>
        public Boolean WindowIsResizable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Ultraviolet context's default window is borderless.
        /// If the context is headless, this setting has no effect.
        /// </summary>
        public Boolean WindowIsBorderless
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the intial size and position of the context's primary window.
        /// </summary>
        public Rectangle InitialWindowPosition
        {
            get;
            set;
        }
    }
}
