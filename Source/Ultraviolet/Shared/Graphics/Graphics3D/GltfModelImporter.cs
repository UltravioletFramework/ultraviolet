using System;
using System.IO;
using SharpGLTF.Schema2;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a content importer which loads GLB model files.
    /// </summary>
    [ContentImporter(".gltf"), CLSCompliant(false)]
    public class GltfModelImporter : ContentImporter<ModelRoot>
    {
        /// <inheritdoc/>
        public override ModelRoot Import(IContentImporterMetadata metadata, Stream stream)
        {
            if (!metadata.IsFile)
                throw new NotSupportedException();

            return ModelRoot.Load(metadata.AssetFilePath);
        }
    }
}
