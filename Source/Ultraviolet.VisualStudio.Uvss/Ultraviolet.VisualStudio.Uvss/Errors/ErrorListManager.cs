using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Manages the error list attached to text view.
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("uvss")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public sealed class ErrorListManager : IWpfTextViewCreationListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorListManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The Visual Studio service provider.</param>
        /// <param name="textDocumentFactoryService">The text document factory service.</param>
        [ImportingConstructor]
        public ErrorListManager(SVsServiceProvider serviceProvider, 
            ITextDocumentFactoryService textDocumentFactoryService)
        {
            this.serviceProvider = serviceProvider;
            this.textDocumentFactoryService = textDocumentFactoryService;
        }

        /// <inheritdoc/>
        public void TextViewCreated(IWpfTextView textView)
        {
            textView.Closed += OnViewClosed;

            var textDocument = default(ITextDocument);
            if (textDocumentFactoryService.TryGetTextDocument(textView.TextBuffer, out textDocument))
            {
                textView.TextBuffer.Properties.GetOrCreateSingletonProperty(() =>
                    new ErrorList(serviceProvider, textDocument));
            }
        }

        /// <summary>
        /// Called when the view is closed.
        /// </summary>
        private static void OnViewClosed(Object sender, EventArgs e)
        {
            var view = (IWpfTextView)sender;
            view.Closed -= OnViewClosed;

            var errorListHelper = view.TextBuffer.GetErrorList();
            errorListHelper?.Dispose();
        }

        // Visual Studio services.
        private readonly SVsServiceProvider serviceProvider;
        private readonly ITextDocumentFactoryService textDocumentFactoryService;
    }
}
