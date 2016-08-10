using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        [MonoNativeFunctionWrapper]
        private delegate IntPtr glMapBufferRangeDelegate(uint target, IntPtr offset, IntPtr length, uint access);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_map_buffer_range")]
        private static readonly glMapBufferRangeDelegate glMapBufferRange = null;

        public static void* MapBufferRange(uint target, int* offset, uint* length, uint access)
        { return glMapBufferRange(target, (IntPtr)offset, (IntPtr)length, access).ToPointer(); }

        [MonoNativeFunctionWrapper]
        private delegate void glFlushMappedBufferRangeDelegate(uint target, IntPtr offset, IntPtr length);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_map_buffer_range")]
        private static readonly glFlushMappedBufferRangeDelegate glFlushMappedBufferRange = null;

        public static void FlushMappedBufferRange(uint target, int* offset, uint* length)
        { glFlushMappedBufferRange(target, (IntPtr)offset, (IntPtr)length); }

        public const UInt32 MAP_READ_BIT = 0x0001;
        public const UInt32 MAP_WRITE_BIT = 0x0002;
        public const UInt32 MAP_INVALIDATE_RANGE_BIT = 0x0004;
        public const UInt32 MAP_INVALIDATE_BUFFER_BIT = 0x0008;
        public const UInt32 MAP_FLUSH_EXPLICIT_BIT = 0x0010;
        public const UInt32 MAP_UNSYNCHRONIZED_BIT = 0x0020;
    }
}
