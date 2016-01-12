using System.ComponentModel.Composition;
using TwistedLogik.Ultraviolet.VisualStudio.Uvss.Parsing;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Tagging
{
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(IErrorTag))]
    [ContentType("uvss")]
    internal sealed class SyntaxErrorTaggerProvider : IViewTaggerProvider
    {
#pragma warning disable 649

        [Import]
        public UvssParserService parserService;
        
#pragma warning restore 649

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(creator: () =>
                new SyntaxErrorTagger(parserService, buffer) as ITagger<T>);
        }
    }
}
