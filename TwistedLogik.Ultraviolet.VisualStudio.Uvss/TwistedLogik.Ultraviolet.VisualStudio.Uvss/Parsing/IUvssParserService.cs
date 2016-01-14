using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents a method which is called when a parser service generates a new UVSS document.
    /// </summary>
    /// <param name="span">The snapshot span for which the document was generated.</param>
    /// <param name="document">The document that was generated for the specified span.</param>
    public delegate void UvssDocumentGeneratedDelegate(SnapshotSpan span, UvssDocumentSyntax document);

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
        
        /// <summary>
        /// Occurs when the parser service generates a document.
        /// </summary>
        event UvssDocumentGeneratedDelegate DocumentGenerated;
    }
}
