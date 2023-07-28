using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents the source code for a particular style sheet.
    /// </summary>
    public sealed class UIStyleSheetSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIStyleSheetSource"/> class.
        /// </summary>
        /// <param name="source">The style sheet source code.</param>
        /// <param name="cultures">The list of cultures under which this style sheet is applied.</param>
        /// <param name="languages">The list of languages under which this style sheet is applied.</param>
        public UIStyleSheetSource(String source, IEnumerable<String> cultures = null, IEnumerable<String> languages = null)
        {
            Contract.Require(source, nameof(source));

            this.Source = source;
            this.Cultures = new List<String>(cultures ?? Enumerable.Empty<String>());
            this.Languages = new List<String>(languages ?? Enumerable.Empty<String>());
        }

        /// <summary>
        /// Gets the style sheet source code.
        /// </summary>
        public String Source { get; }

        /// <summary>
        /// Gets the culture for which this source is applied.
        /// </summary>
        public IReadOnlyList<String> Cultures { get; }

        /// <summary>
        /// Gets the language for which this source is applied.
        /// </summary>
        public IReadOnlyList<String> Languages { get; }
    }
}
