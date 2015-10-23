using System;
using System.Reflection;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the screen that provides the default implementation for the <see cref="MessageBox"/> modal.
    /// </summary>
    public sealed class MessageBoxScreen : UIScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxScreen"/> class.
        /// </summary>
        /// <param name="messageBox">The message box that owns the screen.</param>
        /// <param name="globalContent">The screen's global content manager.</param>
        internal MessageBoxScreen(MessageBoxModal messageBox, ContentManager globalContent) 
            : base(globalContent.RootDirectory, "TwistedLogik.Ultraviolet.UI.Presentation.Resources.Content.UI.Screens.MessageBoxScreen.MessageBoxScreen.xml", globalContent)
        {
            if (View != null)
            {
                View.SetViewModel(new MessageBoxViewModel(messageBox));
            }
        }
        
        /// <inheritdoc/>
        protected override UIPanelDefinition LoadPanelDefinition(String asset)
        {
            if (String.IsNullOrEmpty(asset))
                return null;
            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(asset))
            {
                return LocalContent.LoadFromStream<UIPanelDefinition>(stream, "xml");
            }
        }
    }
}
