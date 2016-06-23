using System;
using System.IO;
using Android.Media;
using Android.Webkit;
using TwistedLogik.Ultraviolet.Graphics;
using AndroidEnvironment = Android.OS.Environment;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers
{
	partial class Game
	{
		partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
		{
			var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";
			var path = filename;

			var dir = AndroidEnvironment.GetExternalStoragePublicDirectory(
				AndroidEnvironment.DirectoryPictures).AbsolutePath;
			path = Path.Combine(dir, filename);

			using (var stream = File.OpenWrite(path))
				surfaceSaver.SaveAsPng(rtarget, stream);
			
			MediaScannerConnection.ScanFile(ApplicationContext, new String[] { path },
				new String[] { MimeTypeMap.Singleton.GetMimeTypeFromExtension("png") }, null);

			confirmMsgText = $"Image saved to photo gallery";
			confirmMsgOpacity = 1;
		}
	}
}

