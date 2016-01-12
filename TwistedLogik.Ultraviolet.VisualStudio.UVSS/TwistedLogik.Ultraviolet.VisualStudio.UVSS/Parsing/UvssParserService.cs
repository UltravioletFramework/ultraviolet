using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents a service which parses text snapshots into UVSS documents.
    /// </summary>
    [Export]
    public partial class UvssParserService : IUvssParserService
    {
        /// <inheritdoc/>
        public Task<UvssDocumentSyntax> GetDocument(SnapshotSpan span)
        {
            var snapshot = span.Snapshot;
            var textBuffer = snapshot.TextBuffer;

            lock (textBuffer)
            {
                var documentCacheMap = default(ConditionalWeakTable<ITextSnapshot, DocumentCache>);
                var documentCache = default(DocumentCache);

                if (!textBuffer.Properties.TryGetProperty(typeof(UvssParserService), out documentCacheMap))
                {
                    documentCacheMap = new ConditionalWeakTable<ITextSnapshot, DocumentCache>();
                    textBuffer.Properties.AddProperty(typeof(UvssParserService), documentCacheMap);
                }

                if (!documentCacheMap.TryGetValue(span.Snapshot, out documentCache))
                {
                    documentCache = new DocumentCache(span.Snapshot);
                    documentCacheMap.Add(span.Snapshot, documentCache);
                }

                return documentCache.GetParsingTaskForSpan(span.Span);
            }
        }
    }
}
