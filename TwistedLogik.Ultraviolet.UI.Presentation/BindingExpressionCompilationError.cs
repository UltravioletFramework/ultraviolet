using System;
using System.Xml;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an error which is raised during binding expression compilation.
    /// </summary>
    public sealed class BindingExpressionCompilationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationError"/> class.
        /// </summary>
        /// <param name="filename">The filename of the source file that contains the error.</param>
        /// <param name="line">The line number associated with the error.</param>
        /// <param name="column">The column number associated with the error.</param>
        /// <param name="errorNumber">The error number.</param>
        /// <param name="errorText">The error text.</param>
        public BindingExpressionCompilationError(String filename, Int32 line, Int32 column, String errorNumber, String errorText)
        {
            this.filename = filename;
            this.line = line;
            this.column = column;
            this.errorNumber = errorNumber;
            this.errorText = errorText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationError"/> class.
        /// </summary>
        /// <param name="source">The <see cref="XObject"/> which is the source of the error.</param>
        /// <param name="filename">The filename of the file which is the source of the error.</param>
        /// <param name="message">The error message.</param>
        public BindingExpressionCompilationError(XObject source, String filename, String message)
        {
            Contract.Require(source, nameof(source));

            var lineInfo = (IXmlLineInfo)source;

            this.filename = filename ?? (String.IsNullOrEmpty(source.BaseUri) ? null : new Uri(source.BaseUri).LocalPath);
            this.line = lineInfo.LineNumber;
            this.column = lineInfo.LinePosition;
            this.errorNumber = String.Empty;
            this.errorText = message;
        }

        /// <summary>
        /// Gets the filename of the source file that contains the error.
        /// </summary>
        public String Filename
        {
            get { return filename; }
        }

        /// <summary>
        /// Gets the line number associated with the error.
        /// </summary>
        public Int32 Line
        {
            get { return line; }
        }

        /// <summary>
        /// Gets the column number associated with the error.
        /// </summary>
        public Int32 Column
        {
            get { return column; }
        }

        /// <summary>
        /// Gets the error number.
        /// </summary>
        public String ErrorNumber
        {
            get { return errorNumber; }
        }

        /// <summary>
        /// Gets the error text.
        /// </summary>
        public String ErrorText
        {
            get { return errorText; }
        }

        // Property values.
        private readonly String filename;
        private readonly Int32 line;
        private readonly Int32 column;
        private readonly String errorNumber;
        private readonly String errorText;
    }
}
