using System;
using System.Collections;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a navigation expression. Navigation expressions are used to drill into a dependency object,
    /// allowing styles and animations to be applied to dependency objects which are themselves properties of
    /// other dependency objects.
    /// </summary>
    public partial struct NavigationExpression : IEquatable<NavigationExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationExpression"/> structure.
        /// </summary>
        /// <param name="propertyName">The name of the navigation property.</param>
        /// <param name="propertyType">The type of the navigation property, if specified.</param>
        /// <param name="propertyIndex">The index of the navigation property, if specified.</param>
        public NavigationExpression(DependencyName propertyName, Type propertyType, Int32? propertyIndex = null)
        {
            this.propertyName = propertyName;
            this.propertyType = propertyType;
            this.propertyIndex = propertyIndex;
        }
        
        /// <summary>
        /// Creates a new instance of the <see cref="NavigationExpression"/> from the
        /// specified <see cref="UvssNavigationExpression"/> object.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uvssexp">The UVSS navigation expression from which to create a new structure.</param>
        /// <returns>The <see cref="NavigationExpression"/> that was created.</returns>
        public static NavigationExpression? FromUvssNavigationExpression(UltravioletContext uv, UvssNavigationExpression uvssexp)
        {
            Contract.Require(uv, nameof(uv));

            if (uvssexp == null)
                return null;

            var upf = uv.GetUI().GetPresentationFoundation();

            var navigationPropertyName = new DependencyName(uvssexp.NavigationProperty);
            var navigationPropertyIndex = uvssexp.NavigationPropertyIndex;
            var navigationPropertyType = default(Type);

            if (uvssexp.NavigationPropertyType != null)
            {
                if (!upf.GetKnownType(uvssexp.NavigationPropertyType, false, out navigationPropertyType))
                    throw new UvssException(PresentationStrings.UnrecognizedType.Format(uvssexp.NavigationPropertyType));
            }

            return new NavigationExpression(navigationPropertyName, navigationPropertyType, navigationPropertyIndex);
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var fmt = propertyType == null ? 
                (propertyIndex.HasValue ? "{0}[{1}]" : "{0}") : 
                (propertyIndex.HasValue ? "{0}[{1}] as {2}" : "{0} as {2}");

            return String.Format(fmt,
                propertyName.QualifiedName,
                propertyIndex.GetValueOrDefault(),
                propertyType == null ? String.Empty : propertyType.Name);
        }

        /// <summary>
        /// Applies the navigation expression to the specified dependency object.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The dependency object to which to apply the expression.</param>
        /// <returns>The dependency object that was navigated to, or <see langword="null"/> if no valid target was found.</returns>
        public DependencyObject ApplyExpression(UltravioletContext uv, DependencyObject source)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(source, nameof(source));

            var dp = DependencyProperty.FindByStylingName(uv, source, propertyName.Owner, propertyName.Name);
            if (dp == null)
                return null;

            var child = source.GetUntypedValue(dp) as DependencyObject;
            if (child != null)
            {
                var isIndexed = propertyIndex.HasValue;
                var isList = child is IList;
                var isIndexable = child is IIndexable;

                if (isIndexed)
                {
                    if (isIndexable)
                    {
                        var indexable = ((IIndexable)child);
                        var index = propertyIndex.Value;
                        if (index < 0 || index >= indexable.Count)
                            return null;

                        return indexable[index] as DependencyObject;
                    }

                    if (isList)
                    {
                        var list = ((IList)child);
                        var index = propertyIndex.Value;
                        if (index < 0 || index >= list.Count)
                            return null;

                        return list[index] as DependencyObject;
                    }

                    return null;
                }

                return child;
            }

            return source;
        }

        /// <summary>
        /// Gets the name of the navigation property.
        /// </summary>
        public DependencyName PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets the type of the navigation property, if specified.
        /// </summary>
        public Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// Gets the index of the navigation property, if specified.
        /// </summary>
        public Int32? PropertyIndex
        {
            get { return propertyIndex; }
        }

        // Property values.
        private readonly DependencyName propertyName;
        private readonly Type propertyType;
        private readonly Int32? propertyIndex;
    }
}
