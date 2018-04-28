using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Uvss.Diagnostics;

namespace Ultraviolet.Presentation.Uvss
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
        public SyntaxNode(SyntaxKind kind)
        {
            this.Kind = kind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxNode"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public SyntaxNode(BinaryReader reader, Int32 version)
        {
            Contract.Require(reader, nameof(reader));

            this.Kind = (SyntaxKind)reader.ReadInt32();
            this.Position = reader.ReadInt32();
            this.Line = reader.ReadInt32();
            this.Column = reader.ReadInt32();
            this.SlotCount = reader.ReadInt32();
            this.IsStale = reader.ReadBoolean();
            this.IsMissing = reader.ReadBoolean();
            this.diagnostics = reader.ReadDiagnosticInfoArray(this, version);
        }
        
        /// <summary>
        /// Gets the child node at the specified slot index.
        /// </summary>
        /// <param name="index">The index of the child node to retrieve.</param>
        /// <returns>The child node at the specified slot index, or null if there 
        /// is no child node in the slot.</returns>
        public abstract SyntaxNode GetSlot(Int32 index);
        
        /// <summary>
        /// Serializes the object to the specified stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to serialize the object.</param>
        /// <param name="version">The file version of the data being written.</param>
        public virtual void Serialize(BinaryWriter writer, Int32 version)
        {
            Contract.Require(writer, nameof(writer));

            writer.Write((Int32)Kind);
            writer.Write(Position);
            writer.Write(Line);
            writer.Write(Column);
            writer.Write(SlotCount);
            writer.Write(IsStale);
            writer.Write(IsMissing);
            writer.Write(diagnostics, version);
        }

        /// <summary>
        /// Gets the width of the node's leading trivia.
        /// </summary>
        /// <returns>The width of the node's leading trivia.</returns>
        public virtual Int32 GetLeadingTriviaWidth()
        {
            return GetFirstTerminal(includeMissing: false)?.GetLeadingTriviaWidth() ?? 0;
        }

        /// <summary>
        /// Gets the width of the node's trailing trivia.
        /// </summary>
        /// <returns>The width of the node's trailing trivia.</returns>
        public virtual Int32 GetTrailingTriviaWidth()
        {
            return GetLastTerminal(includeMissing: false)?.GetTrailingTriviaWidth() ?? 0;
        }

        /// <summary>
        /// Gets the node's leading trivia.
        /// </summary>
        /// <returns>The node's leading trivia.</returns>
        public virtual SyntaxNode GetLeadingTrivia()
        {
            return GetFirstToken(includeMissing: false)?.GetLeadingTrivia();
        }

        /// <summary>
        /// Gets the node's trailing trivia.
        /// </summary>
        /// <returns>The node's trailing trivia.</returns>
        public virtual SyntaxNode GetTrailingTrivia()
        {
            return GetLastToken(includeMissing: false)?.GetTrailingTrivia();
        }

        /// <summary>
        /// Gets the token which occurs immediately prior to this node.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The token which occurs immediately prior to this node, or null
        /// if there is no such token in the tree.</returns>
        public SyntaxToken GetPreviousToken(Boolean includeMissing = true)
        {
            if (Parent == null)
                return null;

            var foundSelf = false;
            for (int i = Parent.SlotCount - 1; i >= 0; i--)
            {
                var sibling = Parent.GetSlot(i);
                if (sibling != null)
                {
                    if (sibling == this)
                    {
                        foundSelf = true;
                    }
                    else
                    {
                        if (foundSelf && (includeMissing || !sibling.IsMissing))
                            return sibling.GetLastToken(includeMissing);
                    }
                }
            }

            return Parent.GetPreviousToken(includeMissing);
        }

        /// <summary>
        /// Gets the token which occurs immediately after this node.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The token which occurs immediately after this node, or null
        /// if there is no such token in the tree.</returns>
        public SyntaxToken GetNextToken(Boolean includeMissing = true)
        {
            if (Parent == null)
                return null;

            var foundSelf = false;
            for (int i = 0; i < Parent.SlotCount; i++)
            {
                var sibling = Parent.GetSlot(i);
                if (sibling != null)
                {
                    if (sibling == this)
                    {
                        foundSelf = true;
                    }
                    else
                    {
                        if (foundSelf && (includeMissing || !sibling.IsMissing))
                            return sibling.GetFirstToken(includeMissing);
                    }
                }
            }

            return Parent.GetNextToken(includeMissing);
        }

        /// <summary>
        /// Gets a collection containing the node's child nodes.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> which contains the node's child nodes.</returns>
        public IEnumerable<SyntaxNode> ChildNodes()
        {
            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null)
                    yield return child;
            }
        }

        /// <summary>
        /// Gets a collection containing the node's descendant nodes.
        /// </summary>
        /// <param name="descendIntoChildren">A predicate which determines whether this method
        /// should descend into the node's children.</param>
        /// <param name="descendIntoTrivia">A value indicating whether this method
        /// should descend into the node's trivia.</param>
        /// <returns>A collection containing the specified descendant nodes.</returns>
        public IEnumerable<SyntaxNode> DescendantNodes(
            Func<SyntaxNode, Boolean> descendIntoChildren = null, Boolean descendIntoTrivia = false)
        {
            var descendants = DescendInternal(descendIntoChildren, descendIntoTrivia, false);
            foreach (var descendant in descendants)
                yield return descendant;
        }

        /// <summary>
        /// Gets a collection containing the node's descendant nodes, plus this node.
        /// </summary>
        /// <param name="descendIntoChildren">A predicate which determines whether this method
        /// should descend into the node's children.</param>
        /// <param name="descendIntoTrivia">A value indicating whether this method
        /// should descend into the node's trivia.</param>
        /// <returns>A collection containing the specified descendant nodes.</returns>
        public IEnumerable<SyntaxNode> DescendantNodesAndSelf(
            Func<SyntaxNode, Boolean> descendIntoChildren = null, Boolean descendIntoTrivia = false)
        {
            var descendants = DescendInternal(descendIntoChildren, descendIntoTrivia, true);
            foreach (var descendant in descendants)
                yield return descendant;
        }

        /// <summary>
        /// Gets the first terminal within the subtree with this node as its root.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The terminal that was found, or null if no terminal was found.</returns>
        public SyntaxNode GetFirstTerminal(Boolean includeMissing = true)
        {
            var node = this;

            do
            {
                var foundChild = false;
                for (int i = 0; i < node.SlotCount; i++)
                {
                    var child = node.GetSlot(i);
                    if (child != null && (includeMissing || !child.IsMissing))
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

            return node as SyntaxToken;
        }

        /// <summary>
        /// Gets the last terminal within the subtree with this node as its root.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The terminal that was found, or null if no terminal was found.</returns>
        public SyntaxNode GetLastTerminal(Boolean includeMissing = true)
        {
            var node = this;

            do
            {
                var foundChild = false;
                for (int i = node.SlotCount - 1; i >= 0; i--)
                {
                    var child = node.GetSlot(i);
                    if (child != null && (includeMissing || !child.IsMissing))
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

            return node as SyntaxToken;
        }

        /// <summary>
        /// Gets the first <see cref="SyntaxToken"/> within the subtree with this node as its root.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The token that was found, or null if no token was found.</returns>
        public SyntaxToken GetFirstToken(Boolean includeMissing = true)
        {
            return GetFirstTerminal(includeMissing) as SyntaxToken;
        }

        /// <summary>
        /// Gets the last <see cref="SyntaxToken"/> within the subtree with this node as its root.
        /// </summary>
        /// <param name="includeMissing">A value indicating whether to consider missing tokens.</param>
        /// <returns>The token that was found, or null if no token was found.</returns>
        public SyntaxToken GetLastToken(Boolean includeMissing = true)
        {
            return GetLastTerminal(includeMissing) as SyntaxToken;
        }

        /// <summary>
        /// Gets the collection of diagnostics reported by this node and its descendants.
        /// </summary>
        /// <returns>A collection containing the diagnostics reported by this node and its descendants.</returns>
        public IEnumerable<DiagnosticInfo> GetDiagnostics()
        {

            if (diagnostics != null)
            {
                foreach (var diagnostic in diagnostics)
                    yield return diagnostic;
            }

            if (IsToken)
            {
                var leadingDiagnostics = GetLeadingTrivia()?.GetDiagnostics();
                if (leadingDiagnostics != null)
                {
                    foreach (var diagnostic in leadingDiagnostics)
                        yield return diagnostic;
                }
            }

            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null)
                {
                    var childDiagnostics = child.GetDiagnostics();
                    foreach (var childDiagnostic in childDiagnostics)
                        yield return childDiagnostic;
                }
            }

            if (IsToken)
            {
                var trailingDiagnostics = GetTrailingTrivia()?.GetDiagnostics();
                if (trailingDiagnostics != null)
                {
                    foreach (var diagnostic in trailingDiagnostics)
                        yield return diagnostic;
                }
            }
        }

        /// <summary>
        /// Creates a string which contains the full text of this node and its children.
        /// </summary>
        /// <returns>The string that was created.</returns>
        public virtual String ToFullString()
        {
            if (IsMissing)
                return null;

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
        /// Gets the node's <see cref="SyntaxKind"/> value.
        /// </summary>
        public SyntaxKind Kind { get; }

        /// <summary>
        /// Gets the node's position within the source text.
        /// </summary>
        public Int32 Position
        {
            get { return position; }
            internal set { position = value; }
        }

        /// <summary>
        /// Gets the index of the line of source text from which this node was parsed.
        /// </summary>
        public Int32 Line
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the index of the column of source text from which this node was parsed.
        /// </summary>
        public Int32 Column
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the width of the node, excluding any leading or trailing trivia.
        /// </summary>
        public Int32 Width => FullWidth - (GetLeadingTriviaWidth() + GetTrailingTriviaWidth());

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
        /// Gets a value indicating whether this node or any of its children have diagnostics.
        /// </summary>
        public Boolean ContainsDiagnostics => GetDiagnostics().Any();

        /// <summary>
        /// Gets a value indicating whether this node has any leading trivia.
        /// </summary>
        public Boolean HasLeadingTrivia => GetLeadingTriviaWidth() > 0;

        /// <summary>
        /// Gets a value indicating whether this node has any trailing trivia.
        /// </summary>
        public Boolean HasTrailingTrivia => GetTrailingTriviaWidth() > 0;

        /// <summary>
        /// Gets a value indicating whether this node's leading trivia
        /// contains any line breaks.
        /// </summary>
        public Boolean HasLeadingLineBreaks
        {
            get
            {
                var trivia = GetLeadingTrivia();
                if (trivia == null)
                    return false;

                if (trivia.IsList)
                {
                    for (int i = 0; i < trivia.SlotCount; i++)
                    {
                        var child = trivia.GetSlot(i);
                        if (child != null && child.Kind == SyntaxKind.EndOfLineTrivia)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return trivia.Kind == SyntaxKind.EndOfLineTrivia;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this node's trailing trivia
        /// contains any line breaks.
        /// </summary>
        public Boolean HasTrailingLineBreaks
        {
            get
            {
                var trivia = GetTrailingTrivia();
                if (trivia == null)
                    return false;

                if (trivia.IsList)
                {
                    for (int i = 0; i < trivia.SlotCount; i++)
                    {
                        var child = trivia.GetSlot(i);
                        if (child != null && child.Kind == SyntaxKind.EndOfLineTrivia)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return trivia.Kind == SyntaxKind.EndOfLineTrivia;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the node has calculated a value for the <see cref="FullWidth"/> property.
        /// </summary>
        public Boolean HasValidFullWidth
        {
            get { return fullWidth >= 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the node has been made stale by changes to the tree.
        /// </summary>
        public Boolean IsStale { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this node is a list.
        /// </summary>
        public Boolean IsList { get { return Kind == SyntaxKind.List; } }

        /// <summary>
        /// Gets a value indicating whether this node represents syntax 
        /// which was not present in the source text.
        /// </summary>
        public Boolean IsMissing { get; internal set; }

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
        /// Sets the node's diagnostics.
        /// </summary>
        /// <param name="diagnostics">The diagnostics to set on the node.</param>
        internal virtual void SetDiagnostics(IEnumerable<DiagnosticInfo> diagnostics)
        {
            this.diagnostics = (diagnostics == null) ? null : diagnostics.ToArray();
        }

        /// <summary>
        /// Changes the node's leading and trailing trivia.
        /// </summary>
        /// <param name="leading">The node's leading trivia.</param>
        /// <param name="trailing">The node's trailing trivia.</param>
        internal virtual void ChangeTrivia(SyntaxNode leading, SyntaxNode trailing)
        {
            var firstToken = GetFirstToken();
            if (firstToken != null)
                firstToken.ChangeLeadingTrivia(leading);

            var lastToken = GetLastToken();
            if (lastToken != null)
                lastToken.ChangeTrailingTrivia(trailing);
        }

        /// <summary>
        /// Changes the node's leading trivia.
        /// </summary>
        /// <param name="trivia">The node's leading trivia.</param>
        internal virtual void ChangeLeadingTrivia(SyntaxNode trivia)
        {
            var firstToken = GetFirstToken();
            if (firstToken != null)
                firstToken.ChangeLeadingTrivia(trivia);
        }

        /// <summary>
        /// Changes the node's trailing trivia.
        /// </summary>
        /// <param name="trivia">The node's trailing trivia.</param>
        internal virtual void ChangeTrailingTrivia(SyntaxNode trivia)
        {
            var lastToken = GetLastToken();
            if (lastToken != null)
                lastToken.ChangeTrailingTrivia(trivia);
        }

        /// <summary>
        /// Gets the node's internal array of diagnostics.
        /// </summary>
        /// <returns>The node's internal array of diagnostics.</returns>
        internal DiagnosticInfo[] GetDiagnosticsArray()
        {
            return diagnostics;
        }
        
        /// <summary>
        /// Compuates the full width of the node, including any leading or trailing trivia.
        /// </summary>
        /// <returns>The full width of the node.</returns>
        protected virtual Int32 ComputeFullWidth()
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

        /// <summary>
        /// Updates the value of the <see cref="IsMissing"/> property.
        /// </summary>
        protected virtual void UpdateIsMissing()
        {
            var children = 0;
            var childrenMissing = 0;

            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null)
                {
                    children++;

                    if (child.IsMissing)
                        childrenMissing++;
                }
            }

            IsMissing = children > 0 && childrenMissing == children;
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
        }

        /// <summary>
        /// Gets a collection containing the node's descendants.
        /// </summary>
        private IEnumerable<SyntaxNode> DescendInternal(
            Func<SyntaxNode, Boolean> descendIntoChildren, Boolean descendIntoTrivia, Boolean descendIntoSelf)
        {
            if (descendIntoSelf)
                yield return this;

            if (descendIntoTrivia)
            {
                var trivia = GetLeadingTrivia();
                if (trivia != null)
                    DescendIntoTrivia(trivia);
            }

            for (int i = 0; i < SlotCount; i++)
            {
                var child = GetSlot(i);
                if (child != null && (descendIntoChildren == null || descendIntoChildren(child)))
                {
                    foreach (var descendant in child.DescendantNodes(descendIntoChildren, descendIntoTrivia))
                        yield return descendant;
                }
            }

            if (descendIntoTrivia)
            {
                var trivia = GetTrailingTrivia();
                if (trivia != null)
                    DescendIntoTrivia(trivia);
            }
        }

        /// <summary>
        /// Gets a collection containing the specified trivia.
        /// </summary>
        private IEnumerable<SyntaxNode> DescendIntoTrivia(SyntaxNode trivia)
        {
            if (trivia.IsList)
            {
                for (int i = 0; i < trivia.SlotCount; i++)
                {
                    var trivium = trivia.GetSlot(i);
                    if (trivium != null)
                        yield return trivium;
                }
            }
            else
            {
                yield return trivia;
            }
        }

        // Property values.
        private Int32 position;
        private Int32 fullWidth = -1;

        // Node diagnostics.
        private DiagnosticInfo[] diagnostics;
    }
}
