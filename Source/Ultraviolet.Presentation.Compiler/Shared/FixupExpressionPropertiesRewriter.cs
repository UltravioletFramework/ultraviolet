using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// A syntax rewriter which adds explicit conversions to expression properties.
    /// </summary>
    internal sealed class FixupExpressionPropertiesRewriter : CSharpSyntaxRewriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixupExpressionPropertiesRewriter"/> class.
        /// </summary>
        /// <param name="model">The semantic model for the syntax tree to rewrite.</param>
        public FixupExpressionPropertiesRewriter(SemanticModel model)
        {
            Contract.Require(model, nameof(model));

            this.model = model;
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var result = base.VisitPropertyDeclaration(node);
            if (result is PropertyDeclarationSyntax pds)
            {
                var newProperty = pds;

                newProperty = FixupExpressionGetterConversions(newProperty);
                newProperty = FixupExpressionSetterConversions(newProperty);

                return newProperty;
            }
            return result;
        }
        
        /// <summary>
        /// Adds explicit conversions to the return statements of expression property getters, if they are required.
        /// </summary>
        private PropertyDeclarationSyntax FixupExpressionGetterConversions(PropertyDeclarationSyntax node)
        {
            if (!node.Identifier.ValueText.StartsWith("__UPF_"))
                return node;

            var propertyGetter = node.AccessorList.Accessors.Where(x => x.Kind() == SyntaxKind.GetAccessorDeclaration).SingleOrDefault();
            if (propertyGetter == null || propertyGetter.Body == null)
                return node;

            var propertyGetterReturn = propertyGetter.Body.DescendantNodes().OfType<ReturnStatementSyntax>().SingleOrDefault();
            if (propertyGetterReturn == null)
                return node;
            
            var propertySymbol = model.GetDeclaredSymbol(node);
            var propertyType = propertySymbol.Type;

            var propertyGetterReturnType = model.GetTypeInfo(propertyGetterReturn.Expression);
            var propertyGetterConversion = model.Compilation.ClassifyConversion(propertyGetterReturnType.Type, propertyType);
            
            // If necessary, add an explicit conversion to the return statement of the expression getter.
            if (propertyGetterConversion.Exists && propertyGetterConversion.IsExplicit)
            {
                node = node.ReplaceNode(propertyGetterReturn, 
                    propertyGetterReturn.WithExpression(
                        SyntaxFactory.CastExpression(
                            SyntaxFactory.ParseTypeName(propertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)), 
                            propertyGetterReturn.Expression)
                    )
                );
            }

            return node;
        }

        /// <summary>
        /// Adds explicit conversions to the assignment expressions of expression property setters, if they are required.
        /// </summary>
        private PropertyDeclarationSyntax FixupExpressionSetterConversions(PropertyDeclarationSyntax node)
        {
            if (!node.Identifier.ValueText.StartsWith("__UPF_"))
                return node;

            var propertySetter = node.AccessorList.Accessors.Where(x => x.Kind() == SyntaxKind.SetAccessorDeclaration).SingleOrDefault();
            if (propertySetter == null || propertySetter.Body == null)
                return node;

            var propertyAssignment = propertySetter.Body.DescendantNodes().OfType<AssignmentExpressionSyntax>().FirstOrDefault();
            if (propertyAssignment != null)
            {
                // If we can't actually assign to the left side of this expression, this is a one-way
                // binding and we should just remove the setter entirely.
                var symbolLeft = model.GetSymbolInfo(propertyAssignment.Left);
                if (symbolLeft.Symbol == null || symbolLeft.CandidateReason == CandidateReason.NotAVariable)
                {
                    node = node.RemoveNode(propertySetter, SyntaxRemoveOptions.KeepNoTrivia);
                }
                else
                {
                    var typeLeft = model.GetTypeInfo(propertyAssignment.Left);
                    var typeRight = model.GetTypeInfo(propertyAssignment.Right);

                    // If necessary, add an explicit conversion to the right side of the assignment statement of the expression setter.
                    var propertySetterConversion = model.Compilation.ClassifyConversion(typeRight.Type, typeLeft.Type);
                    if (propertySetterConversion.Exists && propertySetterConversion.IsExplicit)
                    {
                        var propertySymbol = model.GetDeclaredSymbol(node);
                        var propertyType = propertySymbol.Type;

                        node = node.ReplaceNode(propertyAssignment,
                            propertyAssignment.WithRight(
                                SyntaxFactory.CastExpression(
                                    SyntaxFactory.ParseTypeName(propertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)),
                                    propertyAssignment.Right
                                )
                            )
                        );
                    }
                }
            }
            else
            {
                var propertyInvocations = propertySetter.Body.DescendantNodes().OfType<InvocationExpressionSyntax>().ToList();
                foreach (var propertyInvocation in propertyInvocations)
                {
                    var propertyInvocationSymbol = model.GetSymbolInfo(propertyInvocation);
                    if (propertyInvocationSymbol.CandidateReason == CandidateReason.OverloadResolutionFailure && !propertyInvocationSymbol.CandidateSymbols.IsEmpty)
                    {
                        if (!propertyInvocationSymbol.CandidateSymbols.Any(x => x.Name == "SetValue"))
                            break;

                        // If overload resolution failed for a SetValue<T> method, go through the list of candidates until
                        // we find one that we can match by adding explicit casts.
                        var propertyInvocationArgs = propertyInvocation.ArgumentList.Arguments;
                        var propertyInvocationArgSymbols = propertyInvocationArgs.Select(x => model.GetTypeInfo(x.Expression)).ToList();
                        for (int i = 0; i < propertyInvocationSymbol.CandidateSymbols.Length; i++)
                        {
                            var candidate = propertyInvocationSymbol.CandidateSymbols[i] as IMethodSymbol;
                            if (candidate == null || candidate.Parameters.Length != propertyInvocationArgSymbols.Count)
                                continue;

                            // Does a cast exist from every source argument to every destination parameter?
                            var candidateConversions = Enumerable.Range(0, candidate.Parameters.Length)
                                .Select(x => model.Compilation.ClassifyConversion(propertyInvocationArgSymbols[x].Type, candidate.Parameters[x].Type))
                                .ToList();
                            if (!candidateConversions.Any(x => !x.Exists))
                            {
                                // Construct a new argument list including explicit casts.
                                for (int j = 0; j < candidateConversions.Count; j++)
                                {
                                    if (candidateConversions[j].IsExplicit)
                                    {
                                        var arg = propertyInvocationArgs[j];
                                        node = node.ReplaceNode(arg,
                                            arg.WithExpression(
                                                SyntaxFactory.CastExpression(
                                                    SyntaxFactory.ParseTypeName(candidate.Parameters[j].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)),
                                                    arg.Expression
                                                )
                                            )
                                        );
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return node;
        }

        // The semantic model for the tree which is being rewritten.
        private readonly SemanticModel model;
    }
}
