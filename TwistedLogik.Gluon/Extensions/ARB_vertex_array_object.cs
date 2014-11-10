using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glGenVertexArraysDelegate(int n, uint* arrays);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static readonly glGenVertexArraysDelegate glGenVertexArrays = null;

        public static void GenVertexArrays(int n, uint* arrays) { glGenVertexArrays(n, arrays); }

        public static void GenVertexArrays(uint[] arrays)
        {
            fixed (uint* parrays = arrays)
            {
                glGenVertexArrays(arrays.Length, parrays);
            }
        }

        public static uint GenVertexArray()
        {
            uint value;
            glGenVertexArrays(1, &value);
            return value;
        }

        private delegate void glDeleteVertexArraysDelegate(int n, uint* arrays);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static readonly glDeleteVertexArraysDelegate glDeleteVertexArrays = null;

        public static void DeleteVertexArrays(int n, uint* arrays) { glDeleteVertexArrays(n, arrays); }

        public static void DeleteVertexArrays(uint[] arrays)
        {
            fixed (uint* parrays = arrays)
            {
                glDeleteVertexArrays(arrays.Length, parrays);
            }
        }

        public static void DeleteVertexArray(uint array)
        {
            glDeleteVertexArrays(1, &array);
        }

        private delegate void glBindVertexArrayDelegate(uint array);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static readonly glBindVertexArrayDelegate glBindVertexArray = null;

        public static void BindVertexArray(uint array) { glBindVertexArray(array); }

        private delegate bool glIsVertexArrayDelegate(uint array);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_vertex_array_object")]
        private static readonly glIsVertexArrayDelegate glIsVertexArray = null;

        public static bool IsVertexArray(uint array) { return glIsVertexArray(array); }

        public const UInt32 GL_VERTEX_ARRAY_BINDING = 0x85B5;
    }
}
