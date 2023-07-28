using System;
using System.Xml.Linq;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Represents the metadata for a binding expression which is being compiled.
    /// </summary>
    internal class BindingExpressionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionInfo"/> structure.
        /// </summary>
        /// <param name="source">The XML node that defines the expression.</param>
        /// <param name="expression">The binding expression's text.</param>
        /// <param name="type">The binding expression's type.</param>
        public BindingExpressionInfo(XObject source, String expression, Type type)
        {
            this.source = source;
            this.expression = expression;
            this.type = type;
        }

        /// <summary>
        /// Gets the XML object that defines the expression.
        /// </summary>
        public XObject Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the binding expression's text.
        /// </summary>
        public String Expression
        {
            get { return expression; }
        }

        /// <summary>
        /// Gets the binding expression's type.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the expression compiler should perform nullable fixup on this expression's setter.
        /// </summary>
        public Boolean NullableFixup
        {
            get { return nullableFixup; }
            set { nullableFixup = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a getter should be generated for this expression's property.
        /// </summary>
        public Boolean GenerateGetter
        {
            get { return generateGetter; }
            set { generateGetter = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a setter should be generated for this expression's property.
        /// </summary>
        public Boolean GenerateSetter
        {
            get { return generateSetter; }
            set { generateSetter = value; }
        }

        /// <summary>
        /// Gets or sets the line number of the first line of the expression's getter.
        /// </summary>
        public Int32 GetterLineStart
        {
            get { return getterLineStart; }
            set { getterLineStart = value; }
        }

        /// <summary>
        /// Gets or sets the line number of the last line of the expression's getter.
        /// </summary>
        public Int32 GetterLineEnd
        {
            get { return getterLineEnd; }
            set { getterLineEnd = value; }
        }

        /// <summary>
        /// Gets or sets the line number of the first line of the expression's setter.
        /// </summary>
        public Int32 SetterLineStart
        {
            get { return setterLineStart; }
            set { setterLineStart = value; }
        }

        /// <summary>
        /// Gets or sets the line number of the last line of the expression's setter.
        /// </summary>
        public Int32 SetterLineEnd
        {
            get { return setterLineEnd; }
            set { setterLineEnd = value; }
        }

        /// <summary>
        /// Gets or sets the source type identified by the CS0266 error that was raised for this expression, if this expression raised such an error.
        /// </summary>
        public String CS0266SourceType
        {
            get { return cs0266SourceType; }
            set { cs0266SourceType = value; }
        }

        /// <summary>
        /// Gets or sets the target type identified by the CS0266 error that was raised for this expression, if this expression raised such an error.
        /// </summary>
        public String CS0266TargetType
        {
            get { return cs0266TargetType; }
            set { cs0266TargetType = value; }
        }

        // Property values.
        private readonly XObject source;
        private readonly String expression;
        private readonly Type type;
        private Boolean nullableFixup;
        private Boolean generateGetter;
        private Boolean generateSetter;
        private Int32 getterLineStart;
        private Int32 getterLineEnd;
        private Int32 setterLineStart;
        private Int32 setterLineEnd;
        private String cs0266SourceType;
        private String cs0266TargetType;
    }
}