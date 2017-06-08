using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace UltravioletSample.Sample13_UPFAdvanced.Input
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
            public InputAction NavigateUp { get; private set; }
            public InputAction NavigateDown { get; private set; }
            public InputAction NavigateLeft { get; private set; }
            public InputAction NavigateRight { get; private set; }
            public InputAction NavigatePreviousTabStop { get; private set; }
            public InputAction NavigateNextTabStop { get; private set; }

            /// <inheritdoc/>
            protected override void OnCreatingActions()
            {
                this.ExitApplication =
                    CreateAction("EXIT_APPLICATION");

                this.NavigateUp =
                    CreateAction("NAVIGATE_UP");

                this.NavigateDown =
                    CreateAction("NAVIGATE_DOWN");

                this.NavigateLeft =
                    CreateAction("NAVIGATE_LEFT");

                this.NavigateRight =
                    CreateAction("NAVIGATE_RIGHT");

                this.NavigatePreviousTabStop =
                    CreateAction("NAVIGATE_PREVIOUS_TAB_STOP");

                this.NavigateNextTabStop =
                    CreateAction("NAVIGATE_NEXT_TAB_STOP");

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
                    .Primary = CreateKeyboardBinding(Key.F4, alt: true);

                this.NavigateUp
                    .Primary = CreateKeyboardBinding(Key.Up);

                this.NavigateDown
                    .Primary = CreateKeyboardBinding(Key.Down);

                this.NavigateLeft
                    .Primary = CreateKeyboardBinding(Key.Left);

                this.NavigateRight
                    .Primary = CreateKeyboardBinding(Key.Right);

                this.NavigatePreviousTabStop
                    .Primary = CreateKeyboardBinding(Key.Tab, shift: true);

                this.NavigateNextTabStop
                    .Primary = CreateKeyboardBinding(Key.Tab);
            }

            private void Reset_Android()
            {
                this.ExitApplication
                    .Primary = CreateKeyboardBinding(Key.AppControlBack);

                this.NavigateUp
                    .Primary = CreateKeyboardBinding(Key.Up);

                this.NavigateDown
                    .Primary = CreateKeyboardBinding(Key.Down);

                this.NavigateLeft
                    .Primary = CreateKeyboardBinding(Key.Left);

                this.NavigateRight
                    .Primary = CreateKeyboardBinding(Key.Right);

                this.NavigatePreviousTabStop
                    .Primary = CreateKeyboardBinding(Key.Tab, shift: true);

                this.NavigateNextTabStop
                    .Primary = CreateKeyboardBinding(Key.Tab);
            }
        }
    }
}
