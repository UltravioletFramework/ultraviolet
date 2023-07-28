using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glDrawElementsInstancedBaseInstanceDelegate(uint mode, int count, uint type, IntPtr indices, int primcount, uint baseinstance);
        [Require(MinVersion = "4.2", Extension = "GL_ARB_base_instance")]
        private static glDrawElementsInstancedBaseInstanceDelegate glDrawElementsInstancedBaseInstance = null;

        public static void DrawElementsInstancedBaseInstance(uint mode, int count, uint type, void* indices, int primcount, uint baseinstance) =>
            glDrawElementsInstancedBaseInstance(mode, count, type, (IntPtr)indices, primcount, baseinstance);

        public const UInt32 GL_COMPRESSED_RGBA_BPTC_UNORM = 0x8E8C;
        public const UInt32 GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM = 0x8E8D;
        public const UInt32 GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT = 0x8E8E;
        public const UInt32 GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F;
    }
}
