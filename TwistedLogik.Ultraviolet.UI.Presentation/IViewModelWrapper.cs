using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an object which wraps a view model.
    /// </summary>
    public interface IViewModelWrapper
    {
        /// <summary>
        /// Gets the wrapped view model.
        /// </summary>
        Object ViewModel
        {
            get;
        }
    }
}
