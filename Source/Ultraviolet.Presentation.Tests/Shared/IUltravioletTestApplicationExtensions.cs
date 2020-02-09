using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests
{
    /// <summary>
    /// Contains extension methods for the <see cref="IUltravioletTestApplication"/> interface.
    /// </summary>
    public static class IUltravioletTestApplicationExtensions
    {
        /// <summary>
        /// Specifies that the application should configure the Presentation Foundation.
        /// </summary>
        /// <returns>The Ultraviolet test application.</returns>
        public static IUltravioletTestApplication WithPresentationFoundationConfigured(this IUltravioletTestApplication @this)
        {
            return @this.WithPlugin(new PresentationFoundationPlugin());
        }
    }
}
