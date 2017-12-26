using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a value which is bound to a dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the bound value.</typeparam>
    internal abstract class DependencyBoundValue<T> : IDependencyPropertyChangeNotificationSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyBoundValue{TDependency}"/> class.
        /// </summary>
        /// <param name="value">The dependency property value which created this object.</param>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="dataSourceType">The type of the data source.</param>
        /// <param name="expression">The binding expression.</param>
        public DependencyBoundValue(IDependencyPropertyValue value, Type expressionType, Type dataSourceType, String expression)
        {
            this.dependencyValue = value;
            this.getter = (DataBindingGetter<T>)BindingExpressions.CreateBindingGetter(expressionType, dataSourceType, expression);
            this.setter = (DataBindingSetter<T>)BindingExpressions.CreateBindingSetter(expressionType, dataSourceType, expression);
            this.comparer = BindingExpressions.GetComparisonFunction(expressionType);
            this.cachedValue = GetUnderlyingValue();
            this.dpropReference = BindingExpressions.GetSimpleDependencyProperty(dataSourceType, expression);

            if (dpropReference != null)
            {
                var dataSource = value.Owner.DependencyDataSource;
                if (dataSource != null)
                {
                    HookDependencyProperty(dataSource);
                }
            }
        }

        /// <inheritdoc/>
        public void ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop)
        {
            dependencyValue.DigestImmediately();
        }

        /// <summary>
        /// Called when the bound object's data source is changed.
        /// </summary>
        /// <param name="dataSource">The new value of the <see cref="UIElement.DependencyDataSource"/> property.</param>
        public void HandleDataSourceChanged(Object dataSource)
        {
            UnhookDependencyProperty();

            if (dataSource != null)
                HookDependencyProperty(dataSource);
        }

        /// <summary>
        /// Invalidates any cached display values held by this binding.
        /// </summary>
        public virtual void InvalidateDisplayCache()
        {

        }

        /// <summary>
        /// Modifies the format string used to convert the bound value to a string, if applicable.
        /// </summary>
        public virtual void SetFormatString(String formatString)
        {

        }

        /// <summary>
        /// Checks to see whether the bound value has changed since the last digest cycle.
        /// </summary>
        /// <returns><see langword="true"/> if the value has changed; otherwise, false.</returns>
        public Boolean CheckHasChanged()
        {
            var value = GetUnderlyingValue();
            var changed = typeof(T).IsValueType ?
                !((DataBindingComparer<T>)comparer)(cachedValue, value) :
                !((DataBindingComparer<Object>)comparer)(cachedValue, value);

            if (changed)
            {
                cachedValue = value;
                OnCachedValueChanged(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the bound value is readable.
        /// </summary>
        public Boolean IsReadable
        {
            get { return getter != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the bound value is writable.
        /// </summary>
        public Boolean IsWritable
        {
            get { return setter != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this binding should suppress digestion, even if it would
        /// otherwise need to be part of the digest cycle.
        /// </summary>
        public Boolean SuppressDigestForDataBinding
        {
            get { return dpropReference != null; }
        }

        /// <summary>
        /// Occurs when the cached value changes.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected virtual void OnCachedValueChanged(T value)
        {

        }

        /// <summary>
        /// Gets the cached value.
        /// </summary>
        /// <returns>The cached value.</returns>
        protected T GetCachedValue()
        {
            return cachedValue;
        }

        /// <summary>
        /// Gets the underlying bound value.
        /// </summary>
        /// <returns>The underlying bound value.</returns>
        protected T GetUnderlyingValue()
        {
            if (!IsReadable)
            {
                throw new InvalidOperationException(PresentationStrings.BindingIsWriteOnly);
            }
            
            return getter(PresentationFoundation.GetDataSourceWrapper(dependencyValue.Owner.DependencyDataSource));
        }

        /// <summary>
        /// Sets the underlying bound value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected void SetUnderlyingValue(T value)
        {
            if (!IsWritable)
            {
                throw new InvalidOperationException(PresentationStrings.BindingIsReadOnly);
            }

            var owner    = dependencyValue.Owner;
            var metadata = dependencyValue.Property.GetMetadataForOwner(owner.GetType());
            if (metadata.CoerceValueCallback != null)
            {
                value = metadata.CoerceValue<T>(owner, value);
            }
            
            setter(PresentationFoundation.GetDataSourceWrapper(dependencyValue.Owner.DependencyDataSource), value);
            dependencyValue.DigestImmediately();
        }

        /// <summary>
        /// Hooks into the change notifications for the binding's associated dependency property, if applicable.
        /// </summary>
        private void HookDependencyProperty(Object dataSource)
        {
            if (dpropReference == null)
                return;

            var dobjDataSource = dataSource as DependencyObject;
            if (dobjDataSource == null)
                return;

            this.dataSource = dobjDataSource;

            DependencyProperty.RegisterChangeNotification(dobjDataSource, dpropReference, this);
        }

        /// <summary>
        /// Unhooks from the change notifications for the binding's associated dependency property, if applicable.
        /// </summary>
        private void UnhookDependencyProperty()
        {
            if (dpropReference == null)
                return;

            DependencyProperty.UnregisterChangeNotification(dataSource, dpropReference, this);
        }

        // State values.
        private readonly IDependencyPropertyValue dependencyValue;
        private readonly Delegate comparer;
        private readonly DataBindingGetter<T> getter;
        private readonly DataBindingSetter<T> setter;
        private T cachedValue;

        // The dependency property referenced by this binding, used for optimization.
        private readonly DependencyProperty dpropReference;
        private DependencyObject dataSource;

    }
}
