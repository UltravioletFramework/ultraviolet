using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CSharp;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Represents the state of a compilation performed by the <see cref="ExpressionCompiler"/> class.
    /// </summary>
    internal class ExpressionCompilerState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCompilerState"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="compiler">The compiler used to produce view model code.</param>
        public ExpressionCompilerState(UltravioletContext uv, CSharpCodeProvider compiler)
        {
            this.uv = uv;
            this.compiler = compiler;
            this.knownTypes = new Dictionary<String, Type>();
            this.knownDefaultProperties = new Dictionary<Type, String>();
            this.componentTemplateManager = (uv == null) ? null : uv.GetUI().GetPresentationFoundation().ComponentTemplates;

            LoadKnownTypes(uv);
        }

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownType(String name, out Type type)
        {
            return knownTypes.TryGetValue(name, out type);
        }

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="type">The type of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetElementDefaultProperty(Type type, out String property)
        {
            return knownDefaultProperties.TryGetValue(type, out property);
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets the compiler used to produce view model code.
        /// </summary>
        public CSharpCodeProvider Compiler
        {
            get { return compiler; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the compiler should work in the user's temporary directory
        /// instead of the application directory.
        /// </summary>
        public Boolean WorkInTemporaryDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value specifying whether to write errors to a file in the working directory.
        /// </summary>
        public Boolean WriteErrorsToFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the component template manager that contains the application's registered components.
        /// </summary>
        public ComponentTemplateManager ComponentTemplateManager
        {
            get { return componentTemplateManager; }
        }

        /// <summary>
        /// Loads the set of known types which are available to the compiler.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private void LoadKnownTypes(UltravioletContext uv)
        {
            if (uv != null)
            {
                var upf = uv.GetUI().GetPresentationFoundation();
                foreach (var kvp in upf.GetKnownTypes())
                    knownTypes[kvp.Key] = kvp.Value;

                foreach (var kvp in knownTypes)
                {
                    String property;
                    if (upf.GetElementDefaultProperty(kvp.Value, out property))
                        knownDefaultProperties[kvp.Value] = property;
                }
            }
            else
            {
                var types = from type in typeof(PresentationFoundation).Assembly.GetTypes()
                            let attrKnownType = type.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).Cast<UvmlKnownTypeAttribute>().SingleOrDefault()
                            let attrDefaultProp = type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).Cast<DefaultPropertyAttribute>().SingleOrDefault()
                            where attrKnownType != null
                            select new { Type = type, AttrKnownType = attrKnownType, AttrDefaultProp = attrDefaultProp };

                foreach (var type in types)
                {
                    var name = type.AttrKnownType.Name ?? type.Type.Name;
                    knownTypes[name] = type.Type;

                    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.Type.TypeHandle);

                    if (type.AttrDefaultProp != null)
                        knownDefaultProperties[type.Type] = type.AttrDefaultProp.Name;
                }
            }
        }

        // Property values.
        private readonly UltravioletContext uv;
        private readonly CSharpCodeProvider compiler;
        private readonly ComponentTemplateManager componentTemplateManager;

        // State values.
        private readonly Dictionary<String, Type> knownTypes;
        private readonly Dictionary<Type, String> knownDefaultProperties;
    }
}
