﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.SDL2
{
    /// <summary>
    /// Represents a content processor that loads cursor collections.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    internal sealed class CursorCollectionProcessorFromXDocument : ContentProcessor<XDocument, CursorCollection>
    {
        /// <inheritdoc/>
        public override CursorCollection Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var collectionDesc = new CursorCollectionDescription();
            var collectionCursors = new List<CursorDescription>();

            var image = (String)input.Root.Attribute("Image");
            if (String.IsNullOrEmpty(image))
                throw new InvalidOperationException(UltravioletStrings.InvalidCursorImage);
            
            collectionDesc.Texture = image;
            collectionDesc.Cursors = collectionCursors;

            foreach (var cursorElement in input.Root.Elements().Where(x => x.Name.LocalName == "Cursor"))
            {
                var name = (String)cursorElement.Attribute("Name");
                if (String.IsNullOrEmpty(name))
                    throw new InvalidOperationException(UltravioletStrings.InvalidCursorName);

                var position = 
                    cursorElement.AttributeValue<Point2>("Position");
                var size = 
                    cursorElement.AttributeValue<Size2>("Size");
                var hotspot = 
                    cursorElement.AttributeValue<Point2?>("Hotspot") ?? Point2.Zero;

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
