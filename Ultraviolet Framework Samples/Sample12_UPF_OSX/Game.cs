using AppKit;

namespace UltravioletSample.Sample12_UPF
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

