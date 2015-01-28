using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Input;

namespace UvDebugSandbox.Input
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
        /// Gets or sets the input binding which moves navigation up.
        /// </summary>
        public InputAction NavigateUp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the input binding which moves navigation down.
        /// </summary>
        public InputAction NavigateDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the input binding which moves navigation left.
        /// </summary>
        public InputAction NavigateLeft
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the input binding which moves navigation right.
        /// </summary>
        public InputAction NavigateRight
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

            NavigateUp    = CreateAction("NAVIGATE_UP");
            NavigateDown  = CreateAction("NAVIGATE_DOWN");
            NavigateLeft  = CreateAction("NAVIGATE_LEFT");
            NavigateRight = CreateAction("NAVIGATE_RIGHT");

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

            NavigateUp.Primary    = CreateKeyboardBinding(Key.Up);
            NavigateDown.Primary  = CreateKeyboardBinding(Key.Down);
            NavigateLeft.Primary  = CreateKeyboardBinding(Key.Left);
            NavigateRight.Primary = CreateKeyboardBinding(Key.Right);
#endif

            base.OnResetting();
        }
    }
}
