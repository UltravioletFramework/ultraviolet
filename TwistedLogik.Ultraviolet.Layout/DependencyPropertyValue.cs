using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                this.comparer = GetComparisonFunction();

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
                
                bound             = true;
                dataBindingGetter = BindingExpressions.CreateBindingGetter<T>(viewModelType, expression);
                dataBindingSetter = BindingExpressions.CreateBindingSetter<T>(viewModelType, expression);

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

                    bound             = false;
                    dataBindingGetter = null;
                    dataBindingSetter = null;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <inheritdoc/>
            public void Digest()
            {
                var currentValue = GetValue();
                if (!comparer(currentValue, previousValue))
                {
                    if (Property.Metadata.ChangedCallback != null)
                    {
                        Property.Metadata.ChangedCallback(Owner);
                    }
                    previousValue = currentValue;
                }
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
                    if (dataBindingSetter == null)
                    {
                        throw new InvalidOperationException(LayoutStrings.BindingIsReadOnly);
                    }
                    dataBindingSetter(Owner.DependencyDataSource, value);
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
                    return dataBindingGetter(Owner.DependencyDataSource);
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
            /// Gets the comparison function for the current type.
            /// </summary>
            /// <returns>The comparison function for the current type.</returns>
            private static Func<T, T, Boolean> GetComparisonFunction()
            {
                if (typeof(T).IsClass)
                {
                    return referenceComparer;
                }
                else
                {
                    lock (comparerRegistry)
                    {
                        var typeHandle   = typeof(T).TypeHandle.Value.ToInt64();
                        var typeComparer = default(Func<T, T, Boolean>);

                        if (!comparerRegistry.TryGetValue(typeHandle, out typeComparer))
                        {
                            if (typeof(T).GetInterfaces().Where(x => x == typeof(IEquatable<T>)).Any())
                            {
                                typeComparer = GetIEquatableComparisonFunction();
                            }
                            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                typeComparer = GetNullableComparisonFunction();
                            }
                            else if (typeof(T).IsEnum)
                            {
                                typeComparer = GetEnumComparisonFunction();
                            }
                            else
                            {
                                typeComparer = GetFallbackComparisonFunction();
                            }
                            comparerRegistry[typeHandle] = typeComparer;
                        }

                        return typeComparer;
                    }
                }
            }

            /// <summary>
            /// Gets a fallback comparison function for value types which implement <see cref="IEquatable{T}"/>.
            /// </summary>
            /// <returns>The comparison function for the dependency property value's type.</returns>
            private static Func<T, T, Boolean> GetIEquatableComparisonFunction()
            {
                var equalsMethod = typeof(T).GetMethod("Equals", new[] { typeof(T) });

                var arg1 = Expression.Parameter(typeof(T), "o1");
                var arg2 = Expression.Parameter(typeof(T), "o2");

                return Expression.Lambda<Func<T, T, Boolean>>(
                    Expression.Call(arg1, equalsMethod, arg2), arg1, arg2).Compile();
            }

            /// <summary>
            /// Gets a comparison function for nullable value types.
            /// </summary>
            /// <returns>The comparison function for the dependency property value's type.</returns>
            private static Func<T, T, Boolean> GetNullableComparisonFunction()
            {
                var nullableType = typeof(T).GetGenericArguments()[0];
                var nullableEqualsMethod = typeof(Nullable).GetMethods()
                    .Where(x => x.Name == "Equals" && x.IsGenericMethod)
                    .Single().MakeGenericMethod(nullableType);

                var arg1 = Expression.Parameter(typeof(T), "o1");
                var arg2 = Expression.Parameter(typeof(T), "o2");

                return Expression.Lambda<Func<T, T, Boolean>>(
                    Expression.Call(nullableEqualsMethod, arg1, arg2), arg1, arg2).Compile();
            }

            /// <summary>
            /// Gets a comparison function for enumeration types.
            /// </summary>
            /// <returns>The comparison function for the dependency property value's type.</returns>
            private static Func<T, T, Boolean> GetEnumComparisonFunction()
            {
                var arg1 = Expression.Parameter(typeof(T), "o1");
                var arg2 = Expression.Parameter(typeof(T), "o2");
                var body = Expression.Equal(arg1, arg2);

                return Expression.Lambda<Func<T, T, Boolean>>(body, arg1, arg2).Compile();
            }

            /// <summary>
            /// Gets a fallback comparison function for types which don't fit any optimizable category.
            /// </summary>
            /// <returns>The comparison function for the dependency property value's type.</returns>
            private static Func<T, T, Boolean> GetFallbackComparisonFunction()
            {
                return (o1, o2) =>
                {
                    return o1.Equals(o2);
                };
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

            // Property values.
            private readonly DependencyObject owner;
            private readonly DependencyProperty property;
            private Boolean hasLocalValue;
            private Boolean hasStyledValue;
            private T localValue;
            private T styledValue;
            private T defaultValue;
            private T previousValue;

            // State values.
            private Boolean requiresDigest;
            private Boolean bound;
            private DataBindingGetter<T> dataBindingGetter;
            private DataBindingSetter<T> dataBindingSetter;

            // Comparison functions for various types.
            private static readonly Dictionary<Int64, Func<T, T, Boolean>> comparerRegistry = new Dictionary<Int64, Func<T, T, Boolean>>();
            private static readonly Func<T, T, Boolean> referenceComparer = (o1, o2) => { return (Object)o1 == (Object)o2; };
            private Func<T, T, Boolean> comparer;
        }
    }
}
