using System.IO;
using Ultraviolet.BASS;
using Ultraviolet.Graphics;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }

        partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
        {
            using (var stream = new MemoryStream())
            {
                surfaceSaver.SaveAsPng(rtarget, stream);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                using (var imgData = Foundation.NSData.FromStream(stream))
                using (var img = UIKit.UIImage.LoadFromData(imgData))
                {
                    img.SaveToPhotosAlbum(null);
                }

                confirmMsgText = $"Image saved to photo gallery";
                confirmMsgOpacity = 1;
            }
        }
    }
}

