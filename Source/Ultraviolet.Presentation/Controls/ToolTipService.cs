using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls.Primitives;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the method that is called when a tool tip is opened or closed.
    /// </summary>
    /// <param name="dobj">The tool tip that was opened or closed.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfToolTipEventHandler(DependencyObject dobj, RoutedEventData data);

    /// <summary>
    /// Contains attached properties and events relating to tool tips.
    /// </summary>
    [UvmlKnownType]
    public static class ToolTipService
    {
        /// <summary>
        /// Gets the horizontal offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The horizontal offset between the tool tip's placement point and its position on the screen.</returns>
        public static Double GetHorizontalOffset(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Double>(HorizontalOffsetProperty);
        }

        /// <summary>
        /// Gets the vertical offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The vertical offset between the tool tip's placement point and its position on the screen.</returns>
        public static Double GetVerticalOffset(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Double>(VerticalOffsetProperty);
        }

        /// <summary>
        /// Gets the delay in milliseconds before the tool tip opens.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The delay in milliseconds before the tool tip opens.</returns>
        public static Double GetInitialShowDelay(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Double>(InitialShowDelayProperty);
        }

        /// <summary>
        /// Gets the maximum time in milliseconds after the closing of a tool tip during which another tool tip
        /// can be opened without a delay.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The maximum time in milliseconds after the closing of a tool tip during which another tool tip
        /// can be opened without a delay.</returns>
        public static Double GetBetweenShowDelay(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Double>(BetweenShowDelayProperty);
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that the tool tip remains visible.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The amount of time in milliseconds that the tool tip remains visible.</returns>
        public static Double GetShowDuration(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Double>(ShowDurationProperty);
        }

        /// <summary>
        /// Gets a value indicating whether a tool tip is displayed for the element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if a tool tip is displayed; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetIsEnabled(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsEnabledProperty);
        }

        /// <summary>
        /// Gets a value indicating whether a tool tip is currently open for the element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the tool tip is open; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetIsOpen(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsOpenProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the tool tip has a drop shadow.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the tool tip has a drop shadow; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetHasDropShadow(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(HasDropShadowProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the tool tip is displayed even if the element is disabled.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the tool tip is displayed for a disabled element; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetShowOnDisabled(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(ShowOnDisabledProperty);
        }

        /// <summary>
        /// Gets the tool tip's placement mode.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>A <see cref="PlacementMode"/> value which specifies how the tool tip is arranged on the screen.</returns>
        public static PlacementMode GetPlacement(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<PlacementMode>(PlacementProperty);
        }

        /// <summary>
        /// Gets the tool tip's placement rectangle.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The rectangle relative to which the tool tip popup is placed.</returns>
        public static RectangleD GetPlacementRectangle(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<RectangleD>(PlacementRectangleProperty);
        }

        /// <summary>
        /// Gets the tool tip's placement target.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The element relative to which the tool tip popup is placed.</returns>
        public static UIElement GetPlacementTarget(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<UIElement>(PlacementTargetProperty);
        }

        /// <summary>
        /// Gets the tool tip's content.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The content of the specified element's tool tip, or <see langword="null"/> if the element has no tool tip.</returns>
        public static Object GetToolTip(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Object>(ToolTipProperty);
        }
        
        /// <summary>
        /// Sets the horizontal offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The horizontal offset between the tool tip's placement point and its position on the screen.</param>
        public static void SetHorizontalOffset(DependencyObject element, Double value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Sets the vertical offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The vertical offset between the tool tip's placement point and its position on the screen.</param>
        public static void SetVerticalOffset(DependencyObject element, Double value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Sets the delay in milliseconds before the tool tip opens.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The delay in milliseconds before the tool tip opens.</param>
        public static void SetInitialShowDelay(DependencyObject element, Double value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(InitialShowDelayProperty, value);
        }

        /// <summary>
        /// Sets the maximum time in milliseconds after the closing of a tool tip during which another tool tip
        /// can be opened without a delay.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The maximum time in milliseconds after the closing of a tool tip during which another tool tip
        /// can be opened without a delay.</param>
        public static void SetBetweenShowDelay(DependencyObject element, Double value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(BetweenShowDelayProperty, value);
        }

        /// <summary>
        /// Sets the amount of time in milliseconds that the tool tip remains visible.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The amount of time in milliseconds that the tool tip remains visible.</param>
        public static void SetShowDuration(DependencyObject element, Double value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(ShowDurationProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether a tool tip is displayed for the element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><see langword="true"/> if a tool tip is displayed; otherwise, <see langword="false"/>.</param>
        public static void SetIsEnabled(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the tool tip has a drop shadow.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><see langword="true"/> if the tool tip has a drop shadow; otherwise, <see langword="false"/>.</param>
        public static void SetHasDropShadow(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(HasDropShadowProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the tool tip is displayed even if the element is disabled.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value"><see langword="true"/> if the tool tip is displayed for a disabled element; otherwise, <see langword="false"/>.</param>
        public static void SetShowOnDisabled(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(ShowOnDisabledProperty, value);
        }

        /// <summary>
        /// Sets the tool tip's placement mode.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">A <see cref="PlacementMode"/> value which specifies how the tool tip is arranged on the screen.</param>
        public static void SetPlacement(DependencyObject element, PlacementMode value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// Sets the tool tip's placement rectangle.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The rectangle relative to which the tool tip popup is placed.</param>
        public static void SetPlacementRectangle(DependencyObject element, RectangleD value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(PlacementRectangleProperty, value);
        }

        /// <summary>
        /// Sets the tool tip's placement target.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The element relative to which the tool tip popup is placed.</param>
        public static void SetPlacementTarget(DependencyObject element, UIElement value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(PlacementTargetProperty, value);
        }

        /// <summary>
        /// Sets the tool tip's content.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The content of the specified element's tool tip, or <see langword="null"/> if the element has no tool tip.</param>
        public static void SetToolTip(DependencyObject element, Object value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(ToolTipProperty, value);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipOpening"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddToolTipOpeningHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, nameof(element));

            UIElementHelper.AddHandler(element, ToolTipOpeningEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipClosing"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddToolTipClosingHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, nameof(element));

            UIElementHelper.AddHandler(element, ToolTipClosingEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipOpening"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveToolTipOpeningHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, nameof(element));

            UIElementHelper.RemoveHandler(element, ToolTipOpeningEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipClosing"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveToolTipClosingHandler(DependencyObject element, UpfToolTipEventHandler handler)
        {
            Contract.Require(element, nameof(element));

            UIElementHelper.RemoveHandler(element, ToolTipClosingEvent, handler);
        }

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.HorizontalOffset"/> 
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.HorizontalOffset"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the horizontal offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <value>A <see cref="Double"/> value which describes the horizontal offset in device-independent pixels between
        /// the element's tool tip's placement point and its position on the screen. The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="HorizontalOffsetProperty"/></dpropField>
        ///		<dpropStylingName>hoffset</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", "hoffset", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.VerticalOffset"/> 
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.VerticalOffset"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the vertical offset between the tool tip's placement point and its position on the screen.
        /// </summary>
        /// <value>A <see cref="Double"/> value which describes the vertical offset in device-independent pixels between
        /// the element's tool tip's placement point and its position on the screen. The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="VerticalOffsetProperty"/></dpropField>
        ///		<dpropStylingName>voffset</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", "voffset", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.InitialShowDelay"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.VerticalOffset"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the delay in milliseconds before the element's tool tip opens.
        /// </summary>
        /// <value>A <see cref="Double"/> value which specifies the delay in milliseconds before the tool tip opens.
        /// The default value is determined by the underlying system settings for tool tips.</value>
        /// <dprop>
        ///     <dpropField><see cref="InitialShowDelayProperty"/></dpropField>
        ///     <dpropStylingName>initial-show-delay</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(SystemParameters.MouseHoverTime, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.BetweenShowDelay"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.InitialShowDelay"/>
        /// attached property.</value>
        /// <remarks>
        /// <summary>
        /// Gets or sets the time in milliseconds after the closing of a 
        /// tool tip during which another tool tip can be opened without a delay.
        /// </summary>
        /// <value>A <see cref="Double"/> value which specifies the time in milliseconds after the closing of a 
        /// tool tip during which another tool tip can be opened without a delay. The default value is 100.0.</value>
        /// <dprop>
        ///     <dpropField><see cref="BetweenShowDelayProperty"/></dpropField>
        ///     <dpropStylingName>between-show-delay</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public static readonly DependencyProperty BetweenShowDelayProperty = DependencyProperty.RegisterAttached("BetweenShowDelay", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(100.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ShowDuration"/> 
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ShowDuration"/>
        /// attached property.</value>
        /// <remarks>
        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the element's tool tip remains visible.
        /// </summary>
        /// <value>A <see cref="Double"/> value which specifies the amount of time in milliseconds 
        /// that the tool tip remains visible. The default value is 5000.0.</value>
        /// <dprop>
        ///     <dpropField><see cref="ShowDurationProperty"/></dpropField>
        ///     <dpropStylingName>show-duration</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration", typeof(Double), typeof(ToolTipService),
            new PropertyMetadata<Double>(5000.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.IsEnabled"/> attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.IsEnabled"/>
        /// attached property.</value>
        /// <summary>
        /// Gets or sets a value indicating whether a tool tip is displayed for the element.
        /// </summary>
        /// <value><see langword="true"/> if a tool tip is displayed for the element; otherwise,
        /// <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsEnabledProperty"/></dpropField>
        ///     <dpropStylingName>enabled</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.IsOpen"/> 
        /// read-only attached property.
        /// </summary>
        internal static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.IsOpen"/> 
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.IsOpen"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets a value indicating whether the element's tool tip is open.
        /// </summary>
        /// <value><see langword="true"/> if the tool tip is open; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsOpenProperty"/></dpropField>
        ///     <dpropStylingName>open</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty IsOpenProperty = IsOpenPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.HasDropShadow"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.HasDropShadow"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the element's tool tip has a drop shadow.
        /// </summary>
        /// <value><see langword="true"/> if the element's tool tip has a drop shadow; otherwise,
        /// <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HasDropShadowProperty"/></dpropField>
        ///     <dpropStylingName>has-drop-shadow</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ShowOnDisabled"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ShowOnDisabled"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the element's tool tip is displayed
        /// even if the element is disabled.
        /// </summary>
        /// <value><see langword="true"/> if the element's tool tip is displayed if the element is disabled; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ShowOnDisabledProperty"/></dpropField>
        ///     <dpropStylingName>show-on-disabled</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.RegisterAttached("ShowOnDisabled", typeof(Boolean), typeof(ToolTipService),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.Placement"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ShowOnDisabled"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the placement mode for the element's tool tip.
        /// </summary>
        /// <value>A <see cref="PlacementMode"/> value which specifies how the element's tool tip is positioned
        /// relative to its placement target. The default value is <see cref="PlacementMode.Mouse"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="PlacementProperty"/></dpropField>
        ///     <dpropStylingName>placement</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(ToolTipService),
            new PropertyMetadata<PlacementMode>(PlacementMode.Mouse, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.PlacementRectangle"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.Placement"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the placement rectangle for the element's tool tip.
        /// </summary>
        /// <value>A <see cref="RectangleD"/> value which describes the area in device-independent pixels
        /// relative to which the tool tip is positioned. The default value is <see cref="RectangleD.Empty"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="PlacementProperty"/></dpropField>
        ///     <dpropStylingName>placement</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(RectangleD), typeof(ToolTipService),
            new PropertyMetadata<RectangleD>(RectangleD.Empty, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.PlacementTarget"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.PlacementRectangle"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the placement rectangle for the element's tool tip.
        /// </summary>
        /// <value>The <see cref="UIElement"/> relative to which the tool tip will be positioned. The
        /// default value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="PlacementTargetProperty"/></dpropField>
        ///     <dpropStylingName>placement-target</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(ToolTipService),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ToolTip"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ToolTip"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's tool tip content.
        /// </summary>
        /// <value>An <see cref="Object"/> which represents the content of the element's tool tip, or
        /// <see langword="null"/> if the element does not have a tool tip. The default value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ToolTipProperty"/></dpropField>
        ///     <dpropStylingName>tool-tip</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip", typeof(Object), typeof(ToolTipService),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipOpening"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipOpening"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element's tool tip is opened.
        /// </summary>
        /// <revt>
        ///     <revtField><see cref="ToolTipOpeningEvent"/></revtField>
        ///     <revtStylingName>tool-tip-opening</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfToolTipEventHandler"/></revtDelegate>
        /// </revt>
        /// </AttachedEventComments>
        public static readonly RoutedEvent ToolTipOpeningEvent = EventManager.RegisterRoutedEvent("ToolTipOpening", RoutingStrategy.Direct, 
            typeof(UpfToolTipEventHandler), typeof(ToolTipService));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipClosing"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipClosing"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element's tool tip is closed.
        /// </summary>
        /// <revt>
        ///     <revtField><see cref="ToolTipClosingEvent"/></revtField>
        ///     <revtStylingName>tool-tip-closing</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfToolTipEventHandler"/></revtDelegate>
        /// </revt>
        /// </AttachedEventComments>
        public static readonly RoutedEvent ToolTipClosingEvent = EventManager.RegisterRoutedEvent("ToolTipClosing", RoutingStrategy.Direct,
            typeof(UpfToolTipEventHandler), typeof(ToolTipService));

        /// <summary>
        /// Raises the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipOpening"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseToolTipOpening(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfToolTipEventHandler>(ToolTipOpeningEvent);
            evt?.Invoke(element, data);
        }

        /// <summary>
        /// Raises the <see cref="P:Ultraviolet.Presentation.Controls.ToolTipService.ToolTipClosing"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseToolTipClosing(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfToolTipEventHandler>(ToolTipClosingEvent);
            evt?.Invoke(element, data);
        }
    }
}
