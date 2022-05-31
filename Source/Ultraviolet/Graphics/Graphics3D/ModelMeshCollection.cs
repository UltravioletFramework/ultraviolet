using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of <see cref="ModelMesh"/> instances.
    /// </summary>
    public sealed class ModelMeshCollection : UltravioletCollection<ModelMesh>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMeshCollection"/> class.
        /// </summary>
        /// <param name="meshes">The initial list of <see cref="ModelMesh"/> objects with which to populate this collection.</param>
        public ModelMeshCollection(IList<ModelMesh> meshes = null)
            : base(meshes?.Count ?? 0)
        {
            if (meshes != null)
            {
                this.AddRangeInternal(meshes);
            }
        }
    }
}