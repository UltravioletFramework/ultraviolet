using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Ultraviolet.VisualStudio.Uvss.Parsing;

namespace Ultraviolet.VisualStudio.Uvss.Classification
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
        internal UvssClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.registry = registry;
			this.buffer = UvssTextBuffer.ForBuffer(buffer);
            this.buffer.CommentSpanInvalidated += (obj, span) =>
                RaiseClassificationChanged(new SnapshotSpan(this.buffer.Buffer.CurrentSnapshot, span));
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
			var blockSpan = buffer.GetOutermostBlockSpan(span);

			var mostRecentDocument = default(UvssTextParserResult);
			var task = buffer.Parser.GetParseTask(span.Snapshot, out mostRecentDocument);
			if (task.Status != TaskStatus.RanToCompletion)
			{
				task.ContinueWith(t =>
				{
					RaiseClassificationChanged(blockSpan);
				},
				TaskContinuationOptions.OnlyOnRanToCompletion);
			}

			return VisitDocument(mostRecentDocument, blockSpan) ?? emptySpanList;
		}

        /// <summary>
        /// Visits the specified UVSS document and returns a list of classification spans that
        /// intersect with the specified span.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        /// <param name="span">The span for which to retrieve classification spans.</param>
        /// <returns>The classification spans that intersect with the specified span.</returns>
        public IList<ClassificationSpan> VisitDocument(UvssTextParserResult result, SnapshotSpan span)
        {
            if (result.Document == null)
                return null;

			span = span.TranslateTo(result.Snapshot, SpanTrackingMode.EdgeExclusive);
            
            var results = new List<ClassificationSpan>();
            var visitor = new UvssClassifierVisitor(registry, (start, width, type, kind) =>
            {
                var nodeSpan = new SnapshotSpan(span.Snapshot, start, width);
                if (nodeSpan.IntersectsWith(span))
                    results.Add(new ClassificationSpan(nodeSpan, type));
            });
            visitor.Visit(result.Document);
            
            return results;
        }

        /// <summary>
        /// Raises the <see cref="ClassificationChanged"/> event.
        /// </summary>
        private void RaiseClassificationChanged(SnapshotSpan span)
        {
            var handler = ClassificationChanged;
            if (handler != null)
            {
                handler(this, new ClassificationChangedEventArgs(span));
            }
        }

        // A cached empty list of classification spans.
        private static readonly IList<ClassificationSpan> emptySpanList = new ClassificationSpan[0];

        // Classification services.
        private readonly IClassificationTypeRegistryService registry;
        private readonly UvssTextBuffer buffer;
    }
}
