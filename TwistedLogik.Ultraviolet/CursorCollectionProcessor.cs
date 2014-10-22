using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.SDL2
{
    /// <summary>
    /// Represents a content processor that loads cursor collections.
    /// </summary>
    [ContentProcessor]
    internal sealed class CursorCollectionProcessor : ContentProcessor<XDocument, CursorCollection>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public unsafe override CursorCollection Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var image = input.Root.AttributeValueString("Image");
            if (String.IsNullOrEmpty(image))
                throw new InvalidOperationException(UltravioletStrings.InvalidCursorImage);

            var collection = new CursorCollection(manager.Ultraviolet);
            using (var surface = manager.Load<Surface2D>(image, false))
            {
                foreach (var cursorElement in input.Root.Elements("Cursor"))
                {
                    var name = cursorElement.AttributeValueString("Name");
                    if (String.IsNullOrEmpty(name))
                        throw new InvalidOperationException(UltravioletStrings.InvalidCursorName);

                    var position = cursorElement.AttributeValue<Vector2>("Position");
                    var size = cursorElement.AttributeValue<Size2>("Size");
                    var hotspot = cursorElement.AttributeValue<Vector2>("Hotspot");

                    var region = new Rectangle((int)position.X, (int)position.Y, (int)size.Width, (int)size.Height);
                    using (var cursorSurface = surface.CreateSurface(region))
                    {
                        var cursor = Cursor.Create(cursorSurface, (int)hotspot.X, (int)hotspot.Y);
                        collection[name] = cursor;
                    }
                }
            }
            return collection;
        }
    }
}
