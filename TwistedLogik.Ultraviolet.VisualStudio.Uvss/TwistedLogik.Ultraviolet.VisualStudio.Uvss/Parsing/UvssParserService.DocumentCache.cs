using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing
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
            /// <param name="completed">An action to invoke when parsing is completed.</param>
            /// <returns>A task which parses the specified span of text.</returns>
            public Task<UvssDocumentSyntax> GetParsingTaskForSpan(Span span, Action<UvssDocumentSyntax> completed = null)
            {
                lock (parsingTasks)
                {
                    var task = default(Task<UvssDocumentSyntax>);
                    if (parsingTasks.TryGetValue(span, out task))
                        return task;

					var options = 
						UvssParserOptions.PartialDocument |
						UvssParserOptions.PartialDocumentStartsOnEmptyLine;

					var sourceSpan = new SnapshotSpan(snapshot, span);
                    var source = sourceSpan.GetText();

					var firstLine = snapshot.GetLineFromPosition(sourceSpan.Start.Position);
					var firstLineSource = firstLine.GetText();

					if (firstLine.Start != sourceSpan.Start)
					{
						var precedingSource = firstLineSource.Substring(0, sourceSpan.Start.Position - firstLine.Start.Position);
						if (precedingSource.Any(x => !Char.IsWhiteSpace(x)))
						{
							options &= ~UvssParserOptions.PartialDocumentStartsOnEmptyLine;
						}
					}
				
                    task = Task.Run(() =>
                    {
                        var document = UvssParser.Parse(source, options);
                        if (completed != null)
                        {
                            completed(document);
                        }
                        return document;
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
