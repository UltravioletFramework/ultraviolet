using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a wrapper around a view resource.
    /// </summary>
    public interface IResourceWrapper
    {
        /// <summary>
        /// Gets the underlying resource instance.
        /// </summary>
        Object Resource { get; }
    }
}
