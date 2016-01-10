using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.UVSS.Classification
{
    /// <summary>
    /// Represents a service which parses text snapshots into UVSS documents.
    /// </summary>
    [Export]
    public class UvssParserService : IUvssParserService
    {
        /// <inheritdoc/>
        public Task<UvssDocumentSyntax> GetDocument(ITextSnapshot textSnapshot)
        {
            var textBuffer = textSnapshot.TextBuffer;

            lock (textBuffer)
            {
                var syntaxRootMap = default(ConditionalWeakTable<ITextSnapshot, Task<UvssDocumentSyntax>>);
                var syntaxRootTask = default(Task<UvssDocumentSyntax>);

                if (!textBuffer.Properties.TryGetProperty(typeof(UvssParserService), out syntaxRootMap))
                {
                    syntaxRootMap = new ConditionalWeakTable<ITextSnapshot, Task<UvssDocumentSyntax>>();
                    textBuffer.Properties.AddProperty(typeof(UvssParserService), syntaxRootMap);
                }
                else
                {
                    if (syntaxRootMap.TryGetValue(textSnapshot, out syntaxRootTask))
                    {
                        return syntaxRootTask;
                    }
                }

                syntaxRootTask = Task.Run(() =>
                {
                    return UvssParser.Parse(textSnapshot.GetText());
                });
                syntaxRootMap.Add(textSnapshot, syntaxRootTask);

                return syntaxRootTask;
            }
        }
    }
}
