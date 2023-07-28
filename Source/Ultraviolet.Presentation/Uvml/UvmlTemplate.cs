using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a function which instantiates templated object instances.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="name">The name of the object being instantiated.</param>
    /// <returns>The object that was instantiated.</returns>
    public delegate Object UvmlTemplateInstantiator(UltravioletContext uv, String name);

    /// <summary>
    /// Represents a template which produces object instances based on a UVML document.
    /// </summary>
    public sealed class UvmlTemplate : UvmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlTemplate"/> class
        /// from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element from which to create the template.</param>
        /// <param name="type">The type of the object being instantiated by the template.</param>
        /// <param name="instantiator">A function which instantiates objects for this template,
        /// or <see langword="null"/> to use the default object instantiator.</param>
        /// <param name="mutators">The template's collection of mutators.</param>
        internal UvmlTemplate(XElement element, Type type, UvmlTemplateInstantiator instantiator, IEnumerable<UvmlMutator> mutators = null)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(type, nameof(type));
            
            var templatedObjectName = (String)element.Attribute("Name");
            if (String.IsNullOrWhiteSpace(templatedObjectName))
                templatedObjectName = null;

            this.name = templatedObjectName;
            this.instantiator = instantiator ?? CreateDefaultInstantiator(type);
            this.mutators = new List<UvmlMutator>(mutators ?? Enumerable.Empty<UvmlMutator>());

            var classesString = (String)element.Attribute("Class");
            if (!String.IsNullOrWhiteSpace(classesString))
            {
                this.classes = new List<String>(classesString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
        
        /// <inheritdoc/>
        public override Object Instantiate(UltravioletContext uv, UvmlInstantiationContext context)
        {
            var instance = instantiator(uv, name);
            if (instance == null)
                throw new NullReferenceException(nameof(instance));

            InitializeFrameworkElement(uv, instance, context);
            InitializeElement(uv, instance, context);
            InitializeDependencyObject(uv, instance, context);
            
            var mutatorsWithValues =
                (from mutator in mutators
                 select new
                 {
                     Mutator = mutator,
                     Value = mutator.InstantiateValue(uv, instance, context)
                 }).ToArray();

            return new UvmlTemplateInstance(instance, () =>
            {
                foreach (var mutator in mutatorsWithValues)
                    mutator.Mutator.Mutate(uv, instance, mutator.Value, context);

                InitializeContentPresenter(uv, instance, context);

                if (IsItemsPanelForTemplatedParent)
                {
                    var itemsControl = context.TemplatedParent as ItemsControl;
                    if (itemsControl != null)
                        itemsControl.ItemsPanelElement = instance as Panel;
                }

                var fe = instance as FrameworkElement;
                if (fe != null)
                    fe.EndInit();
            });
        }

        /// <summary>
        /// Gets or sets a value indicating whether this template produces the items panel for its templated parent.
        /// </summary>
        internal Boolean IsItemsPanelForTemplatedParent
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a default instantiator for this template's type.
        /// </summary>
        /// <param name="type">The type of object to instantiate.</param>
        /// <returns>The instantiator function which was created.</returns>
        private static UvmlTemplateInstantiator CreateDefaultInstantiator(Type type)
        {
            var paramUv = Expression.Parameter(typeof(UltravioletContext), "uv");
            var paramName = Expression.Parameter(typeof(String), "name");

            var ctorWithContextAndName = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctorWithContextAndName != null)
            {
                return Expression.Lambda<UvmlTemplateInstantiator>(
                    Expression.New(ctorWithContextAndName, paramUv, paramName), paramUv, paramName).Compile();
            }

            var ctorWithContext = type.GetConstructor(new[] { typeof(UltravioletContext) });
            if (ctorWithContext != null)
            {
                return Expression.Lambda<UvmlTemplateInstantiator>(
                    Expression.New(ctorWithContext, paramUv), paramUv, paramName).Compile();
            }

            var ctorDefault = type.GetConstructor(Type.EmptyTypes);
            if (ctorDefault == null)
                throw new UvmlException(UltravioletStrings.NoValidConstructor.Format(type.Name));

            return Expression.Lambda<UvmlTemplateInstantiator>(
                Expression.New(ctorDefault), paramUv, paramName).Compile();
        }

        /// <summary>
        /// Performs initialization required by instances of the <see cref="FrameworkElement"/> class.
        /// </summary>
        private void InitializeFrameworkElement(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var frameworkElement = instance as FrameworkElement;
            if (frameworkElement == null)
                return;

            if (!String.IsNullOrEmpty(frameworkElement.Name))
                context.Namescope.RegisterName(frameworkElement.Name, frameworkElement);

            frameworkElement.BeginInit();
            frameworkElement.TemplatedParent = context.TemplatedParent as DependencyObject;
        }

        /// <summary>
        /// Performs initialization required by instances of the <see cref="UIElement"/> class.
        /// </summary>
        private void InitializeElement(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var uiElement = instance as UIElement;
            if (uiElement == null)
                return;

            if (uiElement != null && classes != null)
            {
                foreach (var className in classes)
                    uiElement.Classes.Add(className);
            }
        }

        /// <summary>
        /// Performs initialization required by instances of the <see cref="DependencyObject"/> class.
        /// </summary>
        private void InitializeDependencyObject(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var dobj = instance as DependencyObject;
            if (dobj == null)
                return;

            dobj.DeclarativeDataSource = context.DataSource;
        }

        /// <summary>
        /// Performs initialization required by instances of the <see cref="ContentPresenter"/> class.
        /// </summary>
        private void InitializeContentPresenter(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var contentPresenter = instance as ContentPresenter;
            if (contentPresenter == null)
                return;

            if (contentPresenter.HasDefinedValue(ContentPresenter.ContentProperty) || contentPresenter.TemplatedParent == null)
                return;

            var alias = contentPresenter.ContentSource ?? "Content";
            if (alias == String.Empty)
                return;

            var templateType = contentPresenter.TemplatedParent.GetType();
            var templateWrapperType = uv.GetUI().GetPresentationFoundation().GetDataSourceWrapperType(templateType);

            var dpAliasedContent = DependencyProperty.FindByName(alias, templateType);
            if (dpAliasedContent != null)
            {
                contentPresenter.BindValue(ContentPresenter.ContentProperty, templateWrapperType,
                    "{{" + dpAliasedContent.Name + "}}");
            }
            
            if (!contentPresenter.HasDefinedValue(ContentPresenter.ContentStringFormatProperty))
            {
                var dpAliasedContentStringFormat = DependencyProperty.FindByName(alias + "StringFormat", templateType);
                if (dpAliasedContentStringFormat != null)
                {
                    contentPresenter.BindValue(ContentPresenter.ContentStringFormatProperty, templateWrapperType,
                        "{{" + dpAliasedContentStringFormat.Name + "}}");
                }
            }
        }

        // State values.
        private readonly String name;
        private readonly UvmlTemplateInstantiator instantiator;
        private readonly List<UvmlMutator> mutators;
        private readonly List<String> classes;
    }
}
