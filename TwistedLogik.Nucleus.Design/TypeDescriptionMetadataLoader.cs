using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Nucleus.Design
{
    /// <summary>
    /// Contains methods for loading type description metadata at runtime.
    /// </summary>
    public static class TypeDescriptionMetadataLoader
    {
        /// <summary>
        /// Loads the specified type description metadata file.
        /// </summary>
        /// <param name="path">The path to the file that contains the type description metadata.</param>
        public static void Load(String path)
        {
            using (var stream = File.OpenRead(path))
            {
                Load(stream);
            }
        }

        /// <summary>
        /// Loads the specified type description metadata stream.
        /// </summary>
        /// <param name="stream">The stream that contains the type description metadata.</param>
        public static void Load(Stream stream)
        {
            Contract.Require(stream, "stream");

            var metadata = XDocument.Load(stream);
            ProcessMetadata(metadata);
        }

        /// <summary>
        /// Processes the specified type metadata document.
        /// </summary>
        /// <param name="metadata">The type metadata document to process.</param>
        private static void ProcessMetadata(XDocument metadata)
        {
            foreach (var asmElement in metadata.Root.Elements("Assembly"))
            {
                ProcessMetadataAssembly(asmElement);
            }
        }

        /// <summary>
        /// Processes assembly metadata.
        /// </summary>
        /// <param name="asmElement">The XML node that defines the assembly metadata.</param>
        private static void ProcessMetadataAssembly(XElement asmElement)
        {
            var name = asmElement.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidOperationException("Invalid assembly name.");

            var types = asmElement.Element("Types");
            if (types != null)
            {
                foreach (var typeElement in types.Elements("Type"))
                {
                    ProcessMetadataType(name, typeElement);
                }
            }
        }

        /// <summary>
        /// Processes type metadata.
        /// </summary>
        /// <param name="asmName">The name of the current assembly.</param>
        /// <param name="typeElement">The XML node that defines the type metadata.</param>
        private static void ProcessMetadataType(String asmName, XElement typeElement)
        {
            var name = typeElement.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidOperationException("Invalid type name.");

            var typeName = String.Format("{0}, {1}", name, asmName);
            var type = Type.GetType(typeName, false);
            if (type == null)
                throw new InvalidOperationException(String.Format("Invalid type name '{0}'", typeName));

            var attributes = new List<Attribute>();
            foreach (var attributeElement in typeElement.Elements())
            {
                switch (attributeElement.Name.LocalName)
                {
                    case "Editor":
                        attributes.Add(CreateEditorAttribute(attributeElement));
                        break;

                    case "TypeConverter":
                        attributes.Add(CreateTypeConverterAttribute(attributeElement));
                        break;

                    case "DefaultValue":
                        attributes.Add(CreateDefaultValueAttribute(attributeElement, type));
                        break;

                    case "Properties":
                        break;

                    default:
                        throw new InvalidOperationException(String.Format("Invalid element '{0}' in '{1}'", attributeElement.Name.LocalName, typeName));
                }
            }
            if (attributes.Any())
            {
                TypeDescriptor.AddAttributes(type, attributes.ToArray());
            }

            var properties = typeElement.Element("Properties");
            var propertyDescriptors = new List<PropertyDescriptor>();
            if (properties != null)
            {
                foreach (var propertyElement in properties.Elements())
                {
                    var propertyDescriptor = ProcessMetadataProperty(type, propertyElement);
                    propertyDescriptors.Add(propertyDescriptor);
                }
            }

            var providerParent = TypeDescriptor.GetProvider(type);
            var provider = new XmlDrivenCustomTypeDescriptionProvider(providerParent, propertyDescriptors);
            TypeDescriptor.AddProvider(provider, type);
        }

        /// <summary>
        /// Processes property metadata.
        /// </summary>
        /// <param name="type">The type for which metadata is being processed.</param>
        /// <param name="propertyElement">The XML node that defines the property metadata.</param>
        /// <returns>The property descriptor for the property.</returns>
        private static PropertyDescriptor ProcessMetadataProperty(Type type, XElement propertyElement)
        {
            var propertyName = propertyElement.Name.LocalName;
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new InvalidOperationException(String.Format("Property '{0}' does not exist on type '{1}'", propertyName, type.AssemblyQualifiedName));

            var attributes = new List<Attribute>();
            foreach (var attributeElement in propertyElement.Elements())
            {
                switch (attributeElement.Name.LocalName)
                {
                    case "Browsable":
                        attributes.Add(CreateBrowsableAttribute(attributeElement, propertyName));
                        break;

                    case "Category":
                        attributes.Add(CreateCategoryAttribute(attributeElement));
                        break;

                    case "DisplayName":
                        attributes.Add(CreateDisplayNameAttribute(attributeElement));
                        break;

                    case "Description":
                        attributes.Add(CreateDescriptionAttribute(attributeElement));
                        break;

                    case "TypeConverter":
                        attributes.Add(CreateTypeConverterAttribute(attributeElement));
                        break;

                    case "Editor":
                        attributes.Add(CreateEditorAttribute(attributeElement));
                        break;

                    case "DefaultValue":
                        attributes.Add(CreateDefaultValueAttribute(attributeElement, property.PropertyType));
                        break;

                    default:
                        throw new InvalidOperationException(String.Format("Unrecognized attribute '{0}'", attributeElement.Name.LocalName));
                }
            }

            if (attributes.Any())
            {
                return TypeDescriptor.CreateProperty(type, property.Name, property.PropertyType, attributes.ToArray());
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of BrowsableAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <param name="propertyName">The name of the property to which the attribute is being attached.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateBrowsableAttribute(XElement element, String propertyName)
        {
            var browsable = false;
            if (!Boolean.TryParse(element.Value, out browsable))
                throw new InvalidOperationException(String.Format("Invalid value for '{0}' on '{1}'", element.Name.LocalName, propertyName));
            return new BrowsableAttribute(browsable);
        }

        /// <summary>
        /// Creates an instance of DisplayNameAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateDisplayNameAttribute(XElement element)
        {
            var displayName = element.Value;
            return new DisplayNameAttribute(displayName);
        }

        /// <summary>
        /// Creates an instance of DescriptionAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateDescriptionAttribute(XElement element)
        {
            var description = element.Value;
            return new DescriptionAttribute(description);
        }

        /// <summary>
        /// Creates an instance of TypeConverterAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateTypeConverterAttribute(XElement element)
        {
            var typeConverterType = Type.GetType(element.Value);
            return new TypeConverterAttribute(typeConverterType);
        }

        /// <summary>
        /// Creates an instance of EditorAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateEditorAttribute(XElement element)
        {
            var editorType = Type.GetType(element.Value);
            var editorBaseType = Type.GetType(element.AttributeValueString("BaseTypeName") ?? typeof(UITypeEditor).AssemblyQualifiedName);
            return new EditorAttribute(editorType, editorBaseType);
        }

        /// <summary>
        /// Creates an instance of CategoryAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateCategoryAttribute(XElement element)
        {
            var category = element.Value;
            return new CategoryAttribute(category);
        }

        /// <summary>
        /// Creates an instance of DefaultValueAttribute from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the attribute.</param>
        /// <param name="type">The type of the default value.</param>
        /// <returns>The attribute that was created.</returns>
        private static Attribute CreateDefaultValueAttribute(XElement element, Type type)
        {
            var value = ObjectResolver.FromString(element.Value, type);
            return new DefaultValueAttribute(value);
        }
    }
}
