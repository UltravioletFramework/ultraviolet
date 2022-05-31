using System;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for tweening values.
    /// </summary>
    public static class Tweening
    {
        /// <summary>
        /// Initializes the <see cref="Tweening"/> type.
        /// </summary>
        static Tweening()
        {
            Interpolators.Register<Object>(Tween);
            Interpolators.Register<Boolean>(Tween);
            Interpolators.Register<Byte>(Tween);
            Interpolators.Register<SByte>(Tween);
            Interpolators.Register<Char>(Tween);
            Interpolators.Register<Int16>(Tween);
            Interpolators.Register<Int32>(Tween);
            Interpolators.Register<Int64>(Tween);
            Interpolators.Register<UInt16>(Tween);
            Interpolators.Register<UInt32>(Tween);
            Interpolators.Register<UInt64>(Tween);
            Interpolators.Register<Single>(Tween);
            Interpolators.Register<Double>(Tween);
            Interpolators.Register<Decimal>(Tween);

            Interpolators.RegisterDefault<Color>();
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Object Tween(Object tweenStart, Object tweenEnd, EasingFunction fn, Single t)
        {
            return (fn ?? Easings.EaseInLinear)(t) < 1.0f ? tweenStart : tweenEnd;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Object Lerp(Object lerpStart, Object lerpEnd, Single t)
        {
            return t < 1.0f ? lerpStart : lerpEnd;
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Boolean Tween(Boolean tweenStart, Boolean tweenEnd, EasingFunction fn, Single t)
        {
            return (fn ?? Easings.EaseInLinear)(t) < 1.0f ? tweenStart : tweenEnd;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Boolean Lerp(Boolean lerpStart, Boolean lerpEnd, Single t)
        {
            return t < 1.0f ? lerpStart : lerpEnd;
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Byte Tween(Byte tweenStart, Byte tweenEnd, EasingFunction fn, Single t)
        {
            if (tweenEnd < tweenStart)
            {
                return Tween(tweenEnd, tweenStart, fn, 1f - t);
            }
            var delta = (tweenEnd - tweenStart);
            return (Byte)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Byte Lerp(Byte lerpStart, Byte lerpEnd, Single t)
        {
            if (lerpEnd < lerpStart)
            {
                return Lerp(lerpEnd, lerpStart, 1f - t);
            }
            var delta = (lerpEnd - lerpStart);
            return (Byte)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static SByte Tween(SByte tweenStart, SByte tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return (SByte)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static SByte Lerp(SByte lerpStart, SByte lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return (SByte)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Char Tween(Char tweenStart, Char tweenEnd, EasingFunction fn, Single t)
        {
            if (tweenEnd < tweenStart)
            {
                return Tween(tweenEnd, tweenStart, fn, 1f - t);
            }
            var delta = (tweenEnd - tweenStart);
            return (Char)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Char Lerp(Char lerpStart, Char lerpEnd, Single t)
        {
            if (lerpEnd < lerpStart)
            {
                return Lerp(lerpEnd, lerpStart, 1f - t);
            }
            var delta = (lerpEnd - lerpStart);
            return (Char)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int16 Tween(Int16 tweenStart, Int16 tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return (Int16)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int16 Lerp(Int16 lerpStart, Int16 lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return (Int16)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt16 Tween(UInt16 tweenStart, UInt16 tweenEnd, EasingFunction fn, Single t)
        {
            if (tweenEnd < tweenStart)
            {
                return Tween(tweenEnd, tweenStart, fn, 1f - t);
            }
            var delta = (tweenEnd - tweenStart);
            return (UInt16)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt16 Lerp(UInt16 lerpStart, UInt16 lerpEnd, Single t)
        {
            if (lerpEnd < lerpStart)
            {
                return Lerp(lerpEnd, lerpStart, 1f - t);
            }
            var delta = (lerpEnd - lerpStart);
            return (UInt16)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int32 Tween(Int32 tweenStart, Int32 tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return (Int32)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int32 Lerp(Int32 lerpStart, Int32 lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return (Int32)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt32 Tween(UInt32 tweenStart, UInt32 tweenEnd, EasingFunction fn, Single t)
        {
            if (tweenEnd < tweenStart)
            {
                return Tween(tweenEnd, tweenStart, fn, 1f - t);
            }
            var delta = (tweenEnd - tweenStart);
            return (UInt32)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt32 Lerp(UInt32 lerpStart, UInt32 lerpEnd, Single t)
        {
            if (lerpEnd < lerpStart)
            {
                return Lerp(lerpEnd, lerpStart, 1f - t);
            }
            var delta = (lerpEnd - lerpStart);
            return (UInt32)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int64 Tween(Int64 tweenStart, Int64 tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return (Int64)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Int64 Lerp(Int64 lerpStart, Int64 lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return (Int64)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt64 Tween(UInt64 tweenStart, UInt64 tweenEnd, EasingFunction fn, Single t)
        {
            if (tweenEnd < tweenStart)
            {
                return Tween(tweenEnd, tweenStart, fn, t);
            }
            var delta = (tweenEnd - tweenStart);
            return (UInt64)(tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t)));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        [CLSCompliant(false)]
        public static UInt64 Lerp(UInt64 lerpStart, UInt64 lerpEnd, Single t)
        {
            if (lerpEnd < lerpStart)
            {
                return Lerp(lerpEnd, lerpStart, 1f - t);
            }
            var delta = (lerpEnd - lerpStart);
            return (UInt64)(lerpStart + (delta * t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Single Tween(Single tweenStart, Single tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Single Lerp(Single lerpStart, Single lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return lerpStart + (delta * t);
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Double Tween(Double tweenStart, Double tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return tweenStart + (delta * (fn ?? Easings.EaseInLinear)(t));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Double Lerp(Double lerpStart, Double lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return lerpStart + (delta * t);
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Decimal Tween(Decimal tweenStart, Decimal tweenEnd, EasingFunction fn, Single t)
        {
            var delta = (tweenEnd - tweenStart);
            return tweenStart + (delta * (Decimal)(fn ?? Easings.EaseInLinear)(t));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static Decimal Lerp(Decimal lerpStart, Decimal lerpEnd, Single t)
        {
            var delta = (lerpEnd - lerpStart);
            return lerpStart + (delta * (Decimal)t);
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <typeparam name="T">The type of value to tween.</typeparam>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T Tween<T>(T tweenStart, T tweenEnd, EasingFunction fn, Single t)
        {
            Interpolator<T> interpolator;
            if (!Interpolators.TryGet<T>(out interpolator) || interpolator == null)
            {
                return (T)Tween(tweenStart, (Object)tweenEnd, fn, t);
            }
            return interpolator(tweenStart, tweenEnd, fn, t);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <typeparam name="T">The type of value to tween.</typeparam>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T Lerp<T>(T lerpStart, T lerpEnd, Single t)
        {
            Interpolator<T> interpolator;
            if (!Interpolators.TryGet<T>(out interpolator) || interpolator == null)
            {
                return (T)Tween(lerpStart, (Object)lerpEnd, Easings.EaseInLinear, t);
            }
            return interpolator(lerpStart, lerpEnd, Easings.EaseInLinear, t);
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <typeparam name="T">The type of value to tween.</typeparam>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T TweenRef<T>(ref T tweenStart, ref T tweenEnd, EasingFunction fn, Single t) where T : struct
        {
            Interpolator<T> interpolator;
            if (!Interpolators.TryGet<T>(out interpolator) || interpolator == null)
            {
                return (T)Tween(tweenStart, (Object)tweenEnd, fn, t);
            }
            return interpolator(tweenStart, tweenEnd, fn, t);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <typeparam name="T">The type of value to tween.</typeparam>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T LerpRef<T>(ref T lerpStart, ref T lerpEnd, Single t) where T : struct
        {
            Interpolator<T> interpolator;
            if (!Interpolators.TryGet<T>(out interpolator) || interpolator == null)
            {
                return (T)Tween(lerpStart, (Object)lerpEnd, Easings.EaseInLinear, t);
            }
            return interpolator(lerpStart, lerpEnd, Easings.EaseInLinear, t);
        }

        /// <summary>
        /// Gets the tweening system's custom interpolation function registry.
        /// </summary>
        public static TweeningInterpolationRegistry Interpolators
        {
            get { return interpolators; }
        }

        // State values.
        private static readonly TweeningInterpolationRegistry interpolators = 
            new TweeningInterpolationRegistry();
    }
}
