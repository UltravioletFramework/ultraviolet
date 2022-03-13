using System;
using System.Runtime.InteropServices;

namespace Hebron.Runtime
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	unsafe class UnsafeArray1D<T> where T : struct
	{
		private readonly T[] _data;

		internal GCHandle PinHandle { get; }

		public T this[int index]
		{
			get => _data[index];
			set => _data[index] = value;
		}

		public UnsafeArray1D(int size)
		{
			if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));

			_data = new T[size];
			PinHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
		}

		public UnsafeArray1D(T[] data, int sizeOf)
		{
			if (sizeOf <= 0) throw new ArgumentOutOfRangeException(nameof(sizeOf));

			_data = data ?? throw new ArgumentNullException(nameof(data));
			PinHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
		}

		~UnsafeArray1D()
		{
			PinHandle.Free();
		}

		public void* ToPointer()
		{
			return PinHandle.AddrOfPinnedObject().ToPointer();
		}

		public static implicit operator void*(UnsafeArray1D<T> array)
		{
			return array.ToPointer();
		}

		public static void* operator +(UnsafeArray1D<T> array, int delta)
		{
			return array.ToPointer();
		}
	}
}