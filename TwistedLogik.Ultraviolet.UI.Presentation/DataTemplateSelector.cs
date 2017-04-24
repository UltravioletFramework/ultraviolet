using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Provides a way to choose a <see cref="DataTemplate"/> based on the data object and the data-bound element.
    /// </summary>
    public class DataTemplateSelector
    {
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="DataTemplate"/> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a <see cref="DataTemplate"/> or <see langword="null"/>. The default value is <see langword="null"/>.</returns>
        public virtual DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            return null;
        }
    }
}
