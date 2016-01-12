using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.UVSS.Classification
{
    partial class UvssParserService
    {
        /// <summary>
        /// Represents a cache of UVSS documents which have been generated for various spans
        /// of a given text snapshot.
        /// </summary>
        public class DocumentCache
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DocumentCache"/> class.
            /// </summary>
            /// <param name="snapshot">The text snapshot for which documents are being generated.</param>
            public DocumentCache(ITextSnapshot snapshot)
            {
                this.snapshot = snapshot;
            }

            /// <summary>
            /// Gets a task which parses the specified span of text.
            /// </summary>
            /// <param name="span">The span of text which is being parsed.</param>
            /// <returns>A task which parses the specified span of text.</returns>
            public Task<UvssDocumentSyntax> GetParsingTaskForSpan(Span span)
            {
                lock (parsingTasks)
                {
                    var task = default(Task<UvssDocumentSyntax>);
                    if (parsingTasks.TryGetValue(span, out task))
                        return task;

                    var source = new SnapshotSpan(snapshot, span).GetText();

                    task = Task.Run(() =>
                    {
                        return UvssParser.Parse(source);
                    });

                    parsingTasks.Add(span, task);
                    return task;
                }
            }

            // State values.
            private readonly ITextSnapshot snapshot;
            private readonly Dictionary<Span, Task<UvssDocumentSyntax>> parsingTasks =
                new Dictionary<Span, Task<UvssDocumentSyntax>>();
        }
    }
}
