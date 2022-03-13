using System.IO;

namespace StbImageSharp
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	struct ImageInfo
	{
		public int Width;
		public int Height;
		public ColorComponents ColorComponents;
		public int BitsPerChannel;


		public static unsafe ImageInfo? FromStream(Stream stream)
		{
			int width, height, comp;
			var context = new StbImage.stbi__context(stream);

			var is16Bit = StbImage.stbi__is_16_main(context) == 1;
			StbImage.stbi__rewind(context);

			var infoResult = StbImage.stbi__info_main(context, &width, &height, &comp);
			StbImage.stbi__rewind(context);

			if (infoResult == 0) return null;

			return new ImageInfo
			{
				Width = width,
				Height = height,
				ColorComponents = (ColorComponents)comp,
				BitsPerChannel = is16Bit ? 16 : 8
			};
		}
	}
}