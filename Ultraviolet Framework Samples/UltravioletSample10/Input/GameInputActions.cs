using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Input;

namespace UltravioletSample.Input
{
    public class GameInputActions : InputActionCollection
    {
        public GameInputActions(UltravioletContext uv) : base(uv) { }

        public InputAction ExitApplication
        {
            get;
            private set;
        }

        protected override void OnCreatingActions()
        {
            ExitApplication = CreateAction("EXIT_APPLICATION");

            base.OnCreatingActions();
        }

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
