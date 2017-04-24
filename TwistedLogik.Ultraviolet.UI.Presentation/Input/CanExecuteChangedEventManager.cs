using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Collections.Specialized;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an event manager which allows controls to weakly attach event handlers to the <see cref="ICommand.CanExecuteChanged"/> event.
    /// </summary>
    public partial class CanExecuteChangedEventManager : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanExecuteChangedEventManager"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        internal CanExecuteChangedEventManager(UltravioletContext uv)
            : base(uv)
        {
            this.weakHandlerPool = new ExpandingPool<WeakHandler>(8, 32, () => new WeakHandler(this));
            this.weakHandlerTable = new WeakKeyDictionary<Object, List<WeakHandler>>(8);
            this.keepAliveTable = new ConditionalWeakTable<Object, List<EventHandler>>();

            uv.Updating += Context_Updating;
        }

        /// <summary>
        /// Adds the specified delegate as an event handler of the specified source.
        /// </summary>
        /// <param name="source">The source object that raises the <see cref="ICommand.CanExecuteChanged"/> event.</param>
        /// <param name="handler">The delegate that handles the <see cref="ICommand.CanExecuteChanged"/> event.</param>
        public static void AddHandler(ICommand source, EventHandler handler)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(handler, nameof(handler));

            var instance = singleton.Value;
            if (instance == null)
                throw new InvalidOperationException(UltravioletStrings.ContextMissing);

            instance.AddHandlerInternal(source, handler);
        }

        /// <summary>
        /// Removes the specified event handler from the specified source.
        /// </summary>
        /// <param name="source">The source object that raises the <see cref="ICommand.CanExecuteChanged"/> event.</param>
        /// <param name="handler">The delegate that handles the <see cref="ICommand.CanExecuteChanged"/> event.</param>
        public static void RemoveHandler(ICommand source, EventHandler handler)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(handler, nameof(handler));

            var instance = singleton.Value;
            if (instance == null)
                throw new InvalidOperationException(UltravioletStrings.ContextMissing);

            instance.RemoveHandlerInternal(source, handler);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (!(Ultraviolet?.Disposed ?? true))
                {
                    Ultraviolet.Updating -= Context_Updating;
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Requests that the manager be purged of unused entries.
        /// </summary>
        protected void ScheduleCleanup()
        {
            cleanupScheduled = true;
        }

        /// <summary>
        /// Adds the specified delegate as an event handler of the specified source.
        /// </summary>
        private void AddHandlerInternal(ICommand source, EventHandler handler)
        {
            rwlock.EnterWriteLock();
            try
            {
                var weakHandlersForSource = default(List<WeakHandler>);
                if (!weakHandlerTable.TryGetValue(source, out weakHandlersForSource))
                {
                    weakHandlersForSource = new List<WeakHandler>();
                    weakHandlerTable.Add(source, weakHandlersForSource);
                }

                var weakHandler = weakHandlerPool.Retrieve();
                weakHandler.Attach(source, handler);
                weakHandlersForSource.Add(weakHandler);

                AddKeepAlive(handler);
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the specified event handler from the specified source.
        /// </summary>
        private void RemoveHandlerInternal(ICommand source, EventHandler handler)
        {
            rwlock.EnterWriteLock();
            try
            {
                var needsCleanup = false;

                var weakHandlerRemoved = default(WeakHandler);
                var weakHandlersForSource = default(List<WeakHandler>);
                if (weakHandlerTable.TryGetValue(source, out weakHandlersForSource))
                {
                    foreach (var weakHandler in weakHandlersForSource)
                    {
                        if (weakHandler.IsFor(source, handler))
                        {
                            weakHandlerRemoved = weakHandler;
                            break;
                        }

                        if (!weakHandler.IsAlive)
                            needsCleanup = true;
                    }

                    if (weakHandlerRemoved != null)
                    {
                        weakHandlersForSource.Remove(weakHandlerRemoved);
                        weakHandlerRemoved.Detach();
                        weakHandlerPool.Release(weakHandlerRemoved);
                        RemoveKeepAlive(handler);
                    }

                    if (needsCleanup)
                        ScheduleCleanup();
                }
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds an entry to the conditional weak table which keeps handler instances alive.
        /// </summary>
        private void AddKeepAlive(EventHandler handler)
        {
            var target = handler.Target ?? this;
            var list = default(List<EventHandler>);
            if (!keepAliveTable.TryGetValue(target, out list))
            {
                list = new List<EventHandler>();
                keepAliveTable.Add(target, list);
            }
            list.Add(handler);
        }

        /// <summary>
        /// Removes an entry from the conditional weak table which keeps handler instances alive.
        /// </summary>
        private void RemoveKeepAlive(EventHandler handler)
        {
            var target = handler.Target ?? this;
            var list = default(List<EventHandler>);
            if (keepAliveTable.TryGetValue(target, out list))
            {
                if (list.Remove(handler) && list.Count == 0)
                {
                    keepAliveTable.Remove(target);
                }                
            }
        }

        /// <summary>
        /// Handles the <see cref="UltravioletContext.Updating"/> event.
        /// </summary>
        private void Context_Updating(UltravioletContext uv, UltravioletTime time)
        {
            if (!cleanupScheduled)
                return;

            var removedWeakHandlers = default(List<WeakHandler>);

            rwlock.EnterWriteLock();
            try
            {
                var removedSources = default(List<Object>);

                foreach (var weakHandlersKvp in weakHandlerTable)
                {
                    var weakSource = (WeakKeyReference<Object>)weakHandlersKvp.Key;
                    var source = weakSource.Target;
                    if (weakSource.IsAlive)
                    {
                        if (removedWeakHandlers != null)
                            removedWeakHandlers.Clear();

                        // Search the source's list for dead handlers
                        var weakHandlerList = weakHandlersKvp.Value;
                        var handlerList = weakHandlerList;
                        foreach (var weakHandler in handlerList)
                        {
                            if (!weakHandler.IsAlive)
                            {
                                if (removedWeakHandlers == null)
                                    removedWeakHandlers = new List<WeakHandler>();

                                removedWeakHandlers.Add(weakHandler);
                            }
                        }

                        // Remove any dead handlers that we found earlier
                        if (removedWeakHandlers != null)
                        {
                            foreach (var weakHandler in removedWeakHandlers)
                            {
                                var sourceHandler = weakHandler.SourceHandler;

                                handlerList.Remove(weakHandler);
                                weakHandler.Detach();
                                weakHandlerPool.Release(weakHandler);

                                if (sourceHandler != null)
                                    RemoveKeepAlive(sourceHandler);
                            }
                        }
                    }
                    else
                    {
                        // Keep track of dead sources
                        if (removedSources == null)
                            removedSources = new List<Object>();

                        removedSources.Add(source);
                    }

                    // Remove any dead sources that we found earlier
                    if (removedSources != null)
                    {
                        foreach (var removedSource in removedSources)
                            weakHandlerTable.Remove(removedSource);
                    }
                }
            }
            finally
            {
                rwlock.ExitWriteLock();
                cleanupScheduled = false;
            }
        }

        // Singleton instance.
        private static readonly UltravioletSingleton<CanExecuteChangedEventManager> singleton =
            new UltravioletSingleton<CanExecuteChangedEventManager>(UltravioletSingletonFlags.DisabledInServiceMode,
                uv => new CanExecuteChangedEventManager(uv));

        // State values.
        private readonly IPool<WeakHandler> weakHandlerPool;
        private readonly WeakKeyDictionary<Object, List<WeakHandler>> weakHandlerTable;
        private readonly ConditionalWeakTable<Object, List<EventHandler>> keepAliveTable;
        private Boolean cleanupScheduled;

        // Thread synchronization.
        private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
    }
}
