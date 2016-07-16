using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvml;
using System.Globalization;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Describes the visual structure of a data object.
    /// </summary>
    [UvmlKnownType]
    public class DataTemplate : FrameworkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplate"/> class.
        /// </summary>
        /// <param name="template">The UVML template that this instance represents.</param>
        public DataTemplate(UvmlTemplate template)
            : base(template)
        { }

        /// <summary>
        /// Loads a new instance of the <see cref="DataTemplate"/> class from the specified UVML element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="root">The root element of the UVML template to load.</param>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> to use when parsing values.</param>
        /// <returns>The <see cref="DataTemplate"/> instance that was created from the specified UVML element.</returns>
        public static DataTemplate FromUvml(UltravioletContext uv, XElement root, CultureInfo cultureInfo = null)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(root, nameof(root));

            var templatedParentType = default(Type);

            var templatedObjectName = root.Name.LocalName;
            var templatedObjectType = default(Type);

            if (!uv.GetUI().GetPresentationFoundation().GetKnownType(templatedObjectName, out templatedObjectType))
                throw new UvmlException(PresentationStrings.UnrecognizedType.Format(templatedObjectName));

            var template = UvmlLoader.CreateTemplateFromXml(uv, root, templatedParentType, templatedObjectType, cultureInfo);
            var instance = new DataTemplate(template);

            return instance;
        }
    }
}
