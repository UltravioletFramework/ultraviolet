using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glGenQueriesDelegate(int n, IntPtr ids);
        [Require(MinVersion = "1.5")]
        private static glGenQueriesDelegate glGenQueries = null;

        public static void GenQueries(int n, uint* ids) { glGenQueries(n, (IntPtr)ids); }

        public static void GenQueries(uint[] ids)
        {
            fixed (uint* pbuffers = ids)
            {
                glGenQueries(ids.Length, (IntPtr)pbuffers);
            }
        }

        public static uint GenQuery()
        {
            uint id;
            glGenQueries(1, (IntPtr)(&id));
            return id;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteQueriesDelegate(int n, IntPtr ids);
        [Require(MinVersion = "1.5")]
        private static glDeleteQueriesDelegate glDeleteQueries = null;

        public static void DeleteQueries(int n, uint* ids) { glDeleteQueries(n, (IntPtr)ids); }

        public static void DeleteQueries(uint[] ids)
        {
            fixed (uint* pbuffers = ids)
            {
                glDeleteQueries(ids.Length, (IntPtr)pbuffers);
            }
        }

        public static void DeleteQuery(uint id)
        {
            glDeleteQueries(1, (IntPtr)(&id));
        }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsQueryDelegate(uint id);
        [Require(MinVersion = "1.5")]
        private static glIsQueryDelegate glIsQuery = null;

        public static bool IsQuery(uint id) { return glIsQuery(id); }

        [MonoNativeFunctionWrapper]
        private delegate void glBeginQueryDelegate(uint target, uint id);
        [Require(MinVersion = "1.5")]
        private static glBeginQueryDelegate glBeginQuery = null;

        public static void BeginQuery(uint target, uint id) { glBeginQuery(target, id); }

        [MonoNativeFunctionWrapper]
        private delegate void glEndQueryDelegate(uint target);
        [Require(MinVersion = "1.5")]
        private static glEndQueryDelegate glEndQuery = null;

        public static void EndQuery(uint target) { glEndQuery(target); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetQueryivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.5")]
        private static glGetQueryivDelegate glGetQueryiv = null;

        public static void GetQueryiv(uint target, uint pname, int* @params) { glGetQueryiv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetQueryObjectivDelegate(uint id, uint pname, IntPtr @params);
        [Require(MinVersion = "1.5")]
        private static glGetQueryObjectivDelegate glGetQueryObjectiv = null;

        public static void GetQueryObjectiv(uint id, uint pname, int* @params) { glGetQueryObjectiv(id, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetQueryObjectuivDelegate(uint id, uint pname, IntPtr @params);
        [Require(MinVersion = "1.5")]
        private static glGetQueryObjectuivDelegate glGetQueryObjectuiv = null;

        public static void GetQueryObjectuiv(uint id, uint pname, uint* @params) { glGetQueryObjectuiv(id, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glBindBufferDelegate(uint target, uint buffer);
        [Require(MinVersion = "1.5")]
        private static glBindBufferDelegate glBindBuffer = null;

        public static void BindBuffer(uint target, uint buffer) { glBindBuffer(target, buffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteBuffersDelegate(int n, IntPtr buffers);
        [Require(MinVersion = "1.5")]
        private static glDeleteBuffersDelegate glDeleteBuffers = null;

        public static void DeleteBuffers(int n, uint* buffers) { glDeleteBuffers(n, (IntPtr)buffers); }

        public static void DeleteBuffers(uint[] buffers)
        {
            fixed (uint* pbuffers = buffers)
            {
                glDeleteBuffers(buffers.Length, (IntPtr)pbuffers);
            }
        }

        public static void DeleteBuffer(uint buffer)
        {
            glDeleteBuffers(1, (IntPtr)(&buffer));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGenBuffersDelegate(int n, IntPtr buffers);
        [Require(MinVersion = "1.5")]
        private static glGenBuffersDelegate glGenBuffers = null;

        public static void GenBuffers(int n, uint* buffers) { glGenBuffers(n, (IntPtr)buffers); }

        public static void GenBuffers(uint[] buffers)
        {
            fixed (uint* pbuffers = buffers)
            {
                glGenBuffers(buffers.Length, (IntPtr)pbuffers);
            }
        }

        public static uint GenBuffer()
        {
            uint value;
            glGenBuffers(1, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsBufferDelegate(uint buffer);
        [Require(MinVersion = "1.5")]
        private static glIsBufferDelegate glIsBuffer = null;

        public static bool IsBuffer(uint buffer) { return glIsBuffer(buffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glBufferDataDelegate(uint target, IntPtr size, IntPtr data, uint usage);
        [Require(MinVersion = "1.5")]
        private static glBufferDataDelegate glBufferData = null;

        public static void BufferData(uint target, IntPtr size, void* data, uint usage) { glBufferData(target, size, (IntPtr)data, usage); }

        [MonoNativeFunctionWrapper]
        private delegate void glBufferSubDataDelegate(uint target, IntPtr offset, IntPtr size, IntPtr data);
        [Require(MinVersion = "1.5")]
        private static glBufferSubDataDelegate glBufferSubData = null;

        public static void BufferSubData(uint target, IntPtr offset, IntPtr size, void* data) { glBufferSubData(target, offset, size, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBufferSubDataDelegate(uint target, IntPtr offset, IntPtr size, IntPtr data);
        [Require(MinVersion = "1.5")]
        private static glGetBufferSubDataDelegate glGetBufferSubData = null;

        public static void GetBufferSubData(uint target, IntPtr offset, IntPtr size, void* data) { glGetBufferSubData(target, offset, size, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr glMapBufferDelegate(uint target, uint access);
        [Require(MinVersion = "1.5")]
        private static glMapBufferDelegate glMapBuffer = null;

        public static void* MapBuffer(uint target, uint access) { return glMapBuffer(target, access).ToPointer(); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glUnmapBufferDelegate(uint target);
        [Require(MinVersion = "1.5")]
        private static glUnmapBufferDelegate glUnmapBuffer = null;

        public static bool UnmapBuffer(uint target) { return glUnmapBuffer(target); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBufferParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.5")]
        private static glGetBufferParameterivDelegate glGetBufferParameteriv = null;

        public static void GetBufferParameteriv(uint target, uint pname, int* @params) { glGetBufferParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBufferPointervDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.5")]
        private static glGetBufferPointervDelegate glGetBufferPointerv = null;

        public static void GetBufferPointerv(uint target, uint pname, void** @params) { glGetBufferPointerv(target, pname, (IntPtr)@params); }

        public const UInt32 GL_FOG_COORD_SRC = GL_FOG_COORDINATE_SOURCE;
        public const UInt32 GL_FOG_COORD = GL_FOG_COORDINATE;
        public const UInt32 GL_FOG_COORD_ARRAY = GL_FOG_COORDINATE_ARRAY;
        public const UInt32 GL_SRC0_RGB = GL_SOURCE0_RGB;
        public const UInt32 GL_FOG_COORD_ARRAY_POINTER = GL_FOG_COORDINATE_ARRAY_POINTER;
        public const UInt32 GL_FOG_COORD_ARRAY_TYPE = GL_FOG_COORDINATE_ARRAY_TYPE;
        public const UInt32 GL_SRC1_ALPHA = GL_SOURCE1_ALPHA;
        public const UInt32 GL_CURRENT_FOG_COORD = GL_CURRENT_FOG_COORDINATE;
        public const UInt32 GL_FOG_COORD_ARRAY_STRIDE = GL_FOG_COORDINATE_ARRAY_STRIDE;
        public const UInt32 GL_SRC0_ALPHA = GL_SOURCE0_ALPHA;
        public const UInt32 GL_SRC1_RGB = GL_SOURCE1_RGB;
        public const UInt32 GL_FOG_COORD_ARRAY_BUFFER_BINDING = GL_FOG_COORDINATE_ARRAY_BUFFER_BINDING;
        public const UInt32 GL_SRC2_ALPHA = GL_SOURCE2_ALPHA;
        public const UInt32 GL_SRC2_RGB = GL_SOURCE2_RGB;
        public const UInt32 GL_BUFFER_SIZE = 0x8764;
        public const UInt32 GL_BUFFER_USAGE = 0x8765;
        public const UInt32 GL_QUERY_COUNTER_BITS = 0x8864;
        public const UInt32 GL_CURRENT_QUERY = 0x8865;
        public const UInt32 GL_QUERY_RESULT = 0x8866;
        public const UInt32 GL_QUERY_RESULT_AVAILABLE = 0x8867;
        public const UInt32 GL_ARRAY_BUFFER = 0x8892;
        public const UInt32 GL_ELEMENT_ARRAY_BUFFER = 0x8893;
        public const UInt32 GL_ARRAY_BUFFER_BINDING = 0x8894;
        public const UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
        public const UInt32 GL_VERTEX_ARRAY_BUFFER_BINDING = 0x8896;
        public const UInt32 GL_NORMAL_ARRAY_BUFFER_BINDING = 0x8897;
        public const UInt32 GL_COLOR_ARRAY_BUFFER_BINDING = 0x8898;
        public const UInt32 GL_INDEX_ARRAY_BUFFER_BINDING = 0x8899;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_BUFFER_BINDING = 0x889A;
        public const UInt32 GL_EDGE_FLAG_ARRAY_BUFFER_BINDING = 0x889B;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY_BUFFER_BINDING = 0x889C;
        public const UInt32 GL_FOG_COORDINATE_ARRAY_BUFFER_BINDING = 0x889D;
        public const UInt32 GL_WEIGHT_ARRAY_BUFFER_BINDING = 0x889E;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
        public const UInt32 GL_READ_ONLY = 0x88B8;
        public const UInt32 GL_WRITE_ONLY = 0x88B9;
        public const UInt32 GL_READ_WRITE = 0x88BA;
        public const UInt32 GL_BUFFER_ACCESS = 0x88BB;
        public const UInt32 GL_BUFFER_MAPPED = 0x88BC;
        public const UInt32 GL_BUFFER_MAP_POINTER = 0x88BD;
        public const UInt32 GL_STREAM_DRAW = 0x88E0;
        public const UInt32 GL_STREAM_READ = 0x88E1;
        public const UInt32 GL_STREAM_COPY = 0x88E2;
        public const UInt32 GL_STATIC_DRAW = 0x88E4;
        public const UInt32 GL_STATIC_READ = 0x88E5;
        public const UInt32 GL_STATIC_COPY = 0x88E6;
        public const UInt32 GL_DYNAMIC_DRAW = 0x88E8;
        public const UInt32 GL_DYNAMIC_READ = 0x88E9;
        public const UInt32 GL_DYNAMIC_COPY = 0x88EA;
        public const UInt32 GL_SAMPLES_PASSED = 0x8914;
    }
}
