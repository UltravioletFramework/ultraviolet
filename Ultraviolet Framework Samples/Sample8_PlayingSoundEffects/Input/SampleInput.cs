using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Input;

namespace UltravioletSample.Sample8_PlayingSoundEffects.Input
{
    public static class SampleInput
    {
        public static Actions GetActions(this IUltravioletInput input) =>
            Actions.Instance;

        public class Actions : InputActionCollection
        {
            public Actions(UltravioletContext uv)
                : base(uv)
            { }

            public static Actions Instance { get; } = CreateSingleton<Actions>();

            public InputAction ExitApplication { get; private set; }

            /// <inheritdoc/>
            protected override void OnCreatingActions()
            {
                this.ExitApplication =
                    CreateAction("EXIT_APPLICATION");

                base.OnCreatingActions();
            }

            /// <inheritdoc/>
            protected override void OnResetting()
            {
                this.ExitApplication
                    .Primary = CreateKeyboardBinding(Key.F4, alt: true);

                base.OnResetting();
            }
        }
    }
}
