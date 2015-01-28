using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;

namespace TwistedLogik.Ultraviolet
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
            Contract.Require(host, "host");
            Contract.Require(configuration, "configuration");

            AcquireContext();

            this.host = host;

            this.thread = Thread.CurrentThread;

            this.messages = new LocalMessageQueue<UltravioletMessageID>();
            this.messages.Subscribe(this, UltravioletMessages.Quit);

            InitializeFactory(configuration);
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
        /// Processes a single queued work item, if any work items have been queued.
        /// </summary>
        public void ProcessSingleWorkItem()
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Ensure(thread == Thread.CurrentThread, UltravioletStrings.WorkItemsMustBeProcessedOnMainThread);

            Task workItem;
            if (queuedWorkItems.TryDequeue(out workItem))
            {
                workItem.RunSynchronously();
            }
        }

        /// <summary>
        /// Processes all queued work items.
        /// </summary>
        public void ProcessWorkItems()
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Ensure(thread == Thread.CurrentThread, UltravioletStrings.WorkItemsMustBeProcessedOnMainThread);

            Task workItem;
            while (queuedWorkItems.TryDequeue(out workItem))
            {
                workItem.RunSynchronously();
            }
        }

        /// <summary>
        /// Updates the game state while the application is suspended.
        /// </summary>
        /// <remarks>Certain platforms, such as phones and tablets, may suspend the application while it is in the background
        /// in order to save power. Ultraviolet will automatically cease calling <see cref="Update"/> and <see cref="Draw"/> when
        /// this happens; however, some implementations may require that certain processing continues to take place even while 
        /// the application is suspended. The <see cref="UpdateSuspended"/> method is provided for such a scenario.
        /// </remarks>
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
        /// <returns>The default factory method of the specified delegate type, or <c>null</c> if no such factory method is registered.</returns>
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
        /// <returns>The specified named factory method, or <c>null</c> if no such factory method is registered.</returns>
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
            Contract.RequireNotEmpty(name, "name");

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
            Contract.Require(action, "action");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotSpawnTasks);

            var token = cancellationTokenSource.Token;
            var task = new Task(() => action(token), token);

            lock (tasksPending)
            {
                tasksPending.Add(task);
            }
            return task;
        }
    
        /// <summary>
        /// Waits for any pending tasks to complete.
        /// </summary>
        /// <param name="cancel">A value indicating whether to cancel pending tasks.</param>
        public void WaitForPendingTasks(Boolean cancel = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (tasksPending)
            {
                if (tasksPending.Count == 0)
                    return;

                if (cancel)
                    cancellationTokenSource.Cancel();

                while (true)
                {
                    var done = true;
                    foreach (var task in tasksPending)
                    {
                        if (task.Status != TaskStatus.Created && !task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                        {
                            done = false;
                        }
                    }
                    if (done)
                    {
                        break;
                    }
                    ProcessWorkItems();
                    Thread.Yield();
                }
            }
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and waits for the work item to be executed.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        public void QueueWorkItemAndWait(Action workItem, Boolean forceAsync = false)
        {
            QueueWorkItem(workItem, forceAsync).Wait();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and waits for the work item to be executed.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        public void QueueWorkItemAndWait(Action<Object> workItem, Object state, Boolean forceAsync = false)
        {
            QueueWorkItem(workItem, state, forceAsync).Wait();
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public T QueueWorkItemAndWait<T>(Func<T> workItem, Boolean forceAsync = false)
        {
            var task = QueueWorkItem(workItem, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread and waits for the work item to be executed.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>The result of executing the work item.</returns>
        public T QueueWorkItemAndWait<T>(Func<Object, T> workItem, Object state, Boolean forceAsync = false)
        {
            var task = QueueWorkItem(workItem, state, forceAsync);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Action workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, "workItem");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            if (IsExecutingOnCurrentThread && !forceAsync)
            {
                workItem();
                return TaskUtil.FromResult<Boolean>(true);
            }
            else
            {
                var task = new Task(workItem);
                queuedWorkItems.Enqueue(task);
                return task;
            }
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task QueueWorkItem(Action<Object> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, "workItem");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            if (IsExecutingOnCurrentThread && !forceAsync)
            {
                workItem(state);
                return TaskUtil.FromResult<Boolean>(true);
            }
            else
            {
                var task = new Task(workItem, state);
                queuedWorkItems.Enqueue(task);
                return task;
            }
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<T> workItem, Boolean forceAsync = false)
        {
            Contract.Require(workItem, "workItem");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            if (IsExecutingOnCurrentThread && !forceAsync)
            {
                var result = workItem();
                return TaskUtil.FromResult(result);
            }
            else
            {
                var task = new Task<T>(workItem);
                queuedWorkItems.Enqueue(task);
                return task;
            }
        }

        /// <summary>
        /// Queues a work item for execution on Ultraviolet's main thread.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the work item.</typeparam>
        /// <param name="workItem">The work item to execute on Ultraviolet's main thread.</param>
        /// <param name="state">An object containing state to pass to the work item.</param>
        /// <param name="forceAsync">A value indicating whether to force the work item to be queued and executed asynchronously.
        /// If this value is <c>false</c>, then calls to this method from the main Ultraviolet thread will execute synchronously.</param>
        /// <returns>A <see cref="Task"/> that encapsulates the work item.</returns>
        public Task<T> QueueWorkItem<T>(Func<Object, T> workItem, Object state, Boolean forceAsync = false)
        {
            Contract.Require(workItem, "workItem");
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(disposing, UltravioletStrings.CannotQueueWorkItems);

            if (IsExecutingOnCurrentThread && !forceAsync)
            {
                var result = workItem(state);
                return TaskUtil.FromResult(result);
            }
            else
            {
                var task = new Task<T>(workItem, state);
                queuedWorkItems.Enqueue(task);
                return task;
            }
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

#if ANDROID
                return UltravioletPlatform.Android;
#else
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        return UltravioletPlatform.Windows;

                    case PlatformID.Unix:
                        return UltravioletPlatform.Linux;

                    default:
                        throw new NotSupportedException();
                }
#endif
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
        /// Retrieves the current Ultraviolet context, throwing an exception if it does not exist.
        /// </summary>
        /// <returns>The current Ultraviolet context, or <c>null</c> if no contex exists.</returns>
        internal static UltravioletContext RequestCurrent()
        {
            return current;
        }

        /// <summary>
        /// Retrieves the current Ultraviolet context, throwing an exception if it does not exist.
        /// </summary>
        /// <returns>The current Ultraviolet context.</returns>
        internal static UltravioletContext DemandCurrent()
        {
            if (current == null)
                throw new InvalidOperationException(UltravioletStrings.ContextMissing);

            return current;
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
            Contract.Require(asm, "asm");

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
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
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

            this.disposed = true;
            this.disposing = false;

            ReleaseContext();
        }

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

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
        protected virtual void OnInitialized()
        {
            var temp = Initialized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Shutdown"/> event.
        /// </summary>
        protected virtual void OnShutdown()
        {
            var temp = Shutdown;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Processes the context's message queue.
        /// </summary>
        protected virtual void ProcessMessages()
        {
            messages.Process();
        }

        /// <summary>
        /// Gets the context's object factory.
        /// </summary>
        protected UltravioletFactory Factory
        {
            get { return factory; }
        }

        /// <summary>
        /// Raises the <see cref="ContextInitialized"/> event.
        /// </summary>
        private static void OnContextInitialized()
        {
            var temp = ContextInitialized;
            if (temp != null)
            {
                temp(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ContextInvalidated"/> event.
        /// </summary>
        private static void OnContextInvalidated()
        {
            var temp = ContextInvalidated;
            if (temp != null)
            {
                temp(null, EventArgs.Empty);
            }
        }

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
        }

        /// <summary>
        /// Initializes any factory methods exposed by the current platform compatibility shim.
        /// </summary>
        private void InitializeFactoryMethodsInCompatibilityShim()
        {
            try
            {
                Assembly shim = null;

                switch (Platform)
                {
                    case UltravioletPlatform.Windows:
                    case UltravioletPlatform.Linux:
                        shim = Assembly.LoadFrom("TwistedLogik.Ultraviolet.Desktop.dll");
                        break;

                    case UltravioletPlatform.Android:
                        shim = Assembly.Load("TwistedLogik.Ultraviolet.Android.dll");
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
            lock (tasksPending)
            {
                if (tasksPending.Count == 0)
                    return;

                foreach (var task in tasksPending)
                {
                    if (task.Status == TaskStatus.Created)
                    {
                        task.Start();
                    }
                    if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
                    {
                        tasksDead.Add(task);
                    }
                }
                foreach (var task in tasksDead)
                {
                    tasksPending.Remove(task);
                }
                tasksDead.Clear();
            }
        }

        // The singleton instance of the Ultraviolet context.
        private static readonly Object syncObject = new Object();
        private static UltravioletContext current;

        // State values.
        private readonly IUltravioletHost host;
        private readonly UltravioletFactory factory = new UltravioletFactory();
        private readonly ConcurrentQueue<Task> queuedWorkItems = new ConcurrentQueue<Task>();
        private readonly Thread thread;
        private Boolean isInitialized;
        private Boolean disposed;
        private Boolean disposing;

        // The context's list of pending tasks.
        private readonly List<Task> tasksPending = new List<Task>();
        private readonly List<Task> tasksDead = new List<Task>();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // The context event queue.
        private readonly LocalMessageQueue<UltravioletMessageID> messages;
    }
}
