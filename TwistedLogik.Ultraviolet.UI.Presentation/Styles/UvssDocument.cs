using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
	/// <summary>
	/// Represents an Ultraviolet Style Sheet (UVSS) document.
	/// </summary>
	public sealed partial class UvssDocument
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="UvssDocument"/> class with no rules or storyboards.
		/// </summary>
		public UvssDocument()
            : this(null, null)
        {

        }

		/// <summary>
		/// Initializes a new instance of the <see cref="UvssDocument"/> class.
		/// </summary>
		/// <param name="rules">A collection containing the document's rules.</param>
		/// <param name="storyboards">A collection containing the document's storyboards.</param>
		internal UvssDocument(IEnumerable<UvssRuleSet> rules, IEnumerable<UvssStoryboard> storyboards)
		{
			this.rules = (rules ?? Enumerable.Empty<UvssRuleSet>()).ToList();
			this.storyboards = (storyboards ?? Enumerable.Empty<UvssStoryboard>()).ToList();
			this.storyboardsByName = new Dictionary<String, UvssStoryboard>(StringComparer.OrdinalIgnoreCase);
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
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified source text.
        /// </summary>
        /// <param name="source">The source text from which to compile the document.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(String source)
        {
            Contract.Require(source, nameof(source));

            var document = UvssParser.Parse(source);

            return Compile(document);
        }

        /// <summary>
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the document to compile.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            using (var reader = new StreamReader(stream))
            {
                var source = reader.ReadToEnd();
                var document = UvssParser.Parse(source);

                return Compile(document);
            }
        }

        /// <summary>
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified abstract syntax tree.
        /// </summary>
        /// <param name="tree">A <see cref="UvssDocumentSyntax"/> that represents the
        /// abstract syntax tree to compile.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(UvssDocumentSyntax tree)
        {
            Contract.Require(tree, nameof(tree));

            return UvssCompiler.Compile(tree);
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
        public IEnumerable<UvssRuleSet> Rules
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
		/// Gets the culture which is used for UVSS documents which do not specify a culture.
		/// </summary>
		internal static CultureInfo DefaultCulture
		{
			get { return CultureInfo.GetCultureInfo("en-US"); }
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

                var uv = element.Ultraviolet;
                var navexp = NavigationExpression.FromUvssNavigationExpression(uv, selector.NavigationExpression);

                foreach (var style in rule.Rules)
                {
                    prioritizer.Add(uv, selector, navexp, style);
                }

                foreach (var trigger in rule.Triggers)
                {
                    prioritizer.Add(uv, selector, navexp, trigger);
                }
            }

            // Apply styles to element
            prioritizer.Apply(element);
        }

        // State values.
        private readonly UvssStylePrioritizer prioritizer = new UvssStylePrioritizer();

		// Property values.
        private readonly List<UvssRuleSet> rules;
        private readonly List<UvssStoryboard> storyboards;
        private readonly Dictionary<String, UvssStoryboard> storyboardsByName;
        private readonly Dictionary<String, Storyboard> reifiedStoryboardsByName;
        private UltravioletContext uv;
    }
}
