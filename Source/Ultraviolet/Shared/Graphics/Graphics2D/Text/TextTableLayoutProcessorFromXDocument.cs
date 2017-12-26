using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a content processor that loads text table layouts from XML definition files.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class TextTableLayoutProcessorFromXDocument : ContentProcessor<XDocument, TextTableLayout>
    {
        /// <inheritdoc/>
        public override TextTableLayout Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var layoutDesc = new TextTableLayoutDescription();
            layoutDesc.Width = (Int32?)input.Root.Attribute("Width");
            layoutDesc.Height = (Int32?)input.Root.Attribute("Height");

            var rowElements = input.Root.Elements("Row");
            if (rowElements.Any())
            {
                var rowDescCollection = new List<TextTableLayoutRowDescription>();
                layoutDesc.Rows = rowDescCollection;

                foreach (var rowElement in rowElements)
                {
                    var rowDesc = new TextTableLayoutRowDescription();
                    rowDescCollection.Add(rowDesc);

                    var cellElements = rowElement.Elements("Cell");
                    if (cellElements.Any())
                    {
                        var cellDescCollection = new List<TextTableLayoutCellDescription>();
                        rowDesc.Cells = cellDescCollection;

                        foreach (var cellElement in cellElements)
                        {
                            var cellDesc = new TextTableLayoutCellDescription();
                            cellDescCollection.Add(cellDesc);

                            cellDesc.TextFlags = (TextFlags)ObjectResolver.FromString(
                                (String)cellElement.Attribute("TextFlags") ?? String.Empty, typeof(TextFlags));
                            cellDesc.Text = cellElement.Value;
                            cellDesc.Format = (String)cellElement.Attribute("Format");
                            cellDesc.Binding = (String)cellElement.Attribute("Binding");
                            cellDesc.Width = (Int32?)cellElement.Attribute("Width");
                            cellDesc.Height = (Int32?)cellElement.Attribute("Height");
                        }
                    }
                }
            }

            return new TextTableLayout(layoutDesc);
        }
    }
}
