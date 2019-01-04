using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Uvss;
using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssDocument : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="UvssDocument"/> type.
        /// </summary>
        static UvssDocument()
        {
            standardEasingFunctions[KnownEasingFunctions.EaseInLinear] = Easings.EaseInLinear;
            standardEasingFunctions[KnownEasingFunctions.EaseOutLinear] = Easings.EaseOutLinear;
            standardEasingFunctions[KnownEasingFunctions.EaseInCubic] = Easings.EaseInCubic;
            standardEasingFunctions[KnownEasingFunctions.EaseOutCubic] = Easings.EaseOutCubic;
            standardEasingFunctions[KnownEasingFunctions.EaseInQuadratic] = Easings.EaseInQuadratic;
            standardEasingFunctions[KnownEasingFunctions.EaseOutQuadratic] = Easings.EaseOutQuadratic;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutQuadratic] = Easings.EaseInOutQuadratic;
            standardEasingFunctions[KnownEasingFunctions.EaseInQuartic] = Easings.EaseInQuartic;
            standardEasingFunctions[KnownEasingFunctions.EaseOutQuartic] = Easings.EaseOutQuartic;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutQuartic] = Easings.EaseInOutQuartic;
            standardEasingFunctions[KnownEasingFunctions.EaseInQuintic] = Easings.EaseInQuintic;
            standardEasingFunctions[KnownEasingFunctions.EaseOutQuintic] = Easings.EaseInQuintic;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutQuintic] = Easings.EaseInOutQuintic;
            standardEasingFunctions[KnownEasingFunctions.EaseInSin] = Easings.EaseInSin;
            standardEasingFunctions[KnownEasingFunctions.EaseOutSin] = Easings.EaseOutSin;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutSin] = Easings.EaseInOutSin;
            standardEasingFunctions[KnownEasingFunctions.EaseInExponential] = Easings.EaseInExponential;
            standardEasingFunctions[KnownEasingFunctions.EaseOutExponential] = Easings.EaseOutExponential;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutExponential] = Easings.EaseInOutExponential;
            standardEasingFunctions[KnownEasingFunctions.EaseInCircular] = Easings.EaseInCircular;
            standardEasingFunctions[KnownEasingFunctions.EaseOutCircular] = Easings.EaseOutCircular;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutCircular] = Easings.EaseInOutCircular;
            standardEasingFunctions[KnownEasingFunctions.EaseInBack] = Easings.EaseInBack;
            standardEasingFunctions[KnownEasingFunctions.EaseOutBack] = Easings.EaseOutBack;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutBack] = Easings.EaseInOutBack;
            standardEasingFunctions[KnownEasingFunctions.EaseInElastic] = Easings.EaseInElastic;
            standardEasingFunctions[KnownEasingFunctions.EaseOutElastic] = Easings.EaseOutElastic;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutElastic] = Easings.EaseInOutElastic;
            standardEasingFunctions[KnownEasingFunctions.EaseInBounce] = Easings.EaseInBounce;
            standardEasingFunctions[KnownEasingFunctions.EaseOutBounce] = Easings.EaseOutBounce;
            standardEasingFunctions[KnownEasingFunctions.EaseInOutBounce] = Easings.EaseInOutBounce;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocument"/> class with no rules or storyboards.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UvssDocument(UltravioletContext uv)
            : this(uv, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocument"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="rules">A collection containing the document's rules.</param>
        /// <param name="storyboards">A collection containing the document's storyboards.</param>
        public UvssDocument(UltravioletContext uv, IEnumerable<UvssRuleSet> rules, IEnumerable<UvssStoryboard> storyboards)
            : base(uv)
        {
            this.ruleSets = (rules ?? Enumerable.Empty<UvssRuleSet>())
                .ToList();

            this.storyboardDefinitions = (storyboards ?? Enumerable.Empty<UvssStoryboard>())
                .ToList();

            this.storyboardDefinitionsByName =
                new Dictionary<String, UvssStoryboard>(StringComparer.OrdinalIgnoreCase);

            this.storyboardInstancesByName =
                new Dictionary<String, Storyboard>(StringComparer.OrdinalIgnoreCase);

            foreach (var storyboardDefinition in storyboardDefinitions)
                storyboardDefinitionsByName[storyboardDefinition.Name] = storyboardDefinition;

            InstantiateStoryboards();

            CategorizeRuleSets();
        }

        /// <summary>
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified source text.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The source text from which to compile the document.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(UltravioletContext uv, String source)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(source, nameof(source));

            var document = UvssParser.Parse(source);

            return Compile(uv, document);
        }

        /// <summary>
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified stream.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the document to compile.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(UltravioletContext uv, Stream stream)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(stream, nameof(stream));

            using (var reader = new StreamReader(stream))
            {
                var source = reader.ReadToEnd();
                var document = UvssParser.Parse(source);

                return Compile(uv, document);
            }
        }

        /// <summary>
        /// Compiles an Ultraviolet Style Sheet (UVSS) document from the specified abstract syntax tree.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="tree">A <see cref="UvssDocumentSyntax"/> that represents the
        /// abstract syntax tree to compile.</param>
        /// <returns>A new instance of <see cref="UvssDocument"/> that represents the compiled data.</returns>
        public static UvssDocument Compile(UltravioletContext uv, UvssDocumentSyntax tree)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(tree, nameof(tree));

            return UvssCompiler.Compile(uv, tree);
        }

        /// <summary>
        /// Clears the document's lists of rules and storyboards.
        /// </summary>
        public void Clear()
        {
            ruleSets.Clear();
            storyboardDefinitions.Clear();
            storyboardDefinitionsByName.Clear();
            storyboardInstancesByName.Clear();
        }

        /// <summary>
        /// Appends another styling document to the end of this document.
        /// </summary>
        /// <param name="document">The document to append to the end of this document.</param>
        public void Append(UvssDocument document)
        {
            Contract.Require(document, nameof(document));

            Ultraviolet.ValidateResource(document);

            this.ruleSets.AddRange(document.RuleSets);
            this.storyboardDefinitions.AddRange(document.StoryboardDefinitions);

            foreach (var storyboardDefinition in document.storyboardDefinitions)
                this.storyboardDefinitionsByName[storyboardDefinition.Name] = storyboardDefinition;

            foreach (var storyboardInstance in document.storyboardInstancesByName)
                this.storyboardInstancesByName[storyboardInstance.Key] = storyboardInstance.Value;

            CategorizeRuleSets();
        }

        /// <summary>
        /// Gets a <see cref="Storyboard"/> instance for the storyboard with the specified name, if
        /// such a storyboard exists within the styling document.
        /// </summary>
        /// <param name="name">The name of the storyboard to retrieve.</param>
        /// <returns>The <see cref="Storyboard"/> instance that was retrieved, or 
        /// <see langword="null"/> if no such storyboard exists.</returns>
        public Storyboard GetStoryboardInstance(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            Storyboard storyboard;
            storyboardInstancesByName.TryGetValue(name, out storyboard);
            return storyboard;
        }

        /// <summary>
        /// Gets the document's rule sets.
        /// </summary>
        public IEnumerable<UvssRuleSet> RuleSets
        {
            get { return ruleSets; }
        }

        /// <summary>
        /// Gets the document's storyboard definitions.
        /// </summary>
        public IEnumerable<UvssStoryboard> StoryboardDefinitions
        {
            get { return storyboardDefinitions; }
        }

        /// <summary>
        /// Gets the document's storyboard instances.
        /// </summary>
        public IEnumerable<Storyboard> StoryboardInstances
        {
            get { return storyboardInstancesByName.Values; }
        }

        /// <summary>
        /// Gets the culture which is used for UVSS documents which do not specify a culture.
        /// </summary>
        internal static CultureInfo DefaultCulture
        {
            get { return CultureInfo.GetCultureInfo(String.Empty); }
        }

        /// <summary>
        /// Applies styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply styles.</param>
        internal void ApplyStyles(UIElement element)
        {
            Contract.Require(element, nameof(element));

            ApplyStylesInternal(element);
        }

        /// <summary>
        /// Retrieves the registered element type with the specified name.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The name of the type to retrieve.</param>
        /// <returns>The registered element type with the specified name.</returns>
        private static Type ResolveKnownType(UltravioletContext uv, String name)
        {
            Type type;
            if (!uv.GetUI().GetPresentationFoundation().GetKnownType(name, false, out type))
                throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(name));

            return type;
        }

        /// <summary>
        /// Retrieves the type of the dependency property with the specified name that
        /// matches the specified type filter.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="filter">The type filter for which to resolve a property type.</param>
        /// <param name="property">The property name for which to resolve a property type.</param>
        /// <returns>The type of the dependency property that was resolved.</returns>
        private static Type ResolvePropertyTypeFromFilter(UltravioletContext uv, IEnumerable<String> filter, ref String property)
        {
            var propertyName = property;

            var possiblePropertyMatches =
                from f in filter
                let elementType = ResolveKnownType(uv, f)
                let propertyID = DependencyProperty.FindByStylingName(propertyName, elementType)
                let propertyType = (propertyID == null) ? null : propertyID.PropertyType
                where propertyType != null
                select new { PropertyID = propertyID, PropertyType = propertyType };

            var distinctNames = possiblePropertyMatches.Select(x => x.PropertyID.Name).Distinct();
            if (distinctNames.Count() > 1)
                throw new InvalidOperationException(PresentationStrings.AmbiguousDependencyProperty.Format(property));

            if (distinctNames.Any())
                property = distinctNames.Single();

            var distinctTypes = possiblePropertyMatches.Select(x => x.PropertyType).Distinct();
            if (distinctTypes.Count() > 1)
                throw new InvalidOperationException(PresentationStrings.AmbiguousDependencyProperty.Format(property));

            return distinctTypes.FirstOrDefault();
        }

        /// <summary>
        /// Gets the type of <see cref="Animation{T}"/> which is used to animate
        /// the specified type.
        /// </summary>
        /// <param name="type">The type which is being animated.</param>
        /// <returns>The type of <see cref="Animation{T}"/> which is used to animate 
        /// the specified type.</returns>
        private static Type GetAnimationType(Type type)
        {
            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return typeof(Animation<>).MakeGenericType(nullable ? type.GetGenericArguments()[0] : type);
        }

        /// <summary>
        /// Creates new <see cref="Storyboard"/> instances based on the current set of <see cref="UvssStoryboard"/> definitions.
        /// </summary>
        private void InstantiateStoryboards()
        {
            storyboardInstancesByName.Clear();

            foreach (var storyboardDefinition in storyboardDefinitions)
            {
                var storyboardInstance = InstantiateStoryboard(storyboardDefinition);
                storyboardInstancesByName[storyboardDefinition.Name] = storyboardInstance;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Storyboard"/> instance from the specified storyboard definition.
        /// </summary>
        /// <param name="storyboardDefinition">The storyboard definition to instantiate.</param>
        /// <returns>The <see cref="Storyboard"/> instance that was created.</returns>
        private Storyboard InstantiateStoryboard(UvssStoryboard storyboardDefinition)
        {
            var storyboardInstance = new Storyboard(
                Ultraviolet, storyboardDefinition.LoopBehavior);

            foreach (var targetDefinition in storyboardDefinition.Targets)
            {
                var targetInstance = InstantiateStoryboardTarget(targetDefinition);
                storyboardInstance.Targets.Add(targetInstance);
            }

            return storyboardInstance;
        }

        /// <summary>
        /// Creates a new <see cref="StoryboardTarget"/> instance from the specified target definition.
        /// </summary>
        /// <param name="targetDefinition">The storyboard target definition to instantiate.</param>
        /// <returns>The <see cref="StoryboardTarget"/> instance that was created.</returns>
        private StoryboardTarget InstantiateStoryboardTarget(UvssStoryboardTarget targetDefinition)
        {
            var target = new StoryboardTarget(targetDefinition.Selector);

            foreach (var animationDefinition in targetDefinition.Animations)
            {
                var ownerType = default(Type);
                if (animationDefinition.AnimatedProperty.IsAttached)
                    PresentationFoundation.Instance.GetKnownType(animationDefinition.AnimatedProperty.Owner, out ownerType);

                var animatedPropertyName = animationDefinition.AnimatedProperty.Name;
                var animatedPropertyType = default(Type);
                if (animationDefinition.AnimatedProperty.IsAttached)
                {
                    var dp = DependencyPropertySystem.FindByStylingName(animatedPropertyName, ownerType);
                    if (dp != null)
                    {
                        animatedPropertyType = dp.PropertyType;
                        animatedPropertyName = dp.Name;
                    }
                }
                else
                {
                    animatedPropertyType = ResolvePropertyTypeFromFilter(Ultraviolet, targetDefinition.Filter, ref animatedPropertyName);
                }

                var navigationExpression = default(NavigationExpression?);
                var navigationExpressionDef = animationDefinition.NavigationExpression;
                if (navigationExpressionDef != null)
                {
                    animatedPropertyType = ResolvePropertyTypeFromFilter(Ultraviolet,
                        new[] { navigationExpressionDef.NavigationPropertyType }, ref animatedPropertyName);

                    navigationExpression = NavigationExpression.FromUvssNavigationExpression(Ultraviolet, navigationExpressionDef);
                }

                var animationDependencyName = new DependencyName(animationDefinition.AnimatedProperty.IsAttached ? 
                    $"{animationDefinition.AnimatedProperty.Owner}.{animatedPropertyName}" : animatedPropertyName);
                var animationKey = new StoryboardTargetAnimationKey(animationDependencyName, navigationExpression);
                var animation = InstantiateStoryboardAnimation(animationDefinition, animatedPropertyType);
                if (animation != null)
                {
                    target.Animations.Add(animationKey, animation);
                }
            }

            return target;
        }

        /// <summary>
        /// Creates a new <see cref="AnimationBase"/> instance from the specified animation definition.
        /// </summary>
        /// <param name="animationDefinition">The animation definition to instantiate.</param>
        /// <param name="animatedPropertyType">The type of property which is being animated.</param>
        /// <returns>The <see cref="AnimationBase"/> instance that was created.</returns>
        private AnimationBase InstantiateStoryboardAnimation(UvssStoryboardAnimation animationDefinition, Type animatedPropertyType)
        {
            if (animatedPropertyType == null)
                return null;

            var animationType = GetAnimationType(animatedPropertyType);
            if (animationType == null)
                return null;

            var animation = (AnimationBase)Activator.CreateInstance(animationType);

            foreach (var keyframeDefinition in animationDefinition.Keyframes)
            {
                var keyframe = InstantiateStoryboardAnimationKeyframe(keyframeDefinition, animatedPropertyType);
                animation.AddKeyframe(keyframe);
            }

            return animation;
        }

        /// <summary>
        /// Creates a new <see cref="AnimationKeyframeBase"/> instance from the specified keyframe definition.
        /// </summary>
        /// <param name="keyframeDefinition">The keyframe definition to instantiate.</param>
        /// <param name="keyframeValueType">The type of value being animated by the keyframe.</param>
        /// <returns>The <see cref="AnimationKeyframeBase"/> instance that was created.</returns>
        private AnimationKeyframeBase InstantiateStoryboardAnimationKeyframe(UvssStoryboardKeyframe keyframeDefinition, Type keyframeValueType)
        {
            var time = TimeSpan.FromMilliseconds(keyframeDefinition.Time);

            var easing = default(EasingFunction);
            if (!String.IsNullOrEmpty(keyframeDefinition.Easing))
                standardEasingFunctions.TryGetValue(keyframeDefinition.Easing, out easing);

            var valueDef = keyframeDefinition.Value;
            var value = valueDef.IsEmpty ? null :
                ObjectResolver.FromString(valueDef.Value, keyframeValueType, valueDef.Culture, true);

            var keyframeType = typeof(AnimationKeyframe<>).MakeGenericType(keyframeValueType);
            var keyframeInstance = (value == null) ?
                (AnimationKeyframeBase)Activator.CreateInstance(keyframeType, time, easing) :
                (AnimationKeyframeBase)Activator.CreateInstance(keyframeType, time, value, easing);

            return keyframeInstance;
        }

        /// <summary>
        /// Adds the specified rule set to the style prioritizer.
        /// </summary>
        private void AddRuleSetToPrioritizer(UIElement element, UvssSelector selector, UvssRuleSet ruleSet, Int32 index)
        {
            if (!selector.MatchesElement(element))
                return;

            var navexp = NavigationExpression.FromUvssNavigationExpression(Ultraviolet, selector.NavigationExpression);

            foreach (var rule in ruleSet.Rules)
                prioritizer.Add(Ultraviolet, selector, navexp, rule, index);

            foreach (var trigger in ruleSet.Triggers)
                prioritizer.Add(Ultraviolet, selector, navexp, trigger, index);
        }

        /// <summary>
        /// Applies styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply styles.</param>
        private void ApplyStylesInternal(UIElement element)
        {
            try
            {
                element.ClearStyledValues(false);

                // Prioritize styles from name.
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null && !String.IsNullOrEmpty(frameworkElement.Name))
                {
                    var ruleSetsByName = GetRuleSetsByName(frameworkElement.Name, onlyIfExists: true);
                    if (ruleSetsByName != null)
                    {
                        foreach (var categorized in ruleSetsByName)
                            AddRuleSetToPrioritizer(element, categorized.Selector, categorized.RuleSet, categorized.Index);
                    }
                }

                // Prioritize styles from class.
                foreach (var @class in element.Classes)
                {
                    var ruleSetsByCategory = GetRuleSetsByClass(@class, onlyIfExists: true);
                    if (ruleSetsByCategory != null)
                    {
                        foreach (var categorized in ruleSetsByCategory)
                            AddRuleSetToPrioritizer(element, categorized.Selector, categorized.RuleSet, categorized.Index);
                    }
                }

                // Prioritize rules from type.
                var ruleSetsByType = GetRuleSetsByType(element.UvmlName, onlyIfExists: true);
                if (ruleSetsByType != null)
                {
                    foreach (var categorized in ruleSetsByType)
                        AddRuleSetToPrioritizer(element, categorized.Selector, categorized.RuleSet, categorized.Index);
                }

                // Prioritize uncategorized styles.
                foreach (var categorized in ruleSetsWithoutCategory)
                    AddRuleSetToPrioritizer(element, categorized.Selector, categorized.RuleSet, categorized.Index);

                // Apply styles to element
                prioritizer.Apply(element);
            }
            finally
            {
                // Reset the prioritizer even if something blows up.
                prioritizer.Reset();
            }
        }
        
        /// <summary>
        /// Sorts the document's rule sets into categories for faster querying.
        /// </summary>
        private void CategorizeRuleSets()
        {
            ruleSetsWithoutCategory.Clear();
            ruleSetsByName.Clear();
            ruleSetsByType.Clear();
            ruleSetsByClass.Clear();

            for (int i = 0; i < ruleSets.Count; i++)
            {
                var ruleSet = ruleSets[i];

                foreach (var selector in ruleSet.Selectors)
                {
                    var lastSelectorPart = selector[selector.PartCount - 1];
                    var categorized = false;

                    if (!categorized && lastSelectorPart.HasName)
                    {
                        var category = GetRuleSetsByName(lastSelectorPart.Name);
                        category.Add(new CategorizedRuleSet(selector, ruleSet, i));
                        categorized = true;
                    }
                    
                    if (!categorized && lastSelectorPart.HasClasses)
                    {
                        foreach (var @class in lastSelectorPart.Classes)
                        {
                            var category = GetRuleSetsByClass(@class);
                            category.Add(new CategorizedRuleSet(selector, ruleSet, i));
                            categorized = true;
                        }
                    }

                    if (!categorized && lastSelectorPart.HasExactType)
                    {
                        var category = GetRuleSetsByType(lastSelectorPart.Type);
                        category.Add(new CategorizedRuleSet(selector, ruleSet, i));
                        categorized = true;
                    }

                    if (!categorized)
                        GetRuleSetsWithoutCategory().Add(new CategorizedRuleSet(selector, ruleSet, i));
                }
            }
        }

        /// <summary>
        /// Gets the list of rule sets which have no particular category.
        /// </summary>
        private List<CategorizedRuleSet> GetRuleSetsWithoutCategory()
        {
            return ruleSetsWithoutCategory;
        }

        /// <summary>
        /// Gets the list of rule sets which apply to the specified name.
        /// </summary>
        private List<CategorizedRuleSet> GetRuleSetsByName(String name, Boolean onlyIfExists = false)
        {
            return GetRuleSetCategory(ruleSetsByName, name, onlyIfExists);
        }

        /// <summary>
        /// Gets the list of rule sets which apply to the specified type.
        /// </summary>
        private List<CategorizedRuleSet> GetRuleSetsByType(String element, Boolean onlyIfExists = false)
        {
            return GetRuleSetCategory(ruleSetsByType, element, onlyIfExists);
        }

        /// <summary>
        /// Gets the list of rule sets which apply to the specified class.
        /// </summary>
        private List<CategorizedRuleSet> GetRuleSetsByClass(String @class, Boolean onlyIfExists = false)
        {
            return GetRuleSetCategory(ruleSetsByClass, @class, onlyIfExists);
        }

        /// <summary>
        /// Gets the list of rule sets which apply to the specified category.
        /// </summary>
        private List<CategorizedRuleSet> GetRuleSetCategory(Dictionary<String, List<CategorizedRuleSet>> ruleSets, 
            String key, Boolean onlyIfExists = false)
        {
            List<CategorizedRuleSet> category;
            if (!ruleSets.TryGetValue(key, out category))
            {
                if (onlyIfExists)
                    return null;

                category = new List<CategorizedRuleSet>();
                ruleSets[key] = category;
            }
            return category;
        }

        // The standard easing functions which can be specified in an animation.
        private static readonly Dictionary<String, EasingFunction> standardEasingFunctions =
            new Dictionary<String, EasingFunction>();

        // State values.
        private readonly UvssStylePrioritizer prioritizer = new UvssStylePrioritizer();

        // Property values.
        private readonly List<UvssRuleSet> ruleSets;
        private readonly List<UvssStoryboard> storyboardDefinitions;
        private readonly Dictionary<String, UvssStoryboard> storyboardDefinitionsByName;
        private readonly Dictionary<String, Storyboard> storyboardInstancesByName;

        // Categorize rule sets based on their selectors.
        private readonly Dictionary<String, List<CategorizedRuleSet>> ruleSetsByName =
            new Dictionary<String, List<CategorizedRuleSet>>();
        private readonly Dictionary<String, List<CategorizedRuleSet>> ruleSetsByType =
            new Dictionary<String, List<CategorizedRuleSet>>();
        private readonly Dictionary<String, List<CategorizedRuleSet>> ruleSetsByClass =
            new Dictionary<String, List<CategorizedRuleSet>>();

        private readonly List<CategorizedRuleSet> ruleSetsWithoutCategory = 
            new List<CategorizedRuleSet>();
    }
}
