using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glNamedBufferDataEXTDelegate(uint buffer, IntPtr size, void* data, uint usage);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glNamedBufferDataEXTDelegate glNamedBufferDataEXT = null;

        private delegate void glNamedBufferSubDataEXTDelegate(uint buffer, IntPtr offset, IntPtr size, void* data);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glNamedBufferSubDataEXTDelegate glNamedBufferSubDataEXT = null;

        private delegate uint glCheckNamedFramebufferStatusEXTDelegate(uint framebuffer, uint target);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glCheckNamedFramebufferStatusEXTDelegate glCheckNamedFramebufferStatusEXT = null;

        private delegate void glNamedFramebufferTextureEXTDelegate(uint framebuffer, uint attachment, uint texture, int level);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glNamedFramebufferTextureEXTDelegate glNamedFramebufferTextureEXT = null;

        private delegate void glTextureParameteriEXTDelegate(uint texture, uint target, uint pname, int param);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureParameteriEXTDelegate glTextureParameteriEXT = null;

        private delegate void glTextureImage2DEXTDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureImage2DEXTDelegate glTextureImage2DEXT = null;
      
        private delegate void glTextureSubImage2DEXTDelegate(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureSubImage2DEXTDelegate glTextureSubImage2DEXT = null;
    }
}
