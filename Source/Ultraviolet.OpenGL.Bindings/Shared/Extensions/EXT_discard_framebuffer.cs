using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        public const UInt32 GL_COLOR_EXT = 0x1800;
        public const UInt32 GL_DEPTH_EXT = 0x1801;
        public const UInt32 GL_STENCIL_EXT = 0x1802;

        [MonoNativeFunctionWrapper]
        private delegate void glDiscardFramebufferEXTDelegate(UInt32 target, Int32 numAttachments, IntPtr attachments);
        [Require(Extension = "GL_EXT_discard_framebuffer")]
        private static glDiscardFramebufferEXTDelegate glDiscardFramebufferEXT = null;

        public static void DiscardFramebufferEXT(UInt32 target, Int32 numAttachments, IntPtr attachments)
        {
            glDiscardFramebufferEXT(target, numAttachments, attachments);
        }
    }
}
