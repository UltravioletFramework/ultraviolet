using System;
using System.Runtime.InteropServices;

namespace Hebron.Runtime
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	static unsafe class CRuntime
	{
		private static readonly string numbers = "0123456789";

		public static void* malloc(ulong size)
		{
			return malloc((long)size);
		}

		public static void* malloc(long size)
		{
			var ptr = Marshal.AllocHGlobal((int)size);

			MemoryStats.Allocated();

			return ptr.ToPointer();
		}

		public static void free(void* a)
		{
			if (a == null)
				return;

			var ptr = new IntPtr(a);
			Marshal.FreeHGlobal(ptr);
			MemoryStats.Freed();
		}

		public static void memcpy(void* a, void* b, long size)
		{
			var ap = (byte*)a;
			var bp = (byte*)b;
			for (long i = 0; i < size; ++i)
				*ap++ = *bp++;
		}

		public static void memcpy(void* a, void* b, ulong size)
		{
			memcpy(a, b, (long)size);
		}

		public static void memmove(void* a, void* b, long size)
		{
			void* temp = null;

			try
			{
				temp = malloc(size);
				memcpy(temp, b, size);
				memcpy(a, temp, size);
			}

			finally
			{
				if (temp != null)
					free(temp);
			}
		}

		public static void memmove(void* a, void* b, ulong size)
		{
			memmove(a, b, (long)size);
		}

		public static int memcmp(void* a, void* b, long size)
		{
			var result = 0;
			var ap = (byte*)a;
			var bp = (byte*)b;
			for (long i = 0; i < size; ++i)
			{
				if (*ap != *bp)
					result += 1;

				ap++;
				bp++;
			}

			return result;
		}

		public static int memcmp(void* a, void* b, ulong size)
		{
			return memcmp(a, b, (long)size);
		}

		public static int memcmp(byte* a, byte[] b, ulong size)
		{
			fixed (void* bptr = b)
			{
				return memcmp(a, bptr, (long)size);
			}
		}

		public static void memset(void* ptr, int value, long size)
		{
			var bptr = (byte*)ptr;
			var bval = (byte)value;
			for (long i = 0; i < size; ++i)
				*bptr++ = bval;
		}

		public static void memset(void* ptr, int value, ulong size)
		{
			memset(ptr, value, (long)size);
		}

		public static uint _lrotl(uint x, int y)
		{
			return (x << y) | (x >> (32 - y));
		}

		public static void* realloc(void* a, long newSize)
		{
			if (a == null)
				return malloc(newSize);

			var ptr = new IntPtr(a);
			var result = Marshal.ReAllocHGlobal(ptr, new IntPtr(newSize));

			return result.ToPointer();
		}

		public static void* realloc(void* a, ulong newSize)
		{
			return realloc(a, (long)newSize);
		}

		public static int abs(int v)
		{
			return Math.Abs(v);
		}

		public static double pow(double a, double b)
		{
			return Math.Pow(a, b);
		}

		public static void SetArray<T>(T[] data, T value)
		{
			for (var i = 0; i < data.Length; ++i)
				data[i] = value;
		}

		public static double ldexp(double number, int exponent)
		{
			return number * Math.Pow(2, exponent);
		}

		public static int strcmp(sbyte* src, string token)
		{
			var result = 0;

			for (var i = 0; i < token.Length; ++i)
			{
				if (src[i] != token[i])
				{
					++result;
				}
			}

			return result;
		}

		public static int strncmp(sbyte* src, string token, ulong size)
		{
			var result = 0;

			for (var i = 0; i < Math.Min(token.Length, (int)size); ++i)
			{
				if (src[i] != token[i])
				{
					++result;
				}
			}

			return result;
		}

		public static long strtol(sbyte* start, sbyte** end, int radix)
		{
			// First step - determine length
			var length = 0;
			sbyte* ptr = start;
			while (numbers.IndexOf((char)*ptr) != -1)
			{
				++ptr;
				++length;
			}

			long result = 0;

			// Now build up the number
			ptr = start;
			while (length > 0)
			{
				long num = numbers.IndexOf((char)*ptr);
				long pow = (long)Math.Pow(10, length - 1);
				result += num * pow;

				++ptr;
				--length;
			}

			if (end != null)
			{
				*end = ptr;
			}

			return result;
		}
	}
}