using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a dependency property's value.
    /// </summary>
    internal interface IDependencyPropertyValue
    {
        /// <summary>
        /// Evaluates whether the dependency property's value has changed and, if so, invokes the appropriate callbacks.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        void Digest(UltravioletTime time);

        /// <summary>
        /// Applies the specified animation to the property value.
        /// </summary>
        /// <param name="animation">The animation to apply to the value, or <c>null</c> to disable animation.</param>
        /// <param name="clock">The clock which controls the animation's playback.</param>
        void Animate(AnimationBase animation, StoryboardClock clock);

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
        /// Called when the value's associated storyboard clock is started.
        /// </summary>
        void StoryboardClockStarted();

        /// <summary>
        /// Called when the value's associated storyboard clock is stopped.
        /// </summary>
        void StoryboardClockStopped();

        /// <summary>
        /// Called when the value's associated storyboard clock is paused.
        /// </summary>
        void StoryboardClockPaused();

        /// <summary>
        /// Called when the value's associated storyboard clock is resumed.
        /// </summary>
        void StoryboardClockResumed();

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
    }
}
