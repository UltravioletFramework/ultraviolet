using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a custom interpolation function.
    /// </summary>
    /// <typeparam name="T">The type of value being interpolated.</typeparam>
    /// <param name="valueStart">The start value.</param>
    /// <param name="valueEnd">The end value.</param>
    /// <param name="fn">The easing function to apply.</param>
    /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
    /// <returns>A value which is interpolated from the specified start and end values.</returns>
    public delegate T Interpolator<T>(T valueStart, T valueEnd, EasingFunction fn, Single t);

    /// <summary>
    /// Represents the registry of interpolation methods used by the Ultraviolet tweening system.
    /// </summary>
    public sealed class TweeningInterpolationRegistry
    {
        /// <summary>
        /// Registers a custom interpolator function for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value for which to register a custom interpolator.</typeparam>
        /// <param name="interpolator">The custom interpolator function to register for the specified type.</param>
        public void Register<T>(Interpolator<T> interpolator)
        {
            Contract.Require(interpolator, "interpolator");

            interpolators[typeof(T)] = interpolator;
        }

        /// <summary>
        /// Unregisters any custom interpolation function for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value for which to unregister a custom interpolator.</typeparam>
        /// <returns><c>true</c> if the specified type had a function that was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean Unregister<T>()
        {
            return interpolators.Remove(typeof(T));
        }

        /// <summary>
        /// Gets a value indicating whether the specified type has a custom interpolation function.
        /// </summary>
        /// <typeparam name="T">The type of value for which to determine whether a custom interpolator exists.</typeparam>
        /// <returns><c>true</c> if the specified type has a custom interpolation function; otherwise, <c>false</c>.</returns>
        public Boolean Contains<T>()
        {
            return interpolators.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Gets the custom interpolation function for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value for which to retrieve a custom interpolation function.</typeparam>
        /// <returns>The custom interpolation function for the specified type.</returns>
        public Interpolator<T> Get<T>()
        {
            Object interpolator;
            interpolators.TryGetValue(typeof(T), out interpolator);
            return interpolator as Interpolator<T>;
        }

        // State values.
        private readonly Dictionary<Type, Object> interpolators = 
            new Dictionary<Type, Object>();
    }
}
