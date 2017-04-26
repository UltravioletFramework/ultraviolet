using System.Reflection;
using AppKit;

namespace UltravioletSample.Sample14_LoadingImageDataWithSurfaces
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            // HACK: Addresses a race condition in the current version of Xamarin
            try
            {
                Assembly.Load("System.Configuration")
                        ?.GetType("System.Configuration.ConfigurationManager")
						?.GetMethod("GetSection", BindingFlags.Static | BindingFlags.Public)
						?.Invoke(null, new[] { "configuration" });
            }
            catch { }
            NSApplication.Init();
        }
    }
}

