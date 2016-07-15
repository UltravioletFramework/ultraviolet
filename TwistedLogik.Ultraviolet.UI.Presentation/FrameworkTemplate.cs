using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvml;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Enables the instantiation of a tree of <see cref="FrameworkElement"/> objects.
    /// </summary>
    public abstract class FrameworkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkTemplate"/> class.
        /// </summary>
        /// <param name="template">The UVML template that this instance represents.</param>
        protected FrameworkTemplate(UvmlTemplate template)
        {
            Contract.Require(template, nameof(template));

            this.template = template;
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
            var context = new UvmlInstantiationContext(uv, null, dataSource, dataSourceType, null);
            return (DependencyObject)((UvmlTemplateInstance)template.Instantiate(uv, context)).Finalize();
        }

        // The UVML template that this instance represents.
        private readonly UvmlTemplate template;
    }
}
