using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UltravioletApplicationAdapter :
        IMessageSubscriber<UltravioletMessageID>,
        IUltravioletComponent,
        IDisposable
    {

        // Property values.
        private readonly IUltravioletApplicationAdapterHost host;

        private bool disposed;

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get
            {
                return host.Ultraviolet;
            }
        }

        /// <summary>
        /// Gets the application instance hosting this adapter
        /// </summary>
        public IUltravioletApplicationAdapterHost Host
        {
            get
            {
                return host;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">The host</param>
        protected UltravioletApplicationAdapter(IUltravioletApplicationAdapterHost host)
        {
            Contract.Require(host, nameof(host));

            this.host = host;
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
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

        internal void Configure(UltravioletConfiguration configuration)
        {
            OnConfiguring(configuration);
        }

        internal void Initializing()
        {
            OnInitializing();
        }

        internal void Initialized()
        {
            this.Ultraviolet.Messages.Subscribe(this);
            OnInitialized();
        }
        internal void LoadingContent()
        {
            OnLoadingContent();
        }

        internal void Updating(UltravioletTime time)
        {
            OnUpdating(time);
        }

        internal void Drawing(UltravioletTime time)
        {
            OnDrawing(time);
        }

        internal void WindowDrawing(UltravioletTime time, IUltravioletWindow window)
        {
            OnWindowDrawing(time, window);
        }

        internal void WindowDrawn(UltravioletTime time, IUltravioletWindow window)
        {
            OnWindowDrawn(time, window);
        }

        internal void Suspending()
        {
            OnSuspending();
        }

        internal void Suspended()
        {
            OnSuspended();
        }

        internal void Resuming()
        {
            OnResuming();
        }

        internal void Resumed()
        {
            OnResumed();
        }

        internal void ReclaimingMemory()
        {
            OnReclaimingMemory();
        }

        internal void Shutdown()
        {
            this.Ultraviolet.Messages.Unsubscribe(this);
            OnShutdown();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Called when configuring the Ultraviolet context.
        /// <param name="configuration">The supplied configuration</param>
        /// </summary>
        protected virtual void OnConfiguring(UltravioletConfiguration configuration)
        {

        }

        /// <summary>
        /// Called when the application is initializing.
        /// </summary>
        protected virtual void OnInitializing()
        {
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected virtual void OnLoadingContent()
        {
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
        }

        /// <summary>
        /// Called when the scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        protected virtual void OnDrawing(UltravioletTime time)
        {
        }

        /// <summary>
        /// Called when one of the application's windows is about to be drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that is about to be drawn.</param>
        protected virtual void OnWindowDrawing(UltravioletTime time, IUltravioletWindow window)
        {
        }

        /// <summary>
        /// Called after one of the application's windows has been drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that was just drawn.</param>
        protected virtual void OnWindowDrawn(UltravioletTime time, IUltravioletWindow window)
        {
        }

        /// <summary>
        /// Called when the application is about to be suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected virtual void OnSuspending()
        {
        }

        /// <summary>
        /// Called when the application has been suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected virtual void OnSuspended()
        {
        }

        /// <summary>
        /// Called when the application is about to be resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected virtual void OnResuming()
        {
        }

        /// <summary>
        /// Called when the application has been resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected virtual void OnResumed()
        {
        }

        /// <summary>
        /// Called when the operating system is attempting to reclaim memory.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected virtual void OnReclaimingMemory()
        {
        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected virtual void OnShutdown()
        {
        }

        /// <summary>
        /// Occurs when the context receives a message from its queue.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        protected virtual void OnReceivedMessage(UltravioletMessageID type, MessageData data)
        { 
        }
    }
}
