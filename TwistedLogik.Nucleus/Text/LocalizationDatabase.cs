using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TwistedLogik.Nucleus.Text
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
        /// <returns><c>true</c> if the database contains entries for the specified culture; otherwise, <c>false</c>.</returns>
        public Boolean IsCultureLoaded(String culture)
        {
            Contract.RequireNotEmpty(culture, "culture");

            return strings.ContainsKey(culture);
        }

        /// <summary>
        /// Loads localization data from the specified stream. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load localization data.</param>
        public void LoadFromStream(Stream stream)
        {
            Contract.Require(stream, "stream");

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
        }

        /// <summary>
        /// Loads localization data from the specified file. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="paths">An array containing the parts of the path to the file from which to load localization data.</param>
        public void LoadFromFile(params String[] paths)
        {
            Contract.Require(paths, "paths");

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
            Contract.RequireNotEmpty(path, "path");

            using (var stream = File.OpenRead(path))
            {
                LoadFromStream(stream);
            }
        }

        /// <summary>
        /// Loads localization data from any XML files found in the specified directory. Loaded strings
        /// will overwrite any previously loaded strings that share the same key.
        /// </summary>
        /// <param name="paths">An array containing the parts of the path to the directory from which to load localization data.</param>
        public void LoadFromDirectory(params String[] paths)
        {
            Contract.Require(paths, "paths");

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
            Contract.Require(path, "path");

            var files = Directory.GetFiles(path, "*.xml");
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                {
                    LoadFromStream(stream);
                }
            }
        }

        /// <summary>
        /// Removes all of the database's entries.
        /// </summary>
        public void Unload()
        {
            strings.Clear();
        }

        /// <summary>
        /// Removes all of the database's entries for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to remove entries.</param>
        public void UnloadCulture(String culture)
        {
            strings.Remove(culture);
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
        /// <returns><c>true</c> if the database contains a string with the specified key for the current culture; otherwise, <c>false</c>.</returns>
        public Boolean Contains(String key)
        {
            Contract.RequireNotEmpty(key, "key");

            var db = GetCultureStrings(Localization.CurrentCulture);
            return db.ContainsKey(key);
        }

        /// <summary>
        /// Gets a value indicating whether the database contains a string with the specified key for the specified culture.
        /// </summary>
        /// <param name="culture">The culture to evaluate.</param>
        /// <param name="key">The localization key for which to search.</param>
        /// <returns><c>true</c> if the database contains a string with the specified key for the specified culture; otherwise, <c>false</c>.</returns>
        public Boolean Contains(String culture, String key)
        {
            Contract.RequireNotEmpty(key, "key");

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
            Contract.RequireNotEmpty(key, "key");
            Contract.RequireNotEmpty(culture, "culture");

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
        /// Gets the string database for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to retrieve a string database.</param>
        /// <returns>The string database for the specified culture, or the en-US database if that culture doesn't exist.</returns>
        private Dictionary<String, LocalizedString> GetCultureStrings(String culture)
        {
            if (!strings.ContainsKey(culture))
            {
                culture = "en-US";
                if (!strings.ContainsKey(culture))
                {
                    strings[culture] = new Dictionary<String, LocalizedString>();
                }
            }
            return strings[culture];
        }

        // A dictionary associating cultures with dictionaries of localized strings.
        private readonly Dictionary<String, Dictionary<String, LocalizedString>> strings = 
            new Dictionary<String, Dictionary<String, LocalizedString>>();
    }
}
