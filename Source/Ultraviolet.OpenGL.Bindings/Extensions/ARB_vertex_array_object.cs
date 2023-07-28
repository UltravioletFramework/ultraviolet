using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glGenVertexArraysDelegate(int n, IntPtr arrays);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static glGenVertexArraysDelegate glGenVertexArrays = null;

        public static void GenVertexArrays(int n, uint* arrays) { glGenVertexArrays(n, (IntPtr)arrays); }

        public static void GenVertexArrays(uint[] arrays)
        {
            fixed (uint* parrays = arrays)
            {
                glGenVertexArrays(arrays.Length, (IntPtr)parrays);
            }
        }

        public static uint GenVertexArray()
        {
            uint value;
            glGenVertexArrays(1, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteVertexArraysDelegate(int n, IntPtr arrays);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static glDeleteVertexArraysDelegate glDeleteVertexArrays = null;

        public static void DeleteVertexArrays(int n, uint* arrays) { glDeleteVertexArrays(n, (IntPtr)arrays); }

        public static void DeleteVertexArrays(uint[] arrays)
        {
            fixed (uint* parrays = arrays)
            {
                glDeleteVertexArrays(arrays.Length, (IntPtr)parrays);
            }
        }

        public static void DeleteVertexArray(uint array)
        {
            glDeleteVertexArrays(1, (IntPtr)(&array));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glBindVertexArrayDelegate(uint array);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static glBindVertexArrayDelegate glBindVertexArray = null;

        public static void BindVertexArray(uint array) { glBindVertexArray(array); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsVertexArrayDelegate(uint array);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static glIsVertexArrayDelegate glIsVertexArray = null;

        public static bool IsVertexArray(uint array) { return glIsVertexArray(array); }

        public const UInt32 GL_VERTEX_ARRAY_BINDING = 0x85B5;
    }
}
