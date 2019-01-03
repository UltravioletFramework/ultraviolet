using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
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
        /// <returns><see langword="true"/> if the specified property has a defined value on this object; otherwise, <see langword="false"/>.</returns>
        public Boolean HasDefinedValue(DependencyProperty dp)
        {
            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType, false);
            return wrapper != null && wrapper.HasDefinedValue;
        }

        /// <summary>
        /// Gets a value indicating whether this object has the default value for the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to evaluate.</param>
        /// <returns><see langword="true"/> if the specified property has its default value on this object; otherwise, <see langword="false"/>.</returns>
        public Boolean HasDefaultValue(DependencyProperty dp)
        {
            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType, false);
            return wrapper == null || wrapper.HasDefaultValue;
        }

        /// <summary>
        /// Raises any pending change events for this object's dependency properties.
        /// </summary>
        public void RaisePendingChangeEvents()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                value.RaisePendingChangeEvent();
            }
        }

        /// <summary>
        /// Immediately digests the specified dependency property, but only if it is currently data bound.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to digest.</param>
        public void DigestImmediatelyIfDataBound(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            if (wrapper.IsDataBound)
            {
                wrapper.DigestImmediately();
                OnDigestingImmediately(dp);
            }
        }

        /// <summary>
        /// Immediately digests all of the object's dependency properties.
        /// </summary>
        public void DigestImmediately()
        {
            for (int i = 0; i < dependencyPropertyValuesNeedingDigestion.Count; i++)
            {
                var value = dependencyPropertyValuesNeedingDigestion[i];
                value.DigestImmediately();
                OnDigestingImmediately(value.Property);
            }
        }

        /// <summary>
        /// Immediately digests the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to digest.</param>
        public void DigestImmediately(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.DigestImmediately();
            OnDigestingImmediately(dp);
        }

        /// <summary>
        /// Evaluates whether any of the object's dependency property values have changed and, if so, invokes the appropriate callbacks.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Digest(UltravioletTime time)
        {
            var digestCycleID = PresentationFoundation.Instance.DigestCycleID;
            if (digestCycleID == lastDigestedCycleID)
                return;

            lastDigestedCycleID = digestCycleID;

            for (int i = 0; i < dependencyPropertyValuesOfTypeDependencyObject.Count; i++)
            {
                var value = dependencyPropertyValuesOfTypeDependencyObject[i];
                var dobj = (DependencyObject)value.GetUntypedValue();
                if (dobj != null)
                {
                    if (dobj.WasInvalidatedAfterDigest(value.LastChangedDigestCycleID))
                        value.HandleForcedInvalidation();

                    dobj.Digest(time);
                }
            }

            for (int i = 0; i < dependencyPropertyValuesNeedingDigestion.Count; i++)
            {
                var value = dependencyPropertyValuesNeedingDigestion[i];
                value.Digest(time);
            }

            OnDigesting(time);
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
            Contract.Require(dp, nameof(dp));

            GetDependencyPropertyValue(dp, dp.PropertyType);
        }

        /// <summary>
        /// Clears the bindings set on all of the object's dependency properties.
        /// </summary>
        public void ClearBindings()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                if (value.IsDataBound)
                    value.Unbind();
            }
        }

        /// <summary>
        /// Clears the animations on all of the object's dependency properties.
        /// </summary>
        public void ClearAnimations()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                if (!value.Property.IsReadOnly)
                    value.ClearAnimation();
            }
        }

        /// <summary>
        /// Clears the local values on all of the object's dependency properties.
        /// </summary>
        public void ClearLocalValues()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                if (!value.Property.IsReadOnly)
                    value.ClearLocalValue();
            }
        }

        /// <summary>
        /// Clears the styled values of all of the object's dependency properties.
        /// </summary>
        public void ClearStyledValues()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                if (!value.Property.IsReadOnly)
                    value.ClearStyledValue();
            }

            if (attachedTriggers != null)
            {
                foreach (var trigger in attachedTriggers)
                {
                    trigger.DetachInternal(this, false);
                }
                attachedTriggers.Clear();
            }
        }

        /// <summary>
        /// Clears the triggered values of all of the object's dependency properties.
        /// </summary>
        public void ClearTriggeredValues()
        {
            for (int i = 0; i < dependencyPropertyValues.Count; i++)
            {
                var value = dependencyPropertyValues[i];
                if (!value.Property.IsReadOnly)
                    value.ClearTriggeredValue();
            }
        }

        /// <summary>
        /// Applies the specified animation to a dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to animate.</param>
        /// <param name="animation">The animation to apply to the dependency property, or <see langword="null"/> to cease animating the property.</param>
        /// <param name="clock">The clock which controls the animation's playback.</param>
        public void Animate(DependencyProperty dp, AnimationBase animation, Clock clock)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (animation != null)
                Contract.Require(clock, nameof(clock));

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
            Contract.Require(dp, nameof(dp));
            Contract.Require(clock, nameof(clock));

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
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.CoerceValue();
        }

        /// <summary>
        /// Invalidates the value of this dependency object. During the next digest cycle, it will be
        /// treated as if it has changed, even if it is the same instance.
        /// </summary>
        public void InvalidateDependencyObject()
        {
            var digestCycleID = PresentationFoundation.Instance.DigestCycleID;
            invalidatedDigestCycleID = digestCycleID;
        }

        /// <summary>
        /// Invalidates the cached display value for the specified dependency property. This will cause
        /// the interface to display the property's actual value, rather than the value most recently
        /// entered by the user (which may be different if coercion is involved).
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to invalidate.</param>
        public void InvalidateDisplayCache(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.InvalidateDisplayCache();
        }

        /// <summary>
        /// Invalidates the cached inherited values for all of this object's dependency properties.
        /// </summary>
        public void InvalidateInheritanceCache()
        {
            foreach (var dp in dependencyPropertyValues)
                dp.InvalidateInheritanceCache();

            Media.VisualTreeHelper.ForEachChild(this, null, (child, state) =>
            {
                child.InvalidateInheritanceCache();
            });
        }

        /// <summary>
        /// Invalidates the cached inherited values for the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance that identifies the dependency property to invalidate.</param>
        public void InvalidateInheritanceCache(DependencyProperty dp)
        {
            var dpValue = GetDependencyPropertyValue(dp, dp.PropertyType, false);
            if (dpValue != null)
                dpValue.InvalidateInheritanceCache();

            Media.VisualTreeHelper.ForEachChild(this, dp, (child, state) =>
            {
                child.InvalidateInheritanceCache((DependencyProperty)state);
            });
        }

        /// <summary>
        /// Gets the value of the specified dependency property, without regard for its type.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public Object GetUntypedValue(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            return wrapper.GetUntypedValue();
        }

        /// <summary>
        /// Gets the value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValue<T>(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));

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
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to 
        /// the read-only dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValue<T>(DependencyPropertyKey key, T value)
        {
            Contract.Require(key, nameof(key));

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
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies 
        /// the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetDefaultValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.DefaultValue = value;
        }

        /// <summary>
        /// Sets the default value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to 
        /// the read-only dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetDefaultValue<T>(DependencyPropertyKey key, T value)
        {
            Contract.Require(key, nameof(key));

            var dp = key.DependencyProperty;

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.SetValue(value);
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies 
        /// the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetLocalValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.LocalValue = value;
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to 
        /// the read-only dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetLocalValue<T>(DependencyPropertyKey key, T value)
        {
            Contract.Require(key, nameof(key));

            var dp = key.DependencyProperty;

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.LocalValue = value;
        }

        /// <summary>
        /// Sets the styled value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies 
        /// the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetStyledValue<T>(DependencyProperty dp, T value)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.StyledValue = value;
        }

        /// <summary>
        /// Sets the styled value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to 
        /// the read-only dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetStyledValue<T>(DependencyPropertyKey key, T value)
        {
            Contract.Require(key, nameof(key));

            var dp = key.DependencyProperty;

            if (!typeof(T).Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.StyledValue = value;
        }

        /// <summary>
        /// Sets the triggered value of the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies 
        /// the dependency property for which to set a value;.;</param>
        /// <param name="action">The trigger action which will provide the dependency property's value.</param>
        public void SetTriggeredValue(DependencyProperty dp, SetTriggerAction action)
        {
            Contract.Require(dp, nameof(dp));
            Contract.Require(action, nameof(action));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.Trigger(action);
        }

        /// <summary>
        /// Sets the triggered value of the specified dependency property.
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to 
        /// the read-only dependency property for which to set a value.</param>
        /// <param name="action">The trigger action which will provide the dependency property's value.</param>
        public void SetTriggeredValue(DependencyPropertyKey key, SetTriggerAction action)
        {
            Contract.Require(key, nameof(key));
            Contract.Require(action, nameof(action));

            var dp = key.DependencyProperty;

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
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearAnimation();
        }

        /// <summary>
        /// Removes the specified dependency property's current animation, if it has one.
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only 
        /// dependency property to clear.</param>
        public void ClearAnimation(DependencyPropertyKey key)
        {
            Contract.Require(key, nameof(key));

            var wrapper = GetDependencyPropertyValue(key.DependencyProperty, key.DependencyProperty.PropertyType);
            wrapper.ClearAnimation();
        }

        /// <summary>
        /// Clears the local value associated with the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearLocalValue(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearLocalValue();
        }

        /// <summary>
        /// Clears the local value associated with the specified dependency property.
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only 
        /// dependency property to clear.</param>
        public void ClearLocalValue(DependencyPropertyKey key)
        {
            Contract.Require(key, nameof(key));

            var wrapper = GetDependencyPropertyValue(key.DependencyProperty, key.DependencyProperty.PropertyType);
            wrapper.ClearLocalValue();
        }

        /// <summary>
        /// Clears the styled value associated with the specified dependency property. 
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearStyledValue(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearStyledValue();
        }

        /// <summary>
        /// Clears the styled value associated with the specified dependency property. 
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only 
        /// dependency property to clear.</param>
        public void ClearStyledValue(DependencyPropertyKey key)
        {
            Contract.Require(key, nameof(key));

            var wrapper = GetDependencyPropertyValue(key.DependencyProperty, key.DependencyProperty.PropertyType);
            wrapper.ClearStyledValue();
        }

        /// <summary>
        /// Clears the triggered value associated with the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearTriggeredValue(DependencyProperty dp)
        {
            Contract.Require(dp, nameof(dp));

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
            Contract.Require(dp, nameof(dp));
            Contract.Require(action, nameof(action));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            wrapper.ClearTriggeredValue(action);
        }

        /// <summary>
        /// Clears the triggered value associated with the specified dependency property.
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only 
        /// dependency property to clear.</param>
        public void ClearTriggeredValue(DependencyPropertyKey key)
        {
            Contract.Require(key, nameof(key));

            var wrapper = GetDependencyPropertyValue(key.DependencyProperty, key.DependencyProperty.PropertyType);
            wrapper.ClearTriggeredValue();
        }

        /// <summary>
        /// Clears the triggered value associated with the specified dependency property.
        /// </summary>
        /// <param name="key">A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only 
        /// dependency property to clear.</param>
        /// <param name="action">The trigger action to clear from the specified dependency property.</param>
        public void ClearTriggeredValue(DependencyPropertyKey key, SetTriggerAction action)
        {
            Contract.Require(key, nameof(key));
            Contract.Require(action, nameof(action));

            var wrapper = GetDependencyPropertyValue(key.DependencyProperty, key.DependencyProperty.PropertyType);
            wrapper.ClearTriggeredValue(action);
        }

        /// <summary>
        /// Gets a value indicating whether this dependency object was forcibly invalidated
        /// on or after the specified digest cycle.
        /// </summary>
        /// <param name="digestID">The identifier of the digest cycle to compare against 
        /// the cycle during which the object was invalidated.</param>
        /// <returns><see langword="true"/> if the object was invalidated on or after the
        /// specified digest cycle; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean WasInvalidatedAfterDigest(Int64 digestID)
        {
            return invalidatedDigestCycleID >= digestID;
        }

        /// <summary>
        /// Gets a value indicating whether change events are being deferred for this object.
        /// </summary>
        public virtual Boolean IsDeferringChangeEvents
        {
            get { return false; }
        }

        /// <summary>
        /// Attaches a trigger to this object.
        /// </summary>
        /// <param name="trigger">The trigger to attach to this object.</param>
        internal void AttachTrigger(UvssTrigger trigger)
        {
            if (attachedTriggers == null)
                attachedTriggers = new List<UvssTrigger>();

            attachedTriggers.Add(trigger);
        }

        /// <summary>
        /// Detaches a trigger from this object.
        /// </summary>
        /// <param name="trigger">The trigger to detach from this object.</param>
        internal void DetachTrigger(UvssTrigger trigger)
        {
            if (attachedTriggers == null)
                throw new InvalidOperationException();

            attachedTriggers.Remove(trigger);
        }

        /// <summary>
        /// Enlists a dependency property on this object into the specified storyboard instance.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> that identifies the dependency property to enlist.</param>
        /// <param name="storyboardInstance">The <see cref="StoryboardInstance"/> into which to enlist the dependency property.</param>
        /// <param name="animation">The animation to apply to the dependency property.</param>
        internal void EnlistDependencyPropertyInStoryboard(DependencyProperty dp, StoryboardInstance storyboardInstance, AnimationBase animation)
        {
            Contract.Require(dp, nameof(dp));
            Contract.Require(storyboardInstance, nameof(storyboardInstance));
            Contract.Require(animation, nameof(animation));

            if (dp.IsReadOnly)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(dp.Name));

            var wrapper = GetDependencyPropertyValue(dp, dp.PropertyType);
            storyboardInstance.Enlist(wrapper, animation);
        }

        /// <summary>
        /// Gets or sets the object's data source according to the context in which it was declared. Usually this
        /// will either be a view or a control's templated parent.
        /// </summary>
        internal Object DeclarativeDataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the object's declarative view model or templated parent.
        /// </summary>
        internal Object DeclarativeViewModelOrTemplate
        {
            get
            {
                var view = DeclarativeDataSource as PresentationFoundationView;
                if (view != null)
                {
                    return view.ViewModel;
                }
                return DeclarativeDataSource;
            }
        }

        /// <summary>
        /// Gets or sets the data source from which the object's dependency properties will retrieve values if they are data bound.
        /// </summary>
        internal virtual Object DependencyDataSource
        {
            get
            {
                return DeclarativeViewModelOrTemplate;
            }
        }

        /// <summary>
        /// Gets the dependency object's containing object.
        /// </summary>
        internal virtual DependencyObject DependencyContainer
        {
            get { return null; }
        }

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
        /// Occurs when a dependency property which potentially affects the object's visual bounds is changed.
        /// </summary>
        protected internal virtual void OnVisualBoundsAffectingPropertyChanged()
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
        /// Applies a style to the element.
        /// </summary>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        /// <param name="navigationExpression">The navigation expression associated with the style.</param>
        /// <param name="dprop">A <see cref="DependencyProperty"/> that identifies the dependency property which is being styled.</param>
        protected internal virtual void ApplyStyle(UvssRule style, UvssSelector selector, NavigationExpression? navigationExpression, DependencyProperty dprop)
        {
            if (dprop != null)
            {
                dprop.ApplyStyle(this, style);
            }
        }

        /// <summary>
        /// Called when a dependency property on this object is being immediately digested.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> value which identifies the dependency property being digested.</param>
        protected virtual void OnDigestingImmediately(DependencyProperty dp)
        {

        }

        /// <summary>
        /// Called when the object is being digested.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnDigesting(UltravioletTime time)
        {

        }

        /// <summary>
        /// Converts a string to a value to be applied to a styled dependency property.
        /// </summary>
        /// <param name="style">The style to resolve to a value.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The object that was created.</returns>
        private static Object ResolveStyledValue(UvssRule style, Type type, IFormatProvider provider)
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
        /// <param name="createIfMissing">A value indicating whether to create the property value if it does not exist.</param>
        /// <returns>The <see cref="DependencyPropertyValue{T}"/> instance which was retrieved.</returns>
        private DependencyPropertyValue<T> GetDependencyPropertyValue<T>(DependencyProperty dp, Boolean createIfMissing = true)
        {
            return (DependencyPropertyValue<T>)GetDependencyPropertyValue(dp, typeof(T), createIfMissing);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDependencyPropertyValue"/> for the specified value typed dependency property.
        /// </summary>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <param name="type">The type of value contained by the dependency property.</param>
        /// <param name="createIfMissing">A value indicating whether to create the property value if it does not exist.</param>
        /// <returns>The <see cref="IDependencyPropertyValue"/> instance which was retrieved.</returns>
        private IDependencyPropertyValue GetDependencyPropertyValue(DependencyProperty dp, Type type, Boolean createIfMissing = true)
        {
            IDependencyPropertyValue valueWrapper;
            dependencyPropertyValuesByID.TryGetValue(dp.ID, out valueWrapper);

            if (valueWrapper == null && createIfMissing)
            {
                var dpValueType = typeof(DependencyPropertyValue<>).MakeGenericType(type);
                valueWrapper = (IDependencyPropertyValue)Activator.CreateInstance(dpValueType, this, dp);
                dependencyPropertyValuesByID[dp.ID] = valueWrapper;
                dependencyPropertyValues.Add(valueWrapper);

                if (typeof(DependencyObject).IsAssignableFrom(type))
                    dependencyPropertyValuesOfTypeDependencyObject.Add(valueWrapper);
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
                if (!dependencyPropertyValuesNeedingDigestion.Contains(value))
                {
                    dependencyPropertyValuesNeedingDigestion.Add(value);
                }
            }
            else
            {
                dependencyPropertyValuesNeedingDigestion.Remove(value);
            }
        }
        
        // The list of attached triggers.
        private List<UvssTrigger> attachedTriggers;

        // The list of values for this object's dependency properties.
        private readonly Dictionary<Int64, IDependencyPropertyValue> dependencyPropertyValuesByID =
            new Dictionary<Int64, IDependencyPropertyValue>();
        private readonly List<IDependencyPropertyValue> dependencyPropertyValues =
            new List<IDependencyPropertyValue>();
        private readonly List<IDependencyPropertyValue> dependencyPropertyValuesOfTypeDependencyObject =
            new List<IDependencyPropertyValue>();
        private readonly List<IDependencyPropertyValue> dependencyPropertyValuesNeedingDigestion =
            new List<IDependencyPropertyValue>();

        // State values.
        private Int64 lastDigestedCycleID;
        private Int64 invalidatedDigestCycleID = -1;        
    }
}
