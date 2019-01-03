using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    partial class DependencyObject
    {
        /// <summary>
        /// Represents the value contained by a dependency property.
        /// </summary>
        internal class DependencyPropertyValue<T> : IDependencyPropertyValue
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyPropertyValue{T}"/> class.
            /// </summary>
            /// <param name="owner">The dependency object that owns the property value.</param>
            /// <param name="property">The dependency property which has its value represented by this object.</param>
            public DependencyPropertyValue(DependencyObject owner, DependencyProperty property)
            {
                Contract.Require(owner, nameof(owner));
                Contract.Require(property, nameof(property));

                this.owner = owner;
                this.property = property;
                this.comparer = BindingExpressions.GetComparisonFunction(typeof(T));
                this.metadata = property.GetMetadataForOwner(owner.GetType());
                this.flags = DependencyPropertyValueFlags.None;
                this.defaultValue = (T)(metadata.DefaultValue ?? default(T));

                if (typeof(T).GetInterfaces().Any(x => x == typeof(IResourceWrapper)))
                    flags |= DependencyPropertyValueFlags.IsResourceWrapper;
                if (typeof(T).IsClass)
                    flags |= DependencyPropertyValueFlags.IsReferenceType;
                if (typeof(T).IsValueType)
                    flags |= DependencyPropertyValueFlags.IsValueType;

                if (metadata.IsInherited)
                    this.useDefaultValue = false;

                if (metadata.HasDefaultValue && IsCoerced)
                {
                    this.coercedValue = metadata.CoerceValue(owner, this.defaultValue);
                    this.useDefaultValue = false;
                }

                UpdateRequiresDigest(GetValue());
            }

            /// <inheritdoc/>
            public void RaisePendingChangeEvent()
            {
                if (!IsPendingChangeEvent)
                    return;

                IsPendingChangeEvent = false;

                var oldValue = defaultValue;
                var newValue = GetValue();
                metadata.HandleChanged(owner, property, oldValue, newValue);
                lastChangedDigestCycleID = PresentationFoundation.Instance?.DigestCycleID ?? 0;
            }

            /// <inheritdoc/>
            public void HandleForcedInvalidation()
            {
                var value = GetValue();
                HandleChanged(value, value);
            }

            /// <inheritdoc/>
            public void HandleDataSourceChanged(Object dataSource)
            {
                if (cachedBoundValue != null)
                    cachedBoundValue.HandleDataSourceChanged(dataSource);
            }

            /// <inheritdoc/>
            public void DigestImmediately()
            {
                if (IsPendingChangeEvent && !IsDeferringChangeEvents())
                {
                    RaisePendingChangeEvent();
                }

                CheckForChanges();
            }

            /// <inheritdoc/>
            public void Digest(UltravioletTime time)
            {
                CheckForChanges(time);
            }

            /// <inheritdoc/>
            public void Trigger(SetTriggerAction action)
            {
                Contract.Require(action, nameof(action));

                var oldValue = GetValue();

                triggeredValueSource = action;
                triggeredValue = action.GetValue<T>();
                useDefaultValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <inheritdoc/>
            public void Animate(AnimationBase animation, Clock clock)
            {
                Contract.Require(animation, nameof(animation));
                Contract.Require(clock, nameof(clock));

                var animState = GetAnimationState();
                if (animState.Animation == animation && animState.Clock == clock)
                    return;

                var oldValue = GetValue();

                ClearAnimation();

                animState.Animation = (Animation<T>)animation;
                animState.CurrentValue = GetInitialAnimatedValue(oldValue);
                animState.HandOffValue = oldValue;
                animState.Clock = clock;
                animState.Clock.Subscribe(this);
                useDefaultValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <summary>
            /// Animates the dependency value.
            /// </summary>
            /// <param name="value">The animation's target value.</param>
            /// <param name="fn">The animation's easing function.</param>
            /// <param name="clock">The clock which drives the animation.</param>
            public void Animate(T value, EasingFunction fn, Clock clock)
            {
                Contract.Require(clock, nameof(clock));

                var oldValue = GetValue();

                ClearAnimation();

                var animState = GetAnimationState();
                animState.Animation = null;
                animState.CurrentValue = GetInitialAnimatedValue(oldValue);
                animState.TargetValue = value;
                animState.HandOffValue = oldValue;
                animState.EasingFunction = fn ?? Easings.EaseInLinear;
                animState.Clock = clock;
                animState.Clock.Subscribe(this);
                useDefaultValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <summary>
            /// Animates the dependency value using an internally-managed clock.
            /// </summary>
            /// <param name="value">The animation's target value.</param>
            /// <param name="fn">The animation's easing function.</param>
            /// <param name="loopBehavior">A <see cref="LoopBehavior"/> value specifying the loop behavior of the animation.</param>
            /// <param name="duration">The animation's duration.</param>
            public void Animate(T value, EasingFunction fn, LoopBehavior loopBehavior, TimeSpan duration)
            {
                var clock = SimpleClockPool.Instance.Retrieve(this);
                var clockValue = clock.Value;

                clockValue.SetLoopBehavior(loopBehavior);
                clockValue.SetDuration(duration);
                clockValue.Start();
                useDefaultValue = false;

                Animate(value, fn, clockValue);
            }

            /// <inheritdoc/>
            public void BeginStoryboard(AnimationBase animation, StoryboardInstance storyboardInstance)
            {
                Animate(animation, storyboardInstance.StoryboardClock);

                var animState = GetAnimationState();
                animState.StoryboardInstance = storyboardInstance;
                useDefaultValue = false;
            }

            /// <inheritdoc/>
            public void StopStoryboard(AnimationBase animation, StoryboardInstance storyboardInstance)
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                if (animState.StoryboardInstance != storyboardInstance)
                    return;

                ClearAnimation();
            }

            /// <inheritdoc/>
            public void Bind(Type dataSourceType, String expression)
            {
                Contract.Require(dataSourceType, nameof(dataSourceType));
                Contract.RequireNotEmpty(expression, nameof(expression));

                var oldValue = GetValue();

                IsDataBound = true;
                CreateCachedBoundValue(dataSourceType, expression);
                useDefaultValue = false;

                UpdateRequiresDigest(oldValue);
            }

            /// <inheritdoc/>
            public void Unbind()
            {
                if (IsDataBound)
                {
                    var oldValue = GetValue();

                    IsDataBound = false;

                    cachedBoundValue.HandleDataSourceChanged(null);
                    cachedBoundValue = null;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <inheritdoc/>
            public void CoerceValue()
            {
                if (!IsCoerced)
                    return;

                var oldCoercedValue = coercedValue;
                coercedValue = metadata.CoerceValue<T>(owner, GetValueInternal(true, false));

                if (!Compare(coercedValue, oldCoercedValue))
                {
                    HandleChanged(oldCoercedValue, coercedValue);
                    previousValue = coercedValue;
                }
            }

            /// <inheritdoc/>
            public void InvalidateDisplayCache()
            {
                if (cachedBoundValue == null)
                    return;

                cachedBoundValue.InvalidateDisplayCache();
            }

            /// <inheritdoc/>
            public void InvalidateInheritanceCache()
            {
                useCachedInheritedValue = false;
            }

            /// <inheritdoc/>
            public void ClearAnimation()
            {
                var oldValue = GetValue();

                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                if (animState.Clock != null)
                    animState.Clock.Unsubscribe(this);

                ReleasePooledAnimationClock();

                animState.Animation = null;
                animState.Clock = null;
                animState.EasingFunction = null;
                animState.StoryboardInstance = null;

                UpdateRequiresDigest(oldValue);

                this.useDefaultValue = GetValueSource() == ValueSource.DefaultValue;
            }

            /// <inheritdoc/>
            public void ClearLocalValue()
            {
                var oldValue = GetValue();

                HasLocalValue = false;

                UpdateRequiresDigest(oldValue);

                this.useDefaultValue = GetValueSource() == ValueSource.DefaultValue;
            }

            /// <inheritdoc/>
            public void ClearStyledValue()
            {
                var oldValue = GetValue();

                HasStyledValue = false;

                UpdateRequiresDigest(oldValue);

                this.useDefaultValue = GetValueSource() == ValueSource.DefaultValue;
            }

            /// <inheritdoc/>
            public void ClearTriggeredValue()
            {
                var oldValue = GetValue();

                triggeredValue = default(T);
                triggeredValueSource = null;

                UpdateRequiresDigest(oldValue);

                this.useDefaultValue = GetValueSource() == ValueSource.DefaultValue;
            }

            /// <inheritdoc/>
            public void ClearTriggeredValue(SetTriggerAction trigger)
            {
                Contract.Require(trigger, nameof(trigger));

                if (triggeredValueSource != trigger)
                    return;

                ClearTriggeredValue();

                this.useDefaultValue = GetValueSource() == ValueSource.DefaultValue;
            }

            /// <inheritdoc/>
            public void SetFormatString(String formatString)
            {
                if (cachedBoundValue == null)
                    return;

                cachedBoundValue.SetFormatString(formatString);
            }

            /// <inheritdoc/>
            public Object GetUntypedValue()
            {
                return GetValue();
            }

            /// <summary>
            /// Sets the dependency property's value.
            /// </summary>
            /// <param name="value">The value to set.</param>
            public void SetValue(T value)
            {
                useDefaultValue = false;

                if (IsAnimated)
                {
                    ClearAnimation();
                }
                if (IsDataBound)
                {
                    SetCachedBoundValue(value);
                }
                else
                {
                    LocalValue = value;
                }
            }

            /// <summary>
            /// Gets the dependency property's calculated value.
            /// </summary>
            /// <returns>The dependency property's calculated value.</returns>
            public T GetValue()
            {
                return GetValueInternal(true);
            }

            /// <summary>
            /// Gets the dependency property's local value.
            /// </summary>
            public T LocalValue
            {
                get { return localValue; }
                internal set
                {
                    var oldValue = GetValue();

                    value = UpdateCoercedValue(value);

                    localValue = value;
                    useDefaultValue = false;
                    HasLocalValue = true;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets the dependency property's styled value.
            /// </summary>
            public T StyledValue
            {
                get { return styledValue; }
                internal set
                {
                    var oldValue = GetValue();

                    value = UpdateCoercedValue(value);

                    styledValue = value;
                    useDefaultValue = false;
                    HasStyledValue = true;

                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets or sets the dependency property's default value.
            /// </summary>
            public T DefaultValue
            {
                get { return defaultValue; }
                set
                {
                    var oldValue = GetValue();

                    value = UpdateCoercedValue(value);

                    defaultValue = value;
                    UpdateRequiresDigest(oldValue);
                }
            }

            /// <summary>
            /// Gets the dependency property's previous value as of the last call to <see cref="Digest(UltravioletTime)"/>.
            /// </summary>
            public T PreviousValue
            {
                get { return previousValue; }
            }

            /// <inheritdoc/>
            public DependencyObject Owner
            {
                get { return owner; }
            }

            /// <inheritdoc/>
            public DependencyProperty Property
            {
                get { return property; }
            }

            /// <inheritdoc/>
            public Clock AnimationClock
            {
                get { return GetAnimationState(false)?.Clock; }
            }

            /// <summary>
            /// Gets a value indicating whether the value must be digested.
            /// </summary>
            public Boolean RequiresDigest
            {
                get { return (flags & DependencyPropertyValueFlags.RequiresDigest) != 0; }
                private set
                {
                    flags = (flags & ~DependencyPropertyValueFlags.RequiresDigest) |
                        (value ? DependencyPropertyValueFlags.RequiresDigest : DependencyPropertyValueFlags.None);
                }
            }

            /// <summary>
            /// Gets a value indicating whether a change event is going to be raised.
            /// </summary>
            public Boolean IsPendingChangeEvent
            {
                get { return (flags & DependencyPropertyValueFlags.IsPendingChangeEvent) != 0; }
                private set
                {
                    flags = (flags & ~DependencyPropertyValueFlags.IsPendingChangeEvent) |
                        (value ? DependencyPropertyValueFlags.IsPendingChangeEvent : DependencyPropertyValueFlags.None);
                }
            }

            /// <inheritdoc/>
            public Boolean IsResourceWrapper
            {
                get { return (flags & DependencyPropertyValueFlags.IsResourceWrapper) != 0; }
            }

            /// <inheritdoc/>
            public Boolean IsReferenceType
            {
                get { return (flags & DependencyPropertyValueFlags.IsReferenceType) != 0; }
            }

            /// <inheritdoc/>
            public Boolean IsValueType
            {
                get { return (flags & DependencyPropertyValueFlags.IsValueType) != 0; }
            }

            /// <inheritdoc/>
            public Boolean IsDataBound
            {
                get { return (flags & DependencyPropertyValueFlags.IsDataBound) != 0; }
                set
                {
                    flags = (flags & ~DependencyPropertyValueFlags.IsDataBound) |
                        (value ? DependencyPropertyValueFlags.IsDataBound : DependencyPropertyValueFlags.None);
                }
            }

            /// <inheritdoc/>
            public Boolean IsAnimated
            {
                get { return GetAnimationState(false)?.Clock != null; }
            }

            /// <inheritdoc/>
            public Boolean IsCoerced
            {
                get { return metadata.CoerceValueCallback != null; }
            }

            /// <inheritdoc/>
            public Boolean IsInherited
            {
                get { return metadata.IsInherited; }
            }

            /// <inheritdoc/>
            public Boolean HasLocalValue
            {
                get { return (flags & DependencyPropertyValueFlags.HasLocalValue) != 0; }
                private set
                {
                    flags = (flags & ~DependencyPropertyValueFlags.HasLocalValue) |
                        (value ? DependencyPropertyValueFlags.HasLocalValue : DependencyPropertyValueFlags.None);
                }
            }

            /// <inheritdoc/>
            public Boolean HasStyledValue
            {
                get { return (flags & DependencyPropertyValueFlags.HasStyledValue) != 0; }
                private set
                {
                    flags = (flags & ~DependencyPropertyValueFlags.HasStyledValue) |
                        (value ? DependencyPropertyValueFlags.HasStyledValue : DependencyPropertyValueFlags.None);
                }
            }

            /// <inheritdoc/>
            public Boolean HasTriggeredValue
            {
                get { return triggeredValueSource != null; }
            }

            /// <inheritdoc/>
            public Boolean HasDefinedValue
            {
                get
                {
                    return IsDataBound || IsAnimated || HasLocalValue || HasStyledValue;
                }
            }

            /// <inheritdoc/>
            public Boolean HasDefaultValue
            {
                get { return GetValueSource() == ValueSource.DefaultValue; }
            }

            /// <inheritdoc/>
            public Int64 LastChangedDigestCycleID
            {
                get { return lastChangedDigestCycleID; }
            }

            /// <inheritdoc/>
            void IDependencyPropertyValue.ClockStarted()
            {

            }

            /// <inheritdoc/>
            void IDependencyPropertyValue.ClockStopped()
            {
                ClearAnimation();
            }

            /// <inheritdoc/>
            void IDependencyPropertyValue.ClockPaused()
            {

            }

            /// <inheritdoc/>
            void IDependencyPropertyValue.ClockResumed()
            {

            }

            /// <summary>
            /// Gets a value indicating whether the specified pair of types require
            /// special conversion logic.
            /// </summary>
            /// <param name="type1">The first type to evaluate.</param>
            /// <param name="type2">The second type to evaluate.</param>
            /// <returns><see langword="true"/> if the specified types require special conversion logic; otherwise, <see langword="false"/>.</returns>
            private static Boolean TypesRequireSpecialConversion(Type type1, Type type2)
            {
                if (type1 == type2)
                    return false;

                var nullable1 = Nullable.GetUnderlyingType(type1);
                if (nullable1 != null && nullable1.IsMutuallyConvertibleTo(type2))
                {
                    return false;
                }

                var nullable2 = Nullable.GetUnderlyingType(type2);
                if (nullable2 != null && nullable2.IsMutuallyConvertibleTo(type1))
                {
                    return false;
                }

                if (nullable1 != null && nullable2 != null && nullable1.IsMutuallyConvertibleTo(nullable2))
                {
                    return false;
                }

                return !type1.IsMutuallyConvertibleTo(type2);
            }

            /// <summary>
            /// Compiles an expression which produces converting bound values for the specified type.
            /// </summary>
            private static Delegate CreateDependencyPropertyBoundValueConvertingCtor(Type valueType)
            {
                var valueCtor = default(Delegate);
                var valueTypeCtor = valueType.GetConstructor(
                    new[] { typeof(IDependencyPropertyValue), typeof(Type), typeof(Type), typeof(String), typeof(Boolean) });

                var expParamValue = Expression.Parameter(typeof(IDependencyPropertyValue), "value");
                var expParamExpressionType = Expression.Parameter(typeof(Type), "expressionType");
                var expParamDataSourceType = Expression.Parameter(typeof(Type), "dataSourceType");
                var expParamExpression = Expression.Parameter(typeof(String), "expression");
                var expParamCoerceToString = Expression.Parameter(typeof(Boolean), "coerceToString");
                var expCtor = Expression.New(valueTypeCtor,
                    expParamValue, expParamExpressionType, expParamDataSourceType, expParamExpression, expParamCoerceToString);

                lock (cachedBoundValueCtors)
                {
                    valueCtor = Expression.Lambda<DependencyBoundValueConvertingCtor<T>>(expCtor,
                        expParamValue, expParamExpressionType, expParamDataSourceType, expParamExpression, expParamCoerceToString).Compile();

                    cachedBoundValueCtors[valueType] = valueCtor;
                }

                return valueCtor;
            }

            /// <summary>
            /// Compiles an expression which produces non-converting bound values for the specified type.
            /// </summary>
            private static Delegate CreateDependencyPropertyBoundValueNonConvertingCtor(Type valueType)
            {
                var valueCtor = default(Delegate);
                var valueTypeCtor = valueType.GetConstructor(
                    new[] { typeof(IDependencyPropertyValue), typeof(Type), typeof(Type), typeof(String) });

                var expParamValue = Expression.Parameter(typeof(IDependencyPropertyValue), "value");
                var expParamExpressionType = Expression.Parameter(typeof(Type), "expressionType");
                var expParamDataSourceType = Expression.Parameter(typeof(Type), "dataSourceType");
                var expParamExpression = Expression.Parameter(typeof(String), "expression");
                var expCtor = Expression.New(valueTypeCtor,
                    expParamValue, expParamExpressionType, expParamDataSourceType, expParamExpression);

                lock (cachedBoundValueCtors)
                {
                    valueCtor = Expression.Lambda<DependencyBoundValueNonConvertingCtor<T>>(expCtor,
                        expParamValue, expParamExpressionType, expParamDataSourceType, expParamExpression).Compile();

                    cachedBoundValueCtors[valueType] = valueCtor;
                }

                return valueCtor;
            }

            /// <summary>
            /// Gets a function which retrieves the wrapped resource represented by an instance of the <see cref="IResourceWrapper"/> interface.
            /// </summary>
            private static Func<T, Object> GetResourceRetriever()
            {
                if (!cachedIResourceWrapperGetters.TryGetValue(typeof(T).TypeHandle.Value.ToInt64(), out var cachedIResourceWrapperGetter))
                {
                    var miGetResourceFromResourceWrapper = typeof(IResourceWrapperUtil).GetMethod(
                        nameof(IResourceWrapperUtil.GetResourceFromResourceWrapper), BindingFlags.Public | BindingFlags.Static);

                    var getterMethod = miGetResourceFromResourceWrapper.MakeGenericMethod(typeof(T));
                    var getterParameter = Expression.Parameter(typeof(T), "wrapper");
                    var getterLambda = Expression.Lambda<Func<T, Object>>(Expression.Call(null, getterMethod, getterParameter), getterParameter).Compile();

                    cachedIResourceWrapperGetter = getterLambda;
                    cachedIResourceWrapperGetters[typeof(T).TypeHandle.Value.ToInt64()] = getterLambda;
                }

                return (Func<T, Object>)cachedIResourceWrapperGetter;
            }

            /// <summary>
            /// Gets the value's animation state.
            /// </summary>
            private DependencyPropertyValueAnimationState<T> GetAnimationState(Boolean instantiate = true)
            {
                if (animationState == null)
                {
                    if (!instantiate)
                        return null;

                    animationState = new DependencyPropertyValueAnimationState<T>();
                }
                return animationState;
            }

            /// <summary>
            /// Compares two values using the dependency property's comparer function.
            /// </summary>
            private Boolean Compare(T value1, T value2)
            {
                return typeof(T).IsValueType ?
                    ((DataBindingComparer<T>)comparer)(value1, value2) :
                    ((DataBindingComparer<Object>)comparer)(value1, value2);
            }

            /// <summary>
            /// Gets a value indicating whether the property's coerced value is different from its cached bound value.
            /// </summary>
            private Boolean IsCoercedValueDifferentFromCachedBoundValue()
            {
                return IsCoerced && !Compare(coercedValue, cachedBoundValue.Get());
            }

            /// <summary>
            /// Gets a value indicating whether change handler events are currently being deferred.
            /// </summary>
            /// <returns><see langword="true"/> if change handler events are being deferred; otherwise, <see langword="false"/>.</returns>
            private Boolean IsDeferringChangeEvents()
            {
                return owner.IsDeferringChangeEvents;
            }

            /// <summary>
            /// Handles the value of the dependency property being changed.
            /// </summary>
            /// <param name="oldValue">The dependency property's old value.</param>
            /// <param name="newValue">The dependency property's new value.</param>
            private void HandleChanged(T oldValue, T newValue)
            {
                if (IsInherited)
                    Owner?.InvalidateInheritanceCache(property);

                if (IsDeferringChangeEvents())
                {
                    IsPendingChangeEvent = true;
                }
                else
                {
                    metadata.HandleChanged(owner, property, oldValue, newValue);
                    lastChangedDigestCycleID = PresentationFoundation.Instance?.DigestCycleID ?? 0;
                }
            }

            /// <summary>
            /// Sets the property's bound value.
            /// </summary>
            private void SetCachedBoundValue(T value)
            {
                if (!cachedBoundValue.IsWritable)
                    return;

                if (cachedBoundValue.SuppressDigestForDataBinding && GetValueSource() == ValueSource.BoundValue)
                {
                    var oldValue = GetValue();

                    value = UpdateCoercedValue(value);
                    cachedBoundValue.Set(value);

                    var newValue = GetValue();

                    if (!Compare(oldValue, newValue))
                    {
                        HandleChanged(oldValue, newValue);
                    }
                }
                else
                {
                    cachedBoundValue.Set(value);
                }
            }

            /// <summary>
            /// Checks to determine whether the property's underlying value has changed,
            /// and if so, handles it appropriately.
            /// </summary>
            private void CheckForChanges(UltravioletTime time = null)
            {
                var original = GetValue();

                if (IsAnimated && time != null)
                {
                    UpdateAnimation(time);
                }

                var value = default(T);
                var potentiallyChanged = false;

                if (cachedBoundValue != null)
                {
                    if (cachedBoundValue.CheckHasChanged() || IsCoercedValueDifferentFromCachedBoundValue())
                    {
                        value = cachedBoundValue.Get();
                        potentiallyChanged = true;
                    }
                }
                else
                {
                    value = GetValue();
                    potentiallyChanged = !Compare(value, previousValue);
                }

                if (potentiallyChanged)
                {
                    var oldValue = original;
                    var newValue = value;

                    if (IsCoerced)
                    {
                        coercedValue = metadata.CoerceValue(owner, value);
                        potentiallyChanged = !Compare(coercedValue, original);
                        newValue = coercedValue;
                    }

                    var definitelyChanged = potentiallyChanged;
                    if (definitelyChanged)
                    {
                        if (IsResourceWrapper)
                            wrappedResource = GetResourceRetriever()(newValue);

                        HandleChanged(oldValue, newValue);
                    }
                }
                else
                {
                    if (IsResourceWrapper)
                    {
                        var oldResource = wrappedResource;
                        var newResource = GetResourceRetriever()(original);

                        if (!ReferenceEquals(oldResource, newResource))
                        {
                            wrappedResource = newResource;
                            HandleChanged(value, value);
                        }
                    }
                }

                previousValue = value;
            }

            /// <summary>
            /// Creates the dependency property's cached bound value object.
            /// </summary>
            /// <param name="dataSourceType">The type of the data source to which the dependency property is bound.</param>
            /// <param name="expression">The dependency property's binding expression.</param>
            private void CreateCachedBoundValue(Type dataSourceType, String expression)
            {
                var expressionType = BindingExpressions.GetExpressionType(dataSourceType, expression);
                if (expressionType != null && TypesRequireSpecialConversion(expressionType, typeof(T)))
                {
                    var coerce = typeof(T) == typeof(Object) && metadata.CoerceObjectToString;
                    var valueType = typeof(DependencyBoundValueConverting<,>).MakeGenericType(typeof(T), expressionType);
                    var valueCtor = default(Delegate);

                    lock (cachedBoundValueCtors)
                        cachedBoundValueCtors.TryGetValue(valueType, out valueCtor);

                    if (valueCtor == null)
                        valueCtor = CreateDependencyPropertyBoundValueConvertingCtor(valueType);

                    var valueInstance = ((DependencyBoundValueConvertingCtor<T>)valueCtor)(this, expressionType, dataSourceType, expression, coerce);
                    cachedBoundValue = valueInstance;
                }
                else
                {
                    var valueType = typeof(DependencyBoundValueNonConverting<>).MakeGenericType(typeof(T));
                    var valueCtor = default(Delegate);

                    lock (cachedBoundValueCtors)
                        cachedBoundValueCtors.TryGetValue(valueType, out valueCtor);

                    if (valueCtor == null)
                        valueCtor = CreateDependencyPropertyBoundValueNonConvertingCtor(valueType);
                    
                    var valueInstance = ((DependencyBoundValueNonConvertingCtor<T>)valueCtor)(this, typeof(T), dataSourceType, expression);
                    cachedBoundValue = valueInstance;
                }
            }

            /// <summary>
            /// Updates the value which tracks whether this value needs to participate 
            /// in the digest cycle.
            /// </summary>
            /// <param name="oldValue">The property's value before the change which prompted this update.</param>
            private void UpdateRequiresDigest(T oldValue)
            {
                var requiresDigestNew = IsResourceWrapper || IsDataBound || IsAnimated || 
                    (metadata.IsInherited && !HasLocalValue && !HasStyledValue);

                if (GetValueSource() == ValueSource.BoundValue)
                {
                    if (cachedBoundValue != null && cachedBoundValue.SuppressDigestForDataBinding)
                        requiresDigestNew = false;
                }

                if (requiresDigestNew != RequiresDigest)
                {
                    Owner.UpdateDigestParticipation(this, requiresDigestNew);
                    RequiresDigest = requiresDigestNew;
                }

                var newValue = GetValue();
                var changed  = !Compare(oldValue, newValue);
                
                if (changed)
                {
                    HandleChanged(oldValue, newValue);
                }
            }
            
            /// <summary>
            /// Updates the value's animation state.
            /// </summary>
            /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
            private void UpdateAnimation(UltravioletTime time)
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                if (animState.Animation != null)
                {
                    UpdateStoryboardAnimation(time);
                }
                else
                {
                    UpdateSimpleAnimation(time);
                }
            }

            /// <summary>
            /// Updates the value's animation state when using a storyboard animation.
            /// </summary>
            /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
            private void UpdateStoryboardAnimation(UltravioletTime time)
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                var animation = animState.Animation;
                var animationClock = animState.Clock;

                // If our animation has become invalid since it was applied, remove it.
                if (animation.Target == null || animation.Target.Storyboard == null || animation.Keyframes.Count == 0)
                {
                    ClearAnimation();
                    return;
                }

                // Find our current keyframe pair.
                AnimationKeyframe<T> kf1, kf2;
                animation.GetKeyframes(animationClock.ElapsedTime, out kf1, out kf2);

                // Determine which values correspond to our keyframes.
                T value1 = (kf1 == null) ? animState.HandOffValue : (!kf1.HasValue) ? GetValueInternal(false) : kf1.Value;
                T value2 = default(T);
                if (kf2 == null)
                {
                    switch (animation.FillBehavior)
                    {
                        case FillBehavior.HoldEnd:
                            value2 = kf1.HasValue ? kf1.Value : GetValueInternal(false);
                            break;

                        case FillBehavior.Stop:
                            value2 = GetValueInternal(false);
                            break;
                    }
                }
                else
                {
                    value2 = kf2.HasValue ? kf2.Value : GetValueInternal(false);
                }

                // Interpolate between our keyframes.
                var time1 = (kf1 == null) ? 0.0 : kf1.Time.TotalMilliseconds;
                var time2 = (kf2 == null) ? animation.Target.Storyboard.Duration.TotalMilliseconds : kf2.Time.TotalMilliseconds;
                var easing = (kf2 == null) ? null : kf2.EasingFunction;
                var duration = time2 - time1;
                if (duration == 0)
                {
                    animState.CurrentValue = value2;
                }
                else
                {
                    var factor = (float)((animationClock.ElapsedTime.TotalMilliseconds - time1) / duration);
                    animState.CurrentValue = animation.InterpolateValues(value1, value2, easing, factor);
                }

                if (IsCoerced)
                {
                    animState.CurrentValue = UpdateCoercedValue(animState.CurrentValue);
                }
            }

            /// <summary>
            /// Updates the value's animation state when using a simple animation.
            /// </summary>
            /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
            private void UpdateSimpleAnimation(UltravioletTime time)
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                var factor = (Single)(animState.Clock.ElapsedTime.TotalMilliseconds / animState.Clock.Duration.TotalMilliseconds);
                var valueStart = animState.HandOffValue;
                var valueEnd = animState.TargetValue;
                var valueInterpolated = Tweening.Tween(valueStart, valueEnd, animState.EasingFunction, factor);

                if (IsCoerced)
                {
                    valueInterpolated = metadata.CoerceValue<T>(owner, valueInterpolated);
                }

                animState.CurrentValue = valueInterpolated;
            }

            /// <summary>
            /// If the value's animation clock is a simple clock retrieved from the pool,
            /// this method returns the clock to the pool.
            /// </summary>
            private void ReleasePooledAnimationClock()
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return;

                var simpleClock = animState.Clock as SimpleClock;
                if (simpleClock == null)
                    return;

                if (simpleClock.PooledObject != null)
                {
                    SimpleClockPool.Instance.Release(simpleClock.PooledObject);
                    animState.Clock = null;
                }
            }

            /// <summary>
            /// Gets the value to which the property should change when an animation is first applied.
            /// </summary>
            /// <param name="oldValue">The property's old value prior to animation.</param>
            /// <returns>The value to which the property should change.</returns>
            private T GetInitialAnimatedValue(T oldValue)
            {
                var animState = GetAnimationState(false);
                if (animState == null)
                    return default(T);

                if (animState.Animation != null && animState.Animation.Keyframes.Count > 0)
                {
                    var keyframe = animState.Animation.Keyframes[0];
                    if (keyframe.HasValue && keyframe.Time == TimeSpan.Zero)
                    {
                        return keyframe.Value;
                    }
                }
                return oldValue;
            }

            /// <summary>
            /// Gets the dependency property's calculated value.
            /// </summary>
            /// <param name="includeAnimation">A value indicating whether to consider values from animation.</param>
            /// <param name="includeCoerced">A value indicating whether to consider coerced values.</param>
            /// <returns>The dependency property's calculated value.</returns>
            private T GetValueInternal(Boolean includeAnimation, Boolean includeCoerced = true)
            {
                if (useDefaultValue)
                    return defaultValue;

                if (IsCoerced && includeCoerced)
                    return coercedValue;

                if (includeAnimation && IsAnimated)
                    return GetAnimationState().CurrentValue;

                if (IsDataBound)
                    return cachedBoundValue.Get();

                if (HasLocalValue)
                    return localValue;

                if (HasTriggeredValue)
                    return triggeredValue;

                if (HasStyledValue)
                    return styledValue;

                if (metadata.IsInherited && Owner.DependencyContainer != null)
                {
                    if (FindInheritedValue(out var inheritedValue))
                    {
                        useCachedInheritedValue = true;
                        inheritedValueCache = inheritedValue;
                        return inheritedValue;
                    }
                }

                return defaultValue;
            }

            /// <summary>
            /// Gets the value of an inherited property.
            /// </summary>
            private Boolean FindInheritedValue(out T value)
            {
                if (useCachedInheritedValue)
                {
                    value = inheritedValueCache;
                    return true;
                }

                var current = LogicalTreeHelper.GetParent(Owner);
                while (current != null)
                {
                    value = current.GetValue<T>(property);
                    return true;
                }

                value = default(T);
                return false;
            }

            /// <summary>
            /// Updates the property's coerced value, if this property uses coercion.
            /// </summary>
            /// <param name="value">The value that should be coerced.</param>
            /// <returns>The coerced value.</returns>
            private T UpdateCoercedValue(T value)
            {
                if (IsCoerced)
                {
                    coercedValue = metadata.CoerceValue<T>(owner, value);
                    return coercedValue;
                }
                return value;
            }

            /// <summary>
            /// Gets a <see cref="ValueSource"/> value which indicates from where the data property is currently
            /// retrieving its value.
            /// </summary>
            /// <returns>The <see cref="ValueSource"/> value for this dependency property.</returns>
            private ValueSource GetValueSource()
            {
                if (useDefaultValue)
                    return ValueSource.DefaultValue;

                if (IsCoerced)
                    return ValueSource.CoercedValue;

                if (IsAnimated)
                    return ValueSource.AnimatedValue;

                if (IsDataBound)
                    return ValueSource.BoundValue;

                if (HasLocalValue)
                    return ValueSource.LocalValue;

                if (HasTriggeredValue)
                    return ValueSource.TriggeredValue;

                if (HasStyledValue)
                    return ValueSource.StyledValue;

                if (IsInherited)
                    return ValueSource.InheritedValue;

                return ValueSource.DefaultValue;
            }
                        
            // Property values.
            private readonly DependencyObject owner;
            private readonly DependencyProperty property;
            private readonly PropertyMetadata metadata;
            private DependencyPropertyValueFlags flags;
            private Boolean useDefaultValue = true;
            private Boolean useCachedInheritedValue;
            private Int64 lastChangedDigestCycleID;
            private T localValue;
            private T styledValue;
            private T defaultValue;
            private T previousValue;
            private T coercedValue;
            private T triggeredValue;
            private T inheritedValueCache;
            private Object wrappedResource;
            private SetTriggerAction triggeredValueSource;

            // State values.
            private readonly Delegate comparer;
            private IDependencyBoundValue<T> cachedBoundValue;
            private DependencyPropertyValueAnimationState<T> animationState;

            // Cached constructor delegates for bound values.
            private static readonly Dictionary<Type, Delegate> cachedBoundValueCtors =
                new Dictionary<Type, Delegate>();

            // Cached getters for IResourceWrapper types.
            private static readonly Dictionary<Int64, Object> cachedIResourceWrapperGetters =
                new Dictionary<Int64, Object>();
        }
    }
}
