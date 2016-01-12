using Microsoft.VisualStudio.Text;
using System.Threading.Tasks;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.UVSS.Classification
{
    /// <summary>
    /// Represents a service which parses text snapshots into UVSS documents.
    /// </summary>
    public interface IUvssParserService
    {
        /// <summary>
        /// Parses the specified text snapshot into a UVSS document.
        /// </summary>
        /// <param name="span">The snapshot span to parse.</param>
        /// <returns>A task that parses the specified text snapshot into a UVSS document.</returns>
        Task<UvssDocumentSyntax> GetDocument(SnapshotSpan span);
    }
}
