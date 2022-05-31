using System;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a directive indicating how a <see cref="UIPanelDefinition"/> should be processed.
    /// </summary>
    public sealed class UIPanelDirective
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanelDirective"/> class.
        /// </summary>
        /// <param name="type">The directive's type.</param>
        /// <param name="value">The directive's value.</param>
        internal UIPanelDirective(String type, String value)
        {
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// Gets the directive's type.
        /// </summary>
        public String Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the directive's value.
        /// </summary>
        public String Value
        {
            get;
            private set;
        }
    }
}
