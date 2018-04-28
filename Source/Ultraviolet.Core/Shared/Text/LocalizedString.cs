using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a localized string.
    /// </summary>
    public partial class LocalizedString : IEnumerable<KeyValuePair<String, LocalizedStringVariant>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedString"/> class.
        /// </summary>
        /// <param name="culture">The string's associated culture.</param>
        /// <param name="key">The string's localization key.</param>
        /// <param name="html">A value indicating whether the string contains HTML encoded characters.</param>
        /// <param name="nopseudo">A value indicating whether pseudolocalization is disabled for this string.</param>
        private LocalizedString(String culture, String key, Boolean html, Boolean nopseudo)
        {
            this.Culture = culture;
            this.Language = GetLanguageFromCulture(culture);
            this.Key = key;
            this.ContainsHtmlEncodedCharacters = html;
            this.PseudolocalizationDisabled = nopseudo;
        }

        /// <summary>
        /// Implicitly converts a localized string to a string.
        /// </summary>
        /// <param name="str">The localized string to convert.</param>
        /// <returns>The string to which the localized string was converted.</returns>
        public static implicit operator String(LocalizedString str)
        {
            if (str == null)
                return null;

            var defaultVariant = str.variants.DefaultVariant;
            if (defaultVariant == null)
            {
                return str.Key;
            }
            return defaultVariant.Value;
        }

        /// <inheritdoc/>
        public override String ToString() => this;

        /// <summary>
        /// Gets a value indicating whether the string has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasProperty(String prop)
        {
            return properties.ContainsKey(prop);
        }

        /// <summary>
        /// Gets a value indicating whether the string has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasProperty(StringSegment prop)
        {
            return HasPropertyRef(ref prop);
        }

        /// <summary>
        /// Gets a value indicating whether the string has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasPropertyRef(ref StringSegment prop)
        {
            foreach (var kvp in properties)
            {
                if (prop.Equals(kvp.Key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the specified string variant.
        /// </summary>
        /// <param name="group">The name of the variant group of the variant to retrieve.</param>
        /// <returns>The specified string variant.</returns>
        public LocalizedStringVariant GetVariant(ref StringSegment group)
        {
            foreach (var kvp in variants)
            {
                if (group.Equals(kvp.Key))
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the specified string variant.
        /// </summary>
        /// <param name="group">The name of the variant group of the variant to retrieve.</param>
        /// <returns>The specified string variant.</returns>
        public LocalizedStringVariant GetVariant(String group)
        {
            Contract.Require(group, nameof(group));

            var value = (LocalizedStringVariant)null;
            variants.TryGetValue(group, out value);
            return value;
        }

        /// <summary>
        /// Gets the plural variant of this string that corresponds to the specified count.
        /// </summary>
        /// <param name="count">The number of objects.</param>
        /// <returns>The plural variant of this string that corresponds to the specified count.</returns>
        public LocalizedStringVariant GetPluralVariant(Int32 count)
        {
            var group = Localization.GetPluralityGroup(Culture, Language, this, count);
            return GetVariant(group) ?? variants.DefaultVariant;
        }

        /// <summary>
        /// Gets the string's associated culture.
        /// </summary>
        public String Culture
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the string's associated language.
        /// </summary>
        public String Language
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the string's localization key.
        /// </summary>
        public String Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the string contains HTML encoded characters.
        /// </summary>
        public Boolean ContainsHtmlEncodedCharacters
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether pseudolocalization is disabled for this string.
        /// </summary>
        public Boolean PseudolocalizationDisabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of variants defined by this string.
        /// </summary>
        public Int32 VariantCount
        {
            get { return variants.Count; }
        }

        /// <summary>
        /// Creates a set of localized strings from the specified XML element.
        /// </summary>
        /// <param name="xml">The XML element that contains the string definition.</param>
        /// <param name="strings">The dictionary to populate with strings for each loaded culture.</param>
        /// <returns>The localization key for the created strings.</returns>
        internal static String CreateFromXml(XElement xml, Dictionary<String, LocalizedString> strings)
        {
            Contract.Require(xml, nameof(xml));
            Contract.Require(strings, nameof(strings));

            strings.Clear();

            var key = xml.AttributeValueString("Key");
            if (String.IsNullOrEmpty(key))
                throw new InvalidDataException(CoreStrings.LocalizedStringMissingKey);

            var html   = xml.AttributeValueBoolean("Html") ?? false;
            var pseudo = xml.AttributeValueBoolean("Pseudo") ?? true;

            var cultures = xml.Elements();
            foreach (var culture in cultures)
            {
                var cultureName = culture.Name.LocalName;
                var cultureString = new LocalizedString(cultureName, key, html, !pseudo);

                var stringPropertiesAttr = culture.Attribute("Properties");
                if (stringPropertiesAttr != null)
                {
                    var stringProperties = stringPropertiesAttr.Value.Split(',').Select(x => x.Trim());
                    foreach (var stringProperty in stringProperties)
                    {
                        cultureString.properties.Add(stringProperty, true);
                    }
                }

                var variants = culture.Elements("Variant");
                foreach (var variant in variants)
                {
                    var variantGroup = variant.AttributeValueString("Group") ?? "none";
                    var variantValue = variant.Value;
                    var variantProps = (variant.AttributeValueString("Properties") ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

                    cultureString.variants[variantGroup] = new LocalizedStringVariant(cultureString, variantGroup, variantValue, variantProps);
                }

                strings[cultureName] = cultureString;
            }

            return key;
        }

        /// <summary>
        /// Creates a set of localized strings from the specified string description.
        /// </summary>
        /// <param name="description">The string description from which to create the strings.</param>
        /// <param name="strings">The dictionary to populate with strings for each loaded culture.</param>
        /// <returns>The localization key for the created strings.</returns>
        internal static String CreateFromDescription(LocalizedStringDescription description, Dictionary<String, LocalizedString> strings)
        {
            Contract.Require(description, nameof(description));
            Contract.Require(strings, nameof(strings));

            strings.Clear();

            if (description.Variants != null)
            {
                foreach (var culture in description.Variants)
                {
                    var cultureName = culture.Key;
                    var cultureString = new LocalizedString(cultureName, description.Key, description.Html, !description.Pseudo);

                    if (culture.Value != null)
                    {
                        var cultureProperties = culture.Value.Properties?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                        if (cultureProperties != null)
                        {
                            foreach (var cultureProperty in cultureProperties)
                                cultureString.properties.Add(cultureProperty, true);
                        }

                        if (culture.Value.Items != null)
                        {
                            foreach (var variant in culture.Value.Items)
                            {
                                var variantGroup = variant.Group ?? "none";
                                var variantValue = variant.Text;
                                var variantProps = variant.Properties?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

                                cultureString.variants[variantGroup] = new LocalizedStringVariant(cultureString, variantGroup, variantValue, variantProps);
                            }
                        }

                        strings[cultureName] = cultureString;
                    }
                }
            }            

            return description.Key;
        }

        /// <summary>
        /// Creates a fallback string in the event that the specified key does not exist for a culture.
        /// </summary>
        /// <param name="culture">The string's associated culture.</param>
        /// <param name="key">The string's localization key.</param>
        /// <returns>The fallback string that was created.</returns>
        internal static LocalizedString CreateFallback(String culture, String key)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));
            Contract.RequireNotEmpty(key, nameof(key));

            var fallback = new LocalizedString(culture, key, false, false);
            fallback.variants["singular"] = new LocalizedStringVariant(fallback, "singular", key);
            return fallback;
        }

        /// <summary>
        /// Creates a pseudolocalized copy of the specified source string.
        /// </summary>
        /// <param name="source">The source string to pseudolocalize.</param>
        /// <returns>A copy of the source string that is pseudolocalized.</returns>
        internal static LocalizedString CreatePseudolocalized(LocalizedString source)
        {
            Contract.Require(source, nameof(source));

            var pseudoString = new LocalizedString(Localization.PseudolocalizedCulture, source.Key, 
                source.ContainsHtmlEncodedCharacters, source.PseudolocalizationDisabled);

            foreach (var variant in source.variants)
            {
                if (pseudoString.PseudolocalizationDisabled)
                {
                    var pseudoVariant = new LocalizedStringVariant(pseudoString, variant.Value.Group, variant.Value.Value, variant.Value.Properties);
                    pseudoString.variants[variant.Key] = pseudoVariant;
                }
                else
                {
                    var pseudoText = String.Empty;
                    if (pseudoString.ContainsHtmlEncodedCharacters)
                    {
                        pseudoText = Uri.UnescapeDataString(variant.Value.Value);
                        pseudoText = PseudolocalizeString(pseudoText);
                        pseudoText = Uri.EscapeDataString(pseudoText);
                    }
                    else
                    {
                        pseudoText = PseudolocalizeString(variant.Value.Value);
                    }
                    var pseudoVariant = new LocalizedStringVariant(pseudoString, variant.Value.Group, pseudoText, variant.Value.Properties);
                    pseudoString.variants[variant.Key] = pseudoVariant;
                }
            }
            return pseudoString;
        }

        /// <summary>
        /// Gets the two-letter language code associated with the specified culture.
        /// </summary>
        private static String GetLanguageFromCulture(String culture)
        {
            if (String.IsNullOrEmpty(culture))
                return "en";

            var language = CultureInfo.GetCultureInfo(culture)?.TwoLetterISOLanguageName;
            if (language != null)
                return language;

            var components = culture.Split('-');
            return components.Length > 0 ? components[0] : "en";
        }

        /// <summary>
        /// Pseudolocalized the specified string.
        /// </summary>
        /// <param name="str">The string to pseudolocalize.</param>
        private static String PseudolocalizeString(String str)
        {
            if (str == null)
                return null;

            // Find any masked segments.
            var masks = new Dictionary<Int32, StringSegment>();
            
            var matchesFormatting = FormatSpecifierRegex.Matches(str);
            foreach (Match match in matchesFormatting)
                masks[match.Index] = new StringSegment(str, match.Index, match.Length);

            var matchesMarkup = MarkupSpecifierRegex.Matches(str);
            foreach (Match match in matchesMarkup)
                masks[match.Index] = new StringSegment(str, match.Index, match.Length);

            // Pseudolocalize the string.
            var mask = default(StringSegment);
            var builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                // Skip any masked segments.
                while (masks.TryGetValue(i, out mask))
                {
                    for (int j = 0; j < mask.Length; j++)
                    {
                        builder.Append(str[i + j]);
                    }
                    i += mask.Length;
                }
                if (i >= str.Length)
                {
                    break;
                }
                
                // Append the current character to the string.
                var ch = str[i];
                var ix = RomanAlphabet.IndexOf(ch);
                if (ix >= 0)
                {
                    builder.Append(PseudoAlphabet[ix]);
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        // Pseudolocalization properties.
        private const string RomanAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string PseudoAlphabet = "ÅßƇƉƐƑƓĤƗĴƘŁӍƝŌṔQȒȘŦŨṼƜӼȲƵãḃḉδεҒɠɧɩĵĸłɱɳɵƿɋŗşŧũṽώẍýƶ";
        private static readonly Regex FormatSpecifierRegex = new Regex(@"(?<!\{)\{([0-9]+).*?\}(?!})");
        private static readonly Regex MarkupSpecifierRegex = new Regex(@"(?<!\{)\<.*?\>(?!})");

        // The string's properties and variants.
        private readonly Dictionary<String, Boolean> properties = new Dictionary<String, Boolean>();
        private readonly LocalizedStringVariantCollection variants = new LocalizedStringVariantCollection();
    }
}
