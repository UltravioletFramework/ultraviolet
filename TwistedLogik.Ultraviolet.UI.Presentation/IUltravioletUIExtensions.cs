using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            Contract.Require(ui, "ui");

            return instance;
        }

        // The singleton instance of the Ultraviolet Presentation Foundation.
        private static readonly UltravioletSingleton<PresentationFoundation> instance = 
            new UltravioletSingleton<PresentationFoundation>((uv) =>
            {
                var instance = new PresentationFoundation(uv);
                uv.GetUI().Updating += (s, t) =>
                {
                    instance.Update(t);
                };
                return instance;
            });
    }
}
