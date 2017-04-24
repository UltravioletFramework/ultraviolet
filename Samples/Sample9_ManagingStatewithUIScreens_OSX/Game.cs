using AppKit;

namespace UltravioletSample.Sample9_ManagingStateWithUIScreens
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

