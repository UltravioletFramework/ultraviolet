using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a method which is used to determine the plurality group associated with a specified quantity.
    /// </summary>
    /// <param name="source">The localized string which is being pluralized.</param>
    /// <param name="quantity">The quantity for which to determine a plurality group.</param>
    /// <returns>The name of the plurality group associated with the specified quantity.</returns>
    public delegate String LocalizationPluralityEvaluator(LocalizedString source, Int32 quantity);

    /// <summary>
    /// Represents a method which is used to determine which of the specified source string's variants
    /// is the best match for the specified target variant.
    /// </summary>
    /// <param name="source">The source string from which to retrieve a variant.</param>
    /// <param name="target">The target string to match.</param>
    /// <returns>The variant of the source string that is the best match for the specified target variant.</returns>
    public delegate String LocalizationMatchEvaluator(LocalizedString source, LocalizedStringVariant target);

    /// <summary>
    /// Contains methods for managing the application's localized string tables.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Initializes the <see cref="Localization"/> type.
        /// </summary>
        static Localization()
        {
            RegisterStandardPluralityEvaluators();
            RegisterStandardMatchEvaluators();
        }
        
        /// <summary>
        /// Loads any localization plugins defined in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly that contains the plugins to load.</param>
        public static void LoadPlugins(Assembly asm)
        {
            Contract.Require(asm, nameof(asm));

            var plugins = from type in asm.GetTypes()
                          where
                            type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(ILocalizationPlugin))
                          select type;

            foreach (var plugin in plugins)
            {
                var ctor = plugin.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                    throw new InvalidOperationException(CoreStrings.MissingDefaultCtor.Format(plugin.FullName));

                var instance = (ILocalizationPlugin)ctor.Invoke(null);
                LoadPlugin(instance);
            }
        }

        /// <summary>
        /// Loads the specified localization plugin.
        /// </summary>
        /// <param name="plugin">The localization plugin to load.</param>
        public static void LoadPlugin(ILocalizationPlugin plugin)
        {
            Contract.Require(plugin, nameof(plugin));

            var culture = plugin.Culture;
            if (culture != null)
            {
                var pluralityEvaluators = plugin.GetPluralityEvaluators();
                if (pluralityEvaluators != null)
                {
                    foreach (var evaluator in pluralityEvaluators)
                        RegisterPluralityEvaluatorForCulture(culture, evaluator);
                }

                var matchEvaluators = plugin.GetMatchEvaluators();
                if (matchEvaluators != null)
                {
                    foreach (var evaluator in matchEvaluators)
                        RegisterMatchEvaluatorForCulture(culture, evaluator.Name, evaluator.Evaluator);
                }
            }
            else
            {
                var language = plugin.Language;
                if (language != null)
                {
                    var pluralityEvaluators = plugin.GetPluralityEvaluators();
                    if (pluralityEvaluators != null)
                    {
                        foreach (var evaluator in pluralityEvaluators)
                            RegisterPluralityEvaluatorForLanguage(language, evaluator);
                    }

                    var matchEvaluators = plugin.GetMatchEvaluators();
                    if (matchEvaluators != null)
                    {
                        foreach (var evaluator in matchEvaluators)
                            RegisterMatchEvaluatorForLanguage(language, evaluator.Name, evaluator.Evaluator);
                    }
                }
            }
        }

        /// <summary>
        /// Registers a plurality evaluator function for the specified culture. Plurality evaluators are used to determine 
        /// which string variant to use for a given quantity of items.
        /// </summary>
        /// <param name="culture">The culture for which to register a plurality evaluator.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterPluralityEvaluatorForCulture(String culture, LocalizationPluralityEvaluator evaluator)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.Require(evaluator, nameof(evaluator));

            registeredPluralityEvaluatorsByCulture[culture] = evaluator;
        }

        /// <summary>
        /// Registers a match evaluator function for the specified culture. Match evaluators are used to determine 
        /// how to make a localized string match another string variant.
        /// </summary>
        /// <param name="culture">The culture for which to register a match evaluator.</param>
        /// <param name="name">The evaluator's unique name.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterMatchEvaluatorForCulture(String culture, String name, LocalizationMatchEvaluator evaluator)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.Require(evaluator, nameof(evaluator));

            if (!registeredMatchEvaluatorByCulture.ContainsKey(culture))
                registeredMatchEvaluatorByCulture[culture] = new Dictionary<String, LocalizationMatchEvaluator>();

            registeredMatchEvaluatorByCulture[culture][name] = evaluator;
        }

        /// <summary>
        /// Registers a plurality evaluator function for the specified language. Plurality evaluators are used to determine 
        /// which string variant to use for a given quantity of items.
        /// </summary>
        /// <param name="language">The language for which to register a plurality evaluator.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterPluralityEvaluatorForLanguage(String language, LocalizationPluralityEvaluator evaluator)
        {
            Contract.RequireNotEmpty(language, nameof(language));
            Contract.Require(evaluator, nameof(evaluator));

            registeredPluralityEvaluatorsByLanguage[language] = evaluator;
        }

        /// <summary>
        /// Registers a match evaluator function for the specified language. Match evaluators are used to determine 
        /// how to make a localized string match another string variant.
        /// </summary>
        /// <param name="language">The language for which to register a match evaluator.</param>
        /// <param name="name">The evaluator's unique name.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterMatchEvaluatorForLanguage(String language, String name, LocalizationMatchEvaluator evaluator)
        {
            Contract.RequireNotEmpty(language, nameof(language));
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.Require(evaluator, nameof(evaluator));

            if (!registeredMatchEvaluatorsByLanguage.ContainsKey(language))
                registeredMatchEvaluatorsByLanguage[language] = new Dictionary<String, LocalizationMatchEvaluator>();

            registeredMatchEvaluatorsByLanguage[language][name] = evaluator;
        }

        /// <summary>
        /// Removes all registered plurality evaluators and restores the defaults.
        /// </summary>
        public static void ResetPluralityEvaluators()
        {
            registeredPluralityEvaluatorsByCulture.Clear();
            registeredPluralityEvaluatorsByLanguage.Clear();
            RegisterStandardPluralityEvaluators();
        }

        /// <summary>
        /// Removes all registered match evaluators and restores the defaults.
        /// </summary>
        public static void ResetMatchEvaluators()
        {
            registeredMatchEvaluatorByCulture.Clear();
            registeredMatchEvaluatorsByLanguage.Clear();
            RegisterStandardMatchEvaluators();
        }

        /// <summary>
        /// Gets the plurality group associated with the specified culture and quantity.
        /// </summary>
        /// <param name="culture">The culture for which to evaluate a plurality group.</param>
        /// <param name="source">The localized string which is being pluralized.</param>
        /// <param name="quantity">The quantity for which to evaluate a plurality group.</param>
        /// <returns>The plurality group associated with the specified culture and quantity.</returns>
        public static String GetPluralityGroup(String culture, LocalizedString source, Int32 quantity)
        {
            return GetPluralityGroup(culture, null, source, quantity);
        }

        /// <summary>
        /// Gets the plurality group associated with the specified culture and quantity.
        /// </summary>
        /// <param name="culture">The culture for which to evaluate a plurality group.</param>
        /// <param name="language">The language for which to evaluate a plurality group, if no culture match is found.</param>
        /// <param name="source">The localized string which is being pluralized.</param>
        /// <param name="quantity">The quantity for which to evaluate a plurality group.</param>
        /// <returns>The plurality group associated with the specified culture and quantity.</returns>
        public static String GetPluralityGroup(String culture, String language, LocalizedString source, Int32 quantity)
        {
            Contract.Require(source, nameof(source));
            Contract.RequireNotEmpty(culture, nameof(culture));
            
            if (registeredPluralityEvaluatorsByCulture.TryGetValue(culture, out var evaluator))
                return evaluator(source, quantity);

            if (language != null)
            {
                if (registeredPluralityEvaluatorsByLanguage.TryGetValue(language, out evaluator))
                    return evaluator(source, quantity);
            }

            return (quantity == 1) ? "singular" : "plural";
        }

        /// <summary>
        /// Gets the plurality group associated with the current culture and the specified quantity.
        /// </summary>
        /// <param name="source">The localized string which is being pluralized.</param>
        /// <param name="count">The quantity for which to evaluate a plurality group.</param>
        /// <returns>The plurality group associated with the current culture and the specified quantity.</returns>
        public static String GetPluralityGroup(LocalizedString source, Int32 count)
        {
            return GetPluralityGroup(CurrentCulture, CurrentLanguage, source, count);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the current culture and the specified rule.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(LocalizedString source, LocalizedStringVariant target, String rule)
        {
            Contract.Require(target, nameof(target));
            Contract.RequireNotEmpty(rule, nameof(rule));

            return MatchVariantInternal(CurrentCulture, CurrentLanguage, source, target, rule);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        /// <param name="culture">The culture for which to perform the match.</param>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(String culture, LocalizedString source, LocalizedStringVariant target, String rule)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.Require(target, nameof(target));
            Contract.RequireNotEmpty(rule, nameof(rule));

            return MatchVariantInternal(culture, null, source, target, rule);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        /// <param name="culture">The culture for which to perform the match.</param>
        /// <param name="language">The language for which to perform the match, if no culture match is found.</param>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(String culture, String language, LocalizedString source, LocalizedStringVariant target, String rule)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.RequireNotEmpty(language, nameof(language));
            Contract.Require(target, nameof(target));
            Contract.RequireNotEmpty(rule, nameof(rule));

            return MatchVariantInternal(culture, language, source, target, rule);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the current culture and the specified rule.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(LocalizedString source, LocalizedStringVariant target, StringSegment rule)
        {
            Contract.Require(target, nameof(target));

            return MatchVariantInternal(CurrentCulture, CurrentLanguage, source, target, rule);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        /// <param name="culture">The culture for which to perform the match.</param>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(String culture, LocalizedString source, LocalizedStringVariant target, StringSegment rule)
        {
            return MatchVariantInternal(culture, null, source, target, rule);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        /// <param name="culture">The culture for which to perform the match.</param>
        /// <param name="language">The language for which to perform the match, if no culture match is found.</param>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <param name="rule">The rule which defines how to perform the match.</param>
        /// <returns>The variant of <paramref name="source"/> which is the best match for <paramref name="target"/> according to the specified rule.</returns>
        public static String MatchVariant(String culture, String language, LocalizedString source, LocalizedStringVariant target, StringSegment rule)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.Require(target, nameof(target));

            return MatchVariantInternal(culture, language, source, target, rule);
        }

        /// <summary>
        /// Retrieves the string with the specified key from the specified culture.
        /// </summary>
        /// <param name="culture">The culture from which to retrieve the string.</param>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The <see cref="LocalizedString"/> that was retrieved.</returns>
        public static LocalizedString Get(String culture, String key)
        {
            return Strings.Get(culture, key);
        }

        /// <summary>
        /// Retrieves the string with the specified key from the current culture.
        /// </summary>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The <see cref="LocalizedString"/> that was retrieved.</returns>
        public static LocalizedString Get(String key)
        {
            return Strings.Get(key);
        }

        /// <summary>
        /// Gets the application's default localization database.
        /// </summary>
        public static LocalizationDatabase Strings { get; } = new LocalizationDatabase();

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        public static String CurrentCulture
        {
            get 
            {
                var culture = Thread.CurrentThread.CurrentCulture;
                UpdateCurrentCulture(culture);
                return currentCultureName;
            }
            set { Thread.CurrentThread.CurrentCulture = new CultureInfo(value ?? "en-US"); }
        }

        /// <summary>
        /// Gets the language code associated with the current culture.
        /// </summary>
        public static String CurrentLanguage
        {
            get 
            {
                var culture = Thread.CurrentThread.CurrentCulture;
                UpdateCurrentCulture(culture);
                return currentCultureLanguage;
            }
        }

        /// <summary>
        /// Gets the display name of the current culture.
        /// </summary>
        public static String CurrentCultureDisplayName
        {
            get
            {
                var culture = Thread.CurrentThread.CurrentCulture;
                UpdateCurrentCulture(culture);
                return currentCultureDisplayName;
            }
        }

        /// <summary>
        /// Gets the culture code associated with the pseudolocalized culture.
        /// </summary>
        public static String PseudolocalizedCulture
        {
            get { return "qpos-ploc"; }
        }

        /// <summary>
        /// Updates the cached culture info.
        /// </summary>
        private static void UpdateCurrentCulture(CultureInfo current)
        {
            if (currentCulture == current)
                return;

            currentCulture = current;

            var name = currentCulture.Name;
            if (!String.IsNullOrWhiteSpace(name))
            {
                currentCultureName = name;
            }
            else
            {
                var currentRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
                var currentLanguage = currentCulture.TwoLetterISOLanguageName;
                if (!String.IsNullOrWhiteSpace(currentRegion) && !String.IsNullOrWhiteSpace(currentLanguage))
                {
                    currentCultureName = $"{currentLanguage}-{currentRegion}";
                }
                else
                {
                    currentCultureName = "en-US";
                }
            }

            var lang = currentCulture.TwoLetterISOLanguageName;
            if (!String.IsNullOrWhiteSpace(lang))
            {
                currentCultureLanguage = lang;
            }
            else
            {
                currentCultureLanguage = "en";
            }

            var displayName = currentCulture.DisplayName;
            if (!String.IsNullOrWhiteSpace(displayName))
            {
                currentCultureDisplayName = displayName;
            }
            else
            {
                displayName = currentCulture.EnglishName;
                if (!String.IsNullOrWhiteSpace(displayName))
                {
                    currentCultureDisplayName = displayName;
                }
                else
                {
                    currentCultureDisplayName = "Unknown";
                }
            }
        }

        /// <summary>
        /// Registers the localization system's standard plural group evaluators.
        /// </summary>
        private static void RegisterStandardPluralityEvaluators()
        {
            RegisterPluralityEvaluatorForLanguage("en", (source, qty) =>
            {
                return qty == 1 ? "singular" : "plural";
            });
        }
        
        /// <summary>
        /// Registers the localization system's standard match evaluators.
        /// </summary>
        private static void RegisterStandardMatchEvaluators()
        {
            RegisterMatchEvaluatorForLanguage("en", "Indef_Art", (str, target) =>
            {
                if (target.HasProperty("vowel")) return "An";
                return "A";
            });
            RegisterMatchEvaluatorForLanguage("en", "indef_art", (str, target) =>
            {
                if (target.HasProperty("vowel")) return "an";
                return "a";
            });
        }
        
        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        private static String MatchVariantInternal(String culture, String language, LocalizedString source, LocalizedStringVariant target, String rule)
        {
            if (registeredMatchEvaluatorByCulture.TryGetValue(culture, out var registry))
            {
                if (registry.TryGetValue(rule, out var evaluator))
                    return evaluator(source, target);
            }

            if (language != null)
            {
                if (registeredMatchEvaluatorsByLanguage.TryGetValue(language, out registry))
                {
                    if (registry.TryGetValue(rule, out var evaluator))
                        return evaluator(source, target);
                }
            }

            return null;
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        private static String MatchVariantInternal(String culture, String language, LocalizedString source, LocalizedStringVariant target, StringSegment rule)
        {
            if (registeredMatchEvaluatorByCulture.TryGetValue(culture, out var registry))
            {
                foreach (var kvp in registry)
                {
                    if (rule.Equals(kvp.Key))
                    {
                        return (kvp.Value == null) ? null : kvp.Value(source, target);
                    }
                }
            }

            if (language != null)
            {
                if (registeredMatchEvaluatorsByLanguage.TryGetValue(language, out registry))
                {
                    foreach (var kvp in registry)
                    {
                        if (rule.Equals(kvp.Key))
                        {
                            return (kvp.Value == null) ? null : kvp.Value(source, target);
                        }
                    }

                }
            }

            return null;
        }

        // Plurality and match evaluators.
        private static readonly Dictionary<String, LocalizationPluralityEvaluator> registeredPluralityEvaluatorsByCulture =
            new Dictionary<String, LocalizationPluralityEvaluator>();
        private static readonly Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>> registeredMatchEvaluatorByCulture =
            new Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>>();

        private static readonly Dictionary<String, LocalizationPluralityEvaluator> registeredPluralityEvaluatorsByLanguage =
            new Dictionary<String, LocalizationPluralityEvaluator>();
        private static readonly Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>> registeredMatchEvaluatorsByLanguage =
            new Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>>();

        // Cached culture info.
        private static CultureInfo currentCulture;
        private static String currentCultureName;
        private static String currentCultureLanguage;
        private static String currentCultureDisplayName;
    }
}
