
using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of 3D primitives and associated rendering state.
    /// </summary>
    public sealed class ModelMesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMesh"/> class.
        /// </summary>
        /// <param name="name">The mesh's name.</param>
        /// <param name="geometries">The mesh's list of geometries.</param>
        public ModelMesh(String name, IList<ModelMeshGeometry> geometries)
        {
            this.Name = name;
            this.Geometries = new ModelMeshGeometryCollection(geometries);
        }

        /// <summary>
        /// Gets the mesh's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the mesh's collection of geometries.
        /// </summary>
        public ModelMeshGeometryCollection Geometries { get; }
    }
}