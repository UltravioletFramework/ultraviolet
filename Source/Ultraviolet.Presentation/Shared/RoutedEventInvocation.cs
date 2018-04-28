using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for generating invocation delegates for routed events.
    /// </summary>
    internal static class RoutedEventInvocation
    {
        /// <summary>
        /// Initializes the <see cref="RoutedEventInvocation"/> type.
        /// </summary>
        static RoutedEventInvocation()
        {
            miShouldEventBeRaisedForElement = typeof(RoutedEventInvocation).GetMethod(
                nameof(ShouldEventBeRaisedForElement), BindingFlags.NonPublic | BindingFlags.Static);
            miShouldContinueBubbling = typeof(RoutedEventInvocation).GetMethod(
                nameof(ShouldContinueBubbling), BindingFlags.NonPublic | BindingFlags.Static);
            miShouldContinueTunnelling = typeof(RoutedEventInvocation).GetMethod(
                nameof(ShouldContinueTunnelling), BindingFlags.NonPublic | BindingFlags.Static);
            miReleaseTunnellingStack = typeof(RoutedEventInvocation).GetMethod(
                nameof(ReleaseTunnellingStack), BindingFlags.NonPublic | BindingFlags.Static);
            miGetEventHandler = typeof(RoutedEventInvocation).GetMethod(
                nameof(GetEventHandler), BindingFlags.NonPublic | BindingFlags.Static);
            miRaiseRaisedNotification = typeof(RoutedEvent).GetMethod(
                nameof(RoutedEvent.RaiseRaisedNotification), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Creates an invocation delegate for the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to create an invocation delegate.</param>
        /// <returns>The invocation delegate for the specified routed event.</returns>
        public static Delegate CreateInvocationDelegate(RoutedEvent evt)
        {
            Contract.Require(evt, nameof(evt));

            if (!IsValidRoutedEventDelegate(evt.DelegateType))
                throw new InvalidOperationException(PresentationStrings.InvalidRoutedEventDelegate.Format(evt.DelegateType.Name));

            switch (evt.RoutingStrategy)
            {
                case RoutingStrategy.Bubble:
                    return CreateInvocationDelegateForBubbleStrategy(evt);
                
                case RoutingStrategy.Direct:
                    return CreateInvocationDelegateForDirectStrategy(evt);

                case RoutingStrategy.Tunnel:
                    return CreateInvocationDelegateForTunnelStrategy(evt);

                default:
                    throw new InvalidOperationException(PresentationStrings.InvalidRoutingStrategy);
            }
        }

        /// <summary>
        /// Creates an invocation delegate for a routed event which uses the <see cref="RoutingStrategy.Bubble"/> routing strategy.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to create an invocation delegate.</param>
        /// <returns>The invocation delegate for the specified routed event.</returns>
        private static Delegate CreateInvocationDelegateForBubbleStrategy(RoutedEvent evt)
        {
            /* BUBBLE STRATEGY
             * For a given event delegate type TDelegate, we're constructing a method which basically looks like this:
             * 
             * void fn(DependencyObject element, p1, p2, ..., pN, RoutedEventData data)
             * {
             *      var index = 0;
             *      var handler = default(RoutedEventHandlerMetadata);
             *      var current = element;
             *      
             *      while (ShouldContinueBubbling(element, ref current))
             *      {
             *          index = 0;
             *          while (GetEventHandler(current, RoutedEventID, ref index, ref handler))
             *          {
             *              if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
             *              {
             *                  handler.Handler(current, p1, p2, ..., pN, data);
             *              }
             *          }
             *          
             *          RoutedEventID.RaiseRaisedNotification(current, data);     
             *      }
             *      
             *      if (data.AutoRelease)
             *          data.Release();
             * }
             */

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                var evtInvoke = evt.DelegateType.GetMethod("Invoke");
                var evtParams = evtInvoke.GetParameters().ToArray();

                var expParams = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
                var expParamElement = expParams.First();
                var expParamData = expParams.Last();

                var expParts = new List<Expression>();
                var expVars = new List<ParameterExpression>();

                var varIndex = Expression.Variable(typeof(Int32), "index");
                expVars.Add(varIndex);

                var varHandler = Expression.Variable(typeof(RoutedEventHandlerMetadata), "handlers");
                expVars.Add(varHandler);

                var varCurrent = Expression.Variable(typeof(DependencyObject), "current");
                expVars.Add(varCurrent);

                var innerEventHandlerParams = new List<ParameterExpression>();
                innerEventHandlerParams.Add(varCurrent);
                innerEventHandlerParams.AddRange(expParams.Skip(1));

                var expWhileBubbleBreakOuter = Expression.Label();
                var expWhileBubbleBreakInner = Expression.Label();

                var expWhileBubble = Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Call(miShouldContinueBubbling, expParamElement, varCurrent),
                        Expression.Block(
                            Expression.Assign(varIndex, Expression.Constant(0)),
                            Expression.Loop(
                                Expression.IfThenElse(
                                    Expression.Call(miGetEventHandler, varCurrent, Expression.Constant(evt), varIndex, varHandler),
                                    Expression.IfThen(
                                        Expression.Call(miShouldEventBeRaisedForElement, expParamData, Expression.Property(varHandler, "HandledEventsToo")),
                                        Expression.Invoke(Expression.Convert(Expression.Property(varHandler, "Handler"), evt.DelegateType), innerEventHandlerParams)
                                    ),
                                    Expression.Break(expWhileBubbleBreakInner)
                                ),
                                expWhileBubbleBreakInner
                            ),
                            Expression.Call(Expression.Constant(evt), miRaiseRaisedNotification, varCurrent, expParamData)
                        ),
                        Expression.Break(expWhileBubbleBreakOuter)
                    ),
                    expWhileBubbleBreakOuter
                );
                expParts.Add(expWhileBubble);

                expParts.Add(Expression.IfThen(Expression.IsTrue(Expression.Property(expParamData, nameof(RoutedEventData.AutoRelease))),
                    Expression.Call(expParamData, nameof(RoutedEventData.Release), null)));

                return Expression.Lambda(evt.DelegateType, Expression.Block(expVars, expParts), expParams).Compile();
            }
            else
            {
                return CreateDelegateForReflectionBasedImplementation(evt, nameof(ReflectionBasedImplementationForBubbleStrategy));
            }
        }

        /// <summary>
        /// Creates an invocation delegate for a routed event which uses the <see cref="RoutingStrategy.Direct"/> routing strategy.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to create an invocation delegate.</param>
        /// <returns>The invocation delegate for the specified routed event.</returns>
        private static Delegate CreateInvocationDelegateForDirectStrategy(RoutedEvent evt)
        {
            /* DIRECT STRATEGY
             * The simplest strategy; the event is only invoked on the element that raised it. 
             * Our invocation delegate looks something like this:
             * 
             * void fn(DependencyObject element, p1, p2, ..., pN, RoutedEventData data)
             * {
             *      var index = 0;
             *      var handler = default(RoutedEventHandlerMetadata);      
             * 
             *      while (GetEventHandler(element, RoutedEventID, ref index, ref handler))
             *      {
             *          if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
             *          {
             *              handler.Handler(element, p1, p2, ..., pN, data);
             *          }
             *      }
             *      
             *      RoutedEventID.RaiseRaisedNotification(element, data);
             *      
             *      if (data.AutoRelease)
             *          data.Release();
             * }
             */

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                var evtInvoke = evt.DelegateType.GetMethod("Invoke");
                var evtParams = evtInvoke.GetParameters().ToArray();

                var expParams = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
                var expParamElement = expParams.First();
                var expParamData = expParams.Last();

                var expParts = new List<Expression>();
                var expVars = new List<ParameterExpression>();

                var varIndex = Expression.Variable(typeof(Int32), "index");
                expVars.Add(varIndex);

                var varHandler = Expression.Variable(typeof(RoutedEventHandlerMetadata), "handlers");
                expVars.Add(varHandler);

                var expInvokeBreak = Expression.Label();
                var expInvoke = Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Call(miGetEventHandler, expParamElement, Expression.Constant(evt), varIndex, varHandler),
                        Expression.IfThen(
                            Expression.Call(miShouldEventBeRaisedForElement, expParamData, Expression.Property(varHandler, "HandledEventsToo")),
                            Expression.Invoke(Expression.Convert(Expression.Property(varHandler, "Handler"), evt.DelegateType), expParams)
                        ),
                        Expression.Break(expInvokeBreak)
                    ),
                    expInvokeBreak
                );
                expParts.Add(expInvoke);

                var expRaiseRaised = Expression.Call(Expression.Constant(evt), miRaiseRaisedNotification, expParamElement, expParamData);
                expParts.Add(expRaiseRaised);

                expParts.Add(Expression.IfThen(Expression.IsTrue(Expression.Property(expParamData, nameof(RoutedEventData.AutoRelease))),
                    Expression.Call(expParamData, nameof(RoutedEventData.Release), null)));

                return Expression.Lambda(evt.DelegateType, Expression.Block(expVars, expParts), expParams).Compile();
            }
            else
            {
                return CreateDelegateForReflectionBasedImplementation(evt, nameof(ReflectionBasedImplementationForDirectStrategy));
            }
        }

        /// <summary>
        /// Creates an invocation delegate for a routed event which uses the <see cref="RoutingStrategy.Tunnel"/> routing strategy.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to create an invocation delegate.</param>
        /// <returns>The invocation delegate for the specified routed event.</returns>
        private static Delegate CreateInvocationDelegateForTunnelStrategy(RoutedEvent evt)
        {
            /* TUNNEL STRATEGY
             * Basically the opposite of the bubble strategy; we start at the root of the tree and work down.
             * Note that ShouldContinueTunnelling() builds a stack representing the path to take on the first call.
             * 
             * void fn(DependencyObject element, p1, p2, ..., pN, RoutedEventData data)
             * {
             *      var index    = 0;
             *      var current  = default(DependencyObject);
             *      var handlers = default(List<RoutedEventHandlerMetadata>);
             *      var stack    = default(Stack<DependencyObject>);
             *      
             *      while (ShouldContinueTunnelling(element, ref current, ref stack))
             *      {
             *          index = 0;
             *          while (GetEventHandler(current, RoutedEventID, index, out handler))
             *          {
             *              if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
             *              {
             *                  handler.Handler(current, p1, p2, ..., pN, data);
             *              }
             *          }
             *          
             *          RoutedEventID.RaiseRaisedNotification(current, data);     
             *      }
             *      
             *      ReleaseTunnellingStack(stack);
             *      
             *      if (data.AutoRelease)
             *          data.Release();
             * }
             */

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                var evtInvoke = evt.DelegateType.GetMethod("Invoke");
                var evtParams = evtInvoke.GetParameters().ToArray();

                var expParams = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
                var expParamElement = expParams.First();
                var expParamData = expParams.Last();

                var expParts = new List<Expression>();
                var expVars = new List<ParameterExpression>();

                var varIndex = Expression.Variable(typeof(Int32), "index");
                expVars.Add(varIndex);

                var varHandler = Expression.Variable(typeof(RoutedEventHandlerMetadata), "handler");
                expVars.Add(varHandler);

                var varCurrent = Expression.Variable(typeof(DependencyObject), "current");
                expVars.Add(varCurrent);

                var varStack = Expression.Variable(typeof(Stack<DependencyObject>), "stack");
                expVars.Add(varStack);

                var innerEventHandlerParams = new List<ParameterExpression>();
                innerEventHandlerParams.Add(varCurrent);
                innerEventHandlerParams.AddRange(expParams.Skip(1));

                var expWhileTunnelBreakOuter = Expression.Label();
                var expWhileTunnelBreakInner = Expression.Label();

                var expWhileTunnel = Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Call(miShouldContinueTunnelling, expParamElement, varCurrent, varStack),
                        Expression.Block(
                            Expression.Assign(varIndex, Expression.Constant(0)),
                            Expression.Loop(
                                Expression.IfThenElse(
                                    Expression.Call(miGetEventHandler, varCurrent, Expression.Constant(evt), varIndex, varHandler),
                                    Expression.IfThen(
                                        Expression.Call(miShouldEventBeRaisedForElement, expParamData, Expression.Property(varHandler, "HandledEventsToo")),
                                        Expression.Invoke(Expression.Convert(Expression.Property(varHandler, "Handler"), evt.DelegateType), innerEventHandlerParams)
                                    ),
                                    Expression.Break(expWhileTunnelBreakInner)
                                ),
                                expWhileTunnelBreakInner
                            ),
                            Expression.Call(Expression.Constant(evt), miRaiseRaisedNotification, varCurrent, expParamData)
                        ),
                        Expression.Break(expWhileTunnelBreakOuter)
                    ),
                    expWhileTunnelBreakOuter
                );
                expParts.Add(expWhileTunnel);

                expParts.Add(Expression.Call(miReleaseTunnellingStack, varStack));

                expParts.Add(Expression.IfThen(Expression.IsTrue(Expression.Property(expParamData, nameof(RoutedEventData.AutoRelease))),
                    Expression.Call(expParamData, nameof(RoutedEventData.Release), null)));

                return Expression.Lambda(evt.DelegateType, Expression.Block(expVars, expParts), expParams).Compile();
            }
            else
            {
                return CreateDelegateForReflectionBasedImplementation(evt, nameof(ReflectionBasedImplementationForTunnelStrategy));
            }
        }

        /// <summary>
        /// Creates a delegate which wraps a reflection-based routing implementation.
        /// </summary>
        private static Delegate CreateDelegateForReflectionBasedImplementation(RoutedEvent evt, String method)
        {
            var evtInvoke = evt.DelegateType.GetMethod("Invoke");
            var evtParams = evtInvoke.GetParameters().ToArray();

            var implMethod = typeof(RoutedEventInvocation).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static);

            var expParams = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
            var expImplParameters = expParams.Select(x => Expression.Convert(x, typeof(Object)));
            var expImplMethodCall = Expression.Call(implMethod, Expression.Constant(evt),
                Expression.NewArrayInit(typeof(Object), expImplParameters));

            return Expression.Lambda(evt.DelegateType, expImplMethodCall, expParams).Compile();
        }

        /// <summary>
        /// An implementation of the bubble routing strategy which uses reflection rather than
        /// dynamic runtime code generation.
        /// </summary>
        private static void ReflectionBasedImplementationForBubbleStrategy(RoutedEvent evt, Object[] parameters)
        {
            var index = 0;
            var handler = default(RoutedEventHandlerMetadata);
            var element = (DependencyObject)parameters[0];
            var current = default(DependencyObject);
            var data = (RoutedEventData)parameters[parameters.Length - 1];

            while (ShouldContinueBubbling(element, ref current))
            {
                index = 0;
                while (GetEventHandler(current, evt, ref index, ref handler))
                {
                    if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
                    {
                        parameters[0] = current;
                        handler.Handler.DynamicInvoke(parameters);
                    }
                }

                evt.RaiseRaisedNotification(current, data);
            }

            if (data.AutoRelease)
                data.Release();
        }

        /// <summary>
        /// An implementation of the direct routing strategy which uses reflection rather than
        /// dynamic runtime code generation.
        /// </summary>
        private static void ReflectionBasedImplementationForDirectStrategy(RoutedEvent evt, Object[] parameters)
        {
            var index = 0;
            var handler = default(RoutedEventHandlerMetadata);

            var element = (DependencyObject)parameters[0];
            var data = (RoutedEventData)parameters[parameters.Length - 1];

            while (GetEventHandler(element, evt, ref index, ref handler))
            {
                if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
                {
                    handler.Handler.DynamicInvoke(parameters);
                }
            }

            evt.RaiseRaisedNotification(element, data);

            if (data.AutoRelease)
                data.Release();
        }

        /// <summary>
        /// An implementation of the tunnel routing strategy which uses reflection rather than
        /// dynamic runtime code generation.
        /// </summary>
        private static void ReflectionBasedImplementationForTunnelStrategy(RoutedEvent evt, Object[] parameters)
        {
            var index = 0;
            var handler = default(RoutedEventHandlerMetadata);
            var element = (DependencyObject)parameters[0];
            var current = default(DependencyObject);
            var data = (RoutedEventData)parameters[parameters.Length - 1];
            var stack = default(Stack<DependencyObject>);

            while (ShouldContinueTunnelling(element, ref current, ref stack))
            {
                index = 0;
                while (GetEventHandler(current, evt, ref index, ref handler))
                {
                    if (ShouldEventBeRaisedForElement(data, handler.HandledEventsToo))
                    {
                        parameters[0] = current;
                        handler.Handler.DynamicInvoke(parameters);
                    }
                }

                evt.RaiseRaisedNotification(current, data);
            }
            
            ReleaseTunnellingStack(stack);

            if (data.AutoRelease)
                data.Release();
        }

        /// <summary>
        /// Gets a value indicating whether the specified type represents a valid routed event delegate.
        /// </summary>
        /// <param name="delegateType">The delegate type to evaluate.</param>
        /// <returns><see langword="true"/> if the specified delegate type represents a valid routed event delegate; otherwise, <see langword="false"/>.</returns>
        private static Boolean IsValidRoutedEventDelegate(Type delegateType)
        {
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
                return false;

            var invoke     = delegateType.GetMethod("Invoke");
            var parameters = invoke.GetParameters().ToList();

            if (parameters.Count < 2)
                return false;

            var paramFirst = parameters.First();
            var paramLast  = parameters.Last();

            return
                typeof(DependencyObject).IsAssignableFrom(paramFirst.ParameterType) &&
                typeof(RoutedEventData).IsAssignableFrom(paramLast.ParameterType) &&
                invoke.ReturnType == typeof(void);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element should receive the event being processed.
        /// </summary>
        /// <param name="data">The routed event data for the current event.</param>
        /// <param name="handledEventsToo">A value indicating whether the current event handler wants to receive handled events.</param>
        /// <returns><see langword="true"/> if the event should be raised for this object; otherwise, <see langword="false"/>.</returns>
        private static Boolean ShouldEventBeRaisedForElement(RoutedEventData data, Boolean handledEventsToo)
        {
            return !data.Handled || handledEventsToo;
        }

        /// <summary>
        /// Gets a value indicating whether the event being processed should continue bubbling up the 
        /// element hierarchy, and if so, sets the <paramref name="current"/> parameter to
        /// the next object to be processed.
        /// </summary>
        /// <param name="first">The first element to process.</param>
        /// <param name="current">The element that is currently being processed.</param>
        /// <returns><see langword="true"/> if the event should continue bubbling; otherwise, <see langword="false"/>.</returns>
        private static Boolean ShouldContinueBubbling(DependencyObject first, ref DependencyObject current)
        {
            if (current == null)
            {
                current = first;
                return true;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current) ?? current.DependencyContainer;
                if (parent == null)
                {
                    current = null;
                    return false;
                }
                current = parent;
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the event being processed should continue tunnelling down the 
        /// element hierarchy, and if so, sets the <paramref name="current"/> parameter to
        /// the next object to be processed.
        /// </summary>
        /// <param name="first">The first element to process.</param>
        /// <param name="current">The element that is currently being processed.</param>
        /// <param name="stack">The tunnelling stack for this invocation.</param>
        /// <returns><see langword="true"/> if the event should continue tunnelling; otherwise, <see langword="false"/>.</returns>
        private static Boolean ShouldContinueTunnelling(DependencyObject first, ref DependencyObject current, ref Stack<DependencyObject> stack)
        {
            if (current == null)
                PrepareTunnellingStack(first, ref stack);

            if (stack.Count > 0)
            {
                var next = stack.Pop();
                current = next;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prepares the tunnelling stack to process a tunnelled event.
        /// </summary>
        /// <param name="element">The element which raised the event.</param>
        /// <param name="stack">The stack which was prepared.</param>
        private static void PrepareTunnellingStack(DependencyObject element, ref Stack<DependencyObject> stack)
        {
            if (stack == null)
                stack = tunnellingStackPool.Retrieve();
            
            for (var current = element; current != null; current = current.DependencyContainer)
            {
                stack.Push(current);
            }
        }

        /// <summary>
        /// Releases the specified tunnelling stack back into the internal pool.
        /// </summary>
        /// <param name="stack">The stack to release.</param>
        private static void ReleaseTunnellingStack(Stack<DependencyObject> stack)
        {
            if (stack == null)
                return;

            tunnellingStackPool.Release(stack);
        }

        /// <summary>
        /// Gets the next event handler to invoke for the current element.
        /// </summary>
        /// <param name="element">The element for which event handlers are being invoked.</param>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which handlers are being invoked.</param>
        /// <param name="index">The index of the handler to invoke; this value is incremented by one when this method returns.</param>
        /// <param name="handler">The metadata for the handler that corresponds to the specified index within the handler list.</param>
        /// <returns><see langword="true"/> if a handler was retrieved for the specified index; otherwise, <see langword="false"/>.</returns>
        private static Boolean GetEventHandler(DependencyObject element, RoutedEvent evt, ref Int32 index, ref RoutedEventHandlerMetadata handler)
        {
            var indexTemp = index;

            var classHandlers = RoutedEventClassHandlers.GetClassHandlers(element.GetType(), evt);
            if (classHandlers != null)
            {
                lock (classHandlers)
                {
                    if (indexTemp >= 0 && indexTemp < classHandlers.Count)
                    {
                        handler = classHandlers[indexTemp];
                        index++;
                        return true;
                    }
                    indexTemp -= classHandlers.Count;
                }
            }

            var uiElement = element as UIElement;
            if (uiElement != null)
            {
                var instanceHandlers = uiElement.GetHandlers(evt);
                if (instanceHandlers == null)
                    return false;

                lock (instanceHandlers)
                {
                    if (indexTemp >= instanceHandlers.Count)
                    {
                        return false;
                    }
                    handler = instanceHandlers[indexTemp];
                    index++;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // Cached method info for methods used by invocation delegates.
        private static readonly MethodInfo miShouldEventBeRaisedForElement;
        private static readonly MethodInfo miShouldContinueBubbling;
        private static readonly MethodInfo miShouldContinueTunnelling;
        private static readonly MethodInfo miReleaseTunnellingStack;
        private static readonly MethodInfo miGetEventHandler;
        private static readonly MethodInfo miRaiseRaisedNotification;

        // The stack used to track the tunnelling path for tunnelled events.
        private static readonly IPool<Stack<DependencyObject>> tunnellingStackPool =
            new ExpandingPool<Stack<DependencyObject>>(1, () => new Stack<DependencyObject>(), item => item.Clear());
    }
}
