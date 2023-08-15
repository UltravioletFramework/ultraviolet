using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests
{
    /// <summary>
    /// Contains extension methods for the <see cref="IUltravioletTestApplicationAdapter"/> interface.
    /// </summary>
    public static class IUltravioletTestApplicationAdapterExtensions
    {
        /// <summary>
        /// Specifies that the application should configure the Presentation Foundation.
        /// </summary>
        /// <returns>The Ultraviolet test application.</returns>
        public static IUltravioletTestApplicationAdapter WithPresentationFoundationConfigured(this IUltravioletTestApplicationAdapter @this)
        {
            return @this.WithPlugin(new PresentationFoundationPlugin());
        }
    }
}
