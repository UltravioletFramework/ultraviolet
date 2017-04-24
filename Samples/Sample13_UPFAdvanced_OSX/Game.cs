using AppKit;

namespace UltravioletSample.Sample13_UPFAdvanced
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

