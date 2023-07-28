using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains options for the UVSS parser.
    /// </summary>
    [Flags]
    public enum UvssParserOptions
    {
        /// <summary>
        /// No parser options.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The document being parsed is part of a larger document.
        /// </summary>
        PartialDocument = 0x0001,

        /// <summary>
        /// When parsing a partial document, this flag indicates that the first line of
        /// the section being parsed starts on an empty line of text.
        /// </summary>
        PartialDocumentStartsOnEmptyLine = 0x0002,
    }
}
