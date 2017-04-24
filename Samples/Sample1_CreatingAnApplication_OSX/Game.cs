using AppKit;

namespace UltravioletSample.Sample1_CreatingAnApplication
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

