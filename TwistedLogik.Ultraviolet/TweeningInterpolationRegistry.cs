using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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
        /// Initializes a new instance of the <see cref="TweeningInterpolationRegistry"/> class.
        /// </summary>
        internal TweeningInterpolationRegistry()
        {
            miRegisterNullable = GetType().GetMethod("RegisterNullable", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Registers a default interpolator for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to register a default interpolator.</typeparam>
        public void RegisterDefault<T>()
        {
            var interpolateMethod = typeof(T).GetMethod("Interpolate",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(T), typeof(Single) }, null);

            if (interpolateMethod == null)
            {
                Register<T>(null);
                return;
            }

            var paramValueStart = Expression.Parameter(typeof(T), "valueStart");
            var paramValueEnd   = Expression.Parameter(typeof(T), "valueEnd");
            var paramFn         = Expression.Parameter(typeof(EasingFunction), "fn");
            var paramT          = Expression.Parameter(typeof(Single), "t");

            var expInvokeFn   = Expression.Invoke(Expression.Coalesce(paramFn, Expression.Constant(Easings.EaseInLinear)), paramT);
            var expLambdaBody = Expression.Call(paramValueStart, interpolateMethod, paramValueEnd, expInvokeFn);

            var interpolator = Expression.Lambda<Interpolator<T>>(expLambdaBody, paramValueStart, paramValueEnd, paramFn, paramT).Compile();

            Register<T>(interpolator);
        }

        /// <summary>
        /// Registers a custom interpolator function for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value for which to register a custom interpolator.</typeparam>
        /// <param name="interpolator">The custom interpolator function to register for the specified type.</param>
        public void Register<T>(Interpolator<T> interpolator)
        {
            interpolators[typeof(T)] = interpolator;

            if (typeof(T).IsValueType)
            {
                miRegisterNullable.MakeGenericMethod(typeof(T))
                    .Invoke(this, new Object[] { interpolator });
            }
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
        /// Attempts to retrieve the interpolator for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value for which to retrieve an interpolator.</typeparam>
        /// <param name="interpolator">The interpolator for the specified type, if one exists.</param>
        /// <returns><c>true</c> if an interpolator was registered for the specified type; otherwise, <c>false</c>.</returns>
        public Boolean TryGet<T>(out Interpolator<T> interpolator)
        {
            Object interpolatorObj;
            if (interpolators.TryGetValue(typeof(T), out interpolatorObj))
            {
                interpolator = (Interpolator<T>)interpolatorObj;
                return true;
            }
            else
            {
                RegisterDefault<T>();
                if (interpolators.TryGetValue(typeof(T), out interpolatorObj))
                {
                    interpolator = (Interpolator<T>)interpolatorObj;
                    return true;
                }
            }
            interpolator = null;
            return false;
        }

        /// <summary>
        /// Registers an interpolator for the nullable version of the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to register an interpolator.</typeparam>
        /// <param name="interpolator">The interpolator for the non-nullable type.</param>
        private void RegisterNullable<T>(Interpolator<T> interpolator) where T : struct
        {
            if (interpolator == null)
            {
                interpolators[typeof(T?)] = null;
            }
            else
            {
                var nullableInterpolator = new Interpolator<T?>((valueStart, valueEnd, fn, t) =>
                {
                    if (valueStart == null || valueEnd == null)
                        return null;

                    return interpolator(valueStart.GetValueOrDefault(), valueEnd.GetValueOrDefault(), fn, t);
                });
                interpolators[typeof(T?)] = nullableInterpolator;
            }
        }

        // State values.
        private readonly MethodInfo miRegisterNullable;
        private readonly Dictionary<Type, Object> interpolators = 
            new Dictionary<Type, Object>();
    }
}
