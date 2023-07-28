using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Loads 3D surface assets.
    /// </summary>
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
            var isSingleFile = true;

            var filename = Path.GetFileNameWithoutExtension(metadata.AssetFileName);
            if (filename != null && filename.EndsWith("_0") && metadata.IsFile)
            {
                filename = filename.Substring(0, filename.Length - "_0".Length);
                isSingleFile = false;
            }

            return isSingleFile ? ProcessSingleFile(manager, metadata, input) :
                ProcessMultipleFiles(manager, metadata, input, filename);
        }

        /// <summary>
        /// Processes a single file which has all of the layers of the surface within it.
        /// </summary>
        private Surface3D ProcessSingleFile(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var mdat = metadata.As<SDL2Surface3DProcessorMetadata>();
            var srgbEncoded = mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForSurface3D;

            // Layers must be square. Validate our dimensions.
            var layerHeight = input.Height;
            var layerWidth = layerHeight;
            var layerCount = input.Width / layerWidth;
            if (input.Width % layerWidth != 0)
                throw new InvalidDataException(SDL2Strings.SurfaceMustHaveSquareLayers);

            // Create surfaces for each of our layers.
            using (var mainSurface = Surface2D.Create(input))
            {
                mainSurface.SrgbEncoded = srgbEncoded;

                var resultOpts = srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
                var result = new SDL2Surface3D(manager.Ultraviolet, layerWidth, layerHeight, layerCount, mainSurface.BytesPerPixel, resultOpts);
                for (int i = 0; i < layerCount; i++)
                {
                    var layerSurface = mainSurface.CreateSurface(new Rectangle(i * layerWidth, 0, layerWidth, layerHeight));
                    result.SetLayer(i, layerSurface, true);
                }
                return result;
            }
        }

        /// <summary>
        /// Processes a collection of files, each of which represents a separate layer of the surface.
        /// </summary>
        private Surface3D ProcessMultipleFiles(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input, String filename)
        {
            var mdat = metadata.As<SDL2Surface3DProcessorMetadata>();
            var srgbEncoded = mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForSurface3D;

            var layer0 = input.CreateCopy();
            var layers = new Dictionary<Int32, String>();
            var layerIndex = 1;

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

            var surfaceOpts = srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            var surface = new SDL2Surface3D(manager.Ultraviolet, layer0.Width, layer0.Height, 1 + layers.Count, layer0.BytesPerPixel, surfaceOpts);

            var surfaceLayer0 = Surface2D.Create(layer0);
            surfaceLayer0.SrgbEncoded = srgbEncoded;
            surface.SetLayer(0, surfaceLayer0, true);

            for (int i = 0; i < layers.Count; i++)
            {
                var layerAsset = layers[1 + i];
                metadata.AddAssetDependency(layerAsset);

                var layerSurface = manager.Load<Surface2D>(layerAsset, metadata.AssetDensity, false, metadata.IsLoadedFromSolution);
                layerSurface.SrgbEncoded = srgbEncoded;
                surface.SetLayer(1 + i, layerSurface, true);
            }

            return surface;
        }
    }
}
