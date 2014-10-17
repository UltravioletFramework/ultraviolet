using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the layout information for a table of formatted text.
    /// </summary>
    public sealed class TextTableLayout
    {
        /// <summary>
        /// Initializes a new instance of the TextTableLayout class.
        /// </summary>
        /// <param name="xml">The XML document that contains the table layout.</param>
        internal TextTableLayout(XDocument xml)
        {
            Contract.Require(xml, "xml");

            this.xml = xml;
        }

        /// <summary>
        /// Creates a table from the layout information.
        /// </summary>
        /// <param name="renderer">The text renderer used to lay out and render the table's text.</param>
        /// <param name="font">The table's default font.</param>
        /// <returns>The table that was created.</returns>
        public TextTable<T> Create<T>(TextRenderer renderer, SpriteFont font)
        {
            Contract.Require(renderer, "renderer");
            Contract.Require(font, "font");

            var width = xml.Root.AttributeValueInt32("Width") ?? 0;
            var height = xml.Root.AttributeValueInt32("Height") ?? 0;
            var table = new TextTable<T>(renderer, width, height, font);

            var rowElements = xml.Root.Elements("Row");
            foreach (var rowElement in rowElements)
            {
                var row = table.Rows.Add();
                var cellElements = rowElement.Elements("Cell");
                foreach (var cellElement in cellElements)
                {
                    var cell = row.Cells.Add();
                    cell.Format = cellElement.AttributeValueString("Format");
                    cell.Text = cellElement.Value;
                    cell.TextFlags = (TextFlags)ObjectResolver.FromString(cellElement.AttributeValueString("TextFlags") ?? String.Empty, typeof(TextFlags));
                    cell.Width = cellElement.AttributeValueInt32("Width");
                    cell.Height = cellElement.AttributeValueInt32("Height");

                    var binding = cellElement.AttributeValueString("Binding");
                    if (!String.IsNullOrWhiteSpace(binding))
                    {
                        cell.Bind(binding);
                    }
                }
            }

            return table;
        }

        // State values.
        private readonly XDocument xml;
    }
}
