using AppKit;

namespace UvDebugSandbox
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

