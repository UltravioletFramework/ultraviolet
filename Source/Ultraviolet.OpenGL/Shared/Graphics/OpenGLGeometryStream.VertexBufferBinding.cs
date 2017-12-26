using System;

namespace Ultraviolet.OpenGL.Graphics
{
    partial class OpenGLGeometryStream
    {
        /// <summary>
        /// Represents a vertex buffer which has been bound to a geometry stream.
        /// </summary>
        private struct VertexBufferBinding
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="VertexBufferBinding"/> structure.
            /// </summary>
            /// <param name="vbuffer">The vertex buffer which is bound to the geometry stream.</param>
            /// <param name="instanceFrequency">The number of instances which are drawn for each
            /// set of data in the vertex buffer.</param>
            public VertexBufferBinding(OpenGLVertexBuffer vbuffer, Int32 instanceFrequency)
            {
                this.VertexBuffer = vbuffer;
                this.InstanceFrequency = instanceFrequency;
            }

            /// <summary>
            /// Gets the vertex buffer which is bound to the geometry stream.
            /// </summary>
            public OpenGLVertexBuffer VertexBuffer { get; }

            /// <summary>
            /// Gets the number of instances which are drawn for each set of data
            /// in the vertex buffer.
            /// </summary>
            public Int32 InstanceFrequency { get; }
        }
    }
}
