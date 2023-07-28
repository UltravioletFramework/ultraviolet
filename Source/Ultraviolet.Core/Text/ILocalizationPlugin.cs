using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a plugin for the Ultraviolet localization system.
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
        /// Gets the two-letter language code of the language provided by this plugin. Only one of <see cref="Culture"/> and <see cref="Language"/>
        /// should have a non-null value; if both are non-null, then <see cref="Culture"/> takes precedence.
        /// </summary>
        String Language
        {
            get;
        }

        /// <summary>
        /// Gets the culture code of the culture provided by this plugin. Only one of <see cref="Culture"/> and <see cref="Language"/>
        /// should have a non-null value; if both are non-null, then <see cref="Culture"/> takes precedence.
        /// </summary>
        String Culture
        {
            get;
        }
    }
}
