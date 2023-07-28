using System;
using System.Collections.Generic;
using Ultraviolet.Core;

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
        /// <param name="logicalIndex">The logical index of the mesh within its parent model.</param>
        /// <param name="name">The mesh's name.</param>
        /// <param name="geometries">The mesh's list of geometries.</param>
        public ModelMesh(Int32 logicalIndex, String name, IList<ModelMeshGeometry> geometries)
        {
            this.LogicalIndex = logicalIndex;
            this.Name = name;
            this.Geometries = new ModelMeshGeometryCollection(geometries);
        }

        /// <summary>
        /// Gets the logical index of the mesh within its parent model.
        /// </summary>
        public Int32 LogicalIndex { get; }

        /// <summary>
        /// Gets the mesh's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the <see cref="Model"/> that contains this mesh.
        /// </summary>
        public Model ParentModel { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelScene"/> that contains this mesh.
        /// </summary>
        public ModelScene ParentModelScene { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelNode"/> that contains this mesh.
        /// </summary>
        public ModelNode ParentModelNode { get; private set; }

        /// <summary>
        /// Gets the mesh's collection of geometries.
        /// </summary>
        public ModelMeshGeometryCollection Geometries { get; }

        /// <summary>
        /// Sets the mesh's parent model.
        /// </summary>
        /// <param name="parent">The mesh's parent model.</param>
        internal void SetParentModel(Model parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModel != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModel = parent;
        }

        /// <summary>
        /// Sets the mesh's parent scene.
        /// </summary>
        /// <param name="parent">The mesh's parent scene.</param>
        internal void SetParentModelScene(ModelScene parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModelScene != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModelScene = parent;
        }

        /// <summary>
        /// Sets the mesh's parent node.
        /// </summary>
        /// <param name="parent">The mesh's parent node.</param>
        internal void SetParentModelNode(ModelNode parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModelNode != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModelNode = parent;
        }
    }
}