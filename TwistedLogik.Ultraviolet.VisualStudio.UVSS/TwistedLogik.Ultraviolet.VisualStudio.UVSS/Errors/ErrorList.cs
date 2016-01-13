using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;
using TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing;
using TwistedLogik.Ultraviolet.VisualStudio.Uvss.Tagging;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Represents the error list for a document.
    /// </summary>
    public sealed class ErrorList : IErrorList, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorList"/> class.
        /// </summary>
        /// <param name="serviceProvider">The Visual Studio service provider.</param>
        /// <param name="textDocument">The text document to which this error list belongs.</param>
        /// <param name="parserService">The parser service which is generating UVSS documents.</param>
        public ErrorList(
            IServiceProvider serviceProvider, 
            ITextDocument textDocument, 
            IUvssParserService parserService)
        {
            this.serviceProvider = serviceProvider;

            this.errorListProvider = new ErrorListProvider(serviceProvider);

            this.textDocument = textDocument;

            this.parserService = parserService;
            this.parserService.DocumentGenerated += OnDocumentGenerated;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (textDocument != null)
            {
                textDocument = null;
            }

            if (parserService != null)
            {
                parserService.DocumentGenerated -= OnDocumentGenerated;
                parserService = null;
            }

            lock (errors)
            {
                if (errorListProvider != null)
                {
                    errorListProvider.Dispose();
                    errorListProvider = null;
                }

                errors.Clear();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Error> GetErrors()
        {
            return new List<Error>(errors);
        }

        /// <inheritdoc/>
        public IEnumerable<Error> GetErrorsInSpan(SnapshotSpan span)
        {
            return errors.Where(x => x.Span.IntersectsWith(span)).ToList();
        }

        /// <inheritdoc/>
        public void Update()
        {
            lock (errors)
            {
                if (!errorsDirty)
                    return;

                errorListProvider.SuspendRefresh();
                errorListProvider.Tasks.Clear();

                foreach (var error in errors)
                {
                    var task = TaskFromError(error);
                    errorListProvider.Tasks.Add(task);
                }

                errorListProvider.ResumeRefresh();
            }
        }
        
        /// <summary>
        /// Creates a <see cref="Task"/> that represents the specified <see cref="Error"/>.
        /// </summary>
        /// <param name="error">The <see cref="Error"/> for which to create a <see cref="Task"/>.</param>
        /// <returns>The <see cref="Task"/> that represents the specified <see cref="Error"/>.</returns>
        private Task TaskFromError(Error error)
        {
            var line = error.Span.Snapshot.GetLineFromPosition(error.Span.Start);

            var task = new ErrorTask()
            {
                Text = error.Message,
                Line = line.LineNumber,
                Column = error.Span.Start - line.Start.Position,
                Category = TaskCategory.CodeSense,
                ErrorCategory = TaskErrorCategory.Error,
                Priority = TaskPriority.Normal,
                Document = error.File
            };
            task.Navigate += OnTaskNavigate;

            return task;
        }

        /// <summary>
        /// Called when navigating to an error task.
        /// </summary>
        private void OnTaskNavigate(Object sender, EventArgs e)
        {
            var task = (ErrorTask)sender;

            var fullPath = task.Document;
            var logicalView = new Guid(LogicalViewID.TextView);

            var hierarchy = default(IVsUIHierarchy);
            var itemID = default(uint);
            var windowFrame = default(IVsWindowFrame);

            var isClosed = !VsShellUtilities.IsDocumentOpen(serviceProvider, 
                fullPath, logicalView, out hierarchy, out itemID, out windowFrame);

            if (isClosed)
            {
                try
                {
                    VsShellUtilities.OpenDocument(serviceProvider, 
                        fullPath, logicalView, out hierarchy, out itemID, out windowFrame);
                }
                catch { return; }
            }
            
            ErrorHandler.ThrowOnFailure(windowFrame.Show());

            var vsTextView = VsShellUtilities.GetTextView(windowFrame);

            var vsTextBuffer = default(IVsTextLines);
            ErrorHandler.ThrowOnFailure(vsTextView.GetBuffer(out vsTextBuffer));
            
            var startLine = task.Line;
            var startCol = task.Column;

            var endLine = task.Line;
            var endCol = task.Column;

            var vsTextManager = (IVsTextManager)serviceProvider.GetService(typeof(SVsTextManager));
            vsTextManager.NavigateToLineAndColumn(vsTextBuffer, ref logicalView, startLine, startCol, endLine, endCol);
        }

        /// <summary>
        /// Called when a UVSS document is generated by the parser service.
        /// </summary>
        private void OnDocumentGenerated(SnapshotSpan span, UvssDocumentSyntax document)
        {
            if (span.Snapshot.TextBuffer == textDocument.TextBuffer)
            {
                lock (errors)
                {
                    var errorsInNewSnapshot = errors.Select(x => x.TranslateTo(span.Snapshot)).ToList();
                    errorsInNewSnapshot.RemoveAll(x => x.Span.IntersectsWith(span.Span));

                    errors.Clear();
                    errors.AddRange(errorsInNewSnapshot);

                    var visitor = new ErrorVisitor((start, width, message) =>
                    {
                        var absoluteStart = start + span.Start;

                        if (width == 0)
                            width = 1;

                        if (absoluteStart + width > span.Snapshot.Length)
                            absoluteStart = span.Snapshot.Length - width;

                        var errorSpan = new SnapshotSpan(span.Snapshot, absoluteStart, width);
                        var error = new Error(errorSpan, message, textDocument.FilePath);
                        errors.Add(error);
                    });
                    visitor.Visit(document);

                    errorsDirty = true;
                }
            }
        }

        // Visual Studio services.
        private IServiceProvider serviceProvider;
        private ITextDocument textDocument;
        private IUvssParserService parserService;

        // Known errors.
        private ErrorListProvider errorListProvider;
        private Boolean errorsDirty;
        private readonly List<Error> errors = new List<Error>();
    }
}
