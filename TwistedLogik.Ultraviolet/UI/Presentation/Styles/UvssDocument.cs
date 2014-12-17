using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocument"/> class.
        /// </summary>
        /// <param name="rules">A collection containing the document's rules.</param>
        /// <param name="storyboards">A collection containing the document's storyboards.</param>
        internal UvssDocument(IEnumerable<UvssRule> rules, IEnumerable<UvssStoryboard> storyboards)
        {
            Contract.Require(rules, "rules");

            this.rules                    = (rules ?? Enumerable.Empty<UvssRule>()).ToList();
            this.storyboards              = (storyboards ?? Enumerable.Empty<UvssStoryboard>()).ToList();
            this.storyboardsByName        = new Dictionary<String, UvssStoryboard>(StringComparer.OrdinalIgnoreCase);
            this.reifiedStoryboardsByName = new Dictionary<String, Storyboard>(StringComparer.OrdinalIgnoreCase);

            foreach (var storyboard in storyboards)
            {
                this.storyboardsByName[storyboard.Name] = storyboard;
            }
        }

        /// <summary>
        /// Loads an Ultraviolet Stylesheet (UVSS) document from the specified source text.
        /// </summary>
        /// <param name="source">The source text from which to load the document.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the loaded data.</returns>
        public static UvssDocument Parse(String source)
        {
            Contract.Require(source, "source");

            var tokens   = lexer.Lex(source);
            var document = parser.Parse(source, tokens);

            return document;
        }

        /// <summary>
        /// Loads an Ultraviolet Stylesheet (UVSS) document from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the document to load.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the loaded data.</returns>
        public static UvssDocument Load(Stream stream)
        {
            Contract.Require(stream, "stream");

            using (var reader = new StreamReader(stream))
            {
                var source   = reader.ReadToEnd();
                var tokens   = lexer.Lex(source);
                var document = parser.Parse(source, tokens);

                return document;
            }
        }

        /// <summary>
        /// Retrieves a reified instance of the specified storyboard definition, if it has been defined.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The name of the storyboard to retrieve.</param>
        /// <returns>The <see cref="Storyboard"/> instance that was retrieved, or <c>null</c> if no such storyboard exists.</returns>
        public Storyboard InstantiateStoryboardByName(UltravioletContext uv, String name)
        {
            Contract.Require(uv, "uv");
            Contract.RequireNotEmpty(name, "name");

            ReifyStoryboardDefinitions(uv);

            Storyboard storyboard;
            if (reifiedStoryboardsByName.TryGetValue(name, out storyboard))
            {
                return storyboard;
            }

            return null;
        }

        /// <summary>
        /// Gets the document's rules.
        /// </summary>
        public IEnumerable<UvssRule> Rules
        {
            get { return rules; }
        }

        /// <summary>
        /// Gets the document's storyboards.
        /// </summary>
        public IEnumerable<UvssStoryboard> Storyboards
        {
            get { return storyboards; }
        }

        /// <summary>
        /// Applies styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply styles.</param>
        internal void ApplyStyles(UIElement element)
        {
            Contract.Require(element, "element");

            ApplyStylesInternal(element);
        }

        /// <summary>
        /// Recursively applies styles to the specified element and all of its descendants.
        /// </summary>
        /// <param name="element">The element to which to apply styles.</param>
        internal void ApplyStylesRecursively(UIElement element)
        {
            Contract.Require(element, "element");

            ApplyStylesInternal(element);

            var container = element as UIContainer;
            if (container != null)
            {
                foreach (var child in container.Children)
                {
                    ApplyStylesRecursively(child);
                }
            }
        }

        /// <summary>
        /// Gets the lexer instance used to lex Ultraviolet Stylesheet source code.
        /// </summary>
        internal static UvssLexer Lexer
        {
            get { return lexer; }
        }

        /// <summary>
        /// Gets the parser instance used to parse Ultraviolet Stylesheet source code.
        /// </summary>
        internal static UvssParser Parser
        {
            get { return parser; }
        }

        /// <summary>
        /// Creates new <see cref="Storyboard"/> instances based on the current set of <see cref="UvssStoryboard"/> definitions.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private void ReifyStoryboardDefinitions(UltravioletContext uv)
        {
            if (this.uv == uv)
                return;

            this.uv = uv;

            reifiedStoryboardsByName.Clear();

            foreach (var storyboardDefinition in storyboards)
            {
                var reifiedStoryboard = UvssStoryboardReifier.ReifyStoryboard(uv, storyboardDefinition);
                reifiedStoryboardsByName[storyboardDefinition.Name] = reifiedStoryboard;
            }
        }

        /// <summary>
        /// Applies styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply styles.</param>
        private void ApplyStylesInternal(UIElement element)
        {
            element.ClearStyledValues();

            // Gather styles from document
            var selector = default(UvssSelector);
            foreach (var rule in rules)
            {
                if (!rule.MatchesElement(element, out selector))
                    continue;

                foreach (var style in rule.Styles)
                {
                    const Int32 ImportantStylePriority = 1000000000;

                    var styleKey      = new UvssStyleKey(style.QualifiedName, selector.PseudoClass);
                    var stylePriority = selector.Priority + (style.IsImportant ? ImportantStylePriority : 0);

                    PrioritizedStyleData existingStyleData;
                    if (styleAggregator.TryGetValue(styleKey, out existingStyleData))
                    {
                        if (existingStyleData.Priority > stylePriority)
                            continue;
                    }
                    styleAggregator[styleKey] = new PrioritizedStyleData(style, selector, stylePriority);
                }
            }

            // Apply styles to element
            foreach (var kvp in styleAggregator)
            {
                ApplyStyleToElement(element, kvp.Value.Style, kvp.Value.Selector);
            }
            styleAggregator.Clear();
        }

        /// <summary>
        /// Applies a style to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply the style.</param>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        private void ApplyStyleToElement(UIElement element, UvssStyle style, UvssSelector selector)
        {
            var styleIsForContainer = !String.IsNullOrEmpty(style.Container);
            if (styleIsForContainer)
            {
                var styleMatchesContainer = element.Container != null && 
                    String.Equals(element.Container.Name, style.Container, StringComparison.OrdinalIgnoreCase);

                if (styleMatchesContainer)
                {
                    element.ApplyStyle(style, selector, true);
                    return;
                }
            }

            element.ApplyStyle(style, selector, false);
        }

        // State values.
        private static readonly Dictionary<UvssStyleKey, PrioritizedStyleData> styleAggregator = 
            new Dictionary<UvssStyleKey, PrioritizedStyleData>();
        private static readonly UvssLexer lexer   = new UvssLexer();
        private static readonly UvssParser parser = new UvssParser();

        // Property values.
        private readonly List<UvssRule> rules;
        private readonly List<UvssStoryboard> storyboards;
        private readonly Dictionary<String, UvssStoryboard> storyboardsByName;
        private readonly Dictionary<String, Storyboard> reifiedStoryboardsByName;
        private UltravioletContext uv;
    }
}
