using System;
using System.Text;

namespace StbImageWriteSharp
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	static unsafe partial class StbImageWrite
	{
		public static void stbiw__writefv(stbi__write_context s, string fmt, params object[] v)
		{
			var vindex = 0;
			for (var i = 0; i < fmt.Length; ++i)
			{
				var c = fmt[i];
				switch (c)
				{
					case ' ':
						break;
					case '1':
					{
						var x = (byte) ((int) v[vindex++] & 0xff);
						s.func(s.context, &x, 1);
						break;
					}
					case '2':
					{
						var x = (int) v[vindex++];
						var b = stackalloc byte[2];
						b[0] = (byte) (x & 0xff);
						b[1] = (byte) ((x >> 8) & 0xff);
						s.func(s.context, b, 2);
						break;
					}
					case '4':
					{
						var x = Convert.ToUInt32(v[vindex++]);
						var b = stackalloc byte[4];
						b[0] = (byte) (x & 0xff);
						b[1] = (byte) ((x >> 8) & 0xff);
						b[2] = (byte) ((x >> 16) & 0xff);
						b[3] = (byte) ((x >> 24) & 0xff);
						s.func(s.context, b, 4);
						break;
					}
				}
			}
		}

		public static void stbiw__writef(stbi__write_context s, string fmt, params object[] v)
		{
			stbiw__writefv(s, fmt, v);
		}

		public static int stbiw__outfile(stbi__write_context s, int rgb_dir, int vdir, int x, int y, int comp,
			int expand_mono, void* data, int alpha, int pad, string fmt, params object[] v)
		{
			if ((y < 0) || (x < 0))
			{
				return 0;
			}

			stbiw__writefv(s, fmt, v);
			stbiw__write_pixels(s, rgb_dir, vdir, x, y, comp, data, alpha, pad, expand_mono);
			return 1;
		}

		public static int stbi_write_hdr_core(stbi__write_context s, int x, int y, int comp, float* data)
		{
			if ((y <= 0) || (x <= 0) || (data == null))
			{
				return 0;
			}

			var scratch = (byte*) (CRuntime.malloc((ulong) (x*4)));

			int i;
			var header = "#?RADIANCE\n# Written by stb_image_write.h\nFORMAT=32-bit_rle_rgbe\n";
			var bytes = Encoding.UTF8.GetBytes(header);
			fixed (byte* ptr = bytes)
			{
				s.func(s.context, ((sbyte*) ptr), bytes.Length);
			}

			var str = string.Format("EXPOSURE=          1.0000000000000\n\n-Y {0} +X {1}\n", y, x);
			bytes = Encoding.UTF8.GetBytes(str);
			fixed (byte* ptr = bytes)
			{
				s.func(s.context, ((sbyte*) ptr), bytes.Length);
			}
			for (i = 0; i < y; i++)
			{
				stbiw__write_hdr_scanline(s, x, comp, scratch, data + comp*i*x);
			}
			CRuntime.free(scratch);
			return 1;
		}
	}
}