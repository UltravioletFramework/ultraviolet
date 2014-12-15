using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Layout.Animation;
using TwistedLogik.Ultraviolet.Layout.Elements;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
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
            this.storyboardsByName        = this.storyboards.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            this.reifiedStoryboardsByName = new Dictionary<String, Storyboard>(StringComparer.OrdinalIgnoreCase);
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

            UvssStoryboard definition;
            if (!storyboardsByName.TryGetValue(name, out definition))
            {
                storyboard = ReifyStoryboardDefinition(definition);
                reifiedStoryboardsByName[name] = storyboard;
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
        /// Creates a new <see cref="Storyboard"/> instance based on the specified <see cref="UvssStoryboard"/>.
        /// </summary>
        /// <param name="storyboardDefinition">The storyboard definition.</param>
        /// <returns>The reified storyboard.</returns>
        private static Storyboard ReifyStoryboardDefinition(UvssStoryboard storyboardDefinition)
        {
            // TODO: How can we get the UV context here?
            var storyboard = new Storyboard(null);

            foreach (var targetDefinition in storyboardDefinition.Targets)
            {
                var target = ReifyStoryboardTargetDefinition(targetDefinition);
                storyboard.Targets.Add(target);
            }

            return storyboard;
        }

        /// <summary>
        /// Creates a new <see cref="StoryboardTarget"/> instance based on the specified <see cref="UvssStoryboardTarget"/>.
        /// </summary>
        /// <param name="targetDefinition">The storyboard target definition.</param>
        /// <returns>The reified storyboard target.</returns>
        private static StoryboardTarget ReifyStoryboardTargetDefinition(UvssStoryboardTarget targetDefinition)
        {
            var target = new StoryboardTarget(targetDefinition.Selector);

            foreach (var animationDefinition in targetDefinition.Animations)
            {
                var animation = ReifyStoryboardAnimationDefinition(targetDefinition.Filter, animationDefinition);
                if (animation != null)
                {
                    target.Animations.Add(animationDefinition.AnimatedProperty, animation);
                }
            }

            return target;
        }

        /// <summary>
        /// Creates a new <see cref="AnimationBase"/> instance based on the specified <see cref="UvssStoryboardAnimation"/>.
        /// </summary>
        /// <param name="filter">The type filter on the storyboard target.</param>
        /// <param name="animationDefinition">The storyboard animation definition.</param>
        /// <returns>The reified storyboard animation.</returns>
        private static AnimationBase ReifyStoryboardAnimationDefinition(UvssStoryboardTargetFilter filter, UvssStoryboardAnimation animationDefinition)
        {
            var propertyType = GetDependencyPropertyType(filter, animationDefinition.AnimatedProperty);
            if (propertyType == null)
                return null;

            var animationType = GetAnimationType(propertyType);
            if (animationType == null)
                return null;

            var animation = (AnimationBase)Activator.CreateInstance(animationType);

            var keyframeType = typeof(AnimationKeyframe<>).MakeGenericType(propertyType);
            foreach (var keyframeDefinition in animationDefinition.Keyframes)
            {
                var time     = TimeSpan.FromMilliseconds(keyframeDefinition.Time);
                var value    = ObjectResolver.FromString(keyframeDefinition.Value, propertyType);
                var easing   = ParseEasingFunction(keyframeDefinition.Easing);
                var keyframe = Activator.CreateInstance(keyframeType, time, value, easing);

                animation.AddKeyframe(keyframe);
            }

            return animation;
        }

        /// <summary>
        /// Parses the name of an easing function into an instance of the <see cref="EasingFunction"/> delegate.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <returns>The <see cref="EasingFunction"/> delegate instance which was created.</returns>
        private static EasingFunction ParseEasingFunction(String str)
        {
            switch (str.ToLowerInvariant())
            {
                case "ease-in-linear":
                    return Easings.EaseInLinear;

                // TODO: The rest of the standard easings
            }
            return null;
        }

        /// <summary>
        /// Resolves the specified element type name into a <see cref="Type"/> object.
        /// </summary>
        /// <param name="name">The element type name to resolve.</param>
        /// <returns>The resolved <see cref="Type"/> object.</returns>
        private static Type ResolveElementType(String name)
        {
            return UIViewLoader.GetElementTypeFromName(name, false);
        }

        /// <summary>
        /// Gets the type of the specified dependency property.
        /// </summary>
        /// <param name="filter">The type filter on the storyboard target.</param>
        /// <param name="property">The name of the dependency property being animated.</param>
        /// <returns>The type of the specified dependency property.</returns>
        private static Type GetDependencyPropertyType(UvssStoryboardTargetFilter filter, String property)
        {
            var possiblePropertyTypes =
                from f in filter
                let elementType = ResolveElementType(f)
                let propertyID = DependencyProperty.FindByName(property, elementType)
                let propertyType = (propertyID == null) ? null : Type.GetTypeFromHandle(propertyID.PropertyType)
                where propertyType != null
                select propertyType;

            if (possiblePropertyTypes.Count() > 1)
                throw new InvalidOperationException(LayoutStrings.AmbiguousDependencyPropertyType.Format(property));

            return possiblePropertyTypes.SingleOrDefault();
        }

        /// <summary>
        /// Gets the animation type which corresponds to the specified type of value.
        /// </summary>
        /// <param name="type">The type of value being animated.</param>
        /// <returns>The animation type which corresponds to the specified type of value.</returns>
        private static Type GetAnimationType(Type type)
        {
            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
                type = type.GetGenericArguments()[0];

            if (type.IsPrimitive || type == typeof(Decimal))
            {
                var animationTypeName = nullable ? String.Format("Nullable{0}Animation", type.Name) : String.Format("{0}Animation", type.Name);
                return Type.GetType(animationTypeName);
            }

            var interpolatableInterface = typeof(IInterpolatable<>).MakeGenericType(type);
            if (type.GetInterfaces().Contains(interpolatableInterface))
            {
                return nullable ? typeof(NullableInterpolatableAnimation<>).MakeGenericType(type) :
                    typeof(InterpolatableAnimation<>).MakeGenericType(type);
            }

            return typeof(ObjectAnimation<>).MakeGenericType(type);
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
                reifiedStoryboardsByName[storyboardDefinition.Name] = ReifyStoryboardDefinition(storyboardDefinition);
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
                    styleAggregator[styleKey] = new PrioritizedStyleData(style.Container, style.Name, style.Value.Trim(), stylePriority);
                }
            }

            // Apply styles to element
            foreach (var kvp in styleAggregator)
            {
                ApplyStyleToElement(element, kvp.Value.Container, kvp.Value.Name, kvp.Key.PseudoClass, kvp.Value.Value);
            }
            styleAggregator.Clear();
        }

        /// <summary>
        /// Applies a style to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply the style.</param>
        /// <param name="container">The style's container, if it represents an attached property.</param>
        /// <param name="style">The name of the style to apply.</param>
        /// <param name="pseudoClass">The pseudo-class of the style to apply.</param>
        /// <param name="value">The styled value to apply.</param>
        private void ApplyStyleToElement(UIElement element, String container, String style, String pseudoClass, String value)
        {
            var styleIsForContainer = !String.IsNullOrEmpty(container);
            if (styleIsForContainer)
            {
                var styleMatchesContainer = element.Container != null && String.Equals(element.Container.Name, container, StringComparison.OrdinalIgnoreCase);
                if (styleMatchesContainer)
                {
                    element.ApplyStyle(style, pseudoClass, value, true);
                    return;
                }
            }

            element.ApplyStyle(style, pseudoClass, value, false);
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
