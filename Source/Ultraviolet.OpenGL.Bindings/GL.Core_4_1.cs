using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribLPointerDelegate(uint index, int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "4.1")]
        private static glVertexAttribLPointerDelegate glVertexAttribLPointer = null;

        public static void VertexAttribLPointer(uint index, int size, uint type, int stride, void* pointer) { glVertexAttribLPointer(index, size, type, stride, (IntPtr)pointer); }        
    }
}
