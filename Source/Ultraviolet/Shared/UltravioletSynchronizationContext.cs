using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a custom synchronization context which marshalls asynchronous
    /// calls onto the main Ultraviolet context thread.
    /// </summary>
    public sealed class UltravioletSynchronizationContext : SynchronizationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletSynchronizationContext"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        internal UltravioletSynchronizationContext(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            this.Ultraviolet = uv;
        }

        /// <inheritdoc/>
        public override SynchronizationContext CreateCopy()
        {
            return new UltravioletSynchronizationContext(Ultraviolet);
        }

        /// <inheritdoc/>
        public override void Send(SendOrPostCallback d, Object state)
        {
            d(state);
        }

        /// <inheritdoc/>
        public override void Post(SendOrPostCallback d, Object state)
        {
            EnqueueWorkItemTask(new Task(() => d(state)));
        }

        /// <summary>
        /// Processes a single queued work item, if any work items have been queued.
        /// </summary>
        public void ProcessSingleWorkItem()
        {
            if (queuedWorkItems.TryDequeue(out var workItem))
            {
                workItem.RunSynchronously();
            }
        }

        /// <summary>
        /// Processes all queued work items.
        /// </summary>
        public void ProcessWorkItems()
        {
            var count = Interlocked.CompareExchange(ref pendingWorkItemCount, 0, 0);
            if (count == 0)
                return;

            while (queuedWorkItems.TryDequeue(out var workItem))
            {
                workItem.RunSynchronously();
                Interlocked.Decrement(ref pendingWorkItemCount);
            }
        }

        /// <summary>
        /// Gets the Ultraviolet context associated with this synchronization context.
        /// </summary>
        public UltravioletContext Ultraviolet { get; }

        /// <summary>
        /// Adds the specified task to the queue of work items.
        /// </summary>
        private T EnqueueWorkItemTask<T>(T task) where T : Task
        {
            queuedWorkItems.Enqueue(task);
            Interlocked.Increment(ref pendingWorkItemCount);
            return task;
        }

        // The queue of work items waiting to be processed.
        private readonly ConcurrentQueue<Task> queuedWorkItems = new ConcurrentQueue<Task>();
        private Int32 pendingWorkItemCount;
    }
}
