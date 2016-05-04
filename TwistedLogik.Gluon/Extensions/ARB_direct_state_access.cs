using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glNamedBufferDataDelegate(uint buffer, IntPtr size, void* data, uint usage);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedBufferDataDelegate glNamedBufferData = null;

        private delegate void glNamedBufferSubDataDelegate(uint buffer, IntPtr offset, IntPtr size, void* data);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedBufferSubDataDelegate glNamedBufferSubData = null;

        private delegate uint glCheckNamedFramebufferStatusDelegate(uint framebuffer, uint target);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCheckNamedFramebufferStatusDelegate glCheckNamedFramebufferStatus = null;

        private delegate void glNamedFramebufferTextureDelegate(uint framebuffer, uint attachment, uint texture, int level);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferTextureDelegate glNamedFramebufferTexture = null;

        private delegate void glNamedFramebufferRenderbufferDelegate(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferRenderbufferDelegate glNamedFramebufferRenderbuffer = null;

        private delegate void glNamedFramebufferDrawBufferDelegate(uint framebuffer, uint mode);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferDrawBufferDelegate glNamedFramebufferDrawBuffer = null;

        private delegate void glNamedFramebufferDrawBuffersDelegate(uint framebuffer, int size, uint* bufs);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferDrawBuffersDelegate glNamedFramebufferDrawBuffers = null;

        private delegate void glTextureParameteriDelegate(uint texture, uint pname, int param);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureParameteriDelegate glTextureParameteri = null;

        private delegate void glTextureSubImage2DDelegate(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureSubImage2DDelegate glTextureSubImage2D = null;

        private delegate void glTextureStorage1DDelegate(uint texture, int levels, uint internalformat, int width);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage1DDelegate glTextureStorage1D = null;

        private delegate void glTextureStorage2DDelegate(uint texture, int levels, uint internalformat, int width, int height);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage2DDelegate glTextureStorage2D = null;
                
        private delegate void glTextureStorage3DDelegate(uint texture, int levels, uint internalformat, int width, int height, int depth);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage3DDelegate glTextureStorage3D = null;

        private delegate void glCreateBuffersDelegate(int n, uint* buffers);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateBuffersDelegate glCreateBuffers = null;

        public static void CreateBuffers(int n, uint* buffers)
        {
            glCreateBuffers(n, buffers);
        }

        public static uint CreateBuffer()
        {
            unsafe
            {
                uint buffers;
                glCreateBuffers(1, &buffers);
                return buffers;
            }
        }

        private delegate void glCreateTexturesDelegate(uint target, int n, uint* textures);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateTexturesDelegate glCreateTextures = null;

        public static void CreateTextures(uint target, int n, uint* textures)
        {
            glCreateTextures(target, n, textures);
        }

        public static uint CreateTexture(uint target)
        {
            unsafe
            {
                uint textures;
                glCreateTextures(target, 1, &textures);
                return textures;
            }
        }

        private delegate void glCreateFramebuffersDelegate(int n, uint* ids);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateFramebuffersDelegate glCreateFramebuffers = null;

        public static void CreateFramebuffers(int n, uint* ids)
        {
            glCreateFramebuffers(n, ids);
        }

        public static uint CreateFramebuffer()
        {
            unsafe
            {
                uint framebuffers;
                glCreateFramebuffers(1, &framebuffers);
                return framebuffers;
            }
        }

        private delegate void glCreateRenderbuffersDelegate(int n, uint* renderbuffers);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateRenderbuffersDelegate glCreateRenderbuffers = null;

        public static void CreateRenderbuffers(int n, uint* renderbuffers)
        {
            glCreateRenderbuffers(n, renderbuffers);
        }

        public static uint CreateRenderbuffer()
        {
            unsafe
            {
                uint renderbuffers;
                glCreateRenderbuffers(1, &renderbuffers);
                return renderbuffers;
            }
        }
    }
}
