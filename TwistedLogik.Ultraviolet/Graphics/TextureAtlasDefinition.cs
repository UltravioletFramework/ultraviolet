using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the definition of a texture atlas.
    /// </summary>
    internal sealed class TextureAtlasDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlasDefinition"/> class.
        /// </summary>
        /// <param name="xml">The XML document that defines the texture atlas.</param>
        /// <param name="content">The content manager that is loading the texture atlas.</param>
        /// <param name="assetPath">The asset path of the texture atlas that is being loaded.</param>
        public TextureAtlasDefinition(XDocument xml, ContentManager content, String assetPath)
        {
            var metadata = xml.Root.Element("Metadata");
            if (metadata != null)
            {
                rootDirectory = metadata.ElementValueString("RootDirectory") ?? rootDirectory;
                requirePowerOfTwo = metadata.ElementValueBoolean("RequirePowerOfTwo") ?? requirePowerOfTwo;
                requireSquare = metadata.ElementValueBoolean("RequireSquare") ?? requireSquare;
                maximumWidth = metadata.ElementValueInt32("MaximumWidth") ?? maximumWidth;
                maximumHeight = metadata.ElementValueInt32("MaximumHeight") ?? maximumHeight;
                padding = metadata.ElementValueInt32("Padding") ?? padding;
            }

            var assetDirectory = Path.GetDirectoryName(assetPath);
            rootDirectory = ContentManager.NormalizeAssetPath(
                Path.Combine(assetDirectory, rootDirectory ?? String.Empty));

            var images = xml.Root.Element("Images");

            if (!LoadImages(content, images))
            {
                throw new InvalidOperationException(UltravioletStrings.TextureAtlasContainsNoImages);
            }
            SortImagesBySize();
        }

        /// <summary>
        /// Gets the root directory of the atlas images.
        /// </summary>
        public String RootDirectory
        {
            get { return rootDirectory; }
        }

        /// <summary>
        /// Gets a value indicating whether the output texture must have power-of-two dimensions.
        /// </summary>
        public Boolean RequirePowerOfTwo
        {
            get { return requirePowerOfTwo; }
        }

        /// <summary>
        /// Gets a value indicating whether the output texture must be square.
        /// </summary>
        public Boolean RequireSquare
        {
            get { return requireSquare; }
        }

        /// <summary>
        /// Gets the maximum width of the atlas' texture.
        /// </summary>
        public Int32 MaximumWidth
        {
            get { return maximumWidth; }
        }

        /// <summary>
        /// Gets the maximum height of the atlas' texture.
        /// </summary>
        public Int32 MaximumHeight
        {
            get { return maximumHeight; }
        }

        /// <summary>
        /// Gets the texture atlas' padding.
        /// </summary>
        public Int32 Padding
        {
            get { return padding; }
        }

        /// <summary>
        /// Gets the texture atlas' list of image assets.
        /// </summary>
        public IEnumerable<TextureAtlasImage> ImageList
        {
            get { return imageList; }
        }

        /// <summary>
        /// The default maximum width of a texture atlas.
        /// </summary>
        public const Int32 DefaultMaximumWidth = 4096;

        /// <summary>
        /// The default maximum height of a texture atlas.
        /// </summary>
        public const Int32 DefaultMaximumHeight = 4096;

        /// <summary>
        /// Expands a file expression potentially containing wildcard (*) characters.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> which is loading the texture atlas.</param>
        /// <param name="expression">The file expression to expand.</param>
        /// <returns>A collection containing the paths to the files that were resolved from the file expression.</returns>
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
        /// <param name="manager">The <see cref="ContentManager"/> which is loading the texture atlas.</param>
        /// <param name="root">The root directory of the expression that is being expanded.</param>
        /// <param name="expression">The file expression to expand.</param>
        /// <param name="result">A collection to populate with the expanded directories.</param>
        /// <returns>A collection containing the paths to the directories that were resolved from the directory expression.</returns>
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
        /// Gets the size of the specified image.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> with which to load images.</param>
        /// <param name="path">The path to the image for which to retrieve a size.</param>
        /// <returns>The size of the specified image.</returns>
        private static Size2 GetImageSize(ContentManager content, String path)
        {
            using (var image = content.Load<Surface2D>(path, false))
            {
                return new Size2(image.Width, image.Height);
            }
        }

        /// <summary>
        /// Loads the texture atlas' images.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> with which to load images.</param>
        /// <param name="images">The XML element that defines the set of images to load.</param>
        /// <returns>A value indicating whether any images were loaded.</returns>
        private Boolean LoadImages(ContentManager content, XElement images)
        {
            var imageList = new Dictionary<String, TextureAtlasImage>();

            if (images != null)
            {
                foreach (var include in images.Elements("Include"))
                {
                    var path = (String)include;
                    if (String.IsNullOrEmpty(path) || path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                        throw new InvalidDataException(UltravioletStrings.InvalidTextureAtlasImagePath);

                    var name = include.AttributeValueString("Name");

                    if (path.Contains("*"))
                    {
                        if (name != null)
                            throw new InvalidDataException(UltravioletStrings.TextureAtlasWildcardsCannotBeNamed);

                        var files = ExpandFileExpression(content, Path.Combine(RootDirectory, path));
                        foreach (var file in files)
                        {
                            var nameFile = Path.GetFileNameWithoutExtension(file);
                            var nameRoot = String.IsNullOrEmpty(Path.GetDirectoryName(path)) ? null : Path.GetDirectoryName(file)
                                .Replace(Path.DirectorySeparatorChar, '\\')
                                .Replace(Path.AltDirectorySeparatorChar, '\\');

                            name = String.IsNullOrEmpty(nameRoot) ? nameFile : nameRoot + "\\" + nameFile;

                            if (imageList.ContainsKey(name))
                                throw new InvalidOperationException(UltravioletStrings.TextureAtlasAlreadyContainsCell.Format(name));

                            var size = GetImageSize(content, file);
                            imageList[name] = new TextureAtlasImage(name, file, size);
                        }
                    }
                    else
                    {
                        name = name ?? path;
                        path = Path.Combine(RootDirectory, path);

                        if (imageList.ContainsKey(name))
                            throw new InvalidOperationException(UltravioletStrings.TextureAtlasAlreadyContainsCell.Format(name));

                        var size = GetImageSize(content, path);
                        imageList[name] = new TextureAtlasImage(name, path, size);
                    }
                }
            }

            this.imageList = imageList.Values.ToList();
            return this.imageList.Any();
        }

        /// <summary>
        /// Sorts the images in the atlas by size, so that larger images are placed first.
        /// </summary>
        private void SortImagesBySize()
        {
            imageList = imageList.OrderBy(x => x, new FunctorComparer<TextureAtlasImage>((f1, f2) =>
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
            }));
        }

        // Property values.
        private String rootDirectory = String.Empty;
        private Boolean requirePowerOfTwo = true;
        private Boolean requireSquare = false;
        private Int32 maximumWidth = DefaultMaximumWidth;
        private Int32 maximumHeight = DefaultMaximumHeight;
        private Int32 padding = 1;
        private IEnumerable<TextureAtlasImage> imageList;
    }
}
