using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
	{
		[MonoNativeFunctionWrapper]
		private delegate void glVertexAttribDivisorDelegate(uint index, uint divisor);
		[Require(MinVersion = "3.3")]
		private static glVertexAttribDivisorDelegate glVertexAttribDivisor = null;

		public static void VertexAttribDivisor(uint index, uint divisor) { glVertexAttribDivisor(index, divisor); }

        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
		public const UInt32 GL_RGB10_A2UI = 0x906F;
	}
}
