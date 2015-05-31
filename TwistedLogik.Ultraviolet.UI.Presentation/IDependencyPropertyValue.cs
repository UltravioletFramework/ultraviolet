using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a dependency property's value.
    /// </summary>
    internal interface IDependencyPropertyValue
    {
        /// <summary>
        /// Called when the data source attached to the object which owns this value changes.
        /// </summary>
        /// <param name="dataSource">The new value of the <see cref="DependencyObject.DependencyDataSource"/> property.</param>
        void HandleDataSourceChanged(Object dataSource);

        /// <summary>
        /// Immediately digests the dependency property value.
        /// </summary>
        void DigestImmediately();

        /// <summary>
        /// Evaluates whether the dependency property's value has changed and, if so, invokes the appropriate callbacks.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        void Digest(UltravioletTime time);

        /// <summary>
        /// Sets the dependency property to draw its value from the specified trigger action.
        /// </summary>
        /// <param name="action">The trigger action from which to draw the dependency property value.</param>
        void Trigger(SetTriggerAction action);

        /// <summary>
        /// Applies the specified animation to the property value.
        /// </summary>
        /// <param name="animation">The animation to apply to the value, or <c>null</c> to disable animation.</param>
        /// <param name="clock">The clock which controls the animation's playback.</param>
        void Animate(AnimationBase animation, Clock clock);

        /// <summary>
        /// Binds the dependency property.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to which to bind the dependency property.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        void Bind(Type dataSourceType, String expression);

        /// <summary>
        /// Removes the dependency property's two-way binding.
        /// </summary>
        void Unbind();

        /// <summary>
        /// Recalculates the property's coerced value.
        /// </summary>
        void CoerceValue();

        /// <summary>
        /// Invalidates the cached display value for the dependency property. This will cause
        /// the interface to display the property's actual value, rather than the value most recently
        /// entered by the user (which may be different if coercion is involved).
        /// </summary>
        void InvalidateDisplayCache();

        /// <summary>
        /// Removes the property's current animation, if it has one.
        /// </summary>
        void ClearAnimation();

        /// <summary>
        /// Clears the dependency property's local value, if it has one.
        /// </summary>
        void ClearLocalValue();

        /// <summary>
        /// Clears the dependency property's styled value, if it has one.
        /// </summary>
        void ClearStyledValue();

        /// <summary>
        /// Clears the dependency property's triggered value, if it has one.
        /// </summary>
        void ClearTriggeredValue();

        /// <summary>
        /// Clears the dependency property's triggered value, if it has one and it is currently
        /// being provided by the specified trigger action. Otherwise, nothing happens.
        /// </summary>
        void ClearTriggeredValue(SetTriggerAction action);

        /// <summary>
        /// Sets the format string used to convert the dependency property value to a string.
        /// </summary>
        /// <param name="formatString">The format string used to convert the dependency property value to a string.</param>
        void SetFormatString(String formatString);

        /// <summary>
        /// Called when the value's associated clock is started.
        /// </summary>
        void ClockStarted();

        /// <summary>
        /// Called when the value's associated clock is stopped.
        /// </summary>
        void ClockStopped();

        /// <summary>
        /// Called when the value's associated clock is paused.
        /// </summary>
        void ClockPaused();

        /// <summary>
        /// Called when the value's associated clock is resumed.
        /// </summary>
        void ClockResumed();

        /// <summary>
        /// Gets or sets the dependency object which owns this property value.
        /// </summary>
        DependencyObject Owner
        {
            get;
        }

        /// <summary>
        /// Gets the dependency property which has its value represented by this object.
        /// </summary>
        DependencyProperty Property
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property's underlying value is a reference type.
        /// </summary>
        Boolean IsReferenceType
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property's underlying value is a value type.
        /// </summary>
        Boolean IsValueType
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property is bound to a property on a model object.
        /// </summary>
        Boolean IsDataBound
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the value is being animated.
        /// </summary>
        Boolean IsAnimated
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property is being coerced.
        /// </summary>
        Boolean IsCoerced
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property has a local value.
        /// </summary>
        Boolean HasLocalValue
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property has a styled value.
        /// </summary>
        Boolean HasStyledValue
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property has a triggered value.
        /// </summary>
        Boolean HasTriggeredValue
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this property's value has been defined.
        /// </summary>
        Boolean HasDefinedValue
        {
            get;
        }

        /// <summary>
        /// Gets the clock which is driving the value's current animation, if any.
        /// </summary>
        Clock AnimationClock
        {
            get;
        }
    }
}
