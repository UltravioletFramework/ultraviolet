using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents the state of a collection of objects that are in the process of being loaded by the Ultraviolet object loader.
    /// </summary>
    internal class ObjectLoaderState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLoaderState"/> class.
        /// </summary>
        /// <param name="globalAliases">The registry of global aliases.</param>
        /// <param name="culture">The culture information to use when parsing objects.</param>
        /// <param name="resolver">The custom member resolution handler, if any.</param>
        public ObjectLoaderState(Dictionary<String, String> globalAliases, CultureInfo culture, ObjectLoaderMemberResolutionHandler resolver = null)
        {
            this.globalClassAliases = globalAliases;
            this.culture = culture;
            this.resolver = resolver;
        }

        /// <summary>
        /// Parses the class aliases element.
        /// </summary>
        /// <param name="elements">The elements to parse.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        public void ParseClassAliases(IEnumerable<XElement> elements, Type defaultClass)
        {
            if (defaultClass != null)
                classAliasDefault = defaultClass.AssemblyQualifiedName;

            if (elements != null)
            {
                foreach (var child in elements)
                {
                    var aliasName = (String)child.Attribute("Name");
                    if (String.IsNullOrEmpty(aliasName))
                        throw new InvalidOperationException(CoreStrings.DataObjectInvalidClassAlias);

                    var aliasDefault = (Boolean?)child.Attribute("Default") ?? false;
                    var aliasValue = child.Value;

                    localClassAliases[aliasName] = aliasValue;
                    if (aliasDefault)
                    {
                        if (classAliasDefault != null)
                        {
                            throw new InvalidOperationException(CoreStrings.DataObjectAlreadySetDefaultAlias);
                        }
                        classAliasDefault = aliasValue;
                    }
                }
            }
        }

        /// <summary>
        /// Parses the class defaults element.
        /// </summary>
        /// <param name="elements">The elements to parse.</param>
        public void ParseClassDefaults(IEnumerable<XElement> elements)
        {
            foreach (var child in elements)
            {
                var defaultClassRaw = (String)child.Attribute("Class");
                var defaultClassResolved = ResolveClass(defaultClassRaw);
                if (String.IsNullOrEmpty(defaultClassResolved))
                    throw new InvalidOperationException(CoreStrings.DataObjectDefaultMissingClassName);
                
                var defaultClass = Type.GetType(defaultClassResolved, false);
                if (defaultClass == null)
                    throw new InvalidOperationException(CoreStrings.DataObjectDefaultHasInvalidClass.Format(defaultClass));
                
                var defaultValuesForClass = defaultValues[defaultClass] = new Dictionary<String, XElement>();
                foreach (var value in child.Elements())
                {
                    if (ObjectLoaderXmlSerializer.IsReservedKeyword(value.Name.LocalName))
                        throw new InvalidOperationException(CoreStrings.DataObjectDefaultHasReservedKeyword.Format(value.Name));
                    
                    defaultValuesForClass[value.Name.LocalName] = value;
                }
            }
        }

        /// <summary>
        /// Resolves a class name from the specified class name or alias.
        /// </summary>
        /// <param name="name">The class name or alias to resolve.</param>
        /// <returns>The class name that was resolved.</returns>
        public String ResolveClass(String name)
        {
            if (String.IsNullOrEmpty(name))
                return classAliasDefault;

            String alias;
            if (localClassAliases.TryGetValue(name, out alias))
                return alias;

            if (globalClassAliases.TryGetValue(name, out alias))
                return alias;

            return name;
        }

        /// <summary>
        /// Gets the default values for the specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve default values.</param>
        /// <returns>The default values for the specified type, or <see langword="null"/> if there are no default values for that type.</returns>
        public IEnumerable<KeyValuePair<String, XElement>> GetDefaultValues(Type type)
        {
            Dictionary<String, XElement> values;
            if (!defaultValues.TryGetValue(type, out values))
                return null;

            return values;
        }

        /// <summary>
        /// Gets a value indicating which culture the object loader should use
        /// when parsing strings into objects.
        /// </summary>
        public CultureInfo Culture
        {
            get { return culture; }
        }

        /// <summary>
        /// Gets the custom member resolver, if any.
        /// </summary>
        public ObjectLoaderMemberResolutionHandler Resolver
        {
            get { return resolver; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object loader should ignore
        /// members which do not exist on the type being loaded.
        /// </summary>
        public Boolean IgnoreMissingMembers
        {
            get;
            set;
        }

        // Property values.
        private readonly CultureInfo culture;
        private readonly ObjectLoaderMemberResolutionHandler resolver;

        // Loaded class aliases.
        private readonly Dictionary<String, String> globalClassAliases;
        private readonly Dictionary<String, String> localClassAliases = 
            new Dictionary<String, String>();
        private String classAliasDefault;

        // Loaded defaults.
        private readonly Dictionary<Type, Dictionary<String, XElement>> defaultValues =
            new Dictionary<Type, Dictionary<String, XElement>>();
    }
}
