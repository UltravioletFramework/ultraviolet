using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;
using TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Classification
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
        /// <param name="buffer">The text buffer that contains the text being classified.</param>
        internal UvssClassifier(IClassificationTypeRegistryService registry, IUvssParserService parserService, ITextBuffer buffer)
        {
            this.registry = registry;
            this.parserService = parserService;

            buffer.Changed += Buffer_Changed;

            this.multiLineCommentTracker = new MultiLineCommentTracker(buffer);
            this.multiLineCommentTracker.SpanInvalidated += MultiLineCommentTracker_SpanInvalidated;

            this.braceTracker = new BraceTracker(buffer, multiLineCommentTracker);
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

            var blockSpan = braceTracker.GetOutermostBlockSpan(span);

            var task = parserService.GetDocument(blockSpan);
            task.Wait(100);

            if (task.Status == TaskStatus.RanToCompletion)
                return VisitDocument(task.Result, blockSpan, blockSpan.Start);

            task.ContinueWith(t =>
            {
                RaiseClassificationChanged(blockSpan.Snapshot, blockSpan.Start, blockSpan.Length);
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
        /// <param name="offset">The offset to apply to the returned classification spans.</param>
        /// <returns>The classification spans that intersect with the specified span.</returns>
        public IList<ClassificationSpan> VisitDocument(UvssDocumentSyntax document, SnapshotSpan span, Int32 offset)
        {
            if (document == null)
                return null;
            
            var results = new List<ClassificationSpan>();
            var visitor = new UvssClassifierVisitor(registry, (start, width, type, kind) =>
            {
                var nodeSpan = new SnapshotSpan(span.Snapshot, start + offset, width);
                if (nodeSpan.IntersectsWith(span))
                    results.Add(new ClassificationSpan(nodeSpan, type));
            });
            visitor.Visit(document);
            
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

        /// <summary>
        /// Called when the contents of the text buffer are changed.
        /// </summary>
        private void Buffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            if (multiLineCommentTracker != null)
                multiLineCommentTracker.OnBufferChanged(sender, e);

            if (braceTracker != null)
                braceTracker.OnBufferChanged(sender, e);
        }

        /// <summary>
        /// Called when the multi-line comment tracker invalidates a span of text.
        /// </summary>
        private void MultiLineCommentTracker_SpanInvalidated(Object obj, SnapshotSpan span)
        {
            RaiseClassificationChanged(span.Snapshot, span.Start, span.Length);
        }

        // A cached empty list of classification spans.
        private static readonly IList<ClassificationSpan> emptySpanList = new ClassificationSpan[0];

        // Classification services.
        private readonly IClassificationTypeRegistryService registry;
        private readonly IUvssParserService parserService;
        private readonly MultiLineCommentTracker multiLineCommentTracker;
        private readonly BraceTracker braceTracker;
    }
}
