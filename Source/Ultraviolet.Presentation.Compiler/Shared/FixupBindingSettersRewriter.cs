using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// A syntax rewriter which removes data binding setter field declarations whose corresponding
    /// property accessors have been removed.
    /// </summary>
    internal sealed class RemoveUnnecessaryDataBindingSetterFieldsRewriter : CSharpSyntaxRewriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnnecessaryDataBindingSetterFieldsRewriter"/> class.
        /// </summary>
        /// <param name="model">The semantic model for the syntax tree to rewrite.</param>
        public RemoveUnnecessaryDataBindingSetterFieldsRewriter(SemanticModel model)
        {
            Contract.Require(model, nameof(model));

            this.model = model;
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var result = base.VisitFieldDeclaration(node);
            if (result is FieldDeclarationSyntax fds)
            {
                if (fds.Declaration.Variables.Count == 1)
                {
                    var name = fds.Declaration.Variables[0].Identifier.ValueText;
                    if (name.StartsWith("__Set__UPF_Expression"))
                    {
                        var containingClass = fds.FirstAncestorOrSelf<ClassDeclarationSyntax>();
                        if (containingClass != null)
                        {
                            // Find the field's corresponding property.
                            var propertyName = name.Substring("__Set".Length);
                            var property = containingClass.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                                .Where(x => x.Identifier.ValueText == propertyName)
                                .SingleOrDefault();
                            if (property != null)
                            {
                                // If the property doesn't have a set accessor, remove this field.
                                var propertySetter = property.AccessorList.Accessors.Where(x => x.Kind() == SyntaxKind.SetAccessorDeclaration).SingleOrDefault();
                                if (propertySetter == null)
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
                return fds;
            }
            return result;
        }

        // The semantic model for the tree which is being rewritten.
        private readonly SemanticModel model;
    }
}
