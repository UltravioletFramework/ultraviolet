using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.Tests.Graphics
{
    partial class UltravioletGraphicsTests
    {
        /// <summary>
        /// Contains instance-specific vertex data used by tests which perform instanced rendering.
        /// </summary>
        private struct CanRenderInstancedTrianglesData : IVertexType
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CanRenderInstancedTrianglesData"/> structure.
            /// </summary>
            /// <param name="transform"></param>
            /// <param name="color"></param>
            public CanRenderInstancedTrianglesData(Matrix transform, Color color)
            {
                this.Transform = transform;
                this.Color = color;
            }

            /// <inheritdoc/>
            VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
            
            /// <summary>
            /// The vertex declaration.
            /// </summary>
            public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new[] {
                new VertexElement(sizeof(Single) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1),
                new VertexElement(sizeof(Single) * 4, VertexElementFormat.Vector4, VertexElementUsage.Position, 2),
                new VertexElement(sizeof(Single) * 8, VertexElementFormat.Vector4, VertexElementUsage.Position, 3),
                new VertexElement(sizeof(Single) * 12, VertexElementFormat.Vector4, VertexElementUsage.Position, 4),
                new VertexElement(sizeof(Single) * 16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            });

            /// <summary>
            /// The instance's transformation matrix.
            /// </summary>
            public Matrix Transform;

            /// <summary>
            /// The instance's color.
            /// </summary>
            public Color Color;
        }
    }
}
