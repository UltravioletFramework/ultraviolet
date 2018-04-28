using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents structured trivia.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.StructurelessTrivia)]
    public sealed class StructurelessSyntaxTrivia : SyntaxTrivia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructurelessSyntaxTrivia"/> class.
        /// </summary>
        /// <param name="kind">The trivia's <see cref="SyntaxKind"/> value.</param>
        /// <param name="text">The trivia's text.</param>
        public StructurelessSyntaxTrivia(SyntaxKind kind, String text)
            : base(kind)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructurelessSyntaxTrivia"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public StructurelessSyntaxTrivia(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.Text = reader.ReadBoolean() ? 
                reader.ReadString() : null;
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(Text != null);
            if (Text != null)
                writer.Write(Text);
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
        
        /// <inheritdoc/>
        protected override Int32 ComputeFullWidth()
        {
            return
                GetLeadingTriviaWidth() +
                (Text?.Length ?? 0) +
                GetTrailingTriviaWidth();
        }
    }
}
