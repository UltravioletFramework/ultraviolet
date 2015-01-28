using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the method that is called when the value of a property 
    /// changes on an instance of the <see cref="DefinitionBase"/> class.
    /// </summary>
    /// <param name="definition">The definition that raised the event.</param>
    public delegate void DefinitionEventHandler(DefinitionBase definition);

    /// <summary>
    /// Represents the base class for <see cref="RowDefinition"/> and <see cref="ColumnDefinition"/>.
    /// </summary>
    public abstract class DefinitionBase : DependencyObject
    {
        /// <inheritdoc/>
        protected internal sealed override DependencyObject DependencyContainer
        {
            get { return Grid; }
        }

        /// <inheritdoc/>
        protected internal sealed override Object DependencyDataSource
        {
            get { return Grid == null ? null : Grid.DependencyDataSource; }
        }

        /// <summary>
        /// Gets the <see cref="Grid"/> that owns the definition.
        /// </summary>
        internal Grid Grid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this row or column requires a scissor rectangle.
        /// </summary>
        internal Boolean RequiresScissorRectangle
        {
            get;
            set;
        }
    }
}
