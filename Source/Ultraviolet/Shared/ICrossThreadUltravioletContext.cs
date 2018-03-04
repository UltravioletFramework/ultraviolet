using System;
using System.Threading;
using System.Threading.Tasks;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an interface to an <see cref="UltravioletContext"/> which only exposes
    /// members which are safe to call from a background thread.
    /// </summary>
    public interface ICrossThreadUltravioletContext
    {
        /// <inheritdoc cref="UltravioletContext.SpawnTask(Action{CancellationToken})" />
        Task SpawnTask(Action<CancellationToken> action);

        /// <inheritdoc cref="UltravioletContext.QueueWorkItem(Action{object}, object, WorkItemOptions)" />
        Task QueueWorkItem(Action<Object> workItem, Object state = null, WorkItemOptions options = WorkItemOptions.None);

        /// <inheritdoc cref="UltravioletContext.QueueWorkItem(Func{object, Task}, object, WorkItemOptions)" />
        Task QueueWorkItem(Func<Object, Task> workItem, Object state = null, WorkItemOptions options = WorkItemOptions.None);

        /// <inheritdoc cref="UltravioletContext.QueueWorkItem{T}(Func{object, T}, object, WorkItemOptions)" />
        Task<T> QueueWorkItem<T>(Func<Object, T> workItem, Object state = null, WorkItemOptions options = WorkItemOptions.None);

        /// <inheritdoc cref="UltravioletContext.QueueWorkItem{T}(Func{object, Task{T}}, object, WorkItemOptions)" />
        Task<T> QueueWorkItem<T>(Func<Object, Task<T>> workItem, Object state = null, WorkItemOptions options = WorkItemOptions.None);

        /// <inheritdoc cref="UltravioletContext.Runtime" />
        UltravioletRuntime Runtime { get; }

        /// <inheritdoc cref="UltravioletContext.Platform" />
        UltravioletPlatform Platform { get; }

        /// <inheritdoc cref="UltravioletContext.Messages" />
        IMessageQueue<UltravioletMessageID> Messages { get; }
    }
}
