using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="IUltravioletUI"/> interface.
    /// </summary>
    public static class IUltravioletUIExtensions
    {
        /// <summary>
        /// Gets the core management object for the Ultraviolet Presentation Foundation.
        /// </summary>
        /// <param name="ui">The Ultraviolet context's UI subsystem.</param>
        /// <returns>The core management object for the Ultraviolet Presentation Foundation.</returns>
        public static PresentationFoundation GetPresentationFoundation(this IUltravioletUI ui)
        {
            Contract.Require(ui, nameof(ui));

            return PresentationFoundation.Instance;
        }
    }
}
