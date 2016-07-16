using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Represents a cache manifest for a compiled binding expressions assembly, which is used to determine if a recompilation is necessary.
    /// </summary>
    internal class CompilerCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerCache"/> class.
        /// </summary>
        private CompilerCache()
        {
            version = typeof(CompilerCache).Assembly.GetName().Version;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CompilerCache"/> by loading the contents of the specified file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The <see cref="CompilerCache"/> instance that was created.</returns>
        public static CompilerCache FromFile(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            var cache = new CompilerCache();

            var lines = File.ReadAllLines(path);
            var version = default(Version);
            var revision = default(Int32);

            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line))
                    continue;

                if (version == default(Version))
                {
                    if (!Version.TryParse(line, out version))
                        throw new InvalidDataException();

                    continue;
                }

                if (revision == default(Int32))
                {
                    if (!Int32.TryParse(line, out revision) || revision != CompilerRevision)
                        throw new InvalidDataException();

                    continue;
                }

                var components = line.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (components.Length != 2)
                    throw new InvalidDataException();

                var name = components[0];
                var hash = components[1];

                cache.hashes[name] = hash;
            }

            cache.version = version;
            return cache;
        }

        /// <summary>
        /// Attempts to create a new instance of <see cref="CompilerCache"/> by loading the contents of the specified file.
        /// If the file does not exist, or contains invalid data, this method returns <see langword="null"/>.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The <see cref="CompilerCache"/> instance that was created, or <see langword="null"/> if the file could not be loaded.</returns>
        public static CompilerCache TryFromFile(String path)
        {
            try
            {
                return FromFile(path);
            }
            catch (DirectoryNotFoundException) { }
            catch (FileNotFoundException) { }
            catch (InvalidDataException) { }

            return null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CompilerCache"/> from the specified collection of data source wrappers.
        /// </summary>
        /// <param name="dataSourceWrappers">A collection of data source wrappers from which to create a cache object.</param>
        /// <returns>The <see cref="CompilerCache"/> instance that was created.</returns>
        public static CompilerCache FromDataSourceWrappers(IEnumerable<DataSourceWrapperInfo> dataSourceWrappers)
        {
            Contract.Require(dataSourceWrappers, nameof(dataSourceWrappers));

            var cache = new CompilerCache();

            foreach (var dataSourceWrapper in dataSourceWrappers)
            {
                var dataSourceHash = GenerateHashForType(dataSourceWrapper.DataSourceType);
                cache.hashes[dataSourceWrapper.DataSourceType.FullName] = dataSourceHash;

                var dataSourceWrapperHash = GenerateHashForXElement(dataSourceWrapper.DataSourceDefinition.Definition);
                cache.hashes[dataSourceWrapper.DataSourceWrapperName] = dataSourceWrapperHash;
            }

            return cache;
        }

        /// <summary>
        /// Generates a SHA1 hash string for the specified type based on its public member signatures.
        /// </summary>
        /// <param name="type">The type for which to generate a hash.</param>
        /// <returns>The SHA1 hash for the specified type.</returns>
        public static String GenerateHashForType(Type type)
        {
            Contract.Require(type, nameof(type));

            var sb = new StringBuilder(type.FullName);

            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).OrderBy( x=> x.MemberType).ThenBy(x => x.Name).ToList();
            foreach (var member in members)
            {
                sb.AppendFormat("{0} {1}", member.MemberType, member);
                sb.AppendLine();
            }

            return ComputeSHA1String(sb.ToString());
        }

        /// <summary>
        /// Generates a SHA1 hash string for the specified XML element.
        /// </summary>
        /// <param name="element">The XML element for which to generate a SHA1 hash string.</param>
        /// <returns>Thed SHA1 hash for the specified XML element.</returns>
        public static String GenerateHashForXElement(XElement element)
        {
            Contract.Require(element, nameof(element));

            return ComputeSHA1String(element.ToString());
        }

        /// <summary>
        /// Converts the contents of the specified <see cref="String"/> instance into a SHA1 hash string.
        /// </summary>
        private static String ComputeSHA1String(String input)
        {
            var output = new StringBuilder();

            var sha = new SHA1CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(input.ToString());
            var hashBytes = sha.ComputeHash(bytes);

            foreach (var hashByte in hashBytes)
            {
                output.AppendFormat("{0:x2}", hashByte);
            }

            return output.ToString();
        }

        /// <summary>
        /// Saves the cache to the specified file.
        /// </summary>
        /// <param name="path">The path to the file to which to save the cache.</param>
        public void Save(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));
            
            using (var stream = File.Open(path, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(version);
                writer.WriteLine(CompilerRevision);

                foreach (var hash in hashes)
                {
                    writer.WriteLine("{0} {1}", hash.Key, hash.Value);
                }
            }            
        }

        /// <summary>
        /// Gets a value indicating whether anything is different between this cache and the specified cache.
        /// </summary>
        /// <param name="other">The cache to compare against this cache.</param>
        /// <returns><see langword="true"/> if this cache is different from the specified cache; otherwise, <see langword="false"/>.</returns>
        public Boolean IsDifferentFrom(CompilerCache other)
        {
            Contract.Require(other, nameof(other));

            if (!version.Equals(other.version))
                return true;

            var keys = Enumerable.Union(this.hashes.Keys, other.hashes.Keys).ToList();
            foreach (var key in keys)
            {
                String hash1, hash2;

                if (!this.hashes.TryGetValue(key, out hash1))
                    return true;

                if (!other.hashes.TryGetValue(key, out hash2))
                    return true;

                if (!String.Equals(hash1, hash2, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        // The registry of hashes for each referenced type or view.
        private readonly Dictionary<String, String> hashes = new Dictionary<String, String>();

        // Versioning information
        private const Int32 CompilerRevision = 4;
        private Version version;
    }
}
