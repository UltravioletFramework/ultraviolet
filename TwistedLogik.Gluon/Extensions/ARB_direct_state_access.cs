using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        [MonoNativeFunctionWrapper]
        private delegate void glNamedBufferDataDelegate(uint buffer, IntPtr size, IntPtr data, uint usage);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedBufferDataDelegate glNamedBufferData = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedBufferSubDataDelegate(uint buffer, IntPtr offset, IntPtr size, IntPtr data);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedBufferSubDataDelegate glNamedBufferSubData = null;

        [MonoNativeFunctionWrapper]
        private delegate uint glCheckNamedFramebufferStatusDelegate(uint framebuffer, uint target);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCheckNamedFramebufferStatusDelegate glCheckNamedFramebufferStatus = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferTextureDelegate(uint framebuffer, uint attachment, uint texture, int level);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferTextureDelegate glNamedFramebufferTexture = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferRenderbufferDelegate(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferRenderbufferDelegate glNamedFramebufferRenderbuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferDrawBufferDelegate(uint framebuffer, uint mode);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferDrawBufferDelegate glNamedFramebufferDrawBuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferDrawBuffersDelegate(uint framebuffer, int size, IntPtr bufs);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glNamedFramebufferDrawBuffersDelegate glNamedFramebufferDrawBuffers = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureParameteriDelegate(uint texture, uint pname, int param);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureParameteriDelegate glTextureParameteri = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage2DDelegate(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureSubImage2DDelegate glTextureSubImage2D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage1DDelegate(uint texture, int levels, uint internalformat, int width);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage1DDelegate glTextureStorage1D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage2DDelegate(uint texture, int levels, uint internalformat, int width, int height);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage2DDelegate glTextureStorage2D = null;
                
        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage3DDelegate(uint texture, int levels, uint internalformat, int width, int height, int depth);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glTextureStorage3DDelegate glTextureStorage3D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glCreateBuffersDelegate(int n, IntPtr buffers);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateBuffersDelegate glCreateBuffers = null;

        public static void CreateBuffers(int n, uint* buffers)
        {
            glCreateBuffers(n, (IntPtr)buffers);
        }

        public static uint CreateBuffer()
        {
            unsafe
            {
                uint buffers;
                glCreateBuffers(1, (IntPtr)(&buffers));
                return buffers;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateTexturesDelegate(uint target, int n, IntPtr textures);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateTexturesDelegate glCreateTextures = null;

        public static void CreateTextures(uint target, int n, uint* textures)
        {
            glCreateTextures(target, n, (IntPtr)textures);
        }

        public static uint CreateTexture(uint target)
        {
            unsafe
            {
                uint textures;
                glCreateTextures(target, 1, (IntPtr)(&textures));
                return textures;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateFramebuffersDelegate(int n, IntPtr ids);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateFramebuffersDelegate glCreateFramebuffers = null;

        public static void CreateFramebuffers(int n, uint* ids)
        {
            glCreateFramebuffers(n, (IntPtr)ids);
        }

        public static uint CreateFramebuffer()
        {
            unsafe
            {
                uint framebuffers;
                glCreateFramebuffers(1, (IntPtr)(&framebuffers));
                return framebuffers;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateRenderbuffersDelegate(int n, IntPtr renderbuffers);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_direct_state_access")]
        private static readonly glCreateRenderbuffersDelegate glCreateRenderbuffers = null;

        public static void CreateRenderbuffers(int n, uint* renderbuffers)
        {
            glCreateRenderbuffers(n, (IntPtr)renderbuffers);
        }

        public static uint CreateRenderbuffer()
        {
            unsafe
            {
                uint renderbuffers;
                glCreateRenderbuffers(1, (IntPtr)(&renderbuffers));
                return renderbuffers;
            }
        }
    }
}
