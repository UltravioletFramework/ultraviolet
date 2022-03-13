using System;
using System.IO;
using System.Runtime.InteropServices;

namespace StbImageSharp
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	static unsafe partial class StbImage
	{
		public static string stbi__g_failure_reason;
		public static readonly char[] stbi__parse_png_file_invalid_chunk = new char[25];

		public class stbi__context
		{
			public byte[] _tempBuffer;
			public int img_n = 0;
			public int img_out_n = 0;
			public uint img_x = 0;
			public uint img_y = 0;

			public stbi__context(Stream stream)
			{
				if (stream == null)
					throw new ArgumentNullException("stream");

				Stream = stream;
			}

			public Stream Stream { get; }
		}

		private static int stbi__err(string str)
		{
			stbi__g_failure_reason = str;
			return 0;
		}

		public static byte stbi__get8(stbi__context s)
		{
			var b = s.Stream.ReadByte();
			if (b == -1) return 0;

			return (byte)b;
		}

		public static void stbi__skip(stbi__context s, int skip)
		{
			s.Stream.Seek(skip, SeekOrigin.Current);
		}

		public static void stbi__rewind(stbi__context s)
		{
			s.Stream.Seek(0, SeekOrigin.Begin);
		}

		public static int stbi__at_eof(stbi__context s)
		{
			return s.Stream.Position == s.Stream.Length ? 1 : 0;
		}

		public static int stbi__getn(stbi__context s, byte* buf, int size)
		{
			if (s._tempBuffer == null ||
				s._tempBuffer.Length < size)
				s._tempBuffer = new byte[size * 2];

			var result = s.Stream.Read(s._tempBuffer, 0, size);
			Marshal.Copy(s._tempBuffer, 0, new IntPtr(buf), result);

			return result;
		}
	}
}