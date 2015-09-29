using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the method that is called when a tooltip is opened or closed.
    /// </summary>
    /// <param name="dobj">The tooltip that was opened or closed.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfToolTipEventHandler(DependencyObject dobj, ref RoutedEventData data);

    /// <summary>
    /// Contains attached properties and events relating to tool tips.
    /// </summary>
    [UvmlKnownType]
    public static class ToolTipService
    {
        /// <summary>
        /// Gets the horizontal offset between the tooltip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The horizontal offset between the tooltip's placement point and its position on the screen.</returns>
        public static Double GetHorizontalOffset(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(HorizontalOffsetProperty);
        }

        /// <summary>
        /// Gets the vertical offset between the tooltip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The vertical offset between the tooltip's placement point and its position on the screen.</returns>
        public static Double GetVerticalOffset(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(VerticalOffsetProperty);
        }

        /// <summary>
        /// Gets the delay in milliseconds before the tooltip opens.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The delay in milliseconds before the tooltip opens.</returns>
        public static Double GetInitialShowDelay(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(InitialShowDelayProperty);
        }

        /// <summary>
        /// Gets the maximum time in milliseconds after the closing of a tooltip during which another tooltip
        /// can be opened without a delay.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The maximum time in milliseconds after the closing of a tooltip during which another tooltip
        /// can be opened without a delay.</returns>
        public static Double GetBetweenShowDelay(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(BetweenShowDelayProperty);
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that the tooltip remains visible.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The amount of time in milliseconds that the tooltip remains visible.</returns>
        public static Double GetShowDuration(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(ShowDurationProperty);
        }

        /// <summary>
        /// Gets a value indicating whether a tooltip is displayed for the element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if a tooltip is displayed; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsEnabled(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsEnabledProperty);
        }

        /// <summary>
        /// Gets a value indicating whether a tooltip is currently open for the element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the tooltip is open; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsOpen(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsOpenProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the tooltip has a drop shadow.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the tooltip has a drop shadow; otherwise, <c>false</c>.</returns>
        public static Boolean GetHasDropShadow(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(HasDropShadowProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the tooltip is displayed even if the element is disabled.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the tooltip is displayed for a disabled element; otherwise, <c>false</c>.</returns>
        public static Boolean GetShowOnDisabled(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(ShowOnDisabledProperty);
        }

        /// <summary>
        /// Gets the tooltip's placement mode.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>A <see cref="PlacementMode"/> value which specifies how the tooltip is arranged on the screen.</returns>
        public static PlacementMode GetPlacement(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<PlacementMode>(PlacementProperty);
        }

        /// <summary>
        /// Gets the tooltip's placement rectangle.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The rectangle relative to which the tooltip popup is placed.</returns>
        public static RectangleD GetPlacementRectangle(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<RectangleD>(PlacementRectangleProperty);
        }

        /// <summary>
        /// Gets the tooltip's placement target.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The element relative to which the tooltip popup is placed.</returns>
        public static UIElement GetPlacementTarget(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<UIElement>(PlacementTargetProperty);
        }

        /// <summary>
        /// Gets the tooltip's content.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The content of the specified element's tooltip, or <c>null</c> if the element has no tooltip.</returns>
        public static Object GetToolTip(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Object>(ToolTipProperty);
        }
        
        /// <summary>
        /// Sets the horizontal offset between the tooltip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The horizontal offset between the tooltip's placement point and its position on the screen.</param>
        public static void SetHorizontalOffset(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Sets the vertical offset between the tooltip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The vertical offset between the tooltip's placement point and its position on the screen.</param>
        public static void SetVerticalOffset(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Sets the delay in milliseconds before the tooltip opens.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The delay in milliseconds before the tooltip opens.</param>
        public static void SetInitialShowDelay(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue(InitialShowDelayProperty, value);
        }

        /// <summary>
        /// Sets the maximum time in milliseconds after the closing of a tooltip during which another tooltip
        /// can be opened without a delay.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The maximum time in milliseconds after the closing of a tooltip during which another tooltip
        /// can be opened without a delay.</param>
        public static void SetBetweenShowDelay(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue(BetweenShowDelayProperty, value);
        }

        /// <summary>
        /// Sets the amount of time in milliseconds that the tooltip remains visible.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The amount of time in milliseconds that the tooltip remains visible.</param>
        public static void SetShowDuration(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue(ShowDurationProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether a tooltip is displayed for the element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><c>true</c> if a tooltip is displayed; otherwise, <c>false</c>.</param>
        public static void SetIsEnabled(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the tooltip has a drop shadow.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><c>true</c> if the tooltip has a drop shadow; otherwise, <c>false</c>.</param>
        public static void SetHasDropShadow(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(HasDropShadowProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the tooltip is displayed even if the element is disabled.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><c>true</c> if the tooltip is displayed for a disabled element; otherwise, <c>false</c>.</param>
        public static void SetShowOnDisabled(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(ShowOnDisabledProperty, value);
        }

        /// <summary>
        /// Sets the tooltip's placement mode.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">A <see cref="PlacementMode"/> value which specifies how the tooltip is arranged on the screen.</param>
        public static void SetPlacement(DependencyObject element, PlacementMode value)
        {
            Contract.Require(element, "element");

            element.SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// Sets the tooltip's placement rectangle.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The rectangle relative to which the tooltip popup is placed.</param>
        public static void SetPlacementRectangle(DependencyObject element, RectangleD value)
        {
            Contract.Require(element, "element");

            element.SetValue(PlacementRectangleProperty, value);
        }

        /// <summary>
        /// Sets the tooltip's placement target.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The element relative to which the tooltip popup is placed.</param>
        public static void SetPlacementTarget(DependencyObject element, UIElement value)
        {
            Contract.Require(element, "element");

            element.SetValue(PlacementTargetProperty, value);
        }

        /// <summary>
        /// Sets the tooltip's content.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The content of the specified element's tooltip, or <c>null</c> if the element has no tooltip.</param>
        public static void SetToolTip(DependencyObject element, Object value)
        {
            Contract.Require(element, "element");

            element.SetValue(ToolTipProperty, value);
        }

        /// <summary>
        /// Adds a handler for the ToolTipOpening attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddToolTipOpeningHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, "element");

            UIElementHelper.AddHandler(element, ToolTipOpeningEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the ToolTipClosing attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddToolTipClosingHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, "element");

            UIElementHelper.AddHandler(element, ToolTipClosingEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the ToolTipOpening attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveToolTipOpeningHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, "element");

            UIElementHelper.RemoveHandler(element, ToolTipOpeningEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the ToolTipClosing attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveToolTipClosingHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, "element");

            UIElementHelper.RemoveHandler(element, ToolTipClosingEvent, handler);
        }

        /// <summary>
        /// Identifies the HorizontalOffset attached property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));
        
        /// <summary>
        /// Identifies the VerticalOffset attached property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the InitialShowDelay attached property.
        /// </summary>
        public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.Register("InitialShowDelay", typeof(Int32), typeof(ToolTipService),
            new PropertyMetadata<Double>(SystemParameters.MouseHoverTime, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the BetweenShowDelay attached property.
        /// </summary>
        public static readonly DependencyProperty BetweenShowDelayProperty = DependencyProperty.Register("BetweenShowDelay", typeof(Int32), typeof(ToolTipService),
            new PropertyMetadata<Double>(100.0, PropertyMetadataOptions.None));
        
        /// <summary>
        /// Identifies the ShowDuration attached property.
        /// </summary>
        public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.Register("ShowDuration", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(5000.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the IsEnabled attached property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the IsOpen read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the IsOpen attached property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = IsOpenPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the HasDropShadow attached property.
        /// </summary>
        public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.Register("HasDropShadow", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));
        
        /// <summary>
        /// Identifies the ShowOnDisabled attached property.
        /// </summary>
        public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.Register("ShowOnDisabled", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the Placement attached property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(ToolTipService),
            new PropertyMetadata<PlacementMode>(PlacementMode.Mouse, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the PlacementRectangle attached property.
        /// </summary>
        public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.Register("PlacementRectangle", typeof(RectangleD), typeof(ToolTipService),
            new PropertyMetadata<RectangleD>(RectangleD.Empty, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the PlacementTarget attached property.
        /// </summary>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(ToolTipService),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the ToolTip attached property.
        /// </summary>
        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.Register("ToolTip", typeof(Object), typeof(ToolTipService),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the ToolTipOpening attached routed event.
        /// </summary>
        public static readonly RoutedEvent ToolTipOpeningEvent = EventManager.RegisterRoutedEvent("ToolTipOpening", RoutingStrategy.Direct, 
            typeof(UpfToolTipEventHandler), typeof(ToolTipService));

        /// <summary>
        /// Identifies the ToolTipClosing attached routed event.
        /// </summary>
        public static readonly RoutedEvent ToolTipClosingEvent = EventManager.RegisterRoutedEvent("ToolTipClosing", RoutingStrategy.Direct,
            typeof(UpfToolTipEventHandler), typeof(ToolTipService));
        
        /// <summary>
        /// Raises the ToolTipOpening attached event for the specified element.
        /// </summary>
        internal static void RaiseToolTipOpening(DependencyObject element, RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfToolTipEventHandler>(ToolTipOpeningEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }

        /// <summary>
        /// Raises the ToolTipClosing attached event for the specified element.
        /// </summary>
        internal static void RaiseToolTipClosing(DependencyObject element, RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfToolTipEventHandler>(ToolTipClosingEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }
    }
}
