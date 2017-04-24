using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Presentation.Uvss;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{

    /// <summary>
    /// Performs parsing of documents for a text buffer.
    /// </summary>
    public sealed class UvssTextParser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UvssTextParser"/> class.
		/// </summary>
		/// <param name="buffer">The text buffer which will be parsed.</param>
		public UvssTextParser(ITextBuffer buffer)
		{
			this.buffer = buffer;
		}

		/// <summary>
		/// Gets a task which produces a UVSS document for the specified text snapshot.
		/// </summary>
		/// <param name="snapshot">The snapshot for which to return a task.</param>
		/// <param name="mostRecentDocument">The most recent fully-parsed document for the buffer.</param>
		/// <returns>The task that was retrieved for the specified snapshot.</returns>
		public Task<UvssTextParserResult> GetParseTask(ITextSnapshot snapshot, out UvssTextParserResult mostRecentDocument)
		{
			lock (syncObject)
			{
				var requestedVersion = snapshot.Version;

				if (versionComplete != null && versionComplete.VersionNumber >= requestedVersion.VersionNumber)
				{
					mostRecentDocument = this.mostRecentDocument;
					return taskComplete;
				}

				if (versionProcessing != null && versionProcessing.VersionNumber >= requestedVersion.VersionNumber)
				{
					mostRecentDocument = this.mostRecentDocument;
					return taskProcessing;
				}

				if (taskQueued != null)
				{
					mostRecentDocument = this.mostRecentDocument;
					return taskQueued;
				}
			}

			var task = default(Task<UvssTextParserResult>);
			task = new Task<UvssTextParserResult>(() =>
			{
				ITextVersion version;
				lock (syncObject)
				{
					version = buffer.CurrentSnapshot.Version;
					versionProcessing = version;
				}

				var currentSnapshot = buffer.CurrentSnapshot;
				var source = currentSnapshot.GetText();
				var document = UvssParser.Parse(source);
				var result = new UvssTextParserResult(currentSnapshot, document);

				lock (syncObject)
				{
					this.mostRecentDocument = result;

					versionComplete = version;
					versionProcessing = null;

					taskComplete = task;
					taskProcessing = null;

					if (taskQueued != null)
					{
						taskProcessing = taskQueued;
						taskQueued = null;
						taskProcessing.Start();
					}
				}

				RaiseDocumentParsed(new UvssTextParserEventArgs(result));
				return result;
			});

			lock (syncObject)
			{
				if (taskProcessing == null)
				{
					taskProcessing = task;
					taskProcessing.Start();
				}
				else
				{
					taskQueued = new Task<UvssTextParserResult>(() =>
					{
						Task.Delay(QueuedTaskDelay).Wait();
						task.Start();
						return task.Result;
					});
				}
			}

			mostRecentDocument = this.mostRecentDocument;
			return task;
		}

		/// <summary>
		/// Gets the most recently parsed document.
		/// </summary>
		/// <param name="mostRecentDocument">The most recent fully-parsed document for the buffer.</param>
		/// <returns>The task that was used to generate the document.</returns>
		public Task<UvssTextParserResult> GetMostRecent(out UvssTextParserResult mostRecentDocument)
		{
			lock (syncObject)
			{
				mostRecentDocument = this.mostRecentDocument;
				return taskComplete;
			}
		}

		/// <summary>
		/// Gets a value indicating whether parsing is in progress.
		/// </summary>
		public Boolean IsParsing
		{
			get
			{
				lock (syncObject)
				{
					return taskProcessing != null || taskQueued != null;
				}
			}
		}

		/// <summary>
		/// Occurs when a document finishes parsing.
		/// </summary>
		public event UvssTextParserEventHandler DocumentParsed;

		/// <summary>
		/// Raises the <see cref="DocumentParsed"/> event.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		private void RaiseDocumentParsed(UvssTextParserEventArgs args)
		{
			var temp = DocumentParsed;
			if (temp != null)
			{
				temp(this, args);
			}
		}

		// The delay before the queued task executes.
		private const Int32 QueuedTaskDelay = 500;

		// The text buffer that is being wrapped.
		private readonly Object syncObject = new Object();
		private readonly ITextBuffer buffer;

		// Tracks the versions of documents that we have processed.
		private ITextVersion versionComplete;
		private ITextVersion versionProcessing;

		// Tasks for recent parses.
		private Task<UvssTextParserResult> taskComplete;
		private Task<UvssTextParserResult> taskProcessing;
		private Task<UvssTextParserResult> taskQueued;

		// The document that was most recently parsed.
		private UvssTextParserResult mostRecentDocument;
	}
}
