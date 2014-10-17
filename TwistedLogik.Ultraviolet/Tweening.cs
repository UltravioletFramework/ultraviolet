using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Contains methods for tweening values.
    /// </summary>
    public static class Tweening
    {
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
            return fn(t) < 0.5f ? tweenStart : tweenEnd;
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
            return t < 0.5f ? lerpStart : lerpEnd;
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
            return (Byte)(tweenStart + (delta * fn(t)));
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
            return (SByte)(tweenStart + (delta * fn(t)));
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
            return (Char)(tweenStart + (delta * fn(t)));
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
            return (Int16)(tweenStart + (delta * fn(t)));
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
            return (UInt16)(tweenStart + (delta * fn(t)));
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
            return (Int32)(tweenStart + (delta * fn(t)));
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
            return (UInt32)(tweenStart + (delta * fn(t)));
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
            return (Int64)(tweenStart + (delta * fn(t)));
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
            return (UInt64)(tweenStart + (delta * fn(t)));
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
            return (Single)(tweenStart + (delta * fn(t)));
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
            return (Single)(lerpStart + (delta * t));
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
            return (Double)(tweenStart + (delta * fn(t)));
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
            return (Double)(lerpStart + (delta * t));
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
            return (Decimal)(tweenStart + (delta * (Decimal)fn(t)));
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
            return (Decimal)(lerpStart + (delta * (Decimal)t));
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T Tween<T>(T tweenStart, T tweenEnd, EasingFunction fn, Single t) where T : IInterpolatable<T>
        {
            return tweenStart.Interpolate(tweenEnd, fn(t));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T Lerp<T>(T lerpStart, T lerpEnd, Single t) where T : IInterpolatable<T>
        {
            return lerpStart.Interpolate(lerpEnd, t);
        }

        /// <summary>
        /// Tweens the specified values.
        /// </summary>
        /// <param name="tweenStart">The start value.</param>
        /// <param name="tweenEnd">The end value.</param>
        /// <param name="fn">The easing function to apply.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the tween.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T TweenRef<T>(ref T tweenStart, ref T tweenEnd, EasingFunction fn, Single t) where T : struct, IInterpolatable<T>
        {
            return tweenStart.Interpolate(tweenEnd, fn(t));
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values.
        /// </summary>
        /// <param name="lerpStart">The start value.</param>
        /// <param name="lerpEnd">The end value.</param>
        /// <param name="t">A value between 0.0 and 1.0 indicating the current position in the interpolation.</param>
        /// <returns>A value which is interpolated from the specified start and end values.</returns>
        public static T LerpRef<T>(ref T lerpStart, ref T lerpEnd, Single t) where T : struct, IInterpolatable<T>
        {
            return lerpStart.Interpolate(lerpEnd, t);
        }
    }
}
