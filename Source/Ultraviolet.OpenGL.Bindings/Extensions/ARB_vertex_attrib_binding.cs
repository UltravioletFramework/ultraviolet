using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glBindVertexBufferDelegate(uint bindingindex, uint buffer, IntPtr offset, int stride);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glBindVertexBufferDelegate glBindVertexBuffer = null;

        public static void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, int stride)
        {
            glBindVertexBuffer(bindingindex, buffer, offset, stride);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribFormatDelegate(uint attribindex, int size, uint type, [MarshalAs(UnmanagedType.I1)] bool normalized, uint relativeoffset);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glVertexAttribFormatDelegate glVertexAttribFormat = null;

        public static void VertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset)
        {
            glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribIFormatDelegate(uint attribindex, int size, uint type, uint relativeoffset);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glVertexAttribIFormatDelegate glVertexAttribIFormat = null;

        public static void VertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            glVertexAttribIFormat(attribindex, size, type, relativeoffset);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribLFormatDelegate(uint attribindex, int size, uint type, uint relativeoffset);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glVertexAttribLFormatDelegate glVertexAttribLFormat = null;

        public static void VertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            glVertexAttribLFormat(attribindex, size, type, relativeoffset);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribBindingDelegate(uint attribindex, uint bindingindex);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glVertexAttribBindingDelegate glVertexAttribBinding = null;

        public static void VertexAttribBinding(uint attribindex, uint bindingindex)
        {
            glVertexAttribBinding(attribindex, bindingindex);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexBindingDivisorDelegate(uint bindingindex, uint divisor);
        [Require(MinVersion = "4.3", MinVersionES = "3.1")]
        [Require(Extension = "GL_ARB_vertex_attrib_binding")]
        private static glVertexBindingDivisorDelegate glVertexBindingDivisor = null;

        public static void VertexBindingDivisor(uint bindingindex, uint divisor)
        {
            glVertexBindingDivisor(bindingindex, divisor);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayVertexBufferDelegate(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayBindVertexBufferEXT")]
        private static glVertexArrayVertexBufferDelegate glVertexArrayVertexBuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayAttribFormatDelegate(uint vaobj, uint attribindex, int size, uint type, [MarshalAs(UnmanagedType.I1)] bool normalized, uint relativeoffset);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayVertexAttribFormatEXT")]
        private static glVertexArrayAttribFormatDelegate glVertexArrayAttribFormat = null;

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayAttribIFormatDelegate(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayVertexAttribIFormatEXT")]
        private static glVertexArrayAttribIFormatDelegate glVertexArrayAttribIFormat = null;

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayAttribLFormatDelegate(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayVertexAttribLFormatEXT")]
        private static glVertexArrayAttribLFormatDelegate glVertexArrayAttribLFormat = null;

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayAttribBindingDelegate(uint vaobj, uint attribindex, uint bindingindex);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayVertexAttribBindingEXT")]
        private static glVertexArrayAttribBindingDelegate glVertexArrayAttribBinding = null;

        [MonoNativeFunctionWrapper]
        private delegate void glVertexArrayBindingDivisorDelegate(uint vaobj, uint bindingindex, uint divisor);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access && GL_ARB_vertex_attrib_binding", ExtensionFunction = "glVertexArrayVertexBindingDivisorEXT")]
        private static glVertexArrayBindingDivisorDelegate glVertexArrayBindingDivisor = null;
    }
}
