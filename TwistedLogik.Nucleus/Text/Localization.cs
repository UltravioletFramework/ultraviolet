using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TwistedLogik.Nucleus.Text
{    
    /// <summary>
    /// Represents a method which is used to determine the plurality group associated with a specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity for which to determine a plurality group.</param>
    /// <returns>The name of the plurality group associated with the specified quantity.</returns>
    public delegate String LocalizationPluralityEvaluator(Int32 quantity);

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
            Contract.Require(asm, "asm");

            var plugins = from type in asm.GetTypes()
                          where
                            type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(ILocalizationPlugin))
                          select type;

            foreach (var plugin in plugins)
            {
                var ctor = plugin.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                    throw new InvalidOperationException(NucleusStrings.MissingDefaultCtor.Format(plugin.FullName));

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
            Contract.Require(plugin, "plugin");

            var cultures = plugin.Cultures ?? new String[0];

            var pluralityEvaluators = plugin.GetPluralityEvaluators();
            if (pluralityEvaluators != null)
            {
                foreach (var evaluator in pluralityEvaluators)
                {
                    foreach (var culture in cultures)
                        RegisterPluralityEvaluator(culture, evaluator);
                }
            }

            var matchEvaluators = plugin.GetMatchEvaluators();
            if (matchEvaluators != null)
            {
                foreach (var evaluator in matchEvaluators)
                {
                    foreach (var culture in cultures)
                        RegisterMatchEvaluator(culture, evaluator.Name, evaluator.Evaluator);
                }
            }
        }

        /// <summary>
        /// <para>Registers a plurality evaluator function for the specified culture.</para>
        /// <para>Plurality evaluators are used to determine which string variant to use for a given quantity of items.</para>
        /// </summary>
        /// <param name="culture">The culture for which to register a plurality evaluator.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterPluralityEvaluator(String culture, LocalizationPluralityEvaluator evaluator)
        {
            Contract.RequireNotEmpty(culture, "culture");
            Contract.Require(evaluator, "evaluator");

            registeredPluralityEvaluators[culture] = evaluator;
        }

        /// <summary>
        /// <para>Registers a match evaluator function for the specified culture.</para>
        /// <para>Match evaluators are used to determine how to make a localized string match another string variant.</para>
        /// </summary>
        /// <param name="culture">The culture for which to register a match evaluator.</param>
        /// <param name="name">The evaluator's unique name.</param>
        /// <param name="evaluator">The evaluator to register.</param>
        public static void RegisterMatchEvaluator(String culture, String name, LocalizationMatchEvaluator evaluator)
        {
            Contract.RequireNotEmpty(culture, "culture");
            Contract.RequireNotEmpty(name, "name");
            Contract.Require(evaluator, "evaluator");

            if (!registeredMatchEvaluators.ContainsKey(culture))
            {
                registeredMatchEvaluators[culture] = new Dictionary<String, LocalizationMatchEvaluator>();
            }
            registeredMatchEvaluators[culture][name] = evaluator;
        }

        /// <summary>
        /// Removes all registered plurality evaluators and restores the defaults.
        /// </summary>
        public static void ResetPluralityEvaluators()
        {
            registeredPluralityEvaluators.Clear();
            RegisterStandardPluralityEvaluators();
        }

        /// <summary>
        /// Removes all registered match evaluators and restores the defaults.
        /// </summary>
        public static void ResetMatchEvaluators()
        {
            registeredMatchEvaluators.Clear();
            RegisterStandardMatchEvaluators();
        }

        /// <summary>
        /// Gets the plurality group associated with the specified culture and quantity.
        /// </summary>
        /// <param name="culture">The culture for which to evaluate a plurality group.</param>
        /// <param name="quantity">The quantity for which to evaluate a plurality group.</param>
        /// <returns>The plurality group associated with the specified culture and quantity.</returns>
        public static String GetPluralityGroup(String culture, Int32 quantity)
        {
            Contract.RequireNotEmpty(culture, "culture");

            LocalizationPluralityEvaluator evaluator;
            if (registeredPluralityEvaluators.TryGetValue(culture, out evaluator))
            {
                return evaluator(quantity);
            }
            return (quantity == 1) ? "singular" : "plural";
        }

        /// <summary>
        /// Gets the plurality group associated with the current culture and the specified quantity.
        /// </summary>
        /// <param name="count">The quantity for which to evaluate a plurality group.</param>
        /// <returns>The plurality group associated with the current culture and the specified quantity.</returns>
        public static String GetPluralityGroup(Int32 count)
        {
            return GetPluralityGroup(CurrentCulture, count);
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
            Contract.Require(target, "target");
            Contract.RequireNotEmpty(rule, "rule");

            return MatchVariantInternal(CurrentCulture, source, target, rule);
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
            Contract.RequireNotEmpty(culture, "culture");
            Contract.Require(target, "target");
            Contract.RequireNotEmpty(rule, "rule");

            return MatchVariantInternal(culture, source, target, rule);
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
            Contract.Require(target, "target");

            return MatchVariantInternal(CurrentCulture, source, target, rule);
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
            Contract.RequireNotEmpty(culture, "culture");
            Contract.Require(target, "target");

            return MatchVariantInternal(culture, source, target, rule);
        }

        /// <summary>
        /// Retrieves the string with the specified key from the specified culture.
        /// </summary>
        /// <param name="culture">The culture from which to retrieve the string.</param>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The <see cref="LocalizedString"/> that was retrieved.</returns>
        public static LocalizedString Get(String culture, String key)
        {
            return strings.Get(culture, key);
        }

        /// <summary>
        /// Retrieves the string with the specified key from the current culture.
        /// </summary>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The <see cref="LocalizedString"/> that was retrieved.</returns>
        public static LocalizedString Get(String key)
        {
            return strings.Get(key);
        }

        /// <summary>
        /// Gets the application's default localization database.
        /// </summary>
        public static LocalizationDatabase Strings
        {
            get { return strings; }
        }

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        public static String CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture.Name; }
            set { Thread.CurrentThread.CurrentCulture = new CultureInfo(value ?? "en-US"); }
        }

        /// <summary>
        /// Gets the display name of the current culture.
        /// </summary>
        public static String CurrentCultureDisplayName
        {
            get { return Thread.CurrentThread.CurrentCulture.DisplayName; }
        }

        /// <summary>
        /// Gets the culture code associated with the pseudolocalized culture.
        /// </summary>
        public static String PseudolocalizedCulture
        {
            get { return "qpos-ploc"; }
        }

        /// <summary>
        /// Registers the localization system's standard plural group evaluators.
        /// </summary>
        private static void RegisterStandardPluralityEvaluators()
        {
            RegisterStandardPluralityEvaluators_en("en-US");
            RegisterStandardPluralityEvaluators_en("en-GB");
            RegisterStandardPluralityEvaluators_en("en-AU");
            RegisterStandardPluralityEvaluators_fr("fr-FR");
            RegisterStandardPluralityEvaluators_fr("fr-CA");
        }

        /// <summary>
        /// Registers the standard plural group evaluators for the English language.
        /// </summary>
        /// <param name="culture">The culture for which to register the evaluators.</param>
        private static void RegisterStandardPluralityEvaluators_en(String culture)
        {
            RegisterPluralityEvaluator(culture, (qty) =>
            {
                return qty == 1 ? "singular" : "plural";
            });
        }

        /// <summary>
        /// Registers the standard plurality evaluators for the French language.
        /// </summary>
        /// <param name="culture">The culture for which to register the evaluators.</param>
        private static void RegisterStandardPluralityEvaluators_fr(String culture)
        {
            RegisterPluralityEvaluator(culture, (qty) =>
            {
                return qty == 1 ? "singular" : "plural";
            });
        }

        /// <summary>
        /// Registers the localization system's standard match evaluators.
        /// </summary>
        private static void RegisterStandardMatchEvaluators()
        {
            RegisterStandardMatchEvaluators_en("en-US");
            RegisterStandardMatchEvaluators_en("en-GB");
            RegisterStandardMatchEvaluators_en("en-AU");
            RegisterStandardMatchEvaluators_fr("fr-FR");
            RegisterStandardMatchEvaluators_fr("fr-CA");
        }

        /// <summary>
        /// Registers the standard match evaluators for the English language.
        /// </summary>
        /// <param name="culture">The culture for which to register the evaluators.</param>
        private static void RegisterStandardMatchEvaluators_en(String culture)
        {
            RegisterMatchEvaluator(culture, "Indef_Art", (str, target) =>
            {
                if (target.HasProperty("vowel")) return "An";
                return "A";
            });
            RegisterMatchEvaluator(culture, "indef_art", (str, target) =>
            {
                if (target.HasProperty("vowel")) return "an";
                return "a";
            });
        }

        /// <summary>
        /// Registers the standard match evaluators for the French language.
        /// </summary>
        /// <param name="culture">The culture for which to register the evaluators.</param>
        private static void RegisterStandardMatchEvaluators_fr(String culture)
        {
            RegisterMatchEvaluator(culture, "Def_Art", (str, target) =>
            {
                if (target.HasProperty("plural")) return "Les ";
                if (target.HasProperty("vowel")) return "L'";
                if (target.HasProperty("masculine")) return "Le ";
                if (target.HasProperty("feminine")) return "La ";
                return String.Empty;
            });
            RegisterMatchEvaluator(culture, "def_art", (str, target) =>
            {
                if (target.HasProperty("plural")) return "les ";
                if (target.HasProperty("vowel")) return "l'";
                if (target.HasProperty("masculine")) return "le ";
                if (target.HasProperty("feminine")) return "la ";
                return String.Empty;
            });
            RegisterMatchEvaluator(culture, "adj", (str, target) =>
            {
                if (target.HasProperty("plural"))
                {
                    return target.HasProperty("feminine") ?
                        str.GetVariant("plur_feminine") :
                        str.GetVariant("plur_masculine");
                }
                else
                {
                    return target.HasProperty("feminine") ?
                        str.GetVariant("sing_feminine") :
                        str.GetVariant("sing_masculine");
                }
            });
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        private static String MatchVariantInternal(String culture, LocalizedString source, LocalizedStringVariant target, String rule)
        {
            Dictionary<String, LocalizationMatchEvaluator> registry;
            if (!registeredMatchEvaluators.TryGetValue(culture, out registry))
                return null;

            LocalizationMatchEvaluator evaluator;
            if (!registry.TryGetValue(rule, out evaluator))
                return null;

            return (evaluator == null) ? null : evaluator(source, target);
        }

        /// <summary>
        /// Matches a source string to a target variant according to the specified culture and rule.
        /// </summary>
        private static String MatchVariantInternal(String culture, LocalizedString source, LocalizedStringVariant target, StringSegment rule)
        {
            Dictionary<String, LocalizationMatchEvaluator> registry;
            if (!registeredMatchEvaluators.TryGetValue(culture, out registry))
                return null;

            foreach (var kvp in registry)
            {
                if (rule.Equals(kvp.Key))
                {
                    return (kvp.Value == null) ? null : kvp.Value(source, target);
                }
            }
            return null;
        }

        // Plurality and match evaluators.
        private static readonly Dictionary<String, LocalizationPluralityEvaluator> registeredPluralityEvaluators =
            new Dictionary<String, LocalizationPluralityEvaluator>();
        private static readonly Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>> registeredMatchEvaluators =
            new Dictionary<String, Dictionary<String, LocalizationMatchEvaluator>>();

        // The default localization database for the application.
        private static readonly LocalizationDatabase strings = 
            new LocalizationDatabase();
    }
}
