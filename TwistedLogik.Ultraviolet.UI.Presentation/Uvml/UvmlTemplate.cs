using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represents a function which instantiates templated object instances.
    /// </summary>
    /// <typeparam name="T">The type of object which is being instantiated.</typeparam>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="name">The name of the object being instantiated.</param>
    /// <returns>The object that was instantiated.</returns>
    public delegate T UvmlTemplateInstantiator<T>(UltravioletContext uv, String name);

    /// <summary>
    /// Represents a template which produces object instances based on a UVML document.
    /// </summary>
    public sealed class UvmlTemplate<T> : UvmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlTemplate{T}"/> class
        /// from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the template.</param>
        /// <param name="instantiator">A function which instantiates objects for this template,
        /// or <see langword="null"/> to use the default object instantiator.</param>
        internal UvmlTemplate(XElement element, UvmlTemplateInstantiator<T> instantiator = null)
        {
            Contract.Require(element, nameof(element));

            var templatedObjectName = (String)element.Attribute("Name");
            if (String.IsNullOrWhiteSpace(templatedObjectName))
                templatedObjectName = null;

            this.name = templatedObjectName;
            this.instantiator = instantiator ?? CreateDefaultInstantiator();
            this.mutators = new List<UvmlMutator>();
        }

        /// <inheritdoc/>
        public override Object Instantiate(UltravioletContext uv, UvmlInstantiationContext context = null)
        {
            var instance = instantiator(uv, name);
            if (instance == null)
                throw new NullReferenceException(nameof(instance));

            foreach (var mutator in mutators)
            {
                mutator.Mutate(uv, instance, context);
            }

            return instance;
        }

        /// <summary>
        /// Creates a default instantiator for this template's type.
        /// </summary>
        /// <returns>The instantiator function which was created.</returns>
        private static UvmlTemplateInstantiator<T> CreateDefaultInstantiator()
        {
            var type = typeof(T);

            var ctorWithContextAndName = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctorWithContextAndName != null)
                return new UvmlTemplateInstantiator<T> ((uv, name) => (T)ctorWithContextAndName.Invoke(new Object[] { uv, name }));

            var ctorWithContext = type.GetConstructor(new[] { typeof(UltravioletContext) });
            if (ctorWithContext != null)
                return new UvmlTemplateInstantiator<T> ((uv, name) => (T)ctorWithContext.Invoke(new[] { uv }));

            var ctorDefault = type.GetConstructor(Type.EmptyTypes);
            if (ctorDefault == null)
                throw new UvmlException(UltravioletStrings.NoValidConstructor.Format(type.Name));
            
            return new UvmlTemplateInstantiator<T> ((uv, name) => (T)ctorDefault.Invoke(null));
        }

        // State values.
        private readonly String name;
        private readonly UvmlTemplateInstantiator<T> instantiator;
        private readonly List<UvmlMutator> mutators;
    }
}
