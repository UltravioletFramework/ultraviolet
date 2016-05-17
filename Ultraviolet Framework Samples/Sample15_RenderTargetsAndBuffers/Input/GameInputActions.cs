using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Input;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers.Input
{
    public class GameInputActions : InputActionCollection
    {
        public GameInputActions(UltravioletContext uv) : base(uv) { }

        public InputAction ExitApplication
        {
            get;
            private set;
        }

        public InputAction SaveImage
        {
            get;
            private set;
        }

        protected override void OnCreatingActions()
        {
            ExitApplication = CreateAction("EXIT_APPLICATION");
            SaveImage = CreateAction("SAVE_IMAGE");

            base.OnCreatingActions();
        }

        protected override void OnResetting()
        {
#if ANDROID
            ExitApplication.Primary = CreateKeyboardBinding(Key.AppControlBack);
#else
            ExitApplication.Primary = CreateKeyboardBinding(Key.Escape);
            SaveImage.Primary = CreateKeyboardBinding(Key.F1);
#endif

            base.OnResetting();
        }
    }
}
