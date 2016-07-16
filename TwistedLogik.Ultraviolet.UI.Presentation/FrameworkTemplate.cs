using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvml;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Enables the instantiation of a tree of <see cref="FrameworkElement"/> objects.
    /// </summary>
    [UvmlKnownType]
    public abstract class FrameworkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkTemplate"/> class.
        /// </summary>
        /// <param name="template">The UVML template that this instance represents.</param>
        /// <param name="dataSourceWrapperName">The name of the template's data source wrapper type.</param>
        protected FrameworkTemplate(UvmlTemplate template, String dataSourceWrapperName)
        {
            Contract.Require(template, nameof(template));

            this.template = template;
            this.dataSourceWrapperName = dataSourceWrapperName;
        }

        /// <summary>
        /// Loads the content of the template as an instance of an object and returns the root element of the content.
        /// </summary>
        /// <param name="dataSource">The object's data source.</param>
        /// <param name="dataSourceType">The object's data source type.</param>
        /// <returns>The root element of the content.</returns>
        public DependencyObject LoadContent(Object dataSource, Type dataSourceType)
        {
            var uv = UltravioletContext.DemandCurrent();
            var wrapper = CreateDataSourceWrapper(dataSource, dataSourceType);
            var context = new UvmlInstantiationContext(uv, null, wrapper, wrapper?.GetType(), null);
            return (DependencyObject)((UvmlTemplateInstance)template.Instantiate(uv, context)).Finalize();
        }

        /// <summary>
        /// When overridden in a derived class, creates a data source wrapper which implements
        /// the template's binding expressions.
        /// </summary>
        /// <param name="dataSource">The object's data source.</param>
        /// <param name="dataSourceType">The object's data source type.</param>
        /// <returns>The wrapped data source.</returns>
        private Object CreateDataSourceWrapper(Object dataSource, Type dataSourceType)
        {
            if (String.IsNullOrEmpty(dataSourceWrapperName))
                return dataSource;

            var uv = UltravioletContext.DemandCurrent();
            var wrapper = uv.GetUI().GetPresentationFoundation().CreateDataSourceWrapperByName(dataSourceWrapperName, dataSource, new Namescope());

            return wrapper;
        }

        // State values.
        private readonly UvmlTemplate template;
        private readonly String dataSourceWrapperName;
    }
}
