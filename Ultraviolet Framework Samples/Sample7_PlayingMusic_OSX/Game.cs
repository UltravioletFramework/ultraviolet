using AppKit;

namespace UltravioletSample.Sample7_PlayingMusic
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

