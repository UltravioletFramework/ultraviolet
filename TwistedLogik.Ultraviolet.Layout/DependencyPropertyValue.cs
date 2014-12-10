using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout
{
    partial class DependencyObject
    {
        /// <summary>
        /// Represents the value contained by a dependency property.
        /// </summary>
        internal class DependencyPropertyValue<T> : IDependencyPropertyValue
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyPropertyValue{T}"/> class.
            /// </summary>
            /// <param name="owner">The dependency object that owns the property value.</param>
            /// <param name="property">The dependency property which has its value represented by this object.</param>
            public DependencyPropertyValue(DependencyObject owner, DependencyProperty property)
            {
                Contract.Require(owner, "owner");
                Contract.Require(property, "property");

                this.owner    = owner;
                this.property = property;
                this.comparer = (DataBindingComparer<T>)BindingExpressions.GetComparisonFunction(typeof(T));

                this.isReferenceType = typeof(T).IsClass;
                this.isValueType     = typeof(T).IsValueType;

                if (property.Metadata.DefaultCallback != null)
                {
                    this.defaultValue = (T)property.Metadata.DefaultCallback();
                }

                UpdateRequiresDigest(GetValue());
            }

            /// <summary>
            /// Binds the dependency property.
            /// </summary>
            /// <param name="viewModelType">The type of view model to which to bind the dependency property.</param>
            /// <param name="expression">The binding expression with which to bind the dependency property.</param>
            public void Bind(Type viewModelType, String expression)
            {
                Contract.Require(viewModelType, "viewModelType");
                Contract.RequireNotEmpty(expression, "expression");

                var oldValue = GetValue();

                bound = true;
                CreateCachedBoundValue(viewModelType, expression);

                UpdateRequiresDigest(oldValue);
            }

            /// <summary>
            /// Removes the dependency property's two-way binding.
            /// </summary>
            public void Unbind()
            {
                if (bound)
                {
                    var oldValue = GetValue();

                    bound            = false;
                    cachedBoundValue = null;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <inheritdoc/>
            public void Digest()
            {
                var value   = default(T);
                var changed = false;

                if (cachedBoundValue != null)
                {
                    if (cachedBoundValue.CheckHasChanged())
                    {
                        value   = cachedBoundValue.Get();
                        changed = true;
                    }
                }
                else
                {
                    value   = GetValue();
                    changed = !comparer(value, previousValue);
                }

                if (changed && Property.Metadata.ChangedCallback != null)
                {
                    Property.Metadata.ChangedCallback(Owner);
                }
                previousValue = value;
            }

            /// <inheritdoc/>
            public void ClearLocalValue()
            {
                var oldValue = GetValue();

                hasLocalValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <inheritdoc/>
            public void ClearStyledValue()
            {
                var oldValue = GetValue();

                hasStyledValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <summary>
            /// Sets the dependency property's value.
            /// </summary>
            /// <param name="value">The value to set.</param>
            public void SetValue(T value)
            {
                if (IsDataBound)
                {
                    cachedBoundValue.Set(value);
                }
                else
                {
                    LocalValue = value;
                }
            }

            /// <summary>
            /// Gets the dependency property's calculated value.
            /// </summary>
            /// <returns>The dependency property's calculated value.</returns>
            public T GetValue()
            {
                if (IsDataBound)
                {
                    return cachedBoundValue.Get();
                }
                if (hasLocalValue)
                {
                    return localValue;
                }
                if (hasStyledValue)
                {
                    return styledValue;
                }
                if (Property.Metadata.IsInherited && Owner.DependencyContainer != null)
                {
                    return Owner.DependencyContainer.GetValue<T>(Property);
                }
                return defaultValue;
            }

            /// <summary>
            /// Gets the dependency property's local value.
            /// </summary>
            public T LocalValue
            {
                get { return localValue; }
                internal set
                {
                    var oldValue = GetValue();

                    localValue = value;
                    hasLocalValue = true;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets the dependency property's styled value.
            /// </summary>
            public T StyledValue
            {
                get { return styledValue; }
                internal set
                {
                    var oldValue = GetValue();

                    styledValue = value;
                    hasStyledValue = true;
                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets or sets the dependency property's default value.
            /// </summary>
            public T DefaultValue
            {
                get { return defaultValue; }
                set 
                {
                    var oldValue = GetValue();

                    defaultValue = value;
                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets the dependency property's previous value as of the last call to <see cref="Digest()"/>.
            /// </summary>
            public T PreviousValue
            {
                get { return previousValue; }
            }

            /// <inheritdoc/>
            public DependencyObject Owner
            {
                get { return owner; }
            }

            /// <inheritdoc/>
            public DependencyProperty Property
            {
                get { return property; }
            }

            /// <inheritdoc/>
            public Boolean IsReferenceType
            {
                get { return isReferenceType; }
            }

            /// <inheritdoc/>
            public Boolean IsValueType
            {
                get { return isValueType; }
            }

            /// <inheritdoc/>
            public Boolean IsDataBound
            {
                get { return bound; }
            }

            /// <inheritdoc/>
            public Boolean HasLocalValue
            {
                get { return hasLocalValue; }
            }

            /// <inheritdoc/>
            public Boolean HasStyledValue
            {
                get { return hasStyledValue; }
            }

            /// <summary>
            /// Gets a value indicating whether the specified pair of types require
            /// special conversion logic.
            /// </summary>
            /// <param name="type1">The first type to evaluate.</param>
            /// <param name="type2">The second type to evaluate.</param>
            /// <returns><c>true</c> if the specified types require special conversion logic; otherwise, <c>false</c>.</returns>
            private static Boolean TypesRequireSpecialConversion(Type type1, Type type2)
            {
                if (type1 == type2)
                    return false;

                var nullable1 = Nullable.GetUnderlyingType(type1);
                if (nullable1 != null && nullable1.IsMutuallyConvertibleTo(type2))
                {
                    return false;
                }

                var nullable2 = Nullable.GetUnderlyingType(type2);
                if (nullable2 != null && nullable2.IsMutuallyConvertibleTo(type1))
                {
                    return false;
                }

                if (nullable1 != null && nullable2 != null && nullable1.IsMutuallyConvertibleTo(nullable2))
                {
                    return false;
                }

                return !type1.IsMutuallyConvertibleTo(type2);
            }

            /// <summary>
            /// Updates the value which tracks whether this value needs to participate 
            /// in the digest cycle.
            /// </summary>
            /// <param name="oldValue">The property's value before the change which prompted this update.</param>
            private void UpdateRequiresDigest(T oldValue)
            {
                var requiresDigestNew = IsDataBound ||
                    (Property.Metadata.IsInherited && !hasLocalValue && !hasStyledValue);

                if (requiresDigestNew != requiresDigest)
                {
                    Owner.UpdateDigestParticipation(this, requiresDigestNew);
                    requiresDigest = requiresDigestNew;
                }

                var changed = !comparer(oldValue, GetValue());
                if (changed && !requiresDigestNew)
                {
                    if (Property.Metadata != null && Property.Metadata.ChangedCallback != null)
                    {
                        Property.Metadata.ChangedCallback(Owner);
                    }
                }
            }

            /// <summary>
            /// Creates the dependency property's cached bound value object.
            /// </summary>
            /// <param name="viewModelType">The type of view model to which the dependency property is bound.</param>
            /// <param name="expression">The dependency property's binding expression.</param>
            private void CreateCachedBoundValue(Type viewModelType, String expression)
            {
                var expressionType  = BindingExpressions.GetExpressionType(viewModelType, expression);

                if (TypesRequireSpecialConversion(expressionType, typeof(T)))
                {                
                    var valueType       = typeof(DependencyBoundValueConverting<,>).MakeGenericType(typeof(T), expressionType);
                    var valueInstance   = (IDependencyBoundValue<T>)Activator.CreateInstance(valueType, this, expressionType, viewModelType, expression);

                    cachedBoundValue = valueInstance;
                }
                else
                {
                    var valueType       = typeof(DependencyBoundValueNonConverting<>).MakeGenericType(typeof(T));
                    var valueInstance   = (IDependencyBoundValue<T>)Activator.CreateInstance(valueType, this, typeof(T), viewModelType, expression);

                    cachedBoundValue = valueInstance;
                }
            }

            // Property values.
            private readonly DependencyObject owner;
            private readonly DependencyProperty property;
            private readonly Boolean isReferenceType;
            private readonly Boolean isValueType;
            private Boolean hasLocalValue;
            private Boolean hasStyledValue;
            private T localValue;
            private T styledValue;
            private T defaultValue;
            private T previousValue;

            // State values.
            private readonly DataBindingComparer<T> comparer;
            private Boolean requiresDigest;
            private Boolean bound;
            private IDependencyBoundValue<T> cachedBoundValue;
        }
    }
}
