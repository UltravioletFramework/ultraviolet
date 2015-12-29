using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a node in a UVSS syntax tree.
    /// </summary>
    public abstract class SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxNode"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        /// <param name="fullWidth">The full width of the node, including any leading or trailing trivia,
        /// or -1 if the full width of the node is not yet known.</param>
        public SyntaxNode(SyntaxKind kind, Int32 fullWidth = -1)
        {
            this.Kind = kind;
            this.FullWidth = fullWidth;
        }

        /// <summary>
        /// Gets the child node at the specified slot index.
        /// </summary>
        /// <param name="index">The index of the child node to retrieve.</param>
        /// <returns>The child node at the specified slot index, or null if there 
        /// is no child node in the slot.</returns>
        public abstract SyntaxNode GetSlot(Int32 index);

        /// <summary>
        /// Gets the width of the node's leading trivia.
        /// </summary>
        /// <returns>The width of the node's leading trivia.</returns>
        public virtual Int32 GetLeadingTriviaWidth()
        {
            return GetFirstTerminal().GetLeadingTriviaWidth();
        }

        /// <summary>
        /// Gets the width of the node's trailing trivia.
        /// </summary>
        /// <returns>The width of the node's trailing trivia.</returns>
        public virtual Int32 GetTrailingTriviaWidth()
        {
            return GetLastTerminal().GetTrailingTriviaWidth();
        }

        /// <summary>
        /// Gets the node's leading trivia.
        /// </summary>
        /// <returns>The node's leading trivia.</returns>
        public virtual SyntaxNode GetLeadingTrivia()
        {
            return GetFirstToken()?.GetLeadingTrivia();
        }

        /// <summary>
        /// Gets the node's trailing trivia.
        /// </summary>
        /// <returns>The node's trailing trivia.</returns>
        public virtual SyntaxNode GetTrailingTrivia()
        {
            return GetLastToken()?.GetTrailingTrivia();
        }

        /// <summary>
        /// Gets the first terminal within the subtree with this node as its root.
        /// </summary>
        /// <returns>The terminal that was found, or null if no terminal was found.</returns>
        public SyntaxNode GetFirstTerminal()
        {
            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null)
                    return child.GetFirstTerminal();
            }

            return this;
        }

        /// <summary>
        /// Gets the last terminal within the subtree with this node as its root.
        /// </summary>
        /// <returns>The terminal that was found, or null if no terminal was found.</returns>
        public SyntaxNode GetLastTerminal()
        {
            for (int i = SlotCount - 1; i >= 0; i--)
            {
                var child = GetSlot(i);
                if (child != null)
                    return child.GetLastTerminal();
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="SyntaxToken"/> within the subtree with this node as its root.
        /// </summary>
        /// <returns>The token that was found, or null if no token was found.</returns>
        public SyntaxToken GetFirstToken()
        {
            return GetFirstTerminal() as SyntaxToken;
        }

        /// <summary>
        /// Gets the last <see cref="SyntaxToken"/> within the subtree with this node as its root.
        /// </summary>
        /// <returns>The token that was found, or null if no token was found.</returns>
        public SyntaxToken GetLastToken()
        {
            return GetLastToken() as SyntaxToken;
        }

        /// <summary>
        /// Creates a copy of this node with the specified leading trivia.
        /// </summary>
        /// <param name="trivia">The leading trivia to set on the copy of this node that is created.</param>
        /// <returns>The copy of this node that was created.</returns>
        public virtual SyntaxNode WithLeadingTrivia(SyntaxNode trivia)
        {
            return this;
        }

        /// <summary>
        /// Creates a copy opf this node with the specified trailing trivia.
        /// </summary>
        /// <param name="trivia">The trailing trivia to set on the copy of this node that is created.</param>
        /// <returns>The copy of this node that was created.</returns>
        public virtual SyntaxNode WithTrailingTrivia(SyntaxNode trivia)
        {
            return this;
        }

        /// <summary>
        /// Gets the node's parent node.
        /// </summary>
        public SyntaxNode Parent { get; }

        /// <summary>
        /// Gets a collection containing the node's child nodes.
        /// </summary>
        public IEnumerable<SyntaxNode> ChildNodes
        {
            get
            {
                for (int i = 0; i < SlotCount; i++)
                {
                    var child = GetSlot(i);
                    if (child != null)
                        yield return child;
                }
            }
        }

        /// <summary>
        /// Gets the node's <see cref="SyntaxKind"/> value.
        /// </summary>
        public SyntaxKind Kind { get; }

        /// <summary>
        /// Gets the node's position within the source text.
        /// </summary>
        public Int32 Position { get; internal set; }

        /// <summary>
        /// Gets or sets the full width of the node, including any leading or trailing trivia.
        /// </summary>
        public Int32 FullWidth
        {
            get
            {
                if (fullWidth < 0)
                {
                    fullWidth = ComputeFullWidth();
                }
                return fullWidth;
            }
            set
            {
                this.fullWidth = value;
            }
        }

        /// <summary>
        /// Gets the number of slots that this node has allocated for child nodes.
        /// </summary>
        public Int32 SlotCount { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this node has any leading trivia.
        /// </summary>
        public Boolean HasLeadingTrivia => GetLeadingTriviaWidth() > 0;

        /// <summary>
        /// Gets a value indicating whether this node has any trailing trivia.
        /// </summary>
        public Boolean HasTrailingTrivia => GetTrailingTriviaWidth() > 0;

        /// <summary>
        /// Gets a value indicating whether this node is a list.
        /// </summary>
        public Boolean IsList { get { return Kind == SyntaxKind.List; } }

        /// <summary>
        /// Gets a value indicating whether this node is a terminal token.
        /// </summary>
        public virtual Boolean IsToken { get { return false; } }

        /// <summary>
        /// Expands the width of this node by the width of the specified node.
        /// </summary>
        /// <param name="node">The node that is expanding this node.</param>
        protected void ExpandWidth(SyntaxNode node)
        {
            if (fullWidth < 0)
                return;

            fullWidth += node.FullWidth;
        }

        /// <summary>
        /// Compuates the full width of the node, including any leading or trailing trivia.
        /// </summary>
        /// <returns>The full width of the node.</returns>
        private Int32 ComputeFullWidth()
        {
            var width = 0;

            for (int i = 0; i < SlotCount; i++)
            {
                var slot = GetSlot(i);
                if (slot != null)
                {
                    width += slot.FullWidth;
                }
            }

            return width;
        }

        // Property values.
        private Int32 fullWidth;
    }
}
