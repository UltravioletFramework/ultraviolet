using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's configuration settings.
    /// </summary>
    public abstract class UltravioletConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletConfiguration class.
        /// </summary>
        public UltravioletConfiguration()
        {
            WindowIsVisible = true;
            WindowIsResizable = true;
            InitialWindowPosition = new Vector2(100, 100);
            InitialWindowClientSize = new Size2(640, 480);
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
        /// Gets the initial position of the context's primary window.
        /// </summary>
        public Vector2 InitialWindowPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the initial client size of the context's primary window.
        /// </summary>
        public Size2 InitialWindowClientSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the assembly that contains the application's layout provider.
        /// </summary>
        public String LayoutProviderAssembly
        {
            get;
            set;
        }
    }
}
