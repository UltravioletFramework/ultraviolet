using System;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents the base class for UVML mutators which modify event handlers.
    /// </summary>
    internal abstract class UvmlEventMutator : UvmlMutator
    {
        /// <summary>
        /// Creates a handler delegate for an event.
        /// </summary>
        /// <param name="name">The name of the method which will handle the event.</param>
        /// <param name="type">The type of delegate which handles the event.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The delegate which was created to handle the event.</returns>
        protected static Delegate CreateEventHandlerDelegate(String name, Type type, UvmlInstantiationContext context)
        {
            var dataSource = context.DataSource;
            var dataSourceType = context.DataSourceType;

            var templatedParent = context.TemplatedParent as UIElement;
            if (templatedParent != null)
            {
                dataSource = templatedParent;
                PresentationFoundation.Instance.ComponentTemplates.Get(templatedParent, out dataSourceType);
            }

            return BindingExpressions.CreateBoundEventDelegate(dataSource, dataSourceType, type, name);
        }
    }
}
