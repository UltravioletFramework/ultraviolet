using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Represents the error list for a document.
    /// </summary>
    internal interface IErrorList
    {
        /// <summary>
        /// Gets a collection which contains all known errors.
        /// </summary>
        /// <returns>A collection which contains all known errors.</returns>
        IEnumerable<Error> GetErrors();

        /// <summary>
        /// Gets a collection which contains all known errors which intersect
        /// the specified span of text.
        /// </summary>
        /// <param name="span">The span of text against which to intersect errors.</param>
        /// <returns>A collection which contains all known errors which intersect
        /// the specified span of text.</returns>
        IEnumerable<Error> GetErrorsInSpan(SnapshotSpan span);

        /// <summary>
        /// Updates the list.
        /// </summary>
        void Update();
    }
}
