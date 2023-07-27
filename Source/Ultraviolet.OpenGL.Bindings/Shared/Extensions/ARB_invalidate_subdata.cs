using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glInvalidateFramebufferDelegate(UInt32 target, Int32 numAttachments, IntPtr attachments);
        [Require(MinVersion = "4.3", MinVersionES = "3.0")]
        [Require(Extension = "GL_ARB_invalidate_subdata")]
        private static glInvalidateFramebufferDelegate glInvalidateFramebuffer = null;

        public static void InvalidateFramebuffer(UInt32 target, Int32 numAttachments, IntPtr attachments)
        {
            glInvalidateFramebuffer(target, numAttachments, attachments);
        }
    }
}
