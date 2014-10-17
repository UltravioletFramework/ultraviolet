using System;
using System.Collections.Generic;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Represents a plugin for the Nucleus localization system.
    /// </summary>
    public interface ILocalizationPlugin
    {
        /// <summary>
        /// Gets the plurality evaluators provided by this plugin.
        /// Plurality groups are used to categorize words by quantity. In English, there is only a singular (one)
        /// and a plural (many) group, but in some languages there are more.
        /// </summary>
        /// <returns>The collection of plurality group evaluators provided by this plugin.</returns>
        IEnumerable<LocalizationPluralityEvaluator> GetPluralityEvaluators();

        /// <summary>
        /// Gets the match evaluators provided by this plugin.
        /// Match evaluators are used to make words "agree" with one another. For example, the French language
        /// rules include a match evaluator called 'Def_art' which evaluates to either Les, Le, La, or L' depending on
        /// the word being matched.
        /// </summary>
        /// <returns>The collection of match evaluators provided by this plugin.</returns>
        IEnumerable<LocalizationMatchEvaluatorData> GetMatchEvaluators();

        /// <summary>
        /// Gets the collection of cultures provided by this plugin.
        /// </summary>
        IEnumerable<String> Cultures
        {
            get;
        }
    }
}
