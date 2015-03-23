using System;
using UvDebugSandbox.Input;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet;

namespace UvDebugSandbox.UI.Screens
{
    /// <summary>
    /// Represents the base class for screens in this application.
    /// </summary>
    public abstract class UvDebugScreen : UIScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvDebugScreen"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory of the panel's local content manager.</param>
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        /// <param name="uiScreenService">The screen service which created this screen.</param>
        public UvDebugScreen(String rootDirectory, String definitionAsset, ContentManager globalContent, UIScreenService uiScreenService)
            : base(rootDirectory, definitionAsset, globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            this.uiScreenService = uiScreenService;
        }

        /// <summary>
        /// Gets the screen service which created this screen.
        /// </summary>
        protected UIScreenService UIScreenService
        {
            get { return uiScreenService; }
        }

        // Property values.
        private readonly UIScreenService uiScreenService;
    }
}
