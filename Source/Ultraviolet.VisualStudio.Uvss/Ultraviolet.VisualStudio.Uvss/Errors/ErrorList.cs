using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Ultraviolet.Presentation.Uvss.Diagnostics;
using Ultraviolet.VisualStudio.Uvss.Parsing;

namespace Ultraviolet.VisualStudio.Uvss.Errors
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
        public ErrorList(
            IServiceProvider serviceProvider, 
            ITextDocument textDocument)
        {
            this.serviceProvider = serviceProvider;

            this.errorListProvider = new ErrorListProvider(serviceProvider);

			this.textDocument = textDocument;
			this.textBuffer = UvssTextBuffer.ForBuffer(textDocument.TextBuffer);
			this.textBuffer.Parser.DocumentParsed += (sender, e) =>
			{
				var mostRecentResult = default(UvssTextParserResult);
				var mostRecentTask = this.textBuffer.Parser.GetMostRecent(out mostRecentResult);
				OnDocumentGenerated(mostRecentResult);
			};
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (textDocument != null)
            {
                textDocument = null;
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
			lock (errors)
			{
				return new List<Error>(errors);
			}
        }

        /// <inheritdoc/>
        public IEnumerable<Error> GetErrorsInSpan(SnapshotSpan span)
        {
			lock (errors)
			{
				return errors.Where(x =>
				{
					return x.Span.TranslateTo(span.Snapshot, SpanTrackingMode.EdgeExclusive).IntersectsWith(span);
				}).ToList();
			}
        }

		/// <inheritdoc/>
		public void Update()
		{
			lock (errors)
			{
				if (!errorsDirty || textBuffer.Parser.IsParsing)
					return;

				errorListProvider.Tasks.Clear();

				foreach (var error in errors)
					errorListProvider.Tasks.Add(TaskFromError(error));

				errorsDirty = false;
			}
		}

		/// <summary>
		/// Creates a <see cref="Task"/> that represents the specified <see cref="Error"/>.
		/// </summary>
		/// <param name="error">The <see cref="Error"/> for which to create a <see cref="Task"/>.</param>
		/// <returns>The <see cref="Task"/> that represents the specified <see cref="Error"/>.</returns>
		private Task TaskFromError(Error error)
        {
            var taskSeverity = default(TaskErrorCategory);
            switch (error.DiagnosticInfo.Severity)
            {
                case DiagnosticSeverity.Hidden:
                case DiagnosticSeverity.Info:
                    return null;

                case DiagnosticSeverity.Warning:
                    taskSeverity = TaskErrorCategory.Warning;
                    break;

                case DiagnosticSeverity.Error:
                    taskSeverity = TaskErrorCategory.Error;
                    break;
            }

            var taskSpan = error.TagSafeSpan;
            var taskPos = taskSpan.End == taskSpan.Snapshot.Length ? taskSpan.End : taskSpan.Start;
            var taskLine = taskSpan.Snapshot.GetLineFromPosition(taskPos);

            var task = new ErrorTask()
            {
                Text = error.DiagnosticInfo.Message,
                Line = taskLine.LineNumber,
                Column = taskPos - taskLine.Start.Position,
                Category = TaskCategory.CodeSense,
                ErrorCategory = taskSeverity,
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
		private void OnDocumentGenerated(UvssTextParserResult result)
		{
			lock (errors)
			{
				errors.Clear();

				if (result.Document != null)
				{
					var diagnostics = result.Document.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToList();
					var diagnosticErrors = diagnostics.Select(x =>
					{
						var errorSpan = new SnapshotSpan(result.Snapshot, x.Location.Start, x.Location.Length);
						return new Error(textDocument.FilePath, errorSpan, x);
					});

					errors.AddRange(diagnosticErrors);
				}

				errorsDirty = true;
			}
		}

        // Visual Studio services.
        private IServiceProvider serviceProvider;
        private ITextDocument textDocument;

		// The text buffer for this document.
		private UvssTextBuffer textBuffer;

        // Known errors.
        private ErrorListProvider errorListProvider;
        private Boolean errorsDirty;
        private readonly List<Error> errors = new List<Error>();
    }
}
