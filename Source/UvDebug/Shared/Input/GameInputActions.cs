using Ultraviolet;
using Ultraviolet.Input;

namespace UvDebug.Input
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
        /// Gets the input binding which moves navigation up.
        /// </summary>
        public InputAction NavigateUp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input binding which moves navigation down.
        /// </summary>
        public InputAction NavigateDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input binding which moves navigation left.
        /// </summary>
        public InputAction NavigateLeft
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input binding which moves navigation right.
        /// </summary>
        public InputAction NavigateRight
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input binding which moves navigation to the previous tab stop.
        /// </summary>
        public InputAction NavigatePreviousTabStop
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input binding which moves navigation to the next tab stop.
        /// </summary>
        public InputAction NavigateNextTabStop
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

            NavigateUp              = CreateAction("NAVIGATE_UP");
            NavigateDown            = CreateAction("NAVIGATE_DOWN");
            NavigateLeft            = CreateAction("NAVIGATE_LEFT");
            NavigateRight           = CreateAction("NAVIGATE_RIGHT");
            NavigatePreviousTabStop = CreateAction("NAVIGATE_PREV_TAB");
            NavigateNextTabStop     = CreateAction("NAVIGATE_NEXT_TAB");

            base.OnCreatingActions();
        }

        /// <summary>
        /// Called when the collection is being reset to its default values.
        /// </summary>
        protected override void OnResetting()
        {
            ExitApplication.Primary         = CreateKeyboardBinding(Key.F4, alt: true);
            NavigateUp.Primary              = CreateKeyboardBinding(Key.Up);
            NavigateDown.Primary            = CreateKeyboardBinding(Key.Down);
            NavigateLeft.Primary            = CreateKeyboardBinding(Key.Left);
            NavigateRight.Primary           = CreateKeyboardBinding(Key.Right);
            NavigatePreviousTabStop.Primary = CreateKeyboardBinding(Key.Tab, shift: true);
            NavigateNextTabStop.Primary     = CreateKeyboardBinding(Key.Tab);

            base.OnResetting();
        }
    }
}
