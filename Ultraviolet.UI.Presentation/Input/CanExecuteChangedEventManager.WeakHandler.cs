using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    partial class CanExecuteChangedEventManager
    {
        /// <summary>
        /// Represents an object which maintains weak references to an event source and handler.
        /// </summary>
        private class WeakHandler
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WeakHandler"/> class.
            /// </summary>
            /// <param name="manager">The event manager which owns the handler.</param>
            public WeakHandler(CanExecuteChangedEventManager manager)
            {
                Contract.Require(manager, nameof(manager));

                this.manager = manager;
                this.onCanExecuteChanged = OnCanExecuteChanged;
            }

            /// <summary>
            /// Attaches this instance to the specified source and handler.
            /// </summary>
            /// <param name="source">The source command.</param>
            /// <param name="sourceHandler">The source event handler.</param>
            public void Attach(ICommand source, EventHandler sourceHandler)
            {
                if (this.source != null || this.sourceHandler != null)
                    throw new InvalidOperationException();

                this.source = new WeakReference(source);
                this.sourceHandler = new WeakReference(sourceHandler);

                source.CanExecuteChanged += onCanExecuteChanged;
            }

            /// <summary>
            /// Detaches this instance from its current source and handler.
            /// </summary>
            public void Detach()
            {
                var command = (ICommand)source?.Target;
                if (command != null)
                    command.CanExecuteChanged -= onCanExecuteChanged;

                this.source = null;
                this.sourceHandler = null;
            }

            /// <summary>
            /// Gets the source event handler which is being tracked.
            /// </summary>
            public EventHandler SourceHandler => (EventHandler)sourceHandler?.Target;

            /// <summary>
            /// Gets a value indicating whether this weak handler corresponds to the specified source and handler.
            /// </summary>
            /// <param name="source">The source command.</param>
            /// <param name="sourceHandler">The source event handler.</param>
            /// <returns><see langword="true"/> if the weak handler is a match for the specified source;
            /// otherwise, <see langword="false"/>.</returns>
            public Boolean IsFor(ICommand source, EventHandler sourceHandler)
            {
                return this.source?.Target == source && (EventHandler)this.sourceHandler?.Target == sourceHandler;
            }

            /// <summary>
            /// Gets a value indicating whether the weak references being maintained
            /// by this object are still alive.
            /// </summary>
            public Boolean IsAlive => (source?.IsAlive ?? false) && (sourceHandler?.IsAlive ?? false);

            /// <summary>
            /// Handles the <see cref="ICommand.CanExecuteChanged"/> event.
            /// </summary>
            private void OnCanExecuteChanged(Object sender, EventArgs e)
            {
                if (source == null || sourceHandler == null)
                    return;

                var strongSource = source.Target as ICommand;
                var strongHandler = sourceHandler.Target as EventHandler;
                if (strongHandler != null)
                {
                    var target = (sender is CommandManager) ? strongSource : sender;
                    strongHandler(target, e);
                }
                else
                {
                    manager.ScheduleCleanup();
                }
            }

            // State values.
            private readonly CanExecuteChangedEventManager manager;
            private readonly EventHandler onCanExecuteChanged;
            private WeakReference source;
            private WeakReference sourceHandler;
        }
    }
}
