using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

                    vertices[0] = new StlModelVertex { Position = triangle.Vertex1, Normal = triangle.Normal };
                    vertices[1] = new StlModelVertex { Position = triangle.Vertex2, Normal = triangle.Normal };
                    vertices[2] = new StlModelVertex { Position = triangle.Vertex3, Normal = triangle.Normal };

                    vertexBuffer.SetRawData((IntPtr)vertices, 0, facetOffset, facetSizeInBytes, SetDataOptions.NoOverwrite);
                    facetOffset += facetSizeInBytes;
                }
            }

            var geometryStream = GeometryStream.Create();
            geometryStream.Attach(vertexBuffer);

            var globalTransform = 
                Matrix.CreateScale(stlMetadata.Scale) *
                Matrix.CreateRotationX(stlMetadata.RotationX) *
                Matrix.CreateRotationY(stlMetadata.RotationY) *
                Matrix.CreateRotationZ(stlMetadata.RotationZ);

            var modelMeshMaterial = new BasicMaterial() { DiffuseColor = stlMetadata.DiffuseColor };
            var modelMeshGeometry = new ModelMeshGeometry(PrimitiveType.TriangleList, geometryStream, vertexBuffer.VertexCount, 0, modelMeshMaterial);
            var modelMesh = new ModelMesh(null, new[] { modelMeshGeometry });
            var modelNode = new ModelNode(null, modelMesh, null, globalTransform);
            var modelScene = new ModelScene(input.Name, new[] { modelNode });
            var model = new Model(manager.Ultraviolet, new[] { modelScene });

            return model;
        }
    }
}
