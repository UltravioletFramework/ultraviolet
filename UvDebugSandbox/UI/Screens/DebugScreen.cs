using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace UvDebugSandbox.UI.Screens
{
    /// <summary>
    /// Represents the base class for screens in this application.
    /// </summary>
    public abstract class DebugScreen : UIScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugScreen"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        /// <param name="uiScreenService">The screen service which created this screen.</param>
        public DebugScreen(String rootDirectory, String definitionAsset, ContentManager globalContent, UIScreenService uiScreenService)
            : base(rootDirectory, definitionAsset, globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            this.uiScreenService = uiScreenService;
        }

        /// <summary>
        /// Gets the screen service which created this screen.
        /// </summary>
        public UIScreenService UIScreenService
        {
            get { return uiScreenService; }
        }

        // Property values.
        private readonly UIScreenService uiScreenService;
    }
}
