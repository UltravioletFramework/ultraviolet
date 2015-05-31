using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an object that participates in the dependency property system.
    /// </summary>
    public abstract partial class DependencyObject
    {
        /// <summary>
        /// Gets a value indicating whether this object has a defined value for the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to evaluate.</param>
        /// <returns><c>true</c> if the specified property has a defined value on this object; otherwise, <c>false</c>.</returns>
        public Boolean HasDefinedValue(DependencyProperty dp)
        {
            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            return wrapper.HasDefinedValue;
        }

        /// <summary>
        /// Immediately digests the specified dependency property, but only if it is currently data bound.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to digest.</param>
        public void DigestImmediatelyIfDataBound(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            if (wrapper.IsDataBound)
            {
                wrapper.DigestImmediately();
            }
        }

        /// <summary>
        /// Immediately digests the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to digest.</param>
        public void DigestImmediately(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.DigestImmediately();
        }

        /// <summary>
        /// Evaluates whether any of the object's dependency property values have changed and, if so, invokes the appropriate callbacks.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Digest(UltravioletTime time)
        {
            for (int i = 0; i < digestedDependencyProperties.Count; i++)
            {
                digestedDependencyProperties[i].Digest(time);
            }
        }

        /// <summary>
        /// Initializes the object's dependency properties.
        /// </summary>
        public void InitializeDependencyProperties()
        {
            DependencyPropertySystem.InitializeObject(this);
        }

        /// <summary>
        /// Initializes the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to initialize.</param>
        public void InitializeDependencyProperty(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            GetDependencyPropertyValue(dp, dp.PropertyType);
        }

        /// <summary>
        /// Clears the bindings set on all of the object's dependency properties.
        /// </summary>
        public void ClearBindings()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                if (kvp.Value.IsDataBound)
                {
                    kvp.Value.Unbind();
                }
            }
        }

        /// <summary>
        /// Clears the animations on all of the object's dependency properties.
        /// </summary>
        public void ClearAnimations()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                if (!kvp.Value.Property.IsReadOnly)
                {
                    kvp.Value.ClearAnimation();
                }
            }
        }

        /// <summary>
        /// Clears the local values on all of the object's dependency properties.
        /// </summary>
        public void ClearLocalValues()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                if (!kvp.Value.Property.IsReadOnly)
                {
                    kvp.Value.ClearLocalValue();
                }
            }
        }

        /// <summary>
        /// Clears the styled values of all of the object's dependency properties.
        /// </summary>
        public void ClearStyledValues()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                if (!kvp.Value.Property.IsReadOnly)
                {
                    kvp.Value.ClearStyledValue();
                }
            }
            OnClearingStyles();
        }

        /// <summary>
        /// Clears the triggered values of all of the object's dependency properties.
        /// </summary>
        public void ClearTriggeredValues()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                if (!kvp.Value.Property.IsReadOnly)
                {
                    kvp.Value.ClearTriggeredValue();
                }
            }
            OnClearingTriggers();
        }

        /// <summary>
        /// Applies the specified animation to a dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to animate.</param>
        /// <param name="animation">The animation to apply to the dependency property, or <c>null</c> to cease animating the property.</param>
        /// <param name="clock">The clock which controls the animation's playback.</param>
        public void Animate(DependencyProperty dp, AnimationBase animation, Clock clock)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (animation != null)
                Contract.Require(clock, "clock");

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.Animate(animation, clock);
        }

        /// <summary>
        /// Animates the specified dependency property to the specified target value.
        /// </summary>
        /// <typeparam name="T">The type of value being animated.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to animate.</param>
        /// <param name="value">The animation's target value.</param>
        /// <param name="fn">The animation's easing function.</param>
        /// <param name="clock">The clock which drives the animation.</param>
        public void Animate<T>(DependencyProperty dp, T value, EasingFunction fn, Clock clock)
        {
            Contract.Require(dp, "dp");
            Contract.Require(clock, "clock");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.Animate(value, fn, clock);
        }

        /// <summary>
        /// Animates the specified dependency property to the specified target value using an internally-managed clock.
        /// </summary>
        /// <typeparam name="T">The type of value being animated.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to animate.</param>
        /// <param name="value">The animation's target value.</param>
        /// <param name="fn">The animation's easing function.</param>
        /// <param name="loopBehavior">A <see cref="LoopBehavior"/> value specifying the loop behavior of the animation.</param>
        /// <param name="duration">The animation's duration.</param>
        public void Animate<T>(DependencyProperty dp, T value, EasingFunction fn, LoopBehavior loopBehavior, TimeSpan duration)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.Animate(value, fn, loopBehavior, duration);
        }

        /// <summary>
        /// Binds a dependency property to a property on a model.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to bind.</param>
        /// <param name="dataSourceType">The type of the data source to which to bind the property.</param>
        /// <param name="expression">The binding expression with which to bind the property.</param>
        public void BindValue(DependencyProperty dp, Type dataSourceType, String expression)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.Bind(dataSourceType, expression);
        }

        /// <summary>
        /// Unbinds a dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to unbind.</param>
        public void UnbindValue(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.Unbind();
        }

        /// <summary>
        /// Recalculates the coerced value of the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance that identifies the dependency property to coerce.</param>
        public void CoerceValue(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.CoerceValue();
        }

        /// <summary>
        /// Invalidates the cached display value for the specified dependency property. This will cause
        /// the interface to display the property's actual value, rather than the value most recently
        /// entered by the user (which may be different if coercion is involved).
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to invalidate.</param>
        public void InvalidateDisplayCache(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.InvalidateDisplayCache();
        }

        /// <summary>
        /// Gets the value typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValue<T>(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            return wrapper.GetValue();
        }

        /// <summary>
        /// Sets the value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.SetValue(value);
        }

        /// <summary>
        /// Sets the value of the specified read-only dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValue<T>(DependencyPropertyKey key, T value)
        {
            Contract.Require(key, "dp");

            var dp = key.DependencyProperty;

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.SetValue(value);
        }

        /// <summary>
        /// Sets the default value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetDefaultValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.DefaultValue = value;
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetLocalValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.LocalValue = value;
        }

        /// <summary>
        /// Sets the styled value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetStyledValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.StyledValue = value;
        }

        /// <summary>
        /// Sets the triggered value of the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value;.;</param>
        /// <param name="action">The trigger action which will provide the dependency property's value.</param>
        public void SetTriggeredValue(DependencyProperty dp, SetTriggerAction action)
        {
            Contract.Require(dp, "dp");
            Contract.Require(action, "action");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.Trigger(action);
        }

        /// <summary>
        /// Sets the format string used to convert the specified dependency property's value to a string.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a format string.</param>
        /// <param name="formatString">The format string to set for the specified dependency property.</param>
        public void SetFormatString(DependencyProperty dp, String formatString)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var val = GetDependencyPropertyValue(dp, dp.PropertyType);
            val.SetFormatString(formatString);
        }

        /// <summary>
        /// Removes the specified dependency property's current animation, if it has one.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearAnimation(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearAnimation();
        }

        /// <summary>
        /// Clears the local value associated with the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearLocalValue(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearLocalValue();
        }

        /// <summary>
        /// Clears the styled value associated with the specified dependency property. 
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearStyledValue(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearStyledValue();
        }

        /// <summary>
        /// Clears the triggered value associated with the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearTriggeredValue(DependencyProperty dp)
        {
            Contract.Require(dp, "dp");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearTriggeredValue();
        }

        /// <summary>
        /// Clears the triggered value associated with the specified dependency property, if it is currently being provided
        /// by the specified trigger action. Otherwise, nothing happens.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        /// <param name="action">The trigger action to clear from the specified dependency property.</param>
        public void ClearTriggeredValue(DependencyProperty dp, SetTriggerAction action)
        {
            Contract.Require(dp, "dp");
            Contract.Require(action, "action");

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearTriggeredValue(action);
        }

        /// <summary>
        /// Gets or sets the data source from which the object's dependency properties will retrieve values if they are data bound.
        /// </summary>
        internal abstract Object DependencyDataSource
        {
            get;
        }

        /// <summary>
        /// Gets the dependency object's containing object.
        /// </summary>
        internal abstract DependencyObject DependencyContainer
        {
            get;
        }

        /// <summary>
        /// Occurs when the dependency object is clearing its styles.
        /// </summary>
        internal event UpfEventHandler ClearingStyles;

        /// <summary>
        /// Occurs when the dependency object is clearing its triggers.
        /// </summary>
        internal event UpfEventHandler ClearingTriggers;

        /// <summary>
        /// Occurs when the value of one of the object's dependency properties changes.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="property">The dependency property that was changed.</param>
        /// <param name="oldValue">The dependency property's old value.</param>
        /// <param name="newValue">The dependency property's new value.</param>
        protected internal virtual void OnPropertyChanged<T>(DependencyProperty property, T oldValue, T newValue)
        {

        }

        /// <summary>
        /// Occurs when a dependency property which potentially affects the object's measurement state is changed.
        /// </summary>
        protected internal virtual void OnMeasureAffectingPropertyChanged()
        {

        }

        /// <summary>
        /// Occurs when a dependency property which potentially affects the object's arrangement state is changed.
        /// </summary>
        protected internal virtual void OnArrangeAffectingPropertyChanged()
        {

        }

        /// <summary>
        /// Applies the styles from the specified style sheet to this object.
        /// </summary>
        /// <param name="document">The style sheet to apply to this object.</param>
        protected internal virtual void ApplyStyles(UvssDocument document)
        {

        }

        /// <summary>
        /// Applies a style to this object.
        /// </summary>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        /// <param name="dp">The dependency property to which to apply the style, or <c>null</c> if the style does
        /// not apply to a dependency property.</param>
        protected internal virtual void ApplyStyle(UvssStyle style, UvssSelector selector, DependencyProperty dp)
        {
            if (dp != null)
            {
                dp.ApplyStyle(this, style);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="DependencyDataSource"/> property changes.
        /// </summary>
        protected void OnDependencyDataSourceChanged()
        {
            var dataSource = DependencyDataSource;
            
            foreach (var kvp in dependencyPropertyValues)
            {
                kvp.Value.HandleDataSourceChanged(dataSource);
            }
        }

        /// <summary>
        /// Converts a string to a value to be applied to a styled dependency property.
        /// </summary>
        /// <param name="style">The style to resolve to a value.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The object that was created.</returns>
        private static Object ResolveStyledValue(UvssStyle style, Type type, IFormatProvider provider)
        {
            if (style.CachedResolvedValue != null && style.CachedResolvedValue.GetType() == type)
                return style.CachedResolvedValue;

            var value = style.Value.Trim();
            if (value == "null")
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            var resolvedValue = ObjectResolver.FromString(value, type, provider, true);
            style.CachedResolvedValue = resolvedValue;
            return resolvedValue;
        }

        /// <summary>
        /// Gets an instance of <see cref="DependencyPropertyValue{T}"/> for the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <returns>The <see cref="DependencyPropertyValue{T}"/> instance which was retrieved.</returns>
        private DependencyPropertyValue<T> GetDependencyPropertyValue<T>(DependencyProperty dp)
        {
            return (DependencyPropertyValue<T>)GetDependencyPropertyValue(dp, typeof(T));
        }

        /// <summary>
        /// Gets an instance of <see cref="IDependencyPropertyValue"/> for the specified value typed dependency property.
        /// </summary>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <param name="type">The type of value contained by the dependency property.</param>
        /// <returns>The <see cref="IDependencyPropertyValue"/> instance which was retrieved.</returns>
        private IDependencyPropertyValue GetDependencyPropertyValue(DependencyProperty dp, Type type)
        {
            IDependencyPropertyValue valueWrapper;
            dependencyPropertyValues.TryGetValue(dp.ID, out valueWrapper);

            if (valueWrapper == null)
            {
                var dpValueType = typeof(DependencyPropertyValue<>).MakeGenericType(type);
                valueWrapper = (IDependencyPropertyValue)Activator.CreateInstance(dpValueType, this, dp);
                dependencyPropertyValues[dp.ID] = valueWrapper;
            }
            return valueWrapper;
        }

        /// <summary>
        /// Updates the specified dependency property's participation in the digest cycle.
        /// </summary>
        /// <param name="value">The dependency property to update.</param>
        /// <param name="digest">A value indicating whether the dependency property should participate in the digest cycle.</param>
        private void UpdateDigestParticipation(IDependencyPropertyValue value, Boolean digest)
        {
            if (digest)
            {
                if (!digestedDependencyProperties.Contains(value))
                {
                    digestedDependencyProperties.Add(value);
                }
            }
            else
            {
                digestedDependencyProperties.Remove(value);
            }
        }

        /// <summary>
        /// Raises the <see cref="ClearingStyles"/> event.
        /// </summary>
        private void OnClearingStyles()
        {
            var temp = ClearingStyles;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ClearingTriggers"/> event.
        /// </summary>
        private void OnClearingTriggers()
        {
            var temp = ClearingTriggers;
            if (temp != null)
            {
                temp(this);
            }
        }

        // The list of values for this object's dependency properties.
        private readonly Dictionary<Int64, IDependencyPropertyValue> dependencyPropertyValues =
            new Dictionary<Int64, IDependencyPropertyValue>();

        // The list of dependency properties which need to participate in the digest cycle.
        private readonly List<IDependencyPropertyValue> digestedDependencyProperties = 
            new List<IDependencyPropertyValue>();
    }
}
