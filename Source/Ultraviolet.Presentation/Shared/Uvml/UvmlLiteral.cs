using System;
using System.Globalization;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML node which produces a literal value.
    /// </summary>
    public sealed class UvmlLiteral : UvmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlLiteral"/> class.
        /// </summary>
        /// <param name="literal">The literal value which is represented by this node.</param>
        /// <param name="type">The type of value which is produced from the literal.</param>
        /// <param name="culture">The culture with which the literal value is parsed, or 
        /// <see langword="null"/> to use the invariant culture.</param>
        internal UvmlLiteral(String literal, Type type, CultureInfo culture = null)
        {
            Contract.RequireNotEmpty(literal, nameof(literal));
            Contract.Require(type, nameof(type));

            String localizationKey;
            if (IsLocalizedString(literal, out localizationKey))
            {
                literal = Localization.Get(localizationKey);
            }

            this.Literal = literal;
            this.Type = type;
            this.Culture = culture ?? CultureInfo.GetCultureInfo(null); 
        }

        /// <inheritdoc/>
        public override Object Instantiate(UltravioletContext uv, UvmlInstantiationContext context)
        {
            if (String.Equals(Literal, "{{null}}", StringComparison.Ordinal))
                return Type.IsValueType ? Activator.CreateInstance(Type) : null;

            if (typeof(IInputElement).IsAssignableFrom(Type))
            {
                return new UvmlElementReference(Literal);
            }

            return ObjectResolver.FromString(Literal, Type, Culture);
        }

        /// <summary>
        /// Gets the literal value which is represented by this node.
        /// </summary>
        public String Literal { get; }

        /// <summary>
        /// Gets the type of value which is produced from the literal.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the culture with which the literal value is parsed.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Gets a value indicating whether the specified literal is a localized string.
        /// </summary>
        private Boolean IsLocalizedString(String literal, out String key)
        {
            if (literal.StartsWith("[[") && literal.EndsWith("]]"))
            {
                key = literal.Substring(2, literal.Length - 4);
                return true;
            }
            key = null;
            return false;
        }
    }
}
