using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using sspack;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a content processor which loads XML definition files as texture atlases.
    /// </summary>
    /// <remarks>This class is based on code taken from the Sprite Sheet Packer library (see TwistedLogik.Ultraviolet.Licenses.txt).</remarks>
    [ContentProcessor]
    public sealed partial class TextureAtlasProcessor : ContentProcessor<XDocument, TextureAtlas>
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="obj">The asset to export to the stream.</param>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument obj)
        {
            // Pack the texture atlas.
            var definition = new TextureAtlasDefinition(obj, manager, metadata.AssetPath);

            var outputWidth = 0;
            var outputHeight = 0;
            var outputPlacement = PackImageRectangles(definition, out outputWidth, out outputHeight);

            if (outputPlacement == null)
                throw new InvalidOperationException(UltravioletStrings.FailedToPackTextureAtlas);

            if (outputWidth == 0 || outputHeight == 0)
                throw new InvalidOperationException(UltravioletStrings.TextureAtlasContainsNoImages);

            // Write out the texture as a PNG file.
            using (var outputSurface = CreateOutputSurface(definition, manager, outputWidth, outputHeight, outputPlacement))
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
                writer.Write(cell.Value.Width - definition.Padding);
                writer.Write(cell.Value.Height - definition.Padding);
            }
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        public override TextureAtlas ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            // Load the atlas' texture.
            var texture = default(Texture2D);
            var textureSizeInBytes = reader.ReadInt32();
            var textureBytes = reader.ReadBytes(textureSizeInBytes);
            using (var textureStream = new MemoryStream(textureBytes))
            {
                texture = manager.LoadFromStream<Texture2D>(textureStream, "png");
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

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override TextureAtlas Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var definition = new TextureAtlasDefinition(input, manager, metadata.AssetPath);

            var outputWidth = 0;
            var outputHeight = 0;

            var outputPlacement = PackImageRectangles(definition, out outputWidth, out outputHeight);
            if (outputPlacement == null)
                throw new InvalidOperationException(UltravioletStrings.FailedToPackTextureAtlas);

            return CreateTextureAtlas(definition, manager, outputWidth, outputHeight, outputPlacement);
        }

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }

        /// <summary>
        /// Finds the next power of two that is higher than the specified value.
        /// </summary>
        /// <param name="k">The value to evaluate.</param>
        /// <returns>The next power of two that is higher than the specified value.</returns>
        private static Int32 FindNextPowerOfTwo(Int32 k)
        {
            k--;
            for (int i = 1; i < sizeof(int) * 8; i <<= 1)
            {
                k = k | k >> i;
            }
            return k + 1;
        }

        /// <summary>
        /// This method does some trickery type stuff where we perform the TestPackingImages method over and over,
        /// trying to reduce the image size until we have found the smallest possible image we can fit.
        /// </summary>
        private Dictionary<String, Rectangle> PackImageRectangles(TextureAtlasDefinition definition, out Int32 outputWidth, out Int32 outputHeight)
        {
            // create a dictionary for our test image placements
            var testImagePlacement = new Dictionary<String, Rectangle>();
            var imagePlacement = new Dictionary<String, Rectangle>();

            // get the size of our smallest image
            var smallestWidth = Int32.MaxValue;
            var smallestHeight = Int32.MaxValue;
            foreach (var image in definition.ImageList)
            {
                smallestWidth = Math.Min(smallestWidth, image.Size.Width);
                smallestHeight = Math.Min(smallestHeight, image.Size.Height);
            }

            // we need a couple values for testing
            outputWidth = definition.MaximumWidth;
            outputHeight = definition.MaximumHeight;

            var testWidth = definition.MaximumWidth;
            var testHeight = definition.MaximumHeight;

            var shrinkVertical = false;

            // just keep looping...
            while (true)
            {
                // make sure our test dictionary is empty
                testImagePlacement.Clear();

                // try to pack the images into our current test size
                if (!TestPackingImages(definition, testWidth, testHeight, testImagePlacement))
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

                    shrinkVertical = true;
                    testWidth += smallestWidth + definition.Padding + definition.Padding;
                    testHeight += smallestHeight + definition.Padding + definition.Padding;
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
                    testWidth -= definition.Padding;
                }
                testHeight -= definition.Padding;

                // if we require a power of two texture, find the next power of two that can fit this image
                if (definition.RequirePowerOfTwo)
                {
                    testWidth = FindNextPowerOfTwo(testWidth);
                    testHeight = FindNextPowerOfTwo(testHeight);
                }

                // if we require a square texture, set the width and height to the larger of the two
                if (definition.RequireSquare)
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
        private Boolean TestPackingImages(TextureAtlasDefinition definition, Int32 testWidth, Int32 testHeight, Dictionary<String, Rectangle> testImagePlacement)
        {
            var rectanglePacker = new ArevaloRectanglePacker(testWidth, testHeight);
            foreach (var image in definition.ImageList)
            {
                var size = image.Size;
                var origin = Vector2.Zero;
                if (!rectanglePacker.TryPack(size.Width + definition.Padding, size.Height + definition.Padding, out origin))
                {
                    return false;
                }
                testImagePlacement.Add(image.Name, new Rectangle((int)origin.X, (int)origin.Y, 
                    size.Width + definition.Padding, size.Height + definition.Padding));
            }
            return true;
        }

        /// <summary>
        /// Creates the output surface for a texture atlas.
        /// </summary>
        /// <param name="definition">The texture atlas definition.</param>
        /// <param name="content">The content manager with which to load images.</param>
        /// <param name="width">The width of the output texture.</param>
        /// <param name="height">The height of the output texture.</param>
        /// <param name="images">The table of image locations on the texture atlas.</param>
        /// <returns>The output surface that was created.</returns>
        private Surface2D CreateOutputSurface(TextureAtlasDefinition definition, ContentManager content, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            var output = Surface2D.Create(width, height);

            foreach (var image in definition.ImageList)
            {
                var imageArea = images[image.Name];
                using (var imageSurface = content.Load<Surface2D>(image.Path, false))
                {
                    var areaWithoutPadding = new Rectangle(
                        imageArea.X, 
                        imageArea.Y, 
                        imageArea.Width - definition.Padding, 
                        imageArea.Height - definition.Padding);
                    imageSurface.Blit(output, areaWithoutPadding);
                }
            }

            return output;
        }

        /// <summary>
        /// Creates the output texture for a texture atlas.
        /// </summary>
        /// <param name="definition">The texture atlas definition.</param>
        /// <param name="content">The content manager with which to load images.</param>
        /// <param name="width">The width of the output texture.</param>
        /// <param name="height">The height of the output texture.</param>
        /// <param name="images">The table of image locations on the texture atlas.</param>
        /// <returns>The output texture that was created.</returns>
        private Texture2D CreateOutputTexture(TextureAtlasDefinition definition, ContentManager content, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            using (var output = CreateOutputSurface(definition, content, width, height, images))
            {
                return output.CreateTexture();
            }
        }

        /// <summary>
        /// Creates a texture atlas from the specified collection of images.
        /// </summary>
        /// <param name="definition">The texture atlas definition.</param>
        /// <param name="content">The content manager with which to load images.</param>
        /// <param name="width">The width of the output texture.</param>
        /// <param name="height">The height of the output texture.</param>
        /// <param name="images">The table of image locations on the texture atlas.</param>
        /// <returns>The texture atlas that was created.</returns>
        private TextureAtlas CreateTextureAtlas(TextureAtlasDefinition definition, ContentManager content, Int32 width, Int32 height, Dictionary<String, Rectangle> images)
        {
            var texture = CreateOutputTexture(definition, content, width, height, images);
            var atlas = new TextureAtlas(texture, images.ToDictionary(x => x.Key, 
                x => new Rectangle(x.Value.X, x.Value.Y, x.Value.Width - definition.Padding, x.Value.Height - definition.Padding)));
            return atlas;
        }
    }
}
