using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace SAFE_PROJECT_NAME.Input
{
    /// <summary>
    /// Contains the game's input actions.
    /// </summary>
    public sealed class GameInputActions : InputActionCollection
    {
        /// <summary>
        /// Initializes a new instance of the GameInputActions class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        [Preserve]
        public GameInputActions(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets or sets the input binding which exits the application.
        /// </summary>
        public InputAction ExitApplication
        {
            get;
            private set;
        }

        /// <summary>
        /// Called when the collection is creating its actions.
        /// </summary>
        protected override void OnCreatingActions()
        {
            ExitApplication = CreateAction("EXIT_APPLICATION");

            base.OnCreatingActions();
        }

        /// <summary>
        /// Called when the collection is being reset to its default values.
        /// </summary>
        protected override void OnResetting()
        {
#if ANDROID
            ExitApplication.Primary = CreateKeyboardBinding(Key.AppControlBack);
#else
            ExitApplication.Primary = CreateKeyboardBinding(Key.Escape);
#endif

            base.OnResetting();
        }
    }
}
