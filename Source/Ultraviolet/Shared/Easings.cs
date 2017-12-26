using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an easing function.
    /// </summary>
    /// <param name="t">A value from 0.0 to 1.0 indicating the current position within the easing function.</param>
    /// <returns>A value from 0.0 to 1.0 indicating the interpolation factor at the specified point in the easing function.</returns>
    public delegate Single EasingFunction(Single t);

    /// <summary>
    /// Contains standard easing functions.
    /// </summary>
    public static class Easings
    {
        /// <summary>
        /// A linear easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInLinear = (t) => 
        { 
            return t; 
        };

        /// <summary>
        /// A linear easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutLinear = (t) => 
        { 
            return 1f - t; 
        };

        /// <summary>
        /// A cubic easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInCubic = (t) =>
        {
            return t * t * t;
        };

        /// <summary>
        /// A cubic easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutCubic = (t) =>
        {
            t -= 1f;
            return t * t * t + 1;
        };

        /// <summary>
        /// A cubic easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutCubic = (t) =>
        {
            t /= 2f;
            if (t < 1f)
            {
                return 0.5f * t * t * t;
            }
            t -= 2f;
            return 0.5f * (t * t * t + 2);
        };

        /// <summary>
        /// A quadratic easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInQuadratic = (t) =>
        {
            return t * t;
        };

        /// <summary>
        /// A quadratic easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutQuadratic = (t) =>
        {
            return -1f * t * (t - 2);
        };

        /// <summary>
        /// A quadratic easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutQuadratic = (t) =>
        {
            t /= 2f;
            if (t < 1f)
            {
                return 0.5f * t * t;
            }
            t -= 1f;
            return -0.5f * (t * (t - 2) - 1);
        };

        /// <summary>
        /// A quartic easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInQuartic = (t) =>
        {
            return t * t * t * t;
        };

        /// <summary>
        /// A quartic easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutQuartic = (t) =>
        {
            t--;
            return -1f * (t * t * t * t - 1);
        };

        /// <summary>
        /// A quartic easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutQuartic = (t) =>
        {
            t /= 2f;
            if (t < 1f)
            {
                return 0.5f * t * t * t * t;
            }
            return -0.5f * (t * t * t * t - 2);
        };

        /// <summary>
        /// A quintic easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInQuintic = (t) =>
        {
            return t * t * t * t * t;
        };

        /// <summary>
        /// A quintic easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutQuintic = (t) =>
        {
            t -= 1f;
            return (t * t * t * t + 1);
        };

        /// <summary>
        /// A quintic easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutQuintic = (t) =>
        {
            t /= 2f;
            if (t < 1)
            {
                return 0.5f * t * t * t * t * t;
            }
            t -= 2;
            return 0.5f * (t * t * t * t * t + 2);
        };

        /// <summary>
        /// A sinusoidal easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInSin = (t) =>
        {
            return -1f * (float)Math.Cos(t * (Math.PI / 2.0)) + 1f;
        };

        /// <summary>
        /// A sinusoidal easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutSin = (t) =>
        {
            return +1f * (float)Math.Sin(t * (Math.PI / 2.0));
        };

        /// <summary>
        /// A sinusoidal easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutSin = (t) =>
        {
            return -0.5f * ((float)Math.Cos(Math.PI * t) - 1f);
        };

        /// <summary>
        /// An exponential easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInExponential = (t) =>
        {
            return (float)Math.Pow(2, 10 * (t - 1));
        };

        /// <summary>
        /// An exponential easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutExponential = (t) =>
        {
            return (float)-Math.Pow(2, -10 * t) + 1f;
        };

        /// <summary>
        /// An exponential easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutExponential = (t) =>
        {
            t /= 2f;
            if (t < 1)
            {
                return 0.5f * (float)Math.Pow(2, 10 * (t - 1));
            }
            t -= 1f;
            return 0.5f * ((float)-Math.Pow(2, -10 * t) + 2);
        };

        /// <summary>
        /// A circular easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInCircular = (t) =>
        {
            return -1f * ((float)Math.Sqrt(1 - t * t) - 1);
        };

        /// <summary>
        /// A circular easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutCircular = (t) =>
        {
            t -= 1f;
            return (float)Math.Sqrt(1 - t * t);
        };

        /// <summary>
        /// A circular easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutCircular = (t) =>
        {
            t /= 2f;
            if (t < 1)
            {
                return -0.5f * ((float)Math.Sqrt(1 - t * t) - 1);
            }
            return 0.5f * ((float)Math.Sqrt(1 - t * t) + 1);
        };

        /// <summary>
        /// A backtracking easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInBack = (t) =>
        {
            const Single s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        };

        /// <summary>
        /// A backtracking easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutBack = (t) =>
        {
            const Single s = 1.70158f;
            t -= 1f;
            return t * t * ((s + 1) * t + s) + 1;
        };

        /// <summary>
        /// A backtracking easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutBack = (t) =>
        {
            const Single s = 1.70158f;
            const Single s2 = s * 1.525f;
            t /= 2f;
            if (t < 1)
            {
                return 0.5f * (t * t * ((s2 + 1) * t - s));
            }
            t -= 2f;
            return 0.5f * t * t * ((s2 + 1) * t + s) + 2;
        };

        /// <summary>
        /// An elastic easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInElastic = (t) =>
        {
            var s = 1.70158;
            if (t == 0) return 0f;
            if (t == 1) return 1f;
            s = (0.3 / (2.0 * Math.PI) * Math.Asin(1.0));
            t -= 1f;
            return -(float)(Math.Pow(2, 10 * t) * Math.Sin((t - s) * (2.0 * Math.PI) / 0.3));
        };

        /// <summary>
        /// An elastic easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutElastic = (t) =>
        {
            var s = 1.70158;
            if (t == 0) return 0f;
            if (t == 1) return 1f;
            s = (0.3 / (2.0 * Math.PI) * Math.Asin(1.0));
            return (float)((Math.Pow(2, -10 * t) * Math.Sin((t - s) * (2.0 * Math.PI) / 0.3)) + 1.0);
        };

        /// <summary>
        /// An elastic easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutElastic = (t) =>
        {
            var s = 1.70158;
            if (t == 0) return 0f;
            if (t == 1) return 1f;
            var p = 0.3 * 1.5;
            t *= 2f;
            s = (p / (2.0 * Math.PI) * Math.Asin(1.0));
            if (t < 1)
            {
                t -= 1f;
                return -0.5f * (float)(Math.Pow(2, 10 * t) * Math.Sin((t - s) * (2.0 * Math.PI) / p));
            }
            t -= 1f;
            return (float)(Math.Pow(2, -10 * t) * Math.Sin((t - s) * (2.0 * Math.PI) / p) * 0.5) + 1f;
        };

        /// <summary>
        /// A bouncing easing in function.
        /// </summary>
        public static readonly EasingFunction EaseInBounce = (t) =>
        {
            return 1f - EaseOutBounce(1f - t);
        };

        /// <summary>
        /// A bouncing easing out function.
        /// </summary>
        public static readonly EasingFunction EaseOutBounce = (t) =>
        {
            if (t < 1f / 2.75f) 
            {
                return (7.5625f * t * t);
            }
            if (t < 2f / 2.75f) 
            {
                t -= (1.5f / 2.75f);
                return (7.5625f * t * t + 0.75f);
            }
            if (t < 2.5f / 2.75f)
            {
                t -= (2.25f / 2.75f);
                return (7.5625f * t * t + 0.9375f);
            }
            t -= (2.625f / 2.75f);
            return (7.5625f * t * t + 0.984375f);
        };

        /// <summary>
        /// A bouncing easing in/out function.
        /// </summary>
        public static readonly EasingFunction EaseInOutBounce = (t) =>
        {
            if (t < 0.5f)
            {
                return EaseInBounce(t * 2f) * 0.5f;
            }
            return EaseOutBounce(t * 2f - 1f) * 0.5f + 0.5f;
        };
    }
}
