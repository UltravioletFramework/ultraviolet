using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Uvss.Diagnostics;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an error that arises during the process of compiling a UVSS document.
    /// </summary>
    public sealed class UvssError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssError"/> class.
        /// </summary>
        /// <param name="diagnosticInfo">The diagnostic info associated with the error, if any.</param>
        /// <param name="line">The index of the line in the source text at which the error occurred.</param>
        /// <param name="column">The index of the column in the source text at which the error occurred.</param>
        /// <param name="message">The error message.</param>
        private UvssError(DiagnosticInfo diagnosticInfo, Int32 line, Int32 column, String message)
        {
            this.diagnosticInfo = diagnosticInfo;
            this.line = line;
            this.column = column;
            this.message = message;
        }

        /// <summary>
        /// Creates a new <see cref="UvssError"/> instance from the specified diagnostic info.
        /// </summary>
        /// <param name="diagnosticInfo">The diagnostic info from which to create the error.</param>
        /// <returns>The <see cref="UvssError"/> instance which was created.</returns>
        public static UvssError FromDiagnosticInfo(DiagnosticInfo diagnosticInfo)
        {
            Contract.Require(diagnosticInfo, nameof(diagnosticInfo));

            return new UvssError(diagnosticInfo, 0, 0, null);
        }

        /// <summary>
        /// Creates a new <see cref="UvssError"/> instance from the specified error message.
        /// </summary>
        /// <param name="line">The index of the line in the source text
        /// at which the error occurred.</param>
        /// <param name="column">The index of the column in the source text
        /// at which the error occurred.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The <see cref="UvssError"/> instance which was created.</returns>
        public static UvssError FromMessage(Int32 line, Int32 column, String message)
        {
            return new UvssError(null, line, column, message);
        }

        /// <inheritdoc/>
        public override String ToString() => String.Format("{0} ({1}, {2})", Message, Line, Column);

        /// <summary>
        /// Gets the diagnostic info associated with the error, if any.
        /// </summary>
        public DiagnosticInfo DiagnosticInfo
        {
            get { return diagnosticInfo; }
        }

        /// <summary>
        /// Gets the index of the line in the source text at which
        /// the error occurred.
        /// </summary>
        public Int32 Line
        {
            get { return diagnosticInfo?.Node?.Line ?? line; }
        }

        /// <summary>
        /// Gets the index of the column in the source text at which
        /// the error occurred.
        /// </summary>
        public Int32 Column
        {
            get { return diagnosticInfo?.Node?.Column ?? column; }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public String Message
        {
            get { return diagnosticInfo?.Message ?? message; }
        }

        // Property values.
        private readonly DiagnosticInfo diagnosticInfo;
        private readonly Int32 line;
        private readonly Int32 column;
        private readonly String message;
    }
}
