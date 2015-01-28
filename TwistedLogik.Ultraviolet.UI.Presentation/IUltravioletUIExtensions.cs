using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="IUltravioletUI"/> interface.
    /// </summary>
    public static class IUltravioletUIExtensions
    {
        /// <summary>
        /// Gets the core management object for the Ultraviolet Presentation Framework.
        /// </summary>
        /// <param name="ui">The Ultraviolet context's UI subsystem.</param>
        /// <returns>The core management object for the Ultraviolet Presentation Framework.</returns>
        public static PresentationFrameworkManager GetPresentationFramework(this IUltravioletUI ui)
        {
            Contract.Require(ui, "ui");

            return instance;
        }

        // The singleton instance of the framework manager.
        private static readonly UltravioletSingleton<PresentationFrameworkManager> instance = 
            new UltravioletSingleton<PresentationFrameworkManager>((uv) =>
            {
                var instance = new PresentationFrameworkManager(uv);
                uv.GetUI().Updating += (s, t) =>
                {
                    instance.Update(t);
                };
                return instance;
            });
    }
}
