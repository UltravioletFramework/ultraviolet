using System.Threading;

namespace Hebron.Runtime
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	unsafe static class MemoryStats
	{
		private static int _allocations;
		 
		public static int Allocations
		{
			get
			{
				return _allocations;
			}
		}

		internal static void Allocated()
		{
			Interlocked.Increment(ref _allocations);
		}

		internal static void Freed()
		{
			Interlocked.Decrement(ref _allocations);
		}
	}
}