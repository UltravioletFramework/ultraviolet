using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Compiler
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
            versionUltraviolet = FileVersionInfo.GetVersionInfo(typeof(CompilerCache).Assembly.Location).FileVersion;
            versionMscorlib = FileVersionInfo.GetVersionInfo(typeof(Object).Assembly.Location).FileVersion;
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
            var compilerType = default(String);
            var versionUltraviolet = default(String);
            var versionMscorlib = default(String);

            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (String.IsNullOrEmpty(line))
                    continue;

                if (i == 0 && line.StartsWith("#compiler "))
                {
                    compilerType = line.Substring("#compiler ".Length).Trim();
                    continue;
                }

                if (versionUltraviolet == default(String))
                {
                    versionUltraviolet = line;
                    continue;
                }

                if (versionMscorlib == default(String))
                {
                    versionMscorlib = line;
                    continue;
                }
                
                var components = line.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (components.Length != 2)
                    throw new InvalidDataException();

                var name = components[0];
                var hash = components[1];

                cache.hashes[name] = hash;
            }

            cache.compilerType = compilerType;
            cache.versionUltraviolet = versionUltraviolet;
            cache.versionMscorlib = versionMscorlib;
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
        /// <param name="compiler">The compiler which is creating this cache.</param>
        /// <param name="dataSourceWrappers">A collection of data source wrappers from which to create a cache object.</param>
        /// <returns>The <see cref="CompilerCache"/> instance that was created.</returns>
        public static CompilerCache FromDataSourceWrappers(IBindingExpressionCompiler compiler, IEnumerable<DataSourceWrapperInfo> dataSourceWrappers)
        {
            Contract.Require(compiler, nameof(compiler));
            Contract.Require(dataSourceWrappers, nameof(dataSourceWrappers));

            var types = new HashSet<Type>();
            var cache = new CompilerCache();
            cache.compilerType = compiler.GetType().FullName;
            
            foreach (var dataSourceWrapper in dataSourceWrappers)
            {
                if (!cache.hashes.ContainsKey(dataSourceWrapper.DataSourcePath))
                {
                    var dataSourceWrapperHash = GenerateHashForXElement(dataSourceWrapper.DataSourceDefinition.Definition);
                    cache.hashes[dataSourceWrapper.DataSourcePath] = dataSourceWrapperHash;
                }
                
                AddTypeReferences(dataSourceWrapper.DataSourceType, types);

                if (dataSourceWrapper.DependentWrapperInfos != null)
                {
                    foreach (var dependentWrapper in dataSourceWrapper.DependentWrapperInfos)
                        AddTypeReferences(dependentWrapper.DataSourceType, types);
                }
            }
            
            foreach (var type in types)
            {
                var typeHash = GenerateHashForType(type);
                cache.hashes[type.FullName] = typeHash;
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
        /// Saves the cache to the specified file.
        /// </summary>
        /// <param name="path">The path to the file to which to save the cache.</param>
        public void Save(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));
            
            using (var stream = File.Open(path, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                if (compilerType != null)
                    writer.WriteLine("#compiler " + compilerType);

                writer.WriteLine(versionUltraviolet);
                writer.WriteLine(versionMscorlib);

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

            if (!String.Equals(versionUltraviolet, other.versionUltraviolet, StringComparison.Ordinal) ||
                !String.Equals(versionMscorlib, other.versionMscorlib, StringComparison.Ordinal))
            {
                return true;
            }

            var keys = Enumerable.Union(this.hashes.Keys, other.hashes.Keys).ToList();
            foreach (var key in keys)
            {
                if (!this.hashes.TryGetValue(key, out var hash1))
                    return true;

                if (!other.hashes.TryGetValue(key, out var hash2))
                    return true;

                if (!String.Equals(hash1, hash2, StringComparison.Ordinal))
                    return true;
            }

            return false;
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
        /// Adds the types required by the specified constructor to the list of hashed types.
        /// </summary>
        private static void AddConstructorReferences(ConstructorInfo info, ISet<Type> refs)
        {
            foreach (var parameter in info.GetParameters())
                AddTypeReferences(parameter.ParameterType, refs);
        }

        /// <summary>
        /// Adds the types required by the specified event to the list of hashed types.
        /// </summary>
        private static void AddEventReferences(EventInfo info, ISet<Type> refs)
        {
            AddTypeReferences(info.EventHandlerType, refs);
        }

        /// <summary>
        /// Adds the types required by the specified field to the list of hashed types.
        /// </summary>
        private static void AddFieldReferences(FieldInfo info, ISet<Type> refs)
        {
            AddTypeReferences(info.FieldType, refs);
        }

        /// <summary>
        /// Adds the types required by the specified method to the list of hashed types.
        /// </summary>
        private static void AddMethodReferences(MethodInfo info, ISet<Type> refs)
        {
            AddTypeReferences(info.ReturnType, refs);

            foreach (var parameter in info.GetParameters())
                AddTypeReferences(parameter.ParameterType, refs);
        }

        /// <summary>
        /// Adds the types required by the specified property to the list of hashed types.
        /// </summary>
        private static void AddPropertyReferences(PropertyInfo info, ISet<Type> refs)
        {
            AddTypeReferences(info.PropertyType, refs);

            foreach (var index in info.GetIndexParameters())
                AddTypeReferences(index.ParameterType, refs);
        }

        /// <summary>
        /// Adds the types required by the specified type to the list of hashed types.
        /// </summary>
        private static void AddTypeReferences(Type info, ISet<Type> refs)
        {
            if (info.FullName == null)
                return;

            if (info.Assembly == typeof(Object).Assembly ||
                info.Assembly.FullName.StartsWith("System.") || 
                info.Assembly.FullName.StartsWith("Ultraviolet."))
            {
                return;
            }

            if (info.IsByRef || info.IsArray || info.IsPointer)
                info = info.GetElementType();

            if (info.IsGenericType)
            {
                foreach (var argument in info.GetGenericArguments())
                    AddTypeReferences(argument, refs);

                info = info.GetGenericTypeDefinition();
            }

            if (info.IsGenericParameter || refs.Contains(info))
                return;

            refs.Add(info);
            var members = info.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var member in members)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Constructor:
                        AddConstructorReferences((ConstructorInfo)member, refs);
                        break;

                    case MemberTypes.Event:
                        AddEventReferences((EventInfo)member, refs);
                        break;

                    case MemberTypes.Field:
                        AddFieldReferences((FieldInfo)member, refs);
                        break;

                    case MemberTypes.Method:
                        AddMethodReferences((MethodInfo)member, refs);
                        break;

                    case MemberTypes.Property:
                        AddPropertyReferences((PropertyInfo)member, refs);
                        break;

                    case MemberTypes.TypeInfo:
                    case MemberTypes.NestedType:
                        AddTypeReferences((Type)member, refs);
                        break;
                }
            }
        }

        // The registry of hashes for each referenced type or view.
        private readonly Dictionary<String, String> hashes = new Dictionary<String, String>();

        // Versioning information
        private String compilerType;
        private String versionUltraviolet;
        private String versionMscorlib;
    }
}
