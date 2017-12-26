using System;
using System.IO;
using System.Reflection;
using AppKit;
using Ultraviolet.Graphics;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers
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

        partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
        {
            var saver = SurfaceSaver.Create();
            var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);

            using (var stream = File.OpenWrite(path))
                saver.SaveAsPng(rtarget, stream);

            confirmMsgText = $"Image saved to {path}";
            confirmMsgOpacity = 1;
        }
    }
}

