using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Contains methods for loading object definitions from data files.
    /// </summary>
    [CLSCompliant(false)]
    public static class ObjectLoader
    {
        /// <summary>
        /// Gets a value indicating whether the specified name is a reserved keyword used by the object loading system.
        /// </summary>
        /// <param name="name">The name to evaluate.</param>
        /// <returns><c>true</c> if the name is reserved; otherwise, <c>false</c>.</returns>
        public static Boolean IsReservedKeyword(String name)
        {
            switch (name)
            {
                case "Class":
                case "Key":
                case "ID":
                case "Type":
                case "Constructor":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Loads the objects defined in the specified file.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="datatype">The type of data contained in the specified file.</param>
        /// <param name="path">The path of the file that contains the data definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the XML file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(ObjectLoaderDataType datatype, String path, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(path, "path");
            Contract.Require(name, "name");

            // If requested, determine the data type based on the file extension.
            if (datatype == ObjectLoaderDataType.Detect)
            {
                var extension = Path.GetExtension(path).ToUpper();
                switch (extension)
                {
                    case ".XML":
                        datatype = ObjectLoaderDataType.Xml;
                        break;

                    case ".JSON":
                    case ".JS":
                        datatype = ObjectLoaderDataType.Json;
                        break;

                    default:
                        throw new InvalidOperationException(NucleusStrings.UnableToDetectDataTypeFromExt);
                }
            }

            // Load the item definitions.
            using (var stream = File.OpenRead(path))
            {
                switch (datatype)
                {
                    case ObjectLoaderDataType.Xml:
                        return LoadDefinitions<T>(XDocument.Load(stream), name, defaultClass);

                    case ObjectLoaderDataType.Json:
                        using (var sreader = new StreamReader(stream))
                        using (var jreader = new JsonTextReader(sreader))
                        {
                            return LoadDefinitions<T>(JObject.Load(jreader), name, defaultClass);
                        }
                }
            }
            throw new InvalidOperationException(NucleusStrings.UnrecognizedDataType);
        }

        /// <summary>
        /// Loads object definitions from the specified XML file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML document that contains the object definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(XDocument xml, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(xml, "xml");
            Contract.Require(name, "name");

            var root = DataElement.CreateFromXml(xml);
            var aliases = root.Element("Aliases");
            var defaults = root.Element("Defaults");
            return LoadDefinitions<T>(root, 
                (aliases == null) ? Enumerable.Empty<DataElement>() : aliases.Elements("Alias"),
                (defaults == null) ? Enumerable.Empty<DataElement>() : defaults.Elements("Default"), name, defaultClass);
        }

        /// <summary>
        /// Loads object definitions from the specified JSON file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON document that contains the object definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(JObject json, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(json, "json");
            Contract.Require(name, "name");

            var root = DataElement.CreateFromJson(json);
            var aliases = root.Elements("Aliases");
            var defaults = root.Elements("Defaults");
            return LoadDefinitions<T>(root, aliases, defaults, name, defaultClass);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(xml, "xml");

            return LoadObject<T>(xml, Thread.CurrentThread.CurrentCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(xml, "xml");
            Contract.Require(culture, "culture");

            var state = new ObjectLoaderState(globalAliases, culture);
            state.IgnoreMissingMembers = ignoreMissingMembers;
            state.ParseClassAliases(null, typeof(T));

            var objectElement = DataElement.CreateFromXml(xml);
            var objectInstance = CreateObjectFromRootElement<T>(state, objectElement);

            return (T)PopulateObject(state, objectInstance, objectElement);
        }
        
        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(json, "json");

            return LoadObject<T>(json, Thread.CurrentThread.CurrentCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(json, "json");
            Contract.Require(culture, "culture");

            var state = new ObjectLoaderState(globalAliases, culture);
            state.IgnoreMissingMembers = ignoreMissingMembers;
            state.ParseClassAliases(null, typeof(T));

            var objectElement = DataElement.CreateFromJson(json);
            var objectInstance = CreateObjectFromRootElement<T>(state, objectElement);
            
            return (T)PopulateObject(state, objectInstance, objectElement);
        }

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, Type type)
        {
            RegisterGlobalAlias(alias, type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The assembly-qualified name of the type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, String type)
        {
            try
            {
                lockobj.EnterWriteLock();
                if (globalAliases.ContainsKey(alias))
                {
                    throw new InvalidOperationException(NucleusStrings.TypeAliasAlreadyRegistered.Format(alias));
                }
                globalAliases[alias] = type;
            }
            finally
            {
                if (lockobj.IsWriteLockHeld)
                {
                    lockobj.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Unregisters a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to unregister.</param>
        /// <returns><c>true</c> if the alias was unregistered; otherwise, <c>false</c>.</returns>
        public static bool UnregisterGlobalAlias(String alias)
        {
            try
            {
                lockobj.EnterWriteLock();
                return globalAliases.Remove(alias);
            }
            finally
            {
                if (lockobj.IsWriteLockHeld)
                {
                    lockobj.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Loads object definitions from the specified data hierarchy and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="root">The root data element of the document that contains the object definitions to load.</param>
        /// <param name="aliases">The elements defining class aliases.</param>
        /// <param name="defaults">The elements defining class defaults.</param>
        /// <param name="defaultClass">The name of the default class to apply to loaded objects.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        private static IEnumerable<T> LoadDefinitions<T>(DataElement root, IEnumerable<DataElement> aliases, IEnumerable<DataElement> defaults, String name, Type defaultClass) where T : DataObject
        {
            Contract.Require(root, "root");
            Contract.Require(name, "name");

            var culture = new CultureInfo(root.AttributeValue<String>("Culture") ?? "en-US");

            try
            {
                lockobj.EnterReadLock();

                var state = new ObjectLoaderState(globalAliases, culture);
                state.ParseClassAliases(aliases, defaultClass);
                state.ParseClassDefaults(defaults);

                var objectElements = root.Elements(name);
                var objectList = new List<T>();
                foreach (var objectElement in objectElements)
                {
                    var objectInstance = CreateObjectFromRootElement<T>(state, objectElement);
                    PopulateObject(state, objectInstance, objectElement);
                    objectList.Add(objectInstance);
                }
                return objectList;
            }
            finally
            {
                if (lockobj.IsReadLockHeld)
                {
                    lockobj.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Creates an object from the specified root element.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="state">The current loader state.</param>
        /// <param name="element">The element from which to create an object.</param>
        /// <returns>The object that was created.</returns>
        private static T CreateObjectFromRootElement<T>(ObjectLoaderState state, DataElement element)
        {
            // First, ensure that we have a class, key, and identifier.
            var objClassName = state.ResolveClass(element.AttributeValue<String>("Class"));
            if (String.IsNullOrEmpty(objClassName))
                throw new InvalidOperationException(NucleusStrings.DataObjectMissingClass);
            
            // If we're loading a Nucleus data object, parse its unique key and ID.
            var argsBase = default(Object[]);
            if (typeof(DataObject).IsAssignableFrom(typeof(T)))
            {
                var objKey = element.AttributeValue<String>("Key");
                if (String.IsNullOrEmpty(objKey))
                    throw new InvalidOperationException(NucleusStrings.DataObjectMissingKey);
                
                var objID = element.AttributeValue<String>("ID");
                if (String.IsNullOrEmpty(objID))
                    throw new InvalidOperationException(NucleusStrings.DataObjectMissingID);
                
                Guid objIDValue;
                if (!Guid.TryParse(objID, out objIDValue))
                    throw new InvalidOperationException(NucleusStrings.DataObjectInvalidID.Format(objID));
                
                 argsBase = new Object[] { objKey, objIDValue };
            }

            // Attempt to find the object class and make sure it's of the correct type.
            var objClass = Type.GetType(objClassName, false);
            if (objClass == null || !typeof(T).IsAssignableFrom(objClass))
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidClass.Format(objClassName ?? "(null)", argsBase[0]));
            
            // Attempt to instantiate the object.
            return CreateObject<T>(state, objClass, argsBase, GetSpecifiedConstructorArguments(element));
        }

        /// <summary>
        /// Creates an object from the specified root element.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="state">The loader state.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="argsBase">The base set of arguments for this object's constructor.</param>
        /// <param name="argsSpecified">The specified set of arguments for this object's constructor.</param>
        /// <returns>The object that was created.</returns>
        private static T CreateObject<T>(ObjectLoaderState state, Type type, Object[] argsBase, DataElement[] argsSpecified = null)
        {
            var argsBaseLength = (argsBase == null) ? 0 : argsBase.Length;
            var argsSpecifiedLength = (argsSpecified == null) ? 0 : argsSpecified.Length;

            // Try to find a constructor that matches our argument list.
            var ctorArgs = new Object[argsBaseLength + argsSpecifiedLength];
            var ctors = type.GetConstructors();
            if (!ctors.Any() && argsBaseLength == 0 && argsSpecifiedLength == 0)
            {
                return (T)Activator.CreateInstance(type);
            }

            var ctorMatches = ctors.Where(x => x.GetParameters().Count() == ctorArgs.Length).ToList();
            if (!ctorMatches.Any())
                throw new InvalidOperationException(NucleusStrings.CtorMatchNotFound);
            if (ctorMatches.Count() > 1)
                throw new InvalidOperationException(NucleusStrings.CtorMatchIsAmbiguous);
            
            var ctorMatch = ctorMatches.Single();
            var ctorParams = ctorMatch.GetParameters();

            // Build the complete list of constructor arguments.
            for (int i = 0; i < argsBaseLength; i++)
            {
                ctorArgs[i] = argsBase[i];
            }
            for (int i = 0; i < argsSpecifiedLength; i++)
            {
                var ctorArgElement = argsSpecified[i];
                var ctorArgType = GetTypeFromElement(state, ctorParams[i + argsBase.Length].ParameterType, ctorArgElement);
                var ctorArgValue = default(Object);
                if (ctorArgElement.Elements().Any())
                {
                    ctorArgValue = CreateObject<Object>(state, ctorArgType, null, GetSpecifiedConstructorArguments(ctorArgElement));
                    ctorArgValue = PopulateObjectFromElements(state, ctorArgValue, ctorArgElement);
                }
                else
                {
                    ctorArgValue = ParseValue(ctorArgElement.Value, ctorArgType, state.Culture);
                }
                ctorArgs[i + argsBase.Length] = ctorArgValue;
            }

            // Attempt to instantiate the object.
            return (T)ctorMatch.Invoke(ctorArgs);
        }

        /// <summary>
        /// Gets the specified constructor arguments for an element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The specified constructor arguments for the element.</returns>
        private static DataElement[] GetSpecifiedConstructorArguments(DataElement element)
        {
            var ctorElement = element.Element("Constructor");
            if (ctorElement == null)
                return null;
            return ctorElement.Elements("Argument").ToArray();
        }

        /// <summary>
        /// Gets the type defined by the specified element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="baseType">The base type.</param>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The type defined by the specified element.</returns>
        private static Type GetTypeFromElement(ObjectLoaderState state, Type baseType, DataElement element)
        {
            var complexTypeAttr = element.Attribute("Type");
            if (complexTypeAttr != null && String.IsNullOrEmpty(complexTypeAttr.Value))
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidType.Format(element.Name));

            var complexType = (complexTypeAttr == null) ? baseType : Type.GetType(state.ResolveClass(complexTypeAttr.Value), false);
            if (complexType == null)
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidType.Format(element.Name));
            
            if (!baseType.IsAssignableFrom(complexType))
                throw new InvalidOperationException(NucleusStrings.DataObjectIncompatibleType.Format(element.Name));
            
            return complexType;
        }

        /// <summary>
        /// Populates an object with values.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObject(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            objectInstance = PopulateObjectFromDefaults(state, objectInstance, objectElement);
            objectInstance = PopulateObjectFromAttributes(state, objectInstance, objectElement);
            objectInstance = PopulateObjectFromElements(state, objectInstance, objectElement);
            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the currently loaded default values.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromDefaults(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            // Find our inheritance chain.
            var typeStack = new Stack<Type>();
            var typeCurrent = objectInstance.GetType();
            while (typeCurrent != null)
            {
                typeStack.Push(typeCurrent);
                typeCurrent = typeCurrent.BaseType;
            }

            // Populate the default values for the entire inheritance heirarchy.
            while (typeStack.Count > 0)
            {
                // Retrieve the defaults for the current type.
                var typeBeingPopulated = typeStack.Pop();
                var typeDefaults = state.GetDefaultValues(typeBeingPopulated);
                if (typeDefaults == null)
                    continue;

                // Populate the default values.
                foreach (var typeDefault in typeDefaults)
                {
                    objectInstance = PopulateMemberFromElement(state, objectInstance, typeDefault.Value);
                }
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the attributes defined on the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromAttributes(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            foreach (var attr in objectElement.Attributes())
            {
                if (IsReservedKeyword(attr.Name))
                    continue;

                var attrMember = ObjectLoaderMember.Find(objectInstance, attr.Name, state.IgnoreMissingMembers);
                if (attrMember != null)
                {
                    var attrValue = ObjectResolver.FromString(attr.Value, attrMember.MemberType);
                    attrMember.SetValue(attrValue, null);
                }
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the elements descending from the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <param name="filter">A filter which determines which elements are used to populate the object.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromElements(ObjectLoaderState state, Object objectInstance, DataElement objectElement, Func<DataElement, Boolean> filter = null)
        {
            var children = (filter == null) ? objectElement.Elements() : objectElement.Elements().Where(filter);
            foreach (var element in children)
            {
                objectInstance = PopulateMemberFromElement(state, objectInstance, element);
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object member from the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="memberElement">The element that defines the member.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateMemberFromElement(ObjectLoaderState state, Object objectInstance, DataElement memberElement)
        {
            if (IsReservedKeyword(memberElement.Name))
                return objectInstance;

            var member = ObjectLoaderMember.Find(objectInstance, memberElement.Name, state.IgnoreMissingMembers);
            if (member == null)
                return objectInstance;

            // Handle array values.
            if (member.MemberType.IsArray)
            {
                PopulateArray(state, member, memberElement);
                return objectInstance;
            }

            // Handle list values.
            if (typeof(IList).IsAssignableFrom(member.MemberType))
            {
                PopulateList(state, member, memberElement);
                return objectInstance;
            }

            // Handle complex and simple objects.
            if (memberElement.Elements().Any())
            {
                var complexType = GetTypeFromElement(state, member.MemberType, memberElement);
                var complexTypeValue = member.GetValueFromData(memberElement);
                if (complexTypeValue == null)
                {
                    complexTypeValue = CreateObject<Object>(state, complexType, null, GetSpecifiedConstructorArguments(memberElement));
                    if (!complexType.IsValueType)
                        member.SetValueFromData(complexTypeValue, memberElement);
                }

                if (!member.IsIndexer)
                {
                    complexTypeValue = PopulateObjectFromAttributes(state, complexTypeValue, memberElement);
                }
                complexTypeValue = PopulateObjectFromElements(state, complexTypeValue, memberElement);

                if (complexType.IsValueType)
                    member.SetValueFromData(complexTypeValue, memberElement);
            }
            else
            {
                if (String.IsNullOrEmpty(memberElement.Value))
                    return objectInstance;

                var memberValue = ParseValue(memberElement.Value, member.MemberType, state.Culture);
                member.SetValueFromData(memberValue, memberElement);
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an array value.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="member">The member to populate.</param>
        /// <param name="element">The element that defines the member's value.</param>
        private static void PopulateArray(ObjectLoaderState state, ObjectLoaderMember member, DataElement element)
        {
            // Make sure that the array is properly defined.
            var itemsRoot = element.Element("Items");
            if (itemsRoot != null && itemsRoot.Elements().Where(x => x.Name != "Item").Any())
                throw new InvalidOperationException(NucleusStrings.NonItemElementsInArrayDef);
            
            // Create the array.
            var items = (itemsRoot == null) ? new List<DataElement>() : itemsRoot.Elements("Item").ToList();
            var array = (Array)member.GetValueFromData(element);
            if (array == null)
            {
                array = Array.CreateInstance(member.MemberType.GetElementType(), items.Count);
                member.SetValueFromData(array, element);
            }

            // Populate the array's items.
            for (int i = 0; i < items.Count; i++)
            {
                var value = ObjectResolver.FromString(items[i].Value, member.MemberType.GetElementType());
                array.SetValue(value, i);
            }
        }

        /// <summary>
        /// Populates a list value.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="member">The member to populate.</param>
        /// <param name="element">The element that defines the member's value.</param>
        private static void PopulateList(ObjectLoaderState state, ObjectLoaderMember member, DataElement element)
        {
            // Make sure that the list is properly defined.
            var itemsRoot = element.Element("Items");
            if (itemsRoot != null && itemsRoot.Elements().Where(x => x.Name != "Item").Any())
                throw new InvalidOperationException(NucleusStrings.NonItemElementsInListDef);
            
            // Make sure that there's only a single generic argument.
            var listImpls = member.MemberType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)).ToList();
            if (!listImpls.Any())
                throw new InvalidOperationException(NucleusStrings.MemberDoesNotImplementList.Format(element.Name));
            if (listImpls.Count() > 1)
                throw new InvalidOperationException(NucleusStrings.MemberImplementsMultipleListImpls.Format(element.Name));
            var listElementType = listImpls.First().GetGenericArguments()[0];

            // Create the list.
            var list = (IList)member.GetValueFromData(element);
            if (list == null)
            {
                list = (IList)Activator.CreateInstance(member.MemberType);
                member.SetValueFromData(list, element);
            }

            // Populate the list's members.
            if (!member.IsIndexer)
            {
                PopulateObjectFromAttributes(state, list, element);
            }
            PopulateObjectFromElements(state, list, element, x => x.Name != "Items");

            // Populate the list's items.
            if (itemsRoot != null)
            {
                var items = itemsRoot.Elements("Item").ToList();
                for (int i = 0; i < items.Count; i++)
                {
                    var value = default(Object);
                        var type = GetTypeFromElement(state, listElementType, items[i]);
                    if (items[i].Elements().Any())
                    {
                        value = CreateObject<Object>(state, type, null, GetSpecifiedConstructorArguments(items[i]));
                        value = PopulateObjectFromElements(state, value, items[i]);
                    }
                    else
                    {
                        value = ObjectResolver.FromString(items[i].Value, type);
                    }
                    list.Add(value);
                }
            }
        }

        /// <summary>
        /// Attempts to parse a string into a simple value.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="type">The type into which to parse the string.</param>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <returns>The value that was parsed.</returns>
        private static Object ParseValue(String str, Type type, IFormatProvider provider)
        {
            return ObjectResolver.FromString(str, type, provider);
        }

        // The global alias registry.
        private static readonly ReaderWriterLockSlim lockobj = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static readonly Dictionary<String, String> globalAliases =
            new Dictionary<String, String>();
    }
}
