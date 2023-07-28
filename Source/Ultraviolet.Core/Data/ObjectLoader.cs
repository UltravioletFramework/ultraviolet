using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a method which can be passed to the <see cref="ObjectLoader"/> to allow external code
    /// to resolve property values.
    /// </summary>
    /// <param name="obj">The object which is being populated.</param>
    /// <param name="name">The name of the property which is being populated.</param>
    /// <param name="value">The value which is being resolved.</param>
    /// <returns><see langword="true"/> if the property was resolved; otherwise, <see langword="false"/>.</returns>
    public delegate Boolean ObjectLoaderMemberResolutionHandler(Object obj, String name, String value);

    /// <summary>
    /// Contains methods for loading object definitions from data files.
    /// </summary>
    [CLSCompliant(false)]
    public static class ObjectLoader
    {
        /// <summary>
        /// Loads the objects defined in the specified file.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="dataType">The type of data contained in the specified file.</param>
        /// <param name="path">The path of the file that contains the data definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the XML file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(ObjectLoaderDataType dataType, String path, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(path, nameof(path));
            Contract.Require(name, nameof(name));

            // If requested, determine the data type based on the file extension.
            if (dataType == ObjectLoaderDataType.Detect)
            {
                var extension = Path.GetExtension(path)?.ToLowerInvariant();
                switch (extension)
                {
                    case ".xml":
                        dataType = ObjectLoaderDataType.Xml;
                        break;

                    case ".json":
                    case ".js":
                        dataType = ObjectLoaderDataType.Json;
                        break;

                    default:
                        throw new InvalidOperationException(CoreStrings.UnableToDetectDataTypeFromExt);
                }
            }

            // Load the item definitions.
            using (var stream = File.OpenRead(path))
            {
                switch (dataType)
                {
                    case ObjectLoaderDataType.Xml:
                        return LoadDefinitions<T>(XDocument.Load(stream), name, defaultClass);

                    case ObjectLoaderDataType.Json:
                        using (var sreader = new StreamReader(stream))
                        using (var jreader = new JsonTextReader(sreader))
                        {
                            var json = JObject.Load(jreader);
                            return LoadDefinitions<T>(json);
                        }
                }
            }

            throw new InvalidOperationException(CoreStrings.UnrecognizedDataType);
        }

        /// <summary>
        /// Loads object definitions from the specified XML file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML document that contains the object definitions to load.</param>
        /// <param name="name">The name of the type of object to load, which corresponds to the names of the elements in the data file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(XDocument xml, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(xml, nameof(xml));
            Contract.RequireNotEmpty(name, nameof(name));
            
            return xmlSerializer.LoadDefinitions<T>(xml.Root, 
                xml.Root.Element("Aliases")?.Elements("Alias") ?? Enumerable.Empty<XElement>(),
                xml.Root.Element("Defaults")?.Elements("Default") ?? Enumerable.Empty<XElement>(), name, defaultClass);
        }

        /// <summary>
        /// Loads object definitions from the specified JSON file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON document that contains the object definitions to load.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(JObject json) where T : DataObject
        {
            Contract.Require(json, nameof(json));

            var serializer = JsonSerializer.CreateDefault(CoreJsonSerializerSettings.Instance);
            var description = json.ToObject<DataObjectRegistryDescription<T>>(serializer);

            return description.Items;
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject<T>(xml, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, XElement xml, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject<T>(resolver, xml, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, XElement xml, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject(type, xml, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, XElement xml, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject(resolver, type, xml, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject<T>(xml, culture, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject<T>(resolver, xml, culture, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject(type, xml, culture, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false) =>
            xmlSerializer.LoadObject(resolver, type, xml, culture, ignoreMissingMembers);

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json) =>
            (T)LoadObject(typeof(T), json, CultureInfo.InvariantCulture);

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json, CultureInfo culture) =>
            (T)LoadObject(typeof(T), json, culture);

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, JObject json) =>
            LoadObject(type, json, CultureInfo.InvariantCulture);
                
        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, JObject json, CultureInfo culture)
        {
            Contract.Require(type, nameof(type));
            Contract.Require(json, nameof(json));
            Contract.Require(culture, nameof(culture));

            var serializer = JsonSerializer.CreateDefault(CoreJsonSerializerSettings.Instance);
            serializer.Culture = culture;
            
            return json.ToObject(type, serializer);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="serializer">The JSON serializer with which to deserialize the object data.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, JObject json, JsonSerializer serializer)
        {
            Contract.Require(type, nameof(type));
            Contract.Require(json, nameof(json));
            Contract.Require(serializer, nameof(serializer));

            return json.ToObject(type, serializer);
        }

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, Type type) => xmlSerializer.RegisterGlobalAlias(alias, type);

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The assembly-qualified name of the type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, String type) => xmlSerializer.RegisterGlobalAlias(alias, type);

        /// <summary>
        /// Unregisters a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to unregister.</param>
        /// <returns><see langword="true"/> if the alias was unregistered; otherwise, <see langword="false"/>.</returns>
        public static bool UnregisterGlobalAlias(String alias) => xmlSerializer.UnregisterGlobalAlias(alias);

        // The object loader's custom XML serializer.
        private static readonly ObjectLoaderXmlSerializer xmlSerializer =
            new ObjectLoaderXmlSerializer();
    }
}
