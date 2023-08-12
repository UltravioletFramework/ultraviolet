using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using sspack;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a content processor which loads texture atlases.
    /// </summary>
    /// <remarks>This class is based on code taken from the Sprite Sheet Packer library (see LICENSES.MORE.md).</remarks>
    internal sealed partial class TextureAtlasProcessor : ContentProcessor<TextureAtlasDescription, TextureAtlas>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, TextureAtlasDescription input, Boolean delete)
        {
            // Pack the texture atlas.
            var atlasImages = LoadAndSortImages(manager, metadata, input);

            var outputWidth = 0;
            var outputHeight = 0;
            var outputPlacement = PackImageRectangles(input, atlasImages, out outputWidth, out outputHeight);

            if (outputPlacement == null)
                throw new InvalidOperationException(UltravioletStrings.FailedToPackTextureAtlas);

            if (outputWidth == 0 || outputHeight == 0)
                throw new InvalidOperationException(UltravioletStrings.TextureAtlasContainsNoImages);

            // Write out the texture as a PNG file.
            using (var outputSurface = CreateOutputSurface(input, atlasImages, manager, metadata, outputWidth, outputHeight, outputPlacement))
            {
                using (var surfaceStream = new MemoryStream())
                {
                    outputSurface.SaveAsPng(surfaceStream);
                    var surfaceBytes = surfaceStream.ToArray();
                    writer.Write(surfaceBytes.Length);
                    writer.Write(surfaceBytes);
                }
            }

            // Write out the atlas' cells.
            writer.Write(outputPlacement.Count);
            foreach (var cell in outputPlacement)
            {
                writer.Write(cell.Key);
                writer.Write(cell.Value.X);
                writer.Write(cell.Value.Y);
                writer.Write(cell.Value.Width - input.Metadata.Padding);
                writer.Write(cell.Value.Height - input.Metadata.Padding);
            }
        }

        /// <inheritdoc/>
        public override TextureAtlas ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            // Load the atlas' texture.
            var texture = default(Texture2D);
            var textureSizeInBytes = reader.ReadInt32();
            var textureBytes = reader.ReadBytes(textureSizeInBytes);
            using (var textureStream = new MemoryStream(textureBytes))
            {
                texture = manager.LoadFromStream<Texture2D>(textureStream, "png", metadata.AssetDensity);
            }

            // Read the atlas' cells.
            var cells = new Dictionary<String, Rectangle>();
            var cellCount = reader.ReadInt32();
            for (int i = 0; i < cellCount; i++)
            {
                var cellName = reader.ReadString();
                var cellX = reader.ReadInt32();
                var cellY = reader.ReadInt32();
                var cellWidth = reader.ReadInt32();
                var cellHeight = reader.ReadInt32();
                cells[cellName] = new Rectangle(cellX, cellY, cellWidth, cellHeight);
            }

            // Create the atlas.
            return new TextureAtlas(texture, cells);
        }

        /// <inheritdoc/>
        public override TextureAtlas Process(ContentManager manager, IContentProcessorMetadata metadata, TextureAtlasDescription input)
        {
            var atlasImages = LoadAndSortImages(manager, metadata, input);

            var outputWidth = 0;
            var outputHeight = 0;

            var outputPlacement = PackImageRectangles(input, atlasImages, out outputWidth, out outputHeight);
            if (outputPlacement == null)
                throw new InvalidOperationException(UltravioletStrings.FailedToPackTextureAtlas);

            return CreateTextureAtlas(input, atlasImages, manager, metadata, outputWidth, outputHeight, outputPlacement);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

        /// <summary>
        /// Gets the size of the specified image.
        /// </summary>
        private static Size2 GetImageSize(ContentManager content, IContentProcessorMetadata metadata, String path)
        {
            using (var image = content.Load<Surface2D>(path, metadata.AssetDensity, false, metadata.IsLoadedFromSolution))
                return new Size2(image.Width, image.Height);
        }

        /// <summary>
        /// Loads the images specified by an atlas description and sorts them according to size.
        /// </summary>
        private static IEnumerable<TextureAtlasImage> LoadAndSortImages(ContentManager content, IContentProcessorMetadata metadata, TextureAtlasDescription atlasDesc)
        {
            var images = LoadImages(content, metadata, atlasDesc);
            if (images == null)
                throw new InvalidOperationException(UltravioletStrings.TextureAtlasContainsNoImages);

            return SortImages(images);
        }

        /// <summary>
        /// Loads the images specified by an atlas description.
        /// </summary>
        private static IEnumerable<TextureAtlasImage> LoadImages(ContentManager content, IContentProcessorMetadata metadata, TextureAtlasDescription atlasDesc)
        {
            var result = new Dictionary<String, TextureAtlasImage>();

            if (atlasDesc.Images != null)
            {
                foreach (var imageDesc in atlasDesc.Images)
                {
                    var path = imageDesc.Path;
                    if (String.IsNullOrEmpty(path) || path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                        throw new InvalidDataException(UltravioletStrings.InvalidTextureAtlasImagePath);

                    var name = imageDesc.Name;

                    if (path.Contains("*"))
                    {
                        if (name != null)
                            throw new InvalidDataException(UltravioletStrings.TextureAtlasWildcardsCannotBeNamed);

                        var files = ExpandFileExpression(content, ResolveDependencyAssetPath(metadata, Path.Combine(atlasDesc.Metadata.RootDirectory, path)));
                        foreach (var file in files)
                        {
                            var nameFile = Path.GetFileNameWithoutExtension(file);
                            var nameRoot = String.IsNullOrEmpty(Path.GetDirectoryName(path)) ? null : Path.GetDirectoryName(file)
                                .Replace(Path.DirectorySeparatorChar, '\\')
                                .Replace(Path.AltDirectorySeparatorChar, '\\');

                            name = String.IsNullOrEmpty(nameRoot) ? nameFile : nameRoot + "\\" + nameFile;

                            if (atlasDesc.Metadata.FlattenCellName)
                                name = FlattenCellName(name);

                            if (result.ContainsKey(name))
                                throw new InvalidOperationException(UltravioletStrings.TextureAtlasAlreadyContainsCell.Format(name));

                            metadata.AddAssetDependency(file);

                            var size = GetImageSize(content, metadata, file);
                            result[name] = new TextureAtlasImage(name, file, size);
                        }
                    }
                    else
                    {
                        name = name ?? path;
                        path = ResolveDependencyAssetPath(metadata, Path.Combine(atlasDesc.Metadata.RootDirectory, path));

                        if (atlasDesc.Metadata.FlattenCellName)
                            name = FlattenCellName(name);

                        if (result.ContainsKey(name))
                            throw new InvalidOperationException(UltravioletStrings.TextureAtlasAlreadyContainsCell.Format(name));

                        metadata.AddAssetDependency(path);

                        var size = GetImageSize(content, metadata, path);
                        result[name] = new TextureAtlasImage(name, path, size);
                    }
                }
            }

            return result.Any() ? result.Values.ToList() : null;
        }

        /// <summary>
        /// Sorts the specified list of images according to their size.
        /// </summary>
        private static IEnumerable<TextureAtlasImage> SortImages(IEnumerable<TextureAtlasImage> images)
        {
            return images.OrderBy(x => x, new FunctorComparer<TextureAtlasImage>((f1, f2) =>
            {
                var b1 = f1.Size;
                var b2 = f2.Size;

                var c = -b1.Width.CompareTo(b2.Width);
                if (c != 0)
                    return c;

                c = -b1.Height.CompareTo(b2.Height);
                if (c != 0)
                    return c;

                return f1.Path.CompareTo(f2.Path);
            })).ToList();
        }

        /// <summary>
        /// Expands a file expression potentially containing wildcard (*) characters.
        /// </summary>
        private static IEnumerable<String> ExpandFileExpression(ContentManager manager, String expression)
        {
            var root = Path.GetDirectoryName(expression);
            var filter = Path.GetFileName(expression);

            var directories = new List<String>();
            ExpandDirectoryExpression(manager, String.Empty, root, directories);

            var files = new List<String>();
            foreach (var directory in directories)
            {
                files.AddRange(manager.GetAssetsInDirectory(directory, filter));
            }
            return files;
        }

        /// <summary>
        /// Expands a directory expression potentially containing wildcard (*) characters.
        /// </summary>
        private static void ExpandDirectoryExpression(ContentManager manager, String root, String expression, List<String> result)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                result.Add(root);
            }
            else
            {
                var parts = expression.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var newexp = String.Join(Path.DirectorySeparatorChar.ToString(), parts, 1, parts.Length - 1);
                var matches = manager.GetSubdirectories(root, parts[0]);
                foreach (var match in matches)
                {
                    ExpandDirectoryExpression(manager, match, newexp, result);
                }
            }
        }
        
        /// <summary>
        /// This method does some trickery type stuff where we perform the TestPackingImages method over and over,
        /// trying to reduce the image size until we have found the smallest possible image we can fit.
        /// </summary>
        private static Dictionary<String, Rectangle> PackImageRectangles(TextureAtlasDescription atlasDesc, IEnumerable<TextureAtlasImage> atlasImages,
            out Int32 outputWidth, out Int32 outputHeight)
        {
            // create a dictionary for our test image placements
            var testImagePlacement = new Dictionary<String, Rectangle>();
            var imagePlacement = new Dictionary<String, Rectangle>();

            // get the size of our smallest image
            var smallestWidth = Int32.MaxValue;
            var smallestHeight = Int32.MaxValue;
            foreach (var image in atlasImages)
            {
                smallestWidth = Math.Min(smallestWidth, image.Size.Width);
                smallestHeight = Math.Min(smallestHeight, image.Size.Height);
            }

            // we need a couple values for testing
            outputWidth = atlasDesc.Metadata.MaximumWidth;
            outputHeight = atlasDesc.Metadata.MaximumHeight;

            var testWidth = atlasDesc.Metadata.MaximumWidth;
            var testHeight = atlasDesc.Metadata.MaximumHeight;

            var shrinkVertical = false;

            // just keep looping...
            while (true)
            {
                // make sure our test dictionary is empty
                testImagePlacement.Clear();

                // try to pack the images into our current test size
                if (!TestPackingImages(atlasDesc, atlasImages, testWidth, testHeight, testImagePlacement))
                {
                    // if that failed...

                    // if we have no images in imagePlacement, i.e. we've never succeeded at PackImages,
                    // show an error and return false since there is no way to fit the images into our
                    // maximum size texture
                    if (imagePlacement.Count == 0)
                        return null;

                    // otherwise return true to use our last good results
                    if (shrinkVertical)
                        return imagePlacement;

                    var padding = atlasDesc.Metadata.Padding;

                    shrinkVertical = true;
                    testWidth += smallestWidth + padding + padding;
                    testHeight += smallestHeight + padding + padding;
                    continue;
                }

                // clear the imagePlacement dictionary and add our test results in
                imagePlacement.Clear();
                foreach (var pair in testImagePlacement)
                    imagePlacement.Add(pair.Key, pair.Value);

                // figure out the smallest bitmap that will hold all the images
                testWidth = testHeight = 0;
                foreach (var pair in imagePlacement)
                {
                    testWidth = Math.Max(testWidth, pair.Value.Right);
                    testHeight = Math.Max(testHeight, pair.Value.Bottom);
                }

                // subtract the extra padding on the right and bottom
                if (!shrinkVertical)
                {
                    testWidth -= atlasDesc.Metadata.Padding;
                }
                testHeight -= atlasDesc.Metadata.Padding;

                // if we require a power of two texture, find the next power of two that can fit this image
                if (atlasDesc.Metadata.RequirePowerOfTwo)
                {
                    testWidth  = MathUtil.FindNextPowerOfTwo(testWidth);
                    testHeight = MathUtil.FindNextPowerOfTwo(testHeight);
                }

                // if we require a square texture, set the width and height to the larger of the two
                if (atlasDesc.Metadata.RequireSquare)
                {
                    var max = Math.Max(testWidth, testHeight);
                    testWidth = testHeight = max;
                }

                // if the test results are the same as our last output results, we've reached an optimal size,
                // so we can just be done
                if (testWidth == outputWidth && testHeight == outputHeight)
                {
                    if (shrinkVertical)
                        return imagePlacement;

                    shrinkVertical = true;
                }

                // save the test results as our last known good results
                outputWidth = testWidth;
                outputHeight = testHeight;

                // subtract the smallest image size out for the next test iteration
                if (!shrinkVertical)
                {
                    testWidth -= smallestWidth;
                }
                testHeight -= smallestHeight;
            }
        }

        /// <summary>
        /// Determines whether the atlas' images can fit on a texture of the specified size.
        /// </summary>
        private static Boolean TestPackingImages(TextureAtlasDescription atlasDesc, IEnumerable<TextureAtlasImage> atlasImages,
            Int32 testWidth, Int32 testHeight, Dictionary<String, Rectangle> testImagePlacement)
        {
            var rectanglePacker = new ArevaloRectanglePacker(testWidth, testHeight);
            foreach (var image in atlasImages)
            {
                var size = image.Size;
                var origin = Vector2.Zero;
                var padding = atlasDesc.Metadata.Padding;
                if (!rectanglePacker.TryPack(size.Width + padding, size.Height + padding, out origin))
                {
                    return false;
                }
                testImagePlacement.Add(image.Name, new Rectangle((int)origin.X, (int)origin.Y, 
                    size.Width + padding, size.Height + padding));
            }
            return true;
        }

        /// <summary>
        /// Creates the output surface for a texture atlas.
        /// </summary>
        private static Surface2D CreateOutputSurface(TextureAtlasDescription atlasDesc, IEnumerable<TextureAtlasImage> atlasImages,
            ContentManager content, IContentProcessorMetadata metadata, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            var output = Surface2D.Create(width, height);
            
            foreach (var image in atlasImages)
            {
                var imageArea = images[image.Name];
                using (var imageSurface = content.Load<Surface2D>(image.Path, metadata.AssetDensity, false, metadata.IsLoadedFromSolution))
                {
                    var areaWithoutPadding = new Rectangle(
                        imageArea.X, 
                        imageArea.Y, 
                        imageArea.Width - atlasDesc.Metadata.Padding, 
                        imageArea.Height - atlasDesc.Metadata.Padding);
                    imageSurface.Blit(output, areaWithoutPadding);
                }
            }

            return output;
        }

        /// <summary>
        /// Creates the output texture for a texture atlas.
        /// </summary>
        private static Texture2D CreateOutputTexture(TextureAtlasDescription atlasDesc, IEnumerable<TextureAtlasImage> atlasImages,
            ContentManager content, IContentProcessorMetadata metadata, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            using (var output = CreateOutputSurface(atlasDesc, atlasImages, content, metadata, width, height, images))
            {
                var flipdir = content.Ultraviolet.GetGraphics().Capabilities.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None;
                output.FlipAndProcessAlpha(flipdir, true, Color.Magenta);
                return Texture2D.CreateTextureFromSurface(output, unprocessed: true);
            }
        }

        /// <summary>
        /// Creates a texture atlas from the specified collection of images.
        /// </summary>
        private static TextureAtlas CreateTextureAtlas(TextureAtlasDescription atlasDesc, IEnumerable<TextureAtlasImage> atlasImages,
            ContentManager content, IContentProcessorMetadata metadata, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            var padding = atlasDesc.Metadata.Padding;
            var texture = CreateOutputTexture(atlasDesc, atlasImages, content, metadata, width, height, images);
            var atlas = new TextureAtlas(texture, images.ToDictionary(x => x.Key, 
                x => new Rectangle(x.Value.X, x.Value.Y, x.Value.Width - padding, x.Value.Height - padding)));
            return atlas;
        }

        /// <summary>
        /// Flattens an image name by ignoring directory information.
        /// </summary>
        private static String FlattenCellName(String name)
        {
            var ixSeparator = Math.Max(name.LastIndexOf('/'), name.LastIndexOf('\\'));
            if (ixSeparator >= 0)
            {
                return name.Substring(ixSeparator + 1);
            }
            return name;
        }
    }
}
