using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Contains methods for reifying instances of <see cref="UvssStoryboard"/> into instances of <see cref="Storyboard"/>.
    /// </summary>
    internal static class UvssStoryboardReifier
    {
        /// <summary>
        /// Initializes the <see cref="UvssStoryboardReifier"/> type.
        /// </summary>
        static UvssStoryboardReifier()
        {
            standardEasingFunctions["ease-in-linear"]          = Easings.EaseInLinear;
            standardEasingFunctions["ease-out-linear"]         = Easings.EaseOutLinear;
            standardEasingFunctions["ease-in-cubic"]           = Easings.EaseInCubic;
            standardEasingFunctions["ease-out-cubic"]          = Easings.EaseOutCubic;
            standardEasingFunctions["ease-in-quadratic"]       = Easings.EaseInQuadratic;
            standardEasingFunctions["ease-out-quadratic"]      = Easings.EaseOutQuadratic;
            standardEasingFunctions["ease-in-out-quadratic"]   = Easings.EaseInOutQuadratic;
            standardEasingFunctions["ease-in-quartic"]         = Easings.EaseInQuartic;
            standardEasingFunctions["ease-out-quartic"]        = Easings.EaseOutQuartic;
            standardEasingFunctions["ease-in-out-quartic"]     = Easings.EaseInOutQuartic;
            standardEasingFunctions["ease-in-quintic"]         = Easings.EaseInQuintic;
            standardEasingFunctions["ease-out-quintic"]        = Easings.EaseInQuintic;
            standardEasingFunctions["ease-in-out-quintic"]     = Easings.EaseInOutQuintic;
            standardEasingFunctions["ease-in-sin"]             = Easings.EaseInSin;
            standardEasingFunctions["ease-out-sin"]            = Easings.EaseOutSin;
            standardEasingFunctions["ease-in-out-sin"]         = Easings.EaseInOutSin;
            standardEasingFunctions["ease-in-exponential"]     = Easings.EaseInExponential;
            standardEasingFunctions["ease-out-exponential"]    = Easings.EaseOutExponential;
            standardEasingFunctions["ease-in-out-exponential"] = Easings.EaseInOutExponential;
            standardEasingFunctions["ease-in-circular"]        = Easings.EaseInCircular;
            standardEasingFunctions["ease-out-circular"]       = Easings.EaseOutCircular;
            standardEasingFunctions["ease-in-out-circular"]    = Easings.EaseInOutCircular;
            standardEasingFunctions["ease-in-back"]            = Easings.EaseInBack;
            standardEasingFunctions["ease-out-back"]           = Easings.EaseOutBack;
            standardEasingFunctions["ease-in-out-back"]        = Easings.EaseInOutBack;
            standardEasingFunctions["ease-in-elastic"]         = Easings.EaseInElastic;
            standardEasingFunctions["ease-out-elastic"]        = Easings.EaseOutElastic;
            standardEasingFunctions["ease-in-out-elastic"]     = Easings.EaseInOutElastic;
            standardEasingFunctions["ease-in-bounce"]          = Easings.EaseInBounce;
            standardEasingFunctions["ease-out-bounce"]         = Easings.EaseOutBounce;
            standardEasingFunctions["ease-in-out-bounce"]      = Easings.EaseInOutBounce;
        }

        /// <summary>
        /// Reifies an instance of the <see cref="UvssStoryboard"/> class into a new instance of the <see cref="Storyboard"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="storyboardDefinition">The storyboard definition to reify.</param>
        /// <returns>The new instance of <see cref="Storyboard"/> that was created.</returns>
        public static Storyboard ReifyStoryboard(UltravioletContext uv, UvssStoryboard storyboardDefinition)
        {
            Contract.Require(uv, "uv");
            Contract.Require(storyboardDefinition, "storyboardDefinition");

            var storyboard = new Storyboard(uv);
            storyboard.LoopBehavior = storyboardDefinition.LoopBehavior;

            foreach (var targetDefinition in storyboardDefinition.Targets)
            {
                var target = ReifyStoryboardTarget(uv, targetDefinition);
                storyboard.Targets.Add(target);
            }

            return storyboard;
        }

        /// <summary>
        /// Creates a new <see cref="StoryboardTarget"/> instance based on the specified <see cref="UvssStoryboardTarget"/>.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="targetDefinition">The storyboard target definition.</param>
        /// <returns>The reified storyboard target.</returns>
        public static StoryboardTarget ReifyStoryboardTarget(UltravioletContext uv, UvssStoryboardTarget targetDefinition)
        {
            Contract.Require(targetDefinition, "targetDefinition");

            var target = new StoryboardTarget(targetDefinition.Selector);

            foreach (var animationDefinition in targetDefinition.Animations)
            {
                var animatedProperty = String.Empty;
                var animation = ReifyStoryboardAnimation(uv, targetDefinition, animationDefinition, out animatedProperty);
                if (animation != null)
                {
                    target.Animations.Add(animatedProperty, animation);
                }
            }

            return target;
        }

        /// <summary>
        /// Creates a new <see cref="AnimationBase"/> instance based on the specified <see cref="UvssStoryboardAnimation"/>.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="targetDefinition">The type filter on the storyboard target.</param>
        /// <param name="animationDefinition">The storyboard animation definition.</param>
        /// <param name="animatedProperty">The name of the dependency property which is being animated.</param>
        /// <returns>The reified storyboard animation.</returns>
        public static AnimationBase ReifyStoryboardAnimation(UltravioletContext uv, UvssStoryboardTarget targetDefinition, UvssStoryboardAnimation animationDefinition, out String animatedProperty)
        {
            Contract.Require(targetDefinition, "targetDefinition");
            Contract.Require(animationDefinition, "animationDefinition");

            var propertyName = animationDefinition.AnimatedProperty;
            var propertyType = GetDependencyPropertyType(uv, targetDefinition.Filter, ref propertyName);
            animatedProperty = propertyName;

            if (propertyType == null)
                return null;

            var animationType = GetAnimationType(propertyType);
            if (animationType == null)
                return null;

            var animation = (AnimationBase)Activator.CreateInstance(animationType);

            foreach (var keyframeDefinition in animationDefinition.Keyframes)
            {
                var keyframe = ReifyStoryboardAnimationKeyframe(uv, keyframeDefinition, propertyType);
                animation.AddKeyframe(keyframe);
            }

            return animation;
        }

        /// <summary>
        /// Creates a new <see cref="AnimationKeyframeBase"/> instance based on the specified <see cref="UvssStoryboardKeyframe"/>.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="keyframeDefinition">The storyboard keyframe definition.</param>
        /// <param name="animatedType">The type of value which is being animated.</param>
        /// <returns>The reified storyboard animation keyframe.</returns>
        public static AnimationKeyframeBase ReifyStoryboardAnimationKeyframe(UltravioletContext uv, UvssStoryboardKeyframe keyframeDefinition, Type animatedType)
        {
            Contract.Require(keyframeDefinition, "keyframeDefinition");
            Contract.Require(animatedType, "animatedType");

            var keyframeType = typeof(AnimationKeyframe<>).MakeGenericType(animatedType);

            var time   = TimeSpan.FromMilliseconds(keyframeDefinition.Time);
            var easing = ParseEasingFunction(keyframeDefinition.Easing);
            var str    = keyframeDefinition.Value == null ? null : keyframeDefinition.Value.Trim();
            var value  = String.IsNullOrWhiteSpace(str) ? null : ObjectResolver.FromString(str, animatedType, true);

            var keyframe = (value == null) ?
                    (AnimationKeyframeBase)Activator.CreateInstance(keyframeType, time, easing) :
                    (AnimationKeyframeBase)Activator.CreateInstance(keyframeType, time, value, easing);

            return keyframe;
        }

        /// <summary>
        /// Parses the name of an easing function into an instance of the <see cref="EasingFunction"/> delegate.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <returns>The <see cref="EasingFunction"/> delegate instance which was created.</returns>
        private static EasingFunction ParseEasingFunction(String str)
        {
            if (String.IsNullOrEmpty(str))
                return null;

            EasingFunction function;
            standardEasingFunctions.TryGetValue(str, out function);
            return function;
        }

        /// <summary>
        /// Resolves the specified element type name into a <see cref="Type"/> object.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element type name to resolve.</param>
        /// <returns>The resolved <see cref="Type"/> object.</returns>
        private static Type ResolveElementType(UltravioletContext uv, String name)
        {
            Type type;
            if (!uv.GetUI().GetPresentationFoundation().GetKnownElement(name, false, out type))
            {
                throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(name));
            }
            return type;
        }

        /// <summary>
        /// Gets the type of the specified dependency property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="filter">The type filter on the storyboard target.</param>
        /// <param name="property">The name of the dependency property being animated.</param>
        /// <returns>The type of the specified dependency property.</returns>
        private static Type GetDependencyPropertyType(UltravioletContext uv, UvssStoryboardTargetFilter filter, ref String property)
        {
            var propertyName = property;

            var possiblePropertyMatches =
                from f in filter
                let elementType = ResolveElementType(uv, f)
                let propertyID = DependencyProperty.FindByStylingName(propertyName, elementType)
                let propertyType = (propertyID == null) ? null : propertyID.PropertyType
                where propertyType != null
                select new { PropertyID = propertyID, PropertyType = propertyType };

            var distinctNames = possiblePropertyMatches.Select(x => x.PropertyID.Name).Distinct();
            if (distinctNames.Count() > 1)
                throw new InvalidOperationException(PresentationStrings.AmbiguousDependencyProperty.Format(property));

            if (distinctNames.Any())
                property = distinctNames.Single();

            var distinctTypes = possiblePropertyMatches.Select(x => x.PropertyType).Distinct();
            if (distinctTypes.Count() > 1)
                throw new InvalidOperationException(PresentationStrings.AmbiguousDependencyProperty.Format(property));

            return distinctTypes.FirstOrDefault();
        }

        /// <summary>
        /// Gets the animation type which corresponds to the specified type of value.
        /// </summary>
        /// <param name="type">The type of value being animated.</param>
        /// <returns>The animation type which corresponds to the specified type of value.</returns>
        private static Type GetAnimationType(Type type)
        {
            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
                type = type.GetGenericArguments()[0];

            return typeof(Animation<>).MakeGenericType(type);
        }

        // The standard easing functions which can be specified in an animation.
        private static readonly Dictionary<String, EasingFunction> standardEasingFunctions = 
            new Dictionary<String, EasingFunction>();
    }
}
