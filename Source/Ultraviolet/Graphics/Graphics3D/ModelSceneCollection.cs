using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of <see cref="ModelScene"/> objects.
    /// </summary>
    public sealed class ModelSceneCollection : ModelResourceCollection<ModelScene>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSceneCollection"/> class.
        /// </summary>
        /// <param name="scenes">The initial list of <see cref="ModelScene"/> objects with which to populate this collection.</param>
        public ModelSceneCollection(IList<ModelScene> scenes = null)
            : base(scenes)
        {
            if (this.Count > 0)
                this.DefaultScene = this[0];
        }

        /// <summary>
        /// Changes the model's default scene.
        /// </summary>
        /// <param name="index">The index of the default scene within this collection.</param>
        public void ChangeDefaultScene(Int32? index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            DefaultScene = (index == null) ? null : this[index.Value];
        }

        /// <summary>
        /// Gets the model's default scene.
        /// </summary>
        public ModelScene DefaultScene { get; private set; }
    }
}