using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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
            var node = this;

            do
            {
                var foundChild = false;
                for (int i = 0; i < node.SlotCount; i++)
                {
                    var child = node.GetSlot(i);
                    if (child != null)
                    {
                        node = child;
                        foundChild = true;
                        break;
                    }
                }

                if (!foundChild)
                    break;
            }
            while (node.SlotCount != 0);

            return node;
        }

        /// <summary>
        /// Gets the last terminal within the subtree with this node as its root.
        /// </summary>
        /// <returns>The terminal that was found, or null if no terminal was found.</returns>
        public SyntaxNode GetLastTerminal()
        {
            var node = this;

            do
            {
                for (int i = node.SlotCount - 1; i >= 0; i--)
                {
                    var child = node.GetSlot(i);
                    if (child != null)
                    {
                        node = child;
                        break;
                    }
                }
            } while (node.SlotCount != 0);

            return node;
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
            return GetLastTerminal() as SyntaxToken;
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
        /// Creates a string which contains the full text of this node and its children.
        /// </summary>
        /// <returns>The string that was created.</returns>
        public virtual String ToFullString()
        {
            var builder = new StringBuilder();
            var writer = new StringWriter(builder, CultureInfo.InvariantCulture);
            WriteTo(writer);
            return builder.ToString();
        }

        /// <summary>
        /// Writes the full text of this node and its children to the specified <see cref="TextWriter"/> instance.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> to which to write the node's text.</param>
        public virtual void WriteTo(TextWriter writer)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(this);

            while (stack.Count > 0)
                stack.Pop().WriteToOrFlatten(writer, stack);
        }

        /// <summary>
        /// Gets the node's parent node.
        /// </summary>
        public SyntaxNode Parent { get; internal set; }

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
        public Int32 Position
        {
            get
            {
                if (position < 0)
                    position = CalculatePosition();

                return position;
            }
            internal set { position = value; }
        }

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
        /// Gets a value indicating whether the node has been made stale by changes to the tree.
        /// </summary>
        public Boolean IsStale { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this node is a list.
        /// </summary>
        public Boolean IsList { get { return Kind == SyntaxKind.List; } }

        /// <summary>
        /// Gets a value indicating whether this node is a terminal token.
        /// </summary>
        public virtual Boolean IsToken { get { return false; } }
        
        /// <summary>
        /// Accepts the specified syntax visitor.
        /// </summary>
        /// <param name="visitor">The syntax visitor to accept.</param>
        /// <returns>A <see cref="SyntaxNode"/> which will replace this node, or a reference to the node itself
        /// if no changes were made to the node.</returns>
        internal abstract SyntaxNode Accept(SyntaxVisitor visitor);

        /// <summary>
        /// Writes the full text of this node and its children to the specified <see cref="TextWriter"/> instance.
        /// </summary>
        internal virtual void WriteToOrFlatten(TextWriter writer, Stack<SyntaxNode> stack)
        {
            for (var i = SlotCount - 1; i >= 0; i--)
            {
                var node = GetSlot(i);
                if (node != null)
                {
                    stack.Push(node);
                }
            }
        }

        /// <summary>
        /// Changes the specified node's parent to this node.
        /// </summary>
        /// <param name="node">The node to update.</param>
        protected void ChangeParent(SyntaxNode node)
        {
            if (node == null || node.Parent == this)
                return;

            if (node.Parent != null)
                node.Parent.IsStale = true;

            node.Parent = this;
            InvalidatePosition();
        }

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
        /// Invalidates the position of this node and its children.
        /// </summary>
        private void InvalidatePosition()
        {
            if (position < 0)
                return;

            position = -1;

            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null)
                {
                    child.InvalidatePosition();
                }
            }
        }

        /// <summary>
        /// Calculates the position of the node within the source text.
        /// </summary>
        /// <returns>The position of the node within the source text.</returns>
        private Int32 CalculatePosition()
        {
            if (Parent == null)
                return 0;

            var position = Parent.Position;
            for (int i = 0; i < Parent.SlotCount; i++)
            {
                var sibling = Parent.GetSlot(i);
                if (sibling == this)
                    break;

                if (sibling != null)
                    position += sibling.FullWidth;
            }

            return position;
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
        private Int32 position = -1;
        private Int32 fullWidth;
    }
}
