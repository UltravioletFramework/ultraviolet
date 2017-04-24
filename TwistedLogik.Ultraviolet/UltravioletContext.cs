using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Platform;
using Ultraviolet.UI;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a callback that is invoked when the Ultraviolet Framework logs a debug message.
    /// </summary>
    /// <param name="uv">The Ultraviolet Context that logged the message.</param>
    /// <param name="level">A <see cref="DebugLevels"/> value representing the debug level of the message.</param>
    /// <param name="message">The debug message text.</param>
    public delegate void DebugCallback(UltravioletContext uv, DebugLevels level, String message);

    /// <summary>
    /// Represents a method that is called in response to an Ultraviolet context event.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    public delegate void UltravioletContextEventHandler(UltravioletContext uv);

    /// <summary>
    /// Represents the method that is called when an Ultraviolet context is about to draw the current scene.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    public delegate void UltravioletContextDrawEventHandler(UltravioletContext uv, UltravioletTime time);

    /// <summary>
    /// Represents the method that is called when an Ultraviolet context has drawn or is about to draw a particular window.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    /// <param name="window">The window that was drawn or is about to be drawn.</param>
    public delegate void UltravioletContextWindowDrawEventHandler(UltravioletContext uv, UltravioletTime time, IUltravioletWindow window);

    /// <summary>
    /// Represents the method that is called when an Ultraviolet context updates the application state.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
    public delegate void UltravioletContextUpdateEventHandler(UltravioletContext uv, UltravioletTime time);

    /// <summary>
    /// Represents the Ultraviolet Framework and all of its subsystems.
    /// </summary>
    public abstract class UltravioletContext : 
        IMessageSubscriber<UltravioletMessageID>,
        IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletContext"/> class.
        /// </summary>
        /// <param name="host">The object that is hosting the Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        public UltravioletContext(IUltravioletHost host, UltravioletConfiguration configuration)
        {
            Contract.Require(host, nameof(host));
            Contract.Require(configuration, nameof(configuration));

            ProvideHintsToXamarinLinker();

            AcquireContext();
            DetectPlatform();

            this.isRunningInServiceMode = configuration.EnableServiceMode;
            this.supportsHighDensityDisplayModes = configuration.SupportsHighDensityDisplayModes;

            this.host = host;

            this.thread = Thread.CurrentThread;

            this.messages = new LocalMessageQueue<UltravioletMessageID>();
            this.messages.Subscribe(this, UltravioletMessages.Quit);

            this.syncContext = new UltravioletSynchronizationContext(this);
            ChangeSynchronizationContext(syncContext);

            this.taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.taskFactory = new TaskFactory(taskScheduler);

            InitializeFactory(configuration);
        }

        /// <summary>
        /// Retrieves the current Ultraviolet context, throwing an exception if it does not exist.
        /// </summary>
        /// <returns>The current Ultraviolet context, or <see langword="null"/> if no contex exists.</returns>
        public static UltravioletContext RequestCurrent()
        {
            return current;
        }

        /// <summary>
        /// Retrieves the current Ultraviolet context, throwing an exception if it does not exist.
        /// </summary>
        /// <returns>The current Ultraviolet context.</returns>
        public static UltravioletContext DemandCurrent()
        {
            if (current == null)
                throw new InvalidOperationException(UltravioletStrings.ContextMissing);

            return current;
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            Contract.EnsureNotDisposed(this, disposed);

            OnReceivedMessage(type, data);
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Processes the context's message queue.
        /// </summary>
        public void ProcessMessages()
        {
            messages.Process();
        }

        /// <summary>
        /// Processes a single queued work item, if any work items have been queued.
        /// </summary>
        public void ProcessSingleWorkItem()
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Ensure(thread == Thread.CurrentThread, UltravioletStrings.WorkItemsMustBeProcessedOnMainThread);

            var syncContext = SynchronizationContext.Current as UltravioletSynchronizationContext;
            if (syncContext != null)
                syncContext.ProcessSingleWorkItem();
        }

        /// <summary>
        /// Processes all queued work items.
        /// </summary>
        public void ProcessWorkItems()
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Ensure(thread == Thread.CurrentThread, UltravioletStrings.WorkItemsMustBeProcessedOnMainThread);

            var syncContext = SynchronizationContext.Current as UltravioletSynchronizationContext;
            if (syncContext != null)
                syncContext.ProcessWorkItems();
        }
        
        /// <summary>
        /// Called when a new frame is started.
        /// </summary>
        public void HandleFrameStart()
        {
            UltravioletProfiler.BeginSection(UltravioletProfilerSections.Frame);
            OnFrameStart();
        }

        /// <summary>
        /// Called when a frame is completed.
        /// </summary>
        public void HandleFrameEnd()
        {
            OnFrameEnd();
            UltravioletProfiler.EndSection(UltravioletProfilerSections.Frame);
        }

        /// <summary>
        /// Updates the game state while the application is suspended.
        /// </summary>
        public virtual void UpdateSuspended()
        {

        }

        /// <summary>
        /// Updates the game state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public virtual void Update(UltravioletTime time)
        {

        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public virtual void Draw(UltravioletTime time)
        {

        }

        /// <summary>
        /// Gets the platform interop subsystem.
        /// </summary>
        /// <returns>The platform interop subsystem.</returns>
        public abstract IUltravioletPlatform GetPlatform();

        /// <summary>
        /// Gets the content management subsystem.
        /// </summary>
        /// <returns>The content management subsystem.</returns>
        public abstract IUltravioletContent GetContent();

        /// <summary>
        /// Gets the graphics subsystem.
        /// </summary>
        /// <returns>The graphics subsystem.</returns>
        public abstract IUltravioletGraphics GetGraphics();

        /// <summary>
        /// Gets the audio subsystem.
        /// </summary>
        /// <returns>The audio subsystem.</returns>
        public abstract IUltravioletAudio GetAudio();

        /// <summary>
        /// Gets the input subsystem.
        /// </summary>
        /// <returns>The input subsystem.</returns>
        public abstract IUltravioletInput GetInput();

        /// <summary>
        /// Gets the user interface subsystem.
        /// </summary>
        /// <returns>The user interface subsystem.</returns>
        public abstract IUltravioletUI GetUI();

        /// <summary>
        /// Gets the factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <returns>The default factory method of the specified delegate type, or <see langword="null"/> if no such factory method is registered.</returns>
        public T TryGetFactoryMethod<T>() where T : class
        {
            Contract.EnsureNotDisposed(this, disposed);

            return factory.TryGetFactoryMethod<T>();
        }

        /// <summary>
        /// Attempts to retrieve a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <param name="name">The name of the factory method to retrieve.</param>
        /// <returns>The specified named factory method, or <see langword="null"/> if no such factory method is registered.</returns>
        public T TryGetFactoryMethod<T>(String name) where T : class
        {
            Contract.EnsureNotDisposed(this, disposed);

            return factory.TryGetFactoryMethod<T>(name);
        }

        /// <summary>
        /// Gets the factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <returns>The default factory method of the specified delegate type.</returns>
        public T GetFactoryMethod<T>() where T : class
        {
            Contract.EnsureNotDisposed(this, disposed);

            return factory.GetFactoryMethod<T>();
        }

        /// <summary>
        /// Gets a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <param name="name">The name of the factory method to retrieve.</param>
        /// <returns>The specified named factory method.</returns>
        public T GetFactoryMethod<T>(String name) where T : class
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.RequireNotEmpty(name, nameof(name));

            return factory.GetFactoryMethod<T>(name);
        }

        /// <summary>
        /// Spawns a new task.
        /// </summary>
        /// <remarks>Tasks spawned using this method will not be started until the next call to <see cref="Update(UltravioletTime)"/>, and will prevent
        /// the Ultraviolet context from shutting down until they complete or are canceled.  Do not attempt to <see cref="Task.Wait()"/> on these
        /// tasks from the main Ultraviolet thread; doing so will introduce a deadlock.</remarks>
        /// <param name="action">The action to perform within the task.</param>
        /// <returns>The <see cref="Task"/> that was spawned.</returns>
        public Task SpawnTask(Action<CancellationToken> action)
        {
            Contract.Require(action, nameof(action));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotSpawnTasks);

            var token = cancellationTokenSource.Token;
            var task = taskFactory.StartNew(() => action(token), token, TaskCreationOptions.None, TaskScheduler.Default);

            lock (tasksPending)
                tasksPending.Add(task);

            return task;
        }

        /// <summary>
        /// Waits for any pending tasks to complete.
        /// </summary>
        /// <param name="cancel">A value indicating whether to cancel pending tasks.</param>
        public void WaitForPendingTasks(Boolean cancel = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (cancel)
                cancellationTokenSource.Cancel();

            while (true)
            {
                var done = true;

                lock (tasksPending)
                {
                    foreach (var task in tasksPending)
                    {
                        if (task.Status != TaskStatus.Created && !task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                        {
                            done = false;
                        }
                    }
                }

                if (done)
                    break;

                ProcessWorkItems();
                Thread.Yield();
            }
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item 
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        public void QueueWorkItemAndWait(Action workItem, Boolean forceAsync = false)
        {
            QueueWorkItem(workItem, forceAsync).Wait();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        public void QueueWorkItemAndWait(Action<Object> workItem, Object state, Boolean forceAsync = false)
        {
            QueueWorkItem(workItem, state, forceAsync).Wait();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public T QueueWorkItemAndWait<T>(Func<T> workItem, Boolean forceAsync = false)
        {
            var task = QueueWorkItem(workItem, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public T QueueWorkItemAndWait<T>(Func<Object, T> workItem, Object state, Boolean forceAsync = false)
        {
            var task = QueueWorkItem(workItem, state, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <remarks>Unlike <see cref="QueueWorkItemAndWait{T}(Func{T}, bool)"/>, this method will not 
        /// automatically unwrap the inner <see cref="Task"/> which is produced by <paramref name="workItem"/>.</remarks>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public Task<T> QueueWorkItemWrappedAndWait<T>(Func<Task<T>> workItem, Boolean forceAsync = false)
        {
            var task = QueueWorkItemWrapped(workItem, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and
        /// waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <remarks>Unlike <see cref="QueueWorkItemAndWait{T}(Func{object, T}, object, bool)"/>, this method will not 
        /// automatically unwrap the inner <see cref="Task"/> which is produced by <paramref name="workItem"/>.</remarks>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public Task<T> QueueWorkItemWrappedAndWait<T>(Func<Object, Task<T>> workItem, Object state, Boolean forceAsync)
        {
            var task = QueueWorkItemWrapped(workItem, state, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Action workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromAction(workItem) :
                taskFactory.StartNew(workItem);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Action<Object> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromAction(workItem, state) :
                taskFactory.StartNew(workItem, state);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<T> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem()) :
                taskFactory.StartNew(workItem);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<Object, T> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem(state)) :
                taskFactory.StartNew(workItem, state);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Func<Task> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem()).Unwrap() :
                taskFactory.StartNew(workItem).Unwrap();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Func<Object, Task> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem(state)).Unwrap() :
                taskFactory.StartNew(workItem, state).Unwrap();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<Task<T>> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem()).Unwrap() :
                taskFactory.StartNew(workItem).Unwrap();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<Object, Task<T>> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem(state)).Unwrap() :
                taskFactory.StartNew(workItem, state).Unwrap();
       }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<Task> QueueWorkItemWrapped(Func<Task> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem()) :
                taskFactory.StartNew(workItem);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<Task> QueueWorkItemWrapped(Func<Object, Task> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem(state)) :
                taskFactory.StartNew(workItem, state);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <remarks>Unlike <see cref="QueueWorkItem{T}(Func{Task{T}}, bool)"/>, this method will not 
        /// automatically unwrap the inner <see cref="Task"/> which is produced by <paramref name="workItem"/>.</remarks>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<Task<T>> QueueWorkItemWrapped<T>(Func<Task<T>> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem()) :
                taskFactory.StartNew(workItem);
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <remarks>Unlike <see cref="QueueWorkItem{T}(Func{object, Task{T}}, object, bool)"/>, this method will not 
        /// automatically unwrap the inner <see cref="Task"/> which is produced by <paramref name="workItem"/>.</remarks>
        /// <remarks>This method will not automatically unwrap the inner <see cref="Task"/>
        /// which is produced by <paramref name="workItem"/>.</remarks>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item
        /// to be queued and executed asynchronously.
        /// If this value is <see langword="false"/>, then calls to this method from
        /// the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<Task<T>> QueueWorkItemWrapped<T>(Func<Object, Task<T>> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, nameof(workItem));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            return (IsExecutingOnCurrentThread && !forceAsync) ? TaskUtil.FromResult(workItem(state)) :
                taskFactory.StartNew(workItem, state);
        }

        /// <summary>
        /// Ensures that the specified resource was created by this context.
        /// This method is compiled out if the <c>DEBUG</c> compilation symbol is not specified.
        /// </summary>
        /// <param name="resource">The <see cref="UltravioletResource"/> to validate.</param>
        [Conditional("DEBUG")]
        public void ValidateResource(UltravioletResource resource)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (resource != null && resource.Ultraviolet != this)
                throw new InvalidOperationException(UltravioletStrings.InvalidResource);
        }

        /// <summary>
        /// Gets the platform on which this context is running.
        /// </summary>
        public UltravioletPlatform Platform
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.platform;
            }
        }

        /// <summary>
        /// Gets the object that is hosting the Ultraviolet context.
        /// </summary>
        public IUltravioletHost Host
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return host;
            }
        }
        
        /// <summary>
        /// Gets the context's message queue.
        /// </summary>
        public IMessageQueue<UltravioletMessageID> Messages
        {
            get { return messages; }
        }

        /// <summary>
        /// Gets a value indicating whether the context supports high-density display modes
        /// such as Retina and Retina HD. This allows the application to make use of every physical pixel 
        /// on the screen, rather than being scaled to use logical pixels.
        /// </summary>
        public Boolean SupportsHighDensityDisplayModes
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return supportsHighDensityDisplayModes;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the context is currently processing messages
        /// from the physical input devices.
        /// </summary>
        public Boolean IsHardwareInputDisabled
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isHardwareInputDisabled;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                isHardwareInputDisabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the context is running in service mode.
        /// </summary>
        public Boolean IsRunningInServiceMode
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isRunningInServiceMode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current thread is the thread which
        /// created the Ultraviolet context.
        /// </summary>
        /// <remarks>Many tasks, such as content loading, must take place on the Ultraviolet
        /// context's main thread.  Such tasks can be queued using the <see cref="QueueWorkItem(Action, Boolean)"/> method
        /// or one of its overloads, which will run the task at the start of the next update.</remarks>
        public Boolean IsExecutingOnCurrentThread
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return Thread.CurrentThread == thread;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the context has been initialized.
        /// </summary>
        public Boolean IsInitialized
        {
            get { return isInitialized; }
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Occurs when the current context is invalidated.
        /// </summary>
        public static event EventHandler ContextInvalidated;

        /// <summary>
        /// Occurs when the current context is initialized.
        /// </summary>
        public static event EventHandler ContextInitialized;

        /// <summary>
        /// Occurs when a new frame is started.
        /// </summary>
        public event UltravioletContextEventHandler FrameStart;

        /// <summary>
        /// Occurs when a frame is completed.
        /// </summary>
        public event UltravioletContextEventHandler FrameEnd;

        /// <summary>
        /// Occurs when the context is preparing to draw the current scene. This event is called
        /// before the context associates itself to any windows.
        /// </summary>
        public event UltravioletContextDrawEventHandler Drawing;

        /// <summary>
        /// Occurs when the context is preparing to draw a particular window.
        /// </summary>
        public event UltravioletContextWindowDrawEventHandler WindowDrawing;

        /// <summary>
        /// Occurs after the context has drawn a particular window.
        /// </summary>
        public event UltravioletContextWindowDrawEventHandler WindowDrawn;

        /// <summary>
        /// Occurs when the context is about to update the state of its subsystems.
        /// </summary>
        public event UltravioletContextUpdateEventHandler UpdatingSubsystems;

        /// <summary>
        /// Occurs when the context is updating the application's state.
        /// </summary>
        public event UltravioletContextUpdateEventHandler Updating;

        /// <summary>
        /// Occurs when the context is initialized.
        /// </summary>
        public event UltravioletContextEventHandler Initialized;

        /// <summary>
        /// Occurs when the Ultraviolet context is being shut down.
        /// </summary>
        public event UltravioletContextEventHandler Shutdown;

        /// <summary>
        /// Ensures that the specified function produces a valid instance of <see cref="UltravioletContext"/>. If it does not,
        /// then the current context is immediately disposed.
        /// </summary>
        /// <param name="fn">The function which will create the Ultraviolet context.</param>
        /// <returns>The Ultraviolet context that was created.</returns>
        internal static UltravioletContext EnsureSuccessfulCreation(Func<UltravioletContext> fn)
        {
            Contract.Require(fn, nameof(fn));

            try
            {
                var context = fn();
                if (context == null)
                {
                    throw new InvalidOperationException();
                }
                return context;
            }
            catch (Exception e1)
            {
                try
                {
                    var current = RequestCurrent();
                    if (current != null)
                    {
                        current.Dispose();
                    }
                }
                catch (Exception e2)
                {
                    var error = new StringBuilder();
                    error.AppendLine(Assembly.GetEntryAssembly().FullName);
                    error.AppendLine();
                    error.AppendLine($"An exception occurred while creating the Ultraviolet context, and Ultraviolet failed to cleanly shut down.");
                    error.AppendLine();
                    error.AppendLine($"Exception which occurred during context creation:");
                    error.AppendLine();
                    error.AppendLine(e1.ToString());
                    error.AppendLine();
                    error.AppendLine($"Exception which occurred during shutdown:");
                    error.AppendLine();
                    error.AppendLine(e2.ToString());

                    try
                    {
                        var errorDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var errorPath = $"uv-error-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}.txt";
                        File.WriteAllText(Path.Combine(errorDir, errorPath), error.ToString());
                    }
                    catch (IOException) { }
                }
                throw;
            }
        }
        
        /// <summary>
        /// Acquires an exclusive context claim, preventing other instances from being instantiated.
        /// </summary>
        private void AcquireContext()
        {
            lock (syncObject)
            {
                if (current != null)
                    throw new InvalidOperationException(UltravioletStrings.ContextAlreadyExists);

                current = this;
            }
        }

        /// <summary>
        /// Releases the current exclusive context claim.
        /// </summary>
        private void ReleaseContext()
        {
            lock (syncObject)
            {
                if (current == this)
                {
                    current = null;
                    OnContextInvalidated();
                }
            }
        }

        /// <summary>
        /// Initializes the context and marks it ready for use.
        /// </summary>
        protected void InitializeContext()
        {
            isInitialized = true;

            OnInitialized();
            OnContextInitialized();
        }

        /// <summary>
        /// Initializes any factory methods that are exposed by the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly for which to initialize factory methods.</param>
        protected void InitializeFactoryMethodsInAssembly(Assembly asm)
        {
            Contract.Require(asm, nameof(asm));

            var initializerTypes = from t in asm.GetTypes()
                                   where t.IsClass && !t.IsAbstract && typeof(IUltravioletFactoryInitializer).IsAssignableFrom(t)
                                   select t;

            foreach (var initializerType in initializerTypes)
            {
                var initializerInstance = (IUltravioletFactoryInitializer)Activator.CreateInstance(initializerType);
                initializerInstance.Initialize(this, Factory);
            }
        }

        /// <summary>
        /// Initializes the context's view provider.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        protected void InitializeViewProvider(UltravioletConfiguration configuration)
        {
            var initializerFactory = TryGetFactoryMethod<UIViewProviderInitializerFactory>();
            if (initializerFactory != null)
            {
                var initializer = initializerFactory();
                initializer.Initialize(this, configuration.ViewProviderConfiguration);
            }
        }

        /// <summary>
        /// Updates the context's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected void UpdateContext(UltravioletTime time)
        {
            ProcessWorkItems();
            UpdateTasks();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; 
        /// <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                SafeDispose.Dispose(GetUI());
                SafeDispose.Dispose(GetInput());
                SafeDispose.Dispose(GetContent());
                SafeDispose.Dispose(GetPlatform());
                SafeDispose.Dispose(GetGraphics());
                SafeDispose.Dispose(GetAudio());
            }

            WaitForPendingTasks(true);

            this.disposing = true;

            ProcessWorkItems();
            OnShutdown();

            ChangeSynchronizationContext(null);

            this.disposed = true;
            this.disposing = false;

            ReleaseContext();
        }

        /// <summary>
        /// Raises the <see cref="FrameStart"/> event.
        /// </summary>
        protected virtual void OnFrameStart() =>
            FrameStart?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="FrameEnd"/> event.
        /// </summary>
        protected virtual void OnFrameEnd() =>
            FrameEnd?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Drawing"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        protected virtual void OnDrawing(UltravioletTime time) =>
            Drawing?.Invoke(this, time);

        /// <summary>
        /// Raises the <see cref="WindowDrawing"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that is about to be drawn.</param>
        protected virtual void OnWindowDrawing(UltravioletTime time, IUltravioletWindow window) =>
            WindowDrawing?.Invoke(this, time, window);

        /// <summary>
        /// Raises the <see cref="WindowDrawn"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that was just drawn.</param>
        protected virtual void OnWindowDrawn(UltravioletTime time, IUltravioletWindow window) =>
            WindowDrawn?.Invoke(this, time, window);

        /// <summary>
        /// Raises the <see cref="UpdatingSubsystems"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdatingSubsystems(UltravioletTime time) =>
            UpdatingSubsystems?.Invoke(this, time);

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

        /// <summary>
        /// Occurs when the context receives a message from its queue.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        protected virtual void OnReceivedMessage(UltravioletMessageID type, MessageData data)
        {

        }

        /// <summary>
        /// Raises the <see cref="Initialized"/> event.
        /// </summary>
        protected virtual void OnInitialized() =>
            Initialized?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Shutdown"/> event.
        /// </summary>
        protected virtual void OnShutdown() =>
            Shutdown?.Invoke(this);

        /// <summary>
        /// Gets the context's object factory.
        /// </summary>
        protected UltravioletFactory Factory
        {
            get { return factory; }
        }

        /// <summary>
        /// Ensures that the Xamarin linker doesn't remove certain BCL methods which
        /// are required by the Framework, but which are only accessed through reflection.
        /// </summary>
        private static void ProvideHintsToXamarinLinker()
        {
#if ANDROID || IOS
            var nullable = Nullable.Equals<Int32>(123, 456);
            Console.WriteLine($"Ensuring that Nullable.Equals() is linked: [{nullable}]");

            var referenceEquals = Object.ReferenceEquals(new Object(), new Object());
            Console.WriteLine($"Ensuring that Object.ReferenceEquals() is linked: [{referenceEquals}]");

            var equals = Object.Equals(new Object(), new Object());
            Console.WriteLine($"Ensuring that Object.Equals() is linked: [{equals}]");
#endif
        }

        /// <summary>
        /// Raises the <see cref="ContextInitialized"/> event.
        /// </summary>
        private static void OnContextInitialized() =>
            ContextInitialized?.Invoke(null, EventArgs.Empty);

        /// <summary>
        /// Raises the <see cref="ContextInvalidated"/> event.
        /// </summary>
        private static void OnContextInvalidated() =>
            ContextInvalidated?.Invoke(null, EventArgs.Empty);

        /// <summary>
        /// Initializes the context's object factory.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        private void InitializeFactory(UltravioletConfiguration configuration)
        {
            var asmCore = typeof(UltravioletContext).Assembly;
            var asmImpl = GetType().Assembly;

            InitializeFactoryMethodsInAssembly(asmCore);
            InitializeFactoryMethodsInAssembly(asmImpl);
            InitializeFactoryMethodsInCompatibilityShim();
            InitializeFactoryMethodsInViewProvider(configuration);

            var asmEntry = Assembly.GetEntryAssembly();
            if (asmEntry != null)
            {
                InitializeFactoryMethodsInAssembly(asmEntry);
            }
        }

        /// <summary>
        /// Initializes any factory methods exposed by the current platform compatibility shim.
        /// </summary>
        private void InitializeFactoryMethodsInCompatibilityShim()
        {
            try
            {
                var shim = default(Assembly);

                switch (Platform)
                {
                    case UltravioletPlatform.Windows:
                    case UltravioletPlatform.Linux:
                        shim = Assembly.Load("TwistedLogik.Ultraviolet.Desktop");
                        break;

                    case UltravioletPlatform.OSX:
                        shim = Assembly.Load("TwistedLogik.Ultraviolet.OSX");
                        break;

                    case UltravioletPlatform.Android:
                        shim = Assembly.Load("TwistedLogik.Ultraviolet.Android.dll");
                        break;

                    case UltravioletPlatform.iOS:
                        /* On iOS, shim is built into core assembly to avoid linker issues */
                        break;

                    default:
                        throw new NotSupportedException();
                }

                if (shim != null)
                {
                    InitializeFactoryMethodsInAssembly(shim);
                }
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidCompatibilityShimException(UltravioletStrings.MissingCompatibilityShim.Format(e.FileName));
            }
        }

        /// <summary>
        /// Initializes any factory methods exposed by the registered view provider.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        private void InitializeFactoryMethodsInViewProvider(UltravioletConfiguration configuration)
        {
            if (String.IsNullOrEmpty(configuration.ViewProviderAssembly))
                return;

            Assembly asm;
            try
            {
                asm = Assembly.Load(configuration.ViewProviderAssembly);
                InitializeFactoryMethodsInAssembly(asm);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException ||
                    e is FileLoadException ||
                    e is BadImageFormatException)
                {
                    throw new InvalidOperationException(UltravioletStrings.InvalidViewProviderAssembly, e);
                }
                throw;
            }
        }

        /// <summary>
        /// Updates the context's list of tasks.
        /// </summary>
        private void UpdateTasks()
        {
            try
            {
                lock (tasksPending)
                {
                    if (tasksPending.Count == 0)
                        return;

                    foreach (var task in tasksPending)
                        tasksUpdating.Add(task);
                }

                foreach (var task in tasksUpdating)
                {
                    if (task.Status == TaskStatus.Created)
                        task.Start();

                    if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
                        tasksDead.Add(task);
                }

                lock (tasksPending)
                {
                    foreach (var task in tasksDead)
                        tasksPending.Remove(task);
                }
            }
            finally
            {
                tasksDead.Clear();
                tasksUpdating.Clear();
            }
        }

        /// <summary>
        /// Detects the platform on which the context is running.
        /// </summary>
        private void DetectPlatform()
        {
#if ANDROID
            this.platform = UltravioletPlatform.Android;
#elif IOS
            this.platform = UltravioletPlatform.iOS;
#else
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    this.platform = UltravioletPlatform.Windows;
                    break;

                case PlatformID.Unix:
                    {
                        var buf = IntPtr.Zero;
                        try
                        {
                            buf = Marshal.AllocHGlobal(8192);
                            if (UltravioletNative.uname(buf) == 0)
                            {
                                var os = Marshal.PtrToStringAnsi(buf);
                                if (String.Equals("Darwin", os, StringComparison.OrdinalIgnoreCase))
                                {
                                    this.platform = UltravioletPlatform.OSX;
                                }
                                else
                                {
                                    this.platform = UltravioletPlatform.Linux;
                                }
                            }
                        }
                        finally
                        {
                            if (buf != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(buf);
                            }
                        }
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
#endif
        }

        /// <summary>
        /// Changes the current thread's synchronization context.
        /// </summary>
        [SecuritySafeCritical]
        private void ChangeSynchronizationContext(SynchronizationContext syncContext)
        {
            SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        // The singleton instance of the Ultraviolet context.
        private static readonly Object syncObject = new Object();
        private static UltravioletContext current;

        // State values.
        private readonly IUltravioletHost host;
        private readonly UltravioletSynchronizationContext syncContext;
        private readonly UltravioletFactory factory = new UltravioletFactory();
        private readonly Thread thread;
        private Boolean supportsHighDensityDisplayModes;
        private Boolean isHardwareInputDisabled;
        private Boolean isRunningInServiceMode;
        private Boolean isInitialized;
        private Boolean disposed;
        private Boolean disposing;
        private UltravioletPlatform platform;

        // The context's list of pending tasks.
        private readonly TaskScheduler taskScheduler;
        private readonly TaskFactory taskFactory;
        private readonly List<Task> tasksUpdating = new List<Task>();
        private readonly List<Task> tasksPending = new List<Task>();
        private readonly List<Task> tasksDead = new List<Task>();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // The context event queue.
        private readonly LocalMessageQueue<UltravioletMessageID> messages;
    }
}
