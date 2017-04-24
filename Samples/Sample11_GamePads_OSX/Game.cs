using AppKit;

namespace UltravioletSample.Sample11_GamePads
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

