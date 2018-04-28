using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents a content processor that loads cursor collections.
    /// </summary>
    [ContentProcessor]
    internal sealed class CursorCollectionProcessorFromXDocument : ContentProcessor<XDocument, CursorCollection>
    {
        /// <inheritdoc/>
        public override CursorCollection Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var collectionDesc = new CursorCollectionDescription();
            var collectionCursors = new List<CursorDescription>();

            var image = input.Root.AttributeValueString("Image");
            if (String.IsNullOrEmpty(image))
                throw new InvalidOperationException(UltravioletStrings.InvalidCursorImage);
            
            collectionDesc.Texture = image;
            collectionDesc.Cursors = collectionCursors;

            foreach (var cursorElement in input.Root.Elements("Cursor"))
            {
                var name = cursorElement.AttributeValueString("Name");
                if (String.IsNullOrEmpty(name))
                    throw new InvalidOperationException(UltravioletStrings.InvalidCursorName);

                var position = cursorElement.AttributeValue<Point2>("Position");
                var size = cursorElement.AttributeValue<Size2>("Size");
                var hotspot = cursorElement.AttributeValue<Point2>("Hotspot");

                var cursorDesc = new CursorDescription();
                cursorDesc.Name = name;
                cursorDesc.Area = new Rectangle(position.X, position.Y, size.Width, size.Height);
                cursorDesc.Hotspot = hotspot;

                collectionCursors.Add(cursorDesc);
            }

            return innerProcessor.Process(manager, metadata, collectionDesc);
        }

        private static readonly CursorCollectionProcessor innerProcessor =
            new CursorCollectionProcessor();
    }
}
