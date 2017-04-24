using System;
using System.IO;
using Ultraviolet.Graphics;

namespace UltravioletSample.Sample15_RenderTargetsAndBuffers
{
	partial class Game
	{
		partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target)
		{
			var saver = SurfaceSaver.Create();
			var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";
			var path = filename;

			using (var stream = File.OpenWrite(path))
				saver.SaveAsPng(rtarget, stream);
			
			confirmMsgText = $"Image saved to {filename}";
			confirmMsgOpacity = 1;
		}
	}
}

