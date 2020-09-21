using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of <see cref="ModelMeshGeometry"/> instances.
    /// </summary>
    public sealed class ModelMeshGeometryCollection : ModelResourceCollection<ModelMeshGeometry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMeshGeometryCollection"/> class.
        /// </summary>
        /// <param name="geometries">The initial list of <see cref="ModelMeshGeometry"/> objects with which to populate this collection.</param>
        public ModelMeshGeometryCollection(IList<ModelMeshGeometry> geometries = null)
            : base(geometries)
        { }
    }
}