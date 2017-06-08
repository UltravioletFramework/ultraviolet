using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers.Input
{
    public static class SampleInput
    {
        public static Actions GetActions(this IUltravioletInput input) =>
            Actions.Instance;

        public class Actions : InputActionCollection
        {
			[Preserve]
			public Actions(UltravioletContext uv) 
                : base(uv)
            { }

            public static Actions Instance { get; } = CreateSingleton<Actions>();

            public InputAction ExitApplication { get; private set; }
            public InputAction SaveImage { get; private set; }

            /// <inheritdoc/>
            protected override void OnCreatingActions()
            {
                this.ExitApplication = 
                    CreateAction("EXIT_APPLICATION");

                this.SaveImage = 
                    CreateAction("SAVE_IMAGE");

                base.OnCreatingActions();
            }

            /// <inheritdoc/>
            protected override void OnResetting()
            {
                switch (Ultraviolet.Platform)
                {
                    case UltravioletPlatform.Android:
                        Reset_Android();
                        break;

                    default:
                        Reset_Desktop();
                        break;
                }
                base.OnResetting();
            }

            private void Reset_Desktop()
            {
                this.ExitApplication
                    .Primary = CreateKeyboardBinding(Key.Escape);

                this.SaveImage
                    .Primary = CreateKeyboardBinding(Key.F1);
            }

            private void Reset_Android()
            {
                this.ExitApplication
                    .Primary = CreateKeyboardBinding(Key.AppControlBack);
            }
        }
    }
}
