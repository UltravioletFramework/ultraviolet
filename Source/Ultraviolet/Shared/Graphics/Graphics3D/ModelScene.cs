using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a logically related collection of meshes within a model.
    /// </summary>
    public class ModelScene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelScene"/> class.
        /// </summary>
        /// <param name="name">The scene's name.</param>
        /// <param name="nodes">The scene's list of nodes.</param>
        public ModelScene(String name, IList<ModelNode> nodes = null)
        {
            this.Name = name;
            this.Nodes = new ModelNodeCollection(nodes);
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="worldMatrix">The scene's world matrix.</param>
        public void Draw(Camera camera, ref Matrix worldMatrix)
        {
            Contract.Require(camera, nameof(camera));

            void DrawNode(ModelNode node, Matrix transform)
            {
                transform = Matrix.Multiply(node.Transform, transform);

                if (node.Mesh != null)
                {
                    foreach (var geometry in node.Mesh.Geometries)
                    {
                        geometry.Material.Apply();
                        foreach (var pass in geometry.Material.Effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();

                            var gfx = geometry.GeometryStream.Ultraviolet.GetGraphics();
                            gfx.SetGeometryStream(geometry.GeometryStream);

                            if (geometry.GeometryStream.HasIndices)
                            {
                                gfx.DrawIndexedPrimitives(geometry.PrimitiveType, 0, geometry.PrimitiveCount);
                            }
                            else
                            {
                                gfx.DrawPrimitives(geometry.PrimitiveType, 0, geometry.PrimitiveCount);
                            }
                        }
                    }
                }

                foreach (var child in node.Children)
                    DrawNode(child, transform);
            }

            foreach (var node in Nodes)
                DrawNode(node, worldMatrix);
        }

        /// <summary>
        /// Gets the scene's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the scene's collection of nodes.
        /// </summary>
        public ModelNodeCollection Nodes { get; }
    }
}