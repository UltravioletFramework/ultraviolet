using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a navigation expression in a UVSS style sheet.
    /// </summary>
    public sealed class UvssNavigationExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpression"/> class.
        /// </summary>
        /// <param name="navigationProperty">The styling name of the navigation property.</param>
        /// <param name="navigationPropertyType">The styling name of the navigation property's type.</param>
        /// <param name="navigationPropertyIndex">The navigation index, if any; otherwise, <see langword="null"/>.</param>
        public UvssNavigationExpression(String navigationProperty, String navigationPropertyType, Int32? navigationPropertyIndex = null)
        {
            this.navigationProperty = navigationProperty;
            this.navigationPropertyType = navigationPropertyType;
            this.navigationPropertyIndex = navigationPropertyIndex;
        }

        /// <summary>
        /// Gets the styling name of the navigation property.
        /// </summary>
        public String NavigationProperty
        {
            get { return navigationProperty; }
        }

        /// <summary>
        /// Gets the styling name of the navigation property's type.
        /// </summary>
        public String NavigationPropertyType
        {
            get { return navigationPropertyType; }
        }

        /// <summary>
        /// Gets the navigation index, if one has been specified.
        /// </summary>
        public Int32? NavigationPropertyIndex
        {
            get { return navigationPropertyIndex; }
        }

        // Property values.
        private readonly String navigationProperty;
        private readonly String navigationPropertyType;
        private readonly Int32? navigationPropertyIndex;
    }
}
