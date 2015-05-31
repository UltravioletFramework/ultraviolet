using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet (UVSS) document.
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
            this.rules                    = (rules ?? Enumerable.Empty<UvssRule>()).ToList();
            this.storyboards              = (storyboards ?? Enumerable.Empty<UvssStoryboard>()).ToList();
            this.storyboardsByName        = new Dictionary<String, UvssStoryboard>(StringComparer.OrdinalIgnoreCase);
            this.reifiedStoryboardsByName = new Dictionary<String, Storyboard>(StringComparer.OrdinalIgnoreCase);

            if (storyboards != null)
            {
                foreach (var storyboard in storyboards)
                {
                    this.storyboardsByName[storyboard.Name] = storyboard;
                }
            }
        }

        /// <summary>
        /// Loads an Ultraviolet Style Sheet (UVSS) document from the specified source text.
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
        /// Loads an Ultraviolet Style Sheet (UVSS) document from the specified stream.
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
        /// Clears the document's lists of rules and storyboards.
        /// </summary>
        public void Clear()
        {
            uv = null;
            rules.Clear();
            storyboards.Clear();
            storyboardsByName.Clear();
            reifiedStoryboardsByName.Clear();
        }

        /// <summary>
        /// Appends another styling document to the end of this document.
        /// </summary>
        /// <param name="document">The document to append to the end of this document.</param>
        public void Append(UvssDocument document)
        {
            Contract.Require(document, "document");

            this.rules.AddRange(document.Rules);
            this.storyboards.AddRange(document.Storyboards);

            foreach (var storyboard in document.Storyboards)
            {
                this.storyboardsByName[storyboard.Name] = storyboard;
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
        /// Gets the lexer instance used to lex Ultraviolet Style Sheet source code.
        /// </summary>
        internal static UvssLexer Lexer
        {
            get { return lexer; }
        }

        /// <summary>
        /// Gets the parser instance used to parse Ultraviolet Style Sheet source code.
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
            element.ClearStyledValues(false);

            // Gather styles from document
            var selector = default(UvssSelector);
            foreach (var rule in rules)
            {
                if (!rule.MatchesElement(element, out selector))
                    continue;

                foreach (var style in rule.Styles)
                {
                    prioritizer.Add(selector, style);
                }

                foreach (var trigger in rule.Triggers)
                {
                    prioritizer.Add(selector, trigger);
                }
            }

            // Apply styles to element
            prioritizer.Apply(element);
        }

        /// <summary>
        /// Applies a style to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply the style.</param>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        private void ApplyStyleToElement(UIElement element, UvssStyle style, UvssSelector selector)
        {
            var dp = DependencyProperty.FindByStylingName(element.Ultraviolet, element, style.Owner, style.Name);
            element.ApplyStyle(style, selector, dp);
        }

        // State values.
        private static readonly UvssLexer lexer   = new UvssLexer();
        private static readonly UvssParser parser = new UvssParser();
        private readonly UvssStylePrioritizer prioritizer = new UvssStylePrioritizer();

        // Property values.
        private readonly List<UvssRule> rules;
        private readonly List<UvssStoryboard> storyboards;
        private readonly Dictionary<String, UvssStoryboard> storyboardsByName;
        private readonly Dictionary<String, Storyboard> reifiedStoryboardsByName;
        private UltravioletContext uv;
    }
}
