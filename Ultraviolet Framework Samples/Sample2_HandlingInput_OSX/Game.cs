using AppKit;

namespace UltravioletSample.Sample2_HandlingInput
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

