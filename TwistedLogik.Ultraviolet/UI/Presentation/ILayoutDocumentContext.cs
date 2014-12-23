using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the context for a particular layout document, such as a view or a user control.
    /// </summary>
    internal interface ILayoutDocumentContext
    {
        /// <summary>
        /// Registers an element identifier with the document.
        /// </summary>
        /// <param name="element">The element to register.</param>
        void RegisterElementID(UIElement element);

        /// <summary>
        /// Unregisters an element identifier from the document.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        void UnregisterElementID(UIElement element);
    }
}
