using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an item container used to wrap a particular item within an <see cref="ItemsControl"/>.
    /// </summary>
    public interface IItemContainer
    {
        /// <summary>
        /// Prepares the container to host the specified item.
        /// </summary>
        /// <param name="item">The item which will be hosted by the container.</param>
        void PrepareItemContainer(Object item);
    }
}
