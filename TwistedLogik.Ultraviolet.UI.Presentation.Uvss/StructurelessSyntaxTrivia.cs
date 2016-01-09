using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents structured trivia.
    /// </summary>
    public sealed class StructurelessSyntaxTrivia : SyntaxTrivia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructurelessSyntaxTrivia"/> class.
        /// </summary>
        /// <param name="kind">The trivia's <see cref="SyntaxKind"/> value.</param>
        /// <param name="text">The trivia's text.</param>
        public StructurelessSyntaxTrivia(SyntaxKind kind, String text)
            : base(kind, text?.Length ?? 0)
        {
            this.Text = text;
        }

        /// <inheritdoc/>
        public override Boolean HasStructure => false;

        /// <inheritdoc/>
        public override String ToString() => Text;

        /// <inheritdoc/>
        public override String ToFullString() => Text;

        /// <summary>
        /// Gets the trivia's text.
        /// </summary>
        public String Text { get; }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        internal override void WriteToOrFlatten(TextWriter writer, Stack<SyntaxNode> stack)
        {
            writer.Write(Text);
        }
    }
}
