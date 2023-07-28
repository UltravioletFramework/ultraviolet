using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a database of localized strings.
    /// </summary>
    public sealed class LocalizationDatabase
    {
        /// <summary>
        /// Gets a value indicating whether the database contains entries for the specified culture.
        /// </summary>
        /// <param name="culture">The culture to evaluate.</param>
        /// <returns><see langword="true"/> if the database contains entries for the specified culture; otherwise, <see langword="false"/>.</returns>
        public Boolean IsCultureLoaded(String culture)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));

            return strings.ContainsKey(culture);
        }

        /// <summary>
        /// Enumerates all of the strings belonging to the current culture.
        /// </summary>
        /// <returns>A collection of strings belonging to the current culture.</returns>
        public IEnumerable<KeyValuePair<String, LocalizedString>> EnumerateStrings()
        {
            return EnumerateCultureStrings(Localization.CurrentCulture);
        }

        /// <summary>
        /// Enumerates all of the strings belonging to the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to enumerate strings.</param>
        /// <returns>A collection of strings belonging to the specified culture.</returns>
        public IEnumerable<KeyValuePair<String, LocalizedString>> EnumerateCultureStrings(String culture)
        {
            Contract.RequireNotEmpty(culture, nameof(culture));

            return GetCultureStrings(culture);
        }

        /// <summary>
        /// Loads localization data from the specified XML stream. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load localization data.</param>
        public void LoadFromStream(Stream stream)
        {
            LoadFromXmlStream(stream);
        }

        /// <summary>
        /// Loads localization data from the specified XML stream. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load localization data.</param>
        public void LoadFromXmlStream(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var xml = XDocument.Load(stream);
            var elements = xml.Element("LocalizedStrings").Elements("String");
            var results = new Dictionary<String, LocalizedString>();

            foreach (var element in elements)
            {
                var key = LocalizedString.CreateFromXml(element, results);
                foreach (var result in results)
                {
                    if (!strings.ContainsKey(result.Key))
                    {
                        strings[result.Key] = new Dictionary<String, LocalizedString>();
                    }
                    strings[result.Key][key] = result.Value;
                }
            }

            InvalidateFallbackCultures();
        }

        /// <summary>
        /// Loads localization data from the specified JSON stream. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load localization data.</param>
        public void LoadFromJsonStream(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            using (var sreader = new StreamReader(stream))
            using (var jreader = new JsonTextReader(sreader))
            {
                var serializer = JsonSerializer.CreateDefault(CoreJsonSerializerSettings.Instance);
                var description = serializer.Deserialize<LocalizationDatabaseDescription>(jreader);

                var results = new Dictionary<String, LocalizedString>();

                foreach (var lstring in description.Strings ?? Enumerable.Empty<LocalizedStringDescription>())
                {
                    var key = LocalizedString.CreateFromDescription(lstring, results);
                    foreach (var result in results)
                    {
                        if (!strings.ContainsKey(result.Key))
                        {
                            strings[result.Key] = new Dictionary<String, LocalizedString>();
                        }
                        strings[result.Key][key] = result.Value;
                    }
                }
            }

            InvalidateFallbackCultures();
        }

        /// <summary>
        /// Loads localization data from the specified file. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="paths">An array containing the parts of the path to the file from which to load localization data.</param>
        public void LoadFromFile(params String[] paths)
        {
            Contract.Require(paths, nameof(paths));

            var path = Path.Combine(paths);
            LoadFromFile(path);
        }

        /// <summary>
        /// Loads localization data from the specified file. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="path">The path to the file from which to load localization data.</param>
        public void LoadFromFile(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            var extension = Path.GetExtension(path)?.ToLowerInvariant();

            switch (extension)
            {
                case ".json":
                    using (var stream = File.OpenRead(path))
                    {
                        LoadFromJsonStream(stream);
                    }
                    break;

                default:
                    using (var stream = File.OpenRead(path))
                    {
                        LoadFromXmlStream(stream);
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads localization data from any XML files found in the specified directory. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="paths">An array containing the parts of the path to the directory from which to load localization data.</param>
        public void LoadFromDirectory(params String[] paths)
        {
            Contract.Require(paths, nameof(paths));

            var path = Path.Combine(paths);
            LoadFromDirectory(path);
        }

        /// <summary>
        /// Loads localization data from any XML files found in the specified directory. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="path">The path to the directory from which to load localization data.</param>
        public void LoadFromDirectory(String path)
        {
            Contract.Require(path, nameof(path));

            var files = default(String[]);

            files = Directory.GetFiles(path, "*.xml");
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                    LoadFromXmlStream(stream);
            }

            files = Directory.GetFiles(path, ".json");
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                    LoadFromJsonStream(stream);
            }
        }

        /// <summary>
        /// Removes all of the database's entries.
        /// </summary>
        public void Unload()
        {
            strings.Clear();
            InvalidateFallbackCultures();
        }

        /// <summary>
        /// Removes all of the database's entries for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to remove entries.</param>
        public void UnloadCulture(String culture)
        {
            strings.Remove(culture);
            InvalidateFallbackCultures();
        }

        /// <summary>
        /// Creates a pseudolocale for this database.
        /// </summary>
        public void CreatePseudolocale()
        {
            var dbPseudo = new Dictionary<String, LocalizedString>();
            strings[Localization.PseudolocalizedCulture] = dbPseudo;

            var dbEnglish = (Dictionary<String, LocalizedString>)null;
            if (!strings.TryGetValue("en-US", out dbEnglish))
                return;

            foreach (var entry in dbEnglish)
            {
                dbPseudo[entry.Key] = LocalizedString.CreatePseudolocalized(entry.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the database contains a string with the specified key for the current culture.
        /// </summary>
        /// <param name="key">The localization key for which to search.</param>
        /// <returns><see langword="true"/> if the database contains a string with the specified key for the current culture; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            var db = GetCultureStrings(Localization.CurrentCulture);
            return db.ContainsKey(key);
        }

        /// <summary>
        /// Gets a value indicating whether the database contains a string with the specified key for the specified culture.
        /// </summary>
        /// <param name="culture">The culture to evaluate.</param>
        /// <param name="key">The localization key for which to search.</param>
        /// <returns><see langword="true"/> if the database contains a string with the specified key for the specified culture; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(String culture, String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            var db = GetCultureStrings(culture);
            return db.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves the string with the specified key from the specified culture.
        /// </summary>
        /// <param name="culture">The culture from which to retrieve the string.</param>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The string that was retrieved.</returns>
        public LocalizedString Get(String culture, String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));
            Contract.RequireNotEmpty(culture, nameof(culture));

            var db = GetCultureStrings(culture);
            var value = (LocalizedString)null;
            
            if (!db.TryGetValue(key, out value))
            {
                var fallback = LocalizedString.CreateFallback(culture, key);
                db[key] = fallback;
                return fallback;
            }

            return value;
        }

        /// <summary>
        /// Retrieves the string with the specified key from the current culture.
        /// </summary>
        /// <param name="key">The localization key of the string to retrieve.</param>
        /// <returns>The string that was retrieved.</returns>
        public LocalizedString Get(String key)
        {
            return Get(Localization.CurrentCulture, key);
        }

        /// <summary>
        /// Gets the culture which is currently being used as the fallback for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to find a fallback culture.</param>
        /// <returns>The culture which is currently being used as the fallback for the specified
        /// culture, or <see langword="null"/> if the culture does not have a fallback.</returns>
        public String GetFallbackCulture(String culture)
        {
            var fallback = String.Empty;
            if (fallbackCultures.TryGetValue(culture, out fallback))
                return fallback;
            
            try
            {
                var cultureInfo = new CultureInfo(culture);
                var cultureMatch = strings.Keys.Select(c => new CultureInfo(c))
                    .Where(c => c.TwoLetterISOLanguageName == cultureInfo.TwoLetterISOLanguageName).OrderBy(c => c.Name).FirstOrDefault();

                fallback = cultureMatch?.Name;
            }
            catch (CultureNotFoundException)
            {
                fallback = null;
            }

            fallbackCultures[culture] = fallback;
            return fallback;
        }

        /// <summary>
        /// Gets the number of strings defined in this database.
        /// </summary>
        public Int32 StringCount
        {
            get { return strings.Count; }
        }

        /// <summary>
        /// Gets the string database for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to retrieve a string database.</param>
        /// <returns>The string database for the specified culture, or the en-US database if that culture doesn't exist.</returns>
        private Dictionary<String, LocalizedString> GetCultureStrings(String culture)
        {
            if (!strings.ContainsKey(culture))
            {
                culture = GetFallbackCulture(culture) ?? "en-US";
                if (!strings.ContainsKey(culture))
                {
                    strings[culture] = new Dictionary<String, LocalizedString>();
                }
            }
            return strings[culture];
        }

        /// <summary>
        /// Invalidates the fallback culture for the specified culture.
        /// </summary>
        private void InvalidateFallbackCulture(String culture)
        {
            fallbackCultures.Remove(culture);
        }

        /// <summary>
        /// Invalidates all known fallback cultures.
        /// </summary>
        private void InvalidateFallbackCultures()
        {
            fallbackCultures.Clear();
        }

        // A dictionary associating cultures with dictionaries of localized strings.
        private readonly Dictionary<String, Dictionary<String, LocalizedString>> strings = 
            new Dictionary<String, Dictionary<String, LocalizedString>>();

        // A dictionary associating cultures with their closest fallback cultures of the same language.
        private readonly Dictionary<String, String> fallbackCultures =
            new Dictionary<String, String>();
    }
}
