using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            miShouldEventBeRaisedForElement   = typeof(RoutedEventInvocation).GetMethod("ShouldEventBeRaisedForElement", BindingFlags.NonPublic | BindingFlags.Static);
            miShouldContinueBubbling          = typeof(RoutedEventInvocation).GetMethod("ShouldContinueBubbling", BindingFlags.NonPublic | BindingFlags.Static);
            miGetRoutedEventHandlerForElement = typeof(RoutedEventInvocation).GetMethod("GetRoutedEventHandlerForElement", BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        /// Creates an invocation delegate for the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to create an invocation delegate.</param>
        /// <returns>The invocation delegate for the specified routed event.</returns>
        public static Delegate CreateInvocationDelegate(RoutedEvent evt)
        {
            Contract.Require(evt, "evt");

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
             * void fn(UIElement element, p1, p2, ..., pN, ref Boolean handled)
             * {
             *      handled = false;
             *      
             *      var current       = element;
             *      var eventDelegate = default(TDelegate);
             *      
             *      while (ShouldContinueBubbling(element, ref current))
             *      {
             *          if (ShouldEventBeRaisedForElement(current, handled))
             *          {
             *              eventDelegate = GetRoutedEventHandlerForElement(current, RoutedEventID);
             *              if (eventDelegate != null)
             *              {
             *                  eventDelegate(current, p1, p2, ..., pN, ref handled); 
             *              }
             *          }
             *      }
             * }
             */

            var evtInvoke = evt.DelegateType.GetMethod("Invoke");
            var evtParams = evtInvoke.GetParameters().ToArray();

            var expParams       = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
            var expParamElement = expParams.First();
            var expParamHandled = expParams.Last();

            var expParts = new List<Expression>();
            var expVars  = new List<ParameterExpression>();

            var expAssignedHandledToFalse = Expression.Assign(expParamHandled, Expression.Constant(false));
            expParts.Add(expAssignedHandledToFalse);

            var varCurrent = Expression.Variable(typeof(UIElement), "current");
            expVars.Add(varCurrent);

            var varEventDelegate = Expression.Variable(evt.DelegateType, "eventDelegate");
            expVars.Add(varEventDelegate);

            var innerEventHandlerParams = new List<ParameterExpression>();
            innerEventHandlerParams.Add(varCurrent);
            innerEventHandlerParams.AddRange(expParams.Skip(1));

            var expWhileBreak = Expression.Label();
            var expWhileBubble = Expression.Loop(
                Expression.IfThenElse(
                    Expression.Call(miShouldContinueBubbling, expParamElement, varCurrent),
                    Expression.IfThen(
                        Expression.Call(miShouldEventBeRaisedForElement, varCurrent, expParamHandled),
                        Expression.Block(
                            Expression.Assign(varEventDelegate, 
                                Expression.Convert(Expression.Call(miGetRoutedEventHandlerForElement, varCurrent, Expression.Constant(evt)), evt.DelegateType)),
                            Expression.IfThen(
                                Expression.NotEqual(varEventDelegate, Expression.Constant(null)),
                                Expression.Invoke(varEventDelegate, innerEventHandlerParams)
                            )
                        )
                    ),
                    Expression.Break(expWhileBreak)
                ),
                expWhileBreak
            );
            expParts.Add(expWhileBubble);

            return Expression.Lambda(evt.DelegateType, Expression.Block(expVars, expParts), expParams).Compile();
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
             * void fn(UIElement element, p1, p2, ..., pN, ref Boolean handled)
             * {
             *      var handled       = false;
             *      var eventDelegate = GetRoutedEventHandlerForElement(element);
             *      if (eventDelegate != null)
             *      {
             *          eventDelegate(element, p1, p2, ..., pN, ref handled);
             *      }
             * }
             */

            var evtInvoke = evt.DelegateType.GetMethod("Invoke");
            var evtParams = evtInvoke.GetParameters().ToArray();

            var expParams       = evtParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
            var expParamElement = expParams.First();
            var expParamHandled = expParams.Last();

            var expParts = new List<Expression>();
            var expVars  = new List<ParameterExpression>();

            var expAssignedHandledToFalse = Expression.Assign(expParamHandled, Expression.Constant(false));
            expParts.Add(expAssignedHandledToFalse);

            var varEventDelegate = Expression.Variable(evt.DelegateType, "eventDelegate");
            expVars.Add(varEventDelegate);

            var expInvoke = Expression.Block(
                Expression.Assign(varEventDelegate,
                    Expression.Convert(Expression.Call(miGetRoutedEventHandlerForElement, expParamElement, Expression.Constant(evt)), evt.DelegateType)),
                Expression.IfThen(
                    Expression.NotEqual(varEventDelegate, Expression.Constant(null)),
                    Expression.Invoke(varEventDelegate, expParams)
                )
            );
            expParts.Add(expInvoke);

            return Expression.Lambda(evt.DelegateType, Expression.Block(expVars, expParts), expParams).Compile();
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
             * 
             * void fn(UIElement element, p1, p2, ..., pN, ref Boolean handled)
             * {
             *      TODO
             * }
             */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether the specified type represents a valid routed event delegate.
        /// </summary>
        /// <param name="delegateType">The delegate type to evaluate.</param>
        /// <returns><c>true</c> if the specified delegate type represents a valid routed event delegate; otherwise, <c>false</c>.</returns>
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
                typeof(UIElement).IsAssignableFrom(paramFirst.ParameterType) &&
                paramLast.ParameterType == typeof(Boolean).MakeByRefType() &&
                invoke.ReturnType == typeof(void);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element should receive the event being processed.
        /// </summary>
        /// <param name="current">The current element.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <returns><c>true</c> if the event should be raised for this object; otherwise, <c>false</c>.</returns>
        private static Boolean ShouldEventBeRaisedForElement(UIElement current, Boolean handled)
        {
            // TODO: Objects need to be able to register to receive handled events
            return !handled;
        }

        /// <summary>
        /// Gets a value indicating whether the event being processed should continue bubbling up the 
        /// element hierarchy, and if so, sets the <paramref name="current"/> parameter to
        /// the next object to be processed.
        /// </summary>
        /// <param name="first">The first element to process.</param>
        /// <param name="current">The element that is currently being processed.</param>
        /// <returns><c>true</c> if the event should continue bubbling; otherwise, <c>false</c>.</returns>
        private static Boolean ShouldContinueBubbling(UIElement first, ref UIElement current)
        {
            if (current == null)
            {
                current = first;
                return true;
            }
            else
            {
                if (current.DependencyContainer == null)
                {
                    current = null;
                    return false;
                }
                current = current.DependencyContainer as UIElement;
                return true;
            }
        }

        /// <summary>
        /// Gets the specified routed event handler for an element.
        /// </summary>
        /// <param name="current">The element for which to retrieve the routed event handler.</param>
        /// <param name="evt">A <see cref="RoutedEvent"/> which identifies the routed event for which to retrieve a handler.</param>
        /// <returns>A <see cref="Delegate"/> that represents the element's handler for the specified routed event, or <c>null</c> if no handler is registered.</returns>
        private static Delegate GetRoutedEventHandlerForElement(UIElement current, RoutedEvent evt)
        {
            return current.GetHandler(evt);
        }

        // Cached method info for methods used by invocation delegates.
        private static readonly MethodInfo miShouldEventBeRaisedForElement;
        private static readonly MethodInfo miShouldContinueBubbling;
        private static readonly MethodInfo miGetRoutedEventHandlerForElement;
    }
}
