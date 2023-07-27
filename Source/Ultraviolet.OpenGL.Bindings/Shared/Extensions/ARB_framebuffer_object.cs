using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glBindFramebufferDelegate(uint target, uint framebuffer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glBindFramebufferDelegate glBindFramebuffer = null;

        public static void BindFramebuffer(uint target, uint framebuffer) { glBindFramebuffer(target, framebuffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glBindRenderbufferDelegate(uint target, uint renderbuffer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glBindRenderbufferDelegate glBindRenderbuffer = null;

        public static void BindRenderbuffer(uint target, uint renderbuffer) { glBindRenderbuffer(target, renderbuffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glBlitFramebufferDelegate(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glBlitFramebufferDelegate glBlitFramebuffer = null;

        public static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) { glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }

        [MonoNativeFunctionWrapper]
        private delegate uint glCheckFramebufferStatusDelegate(uint target);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glCheckFramebufferStatusDelegate glCheckFramebufferStatus = null;

        public static uint CheckFramebufferStatus(uint target) { return glCheckFramebufferStatus(target); }
        
        [MonoNativeFunctionWrapper]
        private delegate void glDeleteFramebuffersDelegate(int n, IntPtr framebuffers);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glDeleteFramebuffersDelegate glDeleteFramebuffers = null;

        public static void DeleteFramebuffers(int n, uint* framebuffers) { glDeleteFramebuffers(n, (IntPtr)framebuffers); }

        public static void DeleteFramebuffers(uint[] framebuffers)
        {
            fixed (uint* pframebuffers = framebuffers)
            {
                glDeleteFramebuffers(framebuffers.Length, (IntPtr)pframebuffers);
            }
        }

        public static void DeleteFramebuffer(uint framebuffer)
        {
            glDeleteFramebuffers(1, (IntPtr)(&framebuffer));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteRenderbuffersDelegate(int n, IntPtr renderbuffers);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glDeleteRenderbuffersDelegate glDeleteRenderbuffers = null;

        public static void DeleteRenderBuffers(int n, uint* renderbuffers) { glDeleteRenderbuffers(n, (IntPtr)renderbuffers); }

        public static void DeleteRenderBuffers(uint[] renderbuffers)
        {
            fixed (uint* prenderbuffers = renderbuffers)
            {
                glDeleteRenderbuffers(renderbuffers.Length, (IntPtr)prenderbuffers);
            }
        }

        public static void DeleteRenderBuffers(uint renderbuffer)
        {
            glDeleteRenderbuffers(1, (IntPtr)(&renderbuffer));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferRenderbufferDelegate(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glFramebufferRenderbufferDelegate glFramebufferRenderbuffer = null;

        public static void FramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) { glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferTexture1DDelegate(uint target, uint attachment, uint textarget, uint texture, int level);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glFramebufferTexture1DDelegate glFramebufferTexture1D = null;

        public static void FramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level) { glFramebufferTexture1D(target, attachment, textarget, texture, level); }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferTexture2DDelegate(uint target, uint attachment, uint textarget, uint texture, int level);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glFramebufferTexture2DDelegate glFramebufferTexture2D = null;

        public static void FramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) { glFramebufferTexture2D(target, attachment, textarget, texture, level); }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferTexture3DDelegate(uint target, uint attachment, uint textarget, uint texture, int level, int layer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glFramebufferTexture3DDelegate glFramebufferTexture3D = null;

        public static void FramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int layer) { glFramebufferTexture3D(target, attachment, textarget, texture, level, layer); }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferTextureLayerDelegate(uint target, uint attachment, uint texture, int level, int layer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glFramebufferTextureLayerDelegate glFramebufferTextureLayer = null;

        public static void FramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) { glFramebufferTextureLayer(target, attachment, texture, level, layer); }

        [MonoNativeFunctionWrapper]
        private delegate void glGenFramebuffersDelegate(int n, IntPtr framebuffers);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glGenFramebuffersDelegate glGenFramebuffers = null;

        public static void GenFramebuffers(int n, uint* framebuffers) { glGenFramebuffers(n, (IntPtr)framebuffers); }

        public static void GenFramebuffers(uint[] framebuffers)
        {
            fixed (uint* ptextures = framebuffers)
            {
                glGenFramebuffers(framebuffers.Length, (IntPtr)ptextures);
            }
        }

        public static uint GenFramebuffer()
        {
            uint value;
            glGenFramebuffers(1, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGenRenderbuffersDelegate(int n, IntPtr renderbuffers);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glGenRenderbuffersDelegate glGenRenderbuffers = null;

        public static void GenRenderbuffers(int n, uint* renderbuffers) { glGenRenderbuffers(n, (IntPtr)renderbuffers); }

        public static void GenRenderbuffers(uint[] renderbuffers)
        {
            fixed (uint* ptextures = renderbuffers)
            {
                glGenRenderbuffers(renderbuffers.Length, (IntPtr)ptextures);
            }
        }

        public static uint GenRenderbuffer()
        {
            uint value;
            glGenRenderbuffers(1, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGenerateMipmapDelegate(uint target);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glGenerateMipmapDelegate glGenerateMipmap = null;

        public static void GenerateMipmap(uint target) { glGenerateMipmap(target); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetFramebufferAttachmentParameterivDelegate(uint target, uint attachment, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glGetFramebufferAttachmentParameterivDelegate glGetFramebufferAttachmentParameteriv = null;

        public static void GetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* @params) { glGetFramebufferAttachmentParameteriv(target, attachment, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetRenderbufferParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glGetRenderbufferParameterivDelegate glGetRenderbufferParameteriv = null;

        public static void GetRenderbufferParameteriv(uint target, uint pname, int* @params) { glGetRenderbufferParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsFramebufferDelegate(uint framebuffer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glIsFramebufferDelegate glIsFramebuffer = null;

        public static bool IsFramebuffer(uint framebuffer) { return glIsFramebuffer(framebuffer); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsRenderbufferDelegate(uint renderbuffer);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glIsRenderbufferDelegate glIsRenderbuffer = null;

        public static bool IsRenderbuffer(uint renderbuffer) { return glIsRenderbuffer(renderbuffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glRenderbufferStorageDelegate(uint target, uint internalformat, int width, int height);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glRenderbufferStorageDelegate glRenderbufferStorage = null;

        public static void RenderbufferStorage(uint target, uint internalformat, int width, int height) { glRenderbufferStorage(target, internalformat, width, height); }

        [MonoNativeFunctionWrapper]
        private delegate void glRenderbufferStorageMultisampleDelegate(uint target, int samples, uint internalformat, int width, int height);
        [Require(MinVersion = "3.0", MinVersionES = "2.0", Extension = "GL_ARB_framebuffer_object")]
        private static glRenderbufferStorageMultisampleDelegate glRenderbufferStorageMultisample = null;

        public static void RenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) { glRenderbufferStorageMultisample(target, samples, internalformat, width, height); }

        public const UInt32 GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
        public const UInt32 GL_FRAMEBUFFER_DEFAULT = 0x8218;
        public const UInt32 GL_FRAMEBUFFER_UNDEFINED = 0x8219;
        public const UInt32 GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
        public const UInt32 GL_INDEX = 0x8222;
        public const UInt32 GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
        public const UInt32 GL_DEPTH_STENCIL = 0x84F9;
        public const UInt32 GL_UNSIGNED_INT_24_8 = 0x84FA;
        public const UInt32 GL_DEPTH24_STENCIL8 = 0x88F0;
        public const UInt32 GL_TEXTURE_STENCIL_SIZE = 0x88F1;
        public const UInt32 GL_UNSIGNED_NORMALIZED = 0x8C17;
        // Defined in gl.Core_2_1.cs | public const UInt32 GL_SRGB = 0x8C40;
        public const UInt32 GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
        public const UInt32 GL_FRAMEBUFFER_BINDING = 0x8CA6;
        public const UInt32 GL_RENDERBUFFER_BINDING = 0x8CA7;
        public const UInt32 GL_READ_FRAMEBUFFER = 0x8CA8;
        public const UInt32 GL_DRAW_FRAMEBUFFER = 0x8CA9;
        public const UInt32 GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
        public const UInt32 GL_RENDERBUFFER_SAMPLES = 0x8CAB;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
        public const UInt32 GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
        public const UInt32 GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
        public const UInt32 GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
        public const UInt32 GL_COLOR_ATTACHMENT0 = 0x8CE0;
        public const UInt32 GL_COLOR_ATTACHMENT1 = 0x8CE1;
        public const UInt32 GL_COLOR_ATTACHMENT2 = 0x8CE2;
        public const UInt32 GL_COLOR_ATTACHMENT3 = 0x8CE3;
        public const UInt32 GL_COLOR_ATTACHMENT4 = 0x8CE4;
        public const UInt32 GL_COLOR_ATTACHMENT5 = 0x8CE5;
        public const UInt32 GL_COLOR_ATTACHMENT6 = 0x8CE6;
        public const UInt32 GL_COLOR_ATTACHMENT7 = 0x8CE7;
        public const UInt32 GL_COLOR_ATTACHMENT8 = 0x8CE8;
        public const UInt32 GL_COLOR_ATTACHMENT9 = 0x8CE9;
        public const UInt32 GL_COLOR_ATTACHMENT10 = 0x8CEA;
        public const UInt32 GL_COLOR_ATTACHMENT11 = 0x8CEB;
        public const UInt32 GL_COLOR_ATTACHMENT12 = 0x8CEC;
        public const UInt32 GL_COLOR_ATTACHMENT13 = 0x8CED;
        public const UInt32 GL_COLOR_ATTACHMENT14 = 0x8CEE;
        public const UInt32 GL_COLOR_ATTACHMENT15 = 0x8CEF;
        public const UInt32 GL_DEPTH_ATTACHMENT = 0x8D00;
        public const UInt32 GL_STENCIL_ATTACHMENT = 0x8D20;
        public const UInt32 GL_FRAMEBUFFER = 0x8D40;
        public const UInt32 GL_RENDERBUFFER = 0x8D41;
        public const UInt32 GL_RENDERBUFFER_WIDTH = 0x8D42;
        public const UInt32 GL_RENDERBUFFER_HEIGHT = 0x8D43;
        public const UInt32 GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
        public const UInt32 GL_STENCIL_INDEX1 = 0x8D46;
        public const UInt32 GL_STENCIL_INDEX4 = 0x8D47;
        public const UInt32 GL_STENCIL_INDEX8 = 0x8D48;
        public const UInt32 GL_STENCIL_INDEX16 = 0x8D49;
        public const UInt32 GL_RENDERBUFFER_RED_SIZE = 0x8D50;
        public const UInt32 GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
        public const UInt32 GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
        public const UInt32 GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
        public const UInt32 GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
        public const UInt32 GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
        public const UInt32 GL_MAX_SAMPLES = 0x8D57;
    }
}
