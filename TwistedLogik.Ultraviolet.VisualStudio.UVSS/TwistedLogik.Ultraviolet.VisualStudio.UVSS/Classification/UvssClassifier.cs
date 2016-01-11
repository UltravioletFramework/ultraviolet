using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.UVSS.Classification
{
    /// <summary>
    /// Classifies UVSS source text.
    /// </summary>
    internal class UvssClassifier : IClassifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssClassifier"/> class.
        /// </summary>
        /// <param name="registry">The classification registry.</param>
        /// <param name="parserService">The UVSS language parser service.</param>
        internal UvssClassifier(IClassificationTypeRegistryService registry, IUvssParserService parserService)
        {
            this.registry = registry;
            this.parserService = parserService;
        }
        
#pragma warning disable 67

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67
        
        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            if (span.Snapshot.Length > 5000000)
                return emptySpanList;
            
            var task = parserService.GetDocument(span.Snapshot);
            task.Wait(100);

            if (task.Status == TaskStatus.RanToCompletion)
                return VisitDocument(task.Result, span);

            task.ContinueWith(t =>
            {
                RaiseClassificationChanged(span.Snapshot, 0, span.Snapshot.Length);
            }, 
            TaskContinuationOptions.OnlyOnRanToCompletion);

            return emptySpanList;
        }

        /// <summary>
        /// Visits the specified UVSS document and returns a list of classification spans that
        /// intersect with the specified span.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        /// <param name="span">The span for which to retrieve classification spans.</param>
        /// <returns>The classification spans that intersect with the specified span.</returns>
        public IList<ClassificationSpan> VisitDocument(UvssDocumentSyntax document, SnapshotSpan span)
        {
            if (document == null)
                return null;

            var snapshotChanged = (span.Snapshot != oldSnapshot);
            if (snapshotChanged)
                oldSnapshot = span.Snapshot;

            var newMultiLineCommentSpans = new List<SnapshotSpan>();

            var results = new List<ClassificationSpan>();
            var visitor = new UvssClassifierVisitor(registry, (start, width, type, kind) =>
            {
                if (kind == SyntaxKind.MultiLineCommentTrivia)
                    newMultiLineCommentSpans.Add(new SnapshotSpan(span.Snapshot, start, width));

                var nodeSpan = new SnapshotSpan(span.Snapshot, start, width);
                if (nodeSpan.IntersectsWith(span))
                    results.Add(new ClassificationSpan(nodeSpan, type));
            });
            visitor.Visit(document);

            if (snapshotChanged)
            {
                if (oldMultiLineCommentSpans != null)
                {
                    foreach (var oldSpan in oldMultiLineCommentSpans)
                    {
                        var invalidatedSpan = oldSpan.TranslateTo(span.Snapshot, SpanTrackingMode.EdgeExclusive);
                        RaiseClassificationChanged(span.Snapshot, invalidatedSpan.Start, invalidatedSpan.Length);
                    }
                }

                foreach (var newSpan in newMultiLineCommentSpans)
                {
                    var invalidatedSpan = newSpan;
                    RaiseClassificationChanged(span.Snapshot, invalidatedSpan.Start, invalidatedSpan.Length);
                }

                oldMultiLineCommentSpans = newMultiLineCommentSpans;
            }

            return results;
        }

        /// <summary>
        /// Raises the <see cref="ClassificationChanged"/> event.
        /// </summary>
        private void RaiseClassificationChanged(ITextSnapshot snapshot, Int32 start, Int32 length)
        {
            var handler = ClassificationChanged;
            if (handler != null)
            {
                handler(this, new ClassificationChangedEventArgs(
                    new SnapshotSpan(snapshot, start, length)));
            }
        }
        
        // A cached empty list of classification spans.
        private static readonly IList<ClassificationSpan> emptySpanList = new ClassificationSpan[0];

        // Classification services.
        private readonly IClassificationTypeRegistryService registry;
        private readonly IUvssParserService parserService;

        // Tracks the state of the parsed document.
        private ITextSnapshot oldSnapshot;
        private IList<SnapshotSpan> oldMultiLineCommentSpans;
    }
}
