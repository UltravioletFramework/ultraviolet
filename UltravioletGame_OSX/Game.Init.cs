﻿using AppKit;

namespace SAFE_PROJECT_NAME
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            NSApplication.Init();
        }
    }
}

