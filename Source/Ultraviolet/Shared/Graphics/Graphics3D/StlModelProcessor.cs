using System;
using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Loads STL model assets.
    /// </summary>
    [ContentProcessor]
    public class StlModelProcessor : ContentProcessor<StlModelDescription, Model>
    {
        /// <inheritdoc/>
        public override unsafe Model Process(ContentManager manager, IContentProcessorMetadata metadata, StlModelDescription input)
        {
            var stlMetadata = metadata.As<StlModelProcessorMetadata>();

            var vertexBuffer = VertexBuffer.Create<StlModelVertex>((input.Triangles?.Count ?? 0) * 3);
            if (input.Triangles != null)
            {
                var facetOffset = 0;
                var facetSizeInBytes = sizeof(StlModelVertex) * 3;

                var vertices = stackalloc StlModelVertex[3];
                for (var i = 0; i < input.Triangles.Count; i++)
                {
                    var triangle = input.Triangles[i];
                    if (triangle == null)
                        throw new InvalidDataException(UltravioletStrings.MalformedContentFile);

                    if (stlMetadata.SwapWindingOrder)
                    {
                        vertices[0] = new StlModelVertex { Position = triangle.Vertex3, Normal = triangle.Normal };
                        vertices[1] = new StlModelVertex { Position = triangle.Vertex2, Normal = triangle.Normal };
                        vertices[2] = new StlModelVertex { Position = triangle.Vertex1, Normal = triangle.Normal };
                    }
                    else
                    {
                        vertices[0] = new StlModelVertex { Position = triangle.Vertex1, Normal = triangle.Normal };
                        vertices[1] = new StlModelVertex { Position = triangle.Vertex2, Normal = triangle.Normal };
                        vertices[2] = new StlModelVertex { Position = triangle.Vertex3, Normal = triangle.Normal };
                    }

                    vertexBuffer.SetRawData((IntPtr)vertices, 0, facetOffset, facetSizeInBytes, SetDataOptions.NoOverwrite);
                    facetOffset += facetSizeInBytes;
                }
            }

            var geometryStream = GeometryStream.Create();
            geometryStream.Attach(vertexBuffer);

            var globalTransform = Matrix.Identity;
            if (stlMetadata.RotationX != 0.0f || stlMetadata.RotationY != 0.0f || stlMetadata.RotationZ != 0.0f || stlMetadata.Scale != 1.0f)
            {
                Matrix.CreateRotationX(stlMetadata.RotationX, out var rotX);
                Matrix.CreateRotationY(stlMetadata.RotationY, out var rotY);
                Matrix.CreateRotationZ(stlMetadata.RotationZ, out var rotZ);
                Matrix.CreateScale(stlMetadata.Scale, out var scale);

                Matrix.Multiply(ref globalTransform, ref rotZ, out globalTransform);
                Matrix.Multiply(ref globalTransform, ref rotX, out globalTransform);
                Matrix.Multiply(ref globalTransform, ref rotY, out globalTransform);
                Matrix.Multiply(ref globalTransform, ref scale, out globalTransform);
            }

            var modelMeshMaterial = stlMetadata.DefaultMaterial ?? new BasicMaterial() { DiffuseColor = Color.White };
            var modelMeshGeometry = new ModelMeshGeometry(PrimitiveType.TriangleList, geometryStream, vertexBuffer.VertexCount, 0, modelMeshMaterial);
            var modelMesh = new ModelMesh(0, null, new[] { modelMeshGeometry });
            var modelNode = new ModelNode(0, null, modelMesh, null, globalTransform);
            var modelScene = new ModelScene(0, input.Name, new[] { modelNode });
            var model = new Model(manager.Ultraviolet, new[] { modelScene });

            return model;
        }
    }
}
