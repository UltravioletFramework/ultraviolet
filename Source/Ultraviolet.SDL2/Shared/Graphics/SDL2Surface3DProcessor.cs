using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Loads 3D surface assets.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class SDL2Surface3DProcessor : ContentProcessor<PlatformNativeSurface, Surface3D>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Surface3D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var layer0 = input.CreateCopy();
            var layers = new Dictionary<Int32, String>();
            var layerIndex = 1;

            var filename = Path.GetFileNameWithoutExtension(metadata.AssetFileName);
            if (filename != null && filename.EndsWith("_0"))
                filename = filename.Substring(0, filename.Length - "_0".Length);

            var extension = Path.GetExtension(metadata.AssetFileName);
            var directory = (metadata.AssetPath == null) ? String.Empty : Path.GetDirectoryName(metadata.AssetPath);
            if (!String.IsNullOrEmpty(directory))
            {
                var assetsInDirectory = manager.GetAssetFilePathsInDirectory(directory);
                while (true)
                {
                    var layerAsset = assetsInDirectory.Where(x => String.Equals(Path.GetFileName(x), 
                        $"{filename}_{layerIndex}{extension}", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (layerAsset == null)
                        break;

                    layerAsset = ResolveDependencyAssetPath(metadata, Path.GetFileName(layerAsset));

                    layers[layerIndex] = layerAsset;
                    layerIndex++;
                }
            }

            var surface = new SDL2Surface3D(manager.Ultraviolet, layer0.Width, layer0.Height, 1 + layers.Count, layer0.BytesPerPixel);
            for (int i = 0; i < layers.Count; i++)
            {
                var layerAsset = layers[1 + i];
                metadata.AddAssetDependency(layerAsset);

                var layerSurface = manager.Load<Surface2D>(layerAsset, metadata.AssetDensity, true, metadata.IsLoadedFromSolution);
                surface.SetLayer(1 + i, layerSurface);
            }

            return surface;
        }
    }
}
