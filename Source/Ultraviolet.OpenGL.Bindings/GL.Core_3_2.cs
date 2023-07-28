using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glGetInteger64i_vDelegate(uint pname, uint index, IntPtr data);
        [Require(MinVersion = "3.2")]
        private static glGetInteger64i_vDelegate glGetInteger64i_v = null;

        public static void GetInteger64i_v(uint pname, uint index, long* data) { glGetInteger64i_v(pname, index, (IntPtr)data); }

        public static long GetInteger64i(uint pname, uint index)
        {
            var value = 0u;
            var pValue = &value;
            glGetInteger64i_v(pname, index, (IntPtr)pValue);
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBufferParameteri64vDelegate(uint target, uint value, IntPtr data);
        [Require(MinVersion = "3.2")]
        private static glGetBufferParameteri64vDelegate glGetBufferParameteri64v = null;

        public static void GetBufferParameteri64v(uint target, uint value, long* data) { glGetBufferParameteri64v(target, value, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glFramebufferTextureDelegate(uint target, uint attachment, uint texture, int level);
        [Require(MinVersion = "3.2")]
        private static glFramebufferTextureDelegate glFramebufferTexture = null;

        public static void FramebufferTexture(uint target, uint attachment, uint texture, int level) { glFramebufferTexture(target, attachment, texture, level); }

        public const UInt32 GL_CONTEXT_CORE_PROFILE_BIT = 0x00000001;
        public const UInt32 GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;
        public const UInt32 GL_LINES_ADJACENCY = 0x000A;
        public const UInt32 GL_LINE_STRIP_ADJACENCY = 0x000B;
        public const UInt32 GL_TRIANGLES_ADJACENCY = 0x000C;
        public const UInt32 GL_TRIANGLE_STRIP_ADJACENCY = 0x000D;
        public const UInt32 GL_PROGRAM_POINT_SIZE = 0x8642;
        public const UInt32 GL_GEOMETRY_VERTICES_OUT = 0x8916;
        public const UInt32 GL_GEOMETRY_INPUT_TYPE = 0x8917;
        public const UInt32 GL_GEOMETRY_OUTPUT_TYPE = 0x8918;
        public const UInt32 GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = 0x8C29;
        public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_LAYERED = 0x8DA7;
        public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
        public const UInt32 GL_GEOMETRY_SHADER = 0x8DD9;
        public const UInt32 GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = 0x8DDF;
        public const UInt32 GL_MAX_GEOMETRY_OUTPUT_VERTICES = 0x8DE0;
        public const UInt32 GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = 0x8DE1;
        public const UInt32 GL_MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
        public const UInt32 GL_MAX_GEOMETRY_INPUT_COMPONENTS = 0x9123;
        public const UInt32 GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = 0x9124;
        public const UInt32 GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
        public const UInt32 GL_CONTEXT_PROFILE_MASK = 0x9126;
    }
}