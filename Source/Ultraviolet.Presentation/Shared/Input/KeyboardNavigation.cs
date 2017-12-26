using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Contains methods and attached properties used for performing keyboard navigation.
    /// </summary>
    [UvmlKnownType]
    public static class KeyboardNavigation
    {
        /// <summary>
        /// Gets a value indicating whether the specified element accepts the Return character.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element accepts Return; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetAcceptsReturn(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(AcceptsReturnProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is a tab stop.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is a tab stop; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetIsTabStop(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsTabStopProperty);
        }

        /// <summary>
        /// Gets the tab index of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The tab index of the specified element.</returns>
        public static Int32 GetTabIndex(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Int32>(TabIndexProperty);
        }

        /// <summary>
        /// Gets the <see cref="KeyboardNavigationMode"/> which is used when navigating through the
        /// specified element using the Tab key.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The <see cref="KeyboardNavigationMode"/> which is used during navigation.</returns>
        public static KeyboardNavigationMode GetTabNavigation(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<KeyboardNavigationMode>(TabNavigationProperty);
        }

        /// <summary>
        /// Gets the <see cref="KeyboardNavigationMode"/> which is used when navigating through the
        /// specified element using the Control + Tab key combination.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The <see cref="KeyboardNavigationMode"/> which is used during navigation.</returns>
        public static KeyboardNavigationMode GetControlTabNavigation(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<KeyboardNavigationMode>(ControlTabNavigationProperty);
        }

        /// <summary>
        /// Gets the <see cref="KeyboardNavigationMode"/> which is used when navigating through the
        /// specified element using a directional key.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The <see cref="KeyboardNavigationMode"/> which is used during navigation.</returns>
        public static KeyboardNavigationMode GetDirectionalNavigation(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<KeyboardNavigationMode>(DirectionalNavigationProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the specified element accepts the Return character.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAcceptsReturn(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(AcceptsReturnProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the specified element is a tab stop.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsTabStop(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(IsTabStopProperty, value);
        }

        /// <summary>
        /// Sets the tab index of the specified element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetTabIndex(DependencyObject element, Int32 value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(TabIndexProperty, value);
        }

        /// <summary>
        /// Sets the <see cref="KeyboardNavigationMode"/> value which is used when navigating through the specified
        /// element using the Tab key.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetTabNavigation(DependencyObject element, KeyboardNavigationMode value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(TabNavigationProperty, value);
        }

        /// <summary>
        /// Sets the <see cref="KeyboardNavigationMode"/> value which is used when navigating through the specified
        /// element using the Control + Tab key combination.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetControlTabNavigation(DependencyObject element, KeyboardNavigationMode value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(ControlTabNavigationProperty, value);
        }

        /// <summary>
        /// Sets the <see cref="KeyboardNavigationMode"/> value which is used when navigating through the specified
        /// element using a directional key.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDirectionalNavigation(DependencyObject element, KeyboardNavigationMode value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(DirectionalNavigationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.AcceptsReturn"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.AcceptsReturn"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the element accepts the Return character.
        /// </summary>
        /// <value><see langword="true"/> if the element accepts the Return character; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="AcceptsReturnProperty"/></dpropField>
        ///     <dpropStylingName>accepts-return</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.RegisterAttached("AcceptsReturn", typeof(Boolean), typeof(KeyboardNavigation),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.IsTabStop"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.IsTabStop"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the element is a tab stop.
        /// </summary>
        /// <value><see langword="true"/> if the element is a tab stop character; otherwise,
        /// <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsTabStopProperty"/></dpropField>
        ///     <dpropStylingName>tab-stop</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.RegisterAttached("IsTabStop", typeof(Boolean), typeof(KeyboardNavigation),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.TabIndex"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.TabIndex"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's tab index.
        /// </summary>
        /// <value>A <see cref="Int32"/> value which represents the element's position within its
        /// container's tab navigation order. The default value is <see cref="Int32.MaxValue"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TabIndexProperty"/></dpropField>
        ///     <dpropStylingName>tab-index</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty TabIndexProperty = DependencyProperty.RegisterAttached("TabIndex", typeof(Int32), typeof(KeyboardNavigation),
            new PropertyMetadata<Int32>(Int32.MaxValue, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.TabNavigation"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.TabNavigation"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's TAB navigation mode.
        /// </summary>
        /// <value>A <see cref="KeyboardNavigationMode"/> value which represents the element's 
        /// behavior when navigating by pressing the TAB key. The default value is <see cref="KeyboardNavigationMode.Continue"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TabNavigationProperty"/></dpropField>
        ///     <dpropStylingName>tab-navigation</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty TabNavigationProperty = DependencyProperty.RegisterAttached("TabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));
        
        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.ControlTabNavigation"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.ControlTabNavigation"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's CTRL+TAB navigation mode.
        /// </summary>
        /// <value>A <see cref="KeyboardNavigationMode"/> value which represents the element's 
        /// behavior when navigating by pressing the CTRL+TAB keys. The default value is <see cref="KeyboardNavigationMode.Continue"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ControlTabNavigationProperty"/></dpropField>
        ///     <dpropStylingName>control-tab-navigation</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ControlTabNavigationProperty = DependencyProperty.RegisterAttached("ControlTabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.DirectionalNavigation"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Input.KeyboardNavigation.DirectionalNavigation"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's directional navigation mode.
        /// </summary>
        /// <value>A <see cref="KeyboardNavigationMode"/> value which represents the element's 
        /// behavior when navigating by pressing one of the directional keys. The default value is 
        /// <see cref="KeyboardNavigationMode.Continue"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="DirectionalNavigationProperty"/></dpropField>
        ///     <dpropStylingName>directional-navigation</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty DirectionalNavigationProperty = DependencyProperty.RegisterAttached("DirectionalNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));

        /// <summary>
        /// Gets the last focused element within the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The last focused element within the specified element, or <see langword="null"/> if there is no such element.</returns>
        internal static DependencyObject GetLastFocusedElement(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            var weakref = element.GetValue<WeakReference>(LastFocusedElementProperty);

            var target = weakref?.Target as DependencyObject;
            if (target != null)
            {
                if (VisualTreeHelper.GetRoot(element) == VisualTreeHelper.GetRoot(target))
                    return target;

                element.SetValue<WeakReference>(LastFocusedElementProperty, null);
                WeakReferencePool.Instance.Release(weakref);
            }

            return null;
        }

        /// <summary>
        /// Gets the last focused element within the tree rooted in specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The last focused element within the tree rooted in the specified element, or <see langword="null"/> if there is no such element.</returns>
        internal static DependencyObject GetLastFocusedElementRecursive(DependencyObject element)
        {
            var result = default(DependencyObject);

            for (var current = GetLastFocusedElement(element); current != null; current = GetLastFocusedElement(current))
            {
                var uiElement = current as UIElement;
                if (uiElement != null && uiElement.Focusable && uiElement.IsEnabled && uiElement.IsVisible)
                    result = uiElement;
            }

            return result;
        }

        /// <summary>
        /// Sets the last focused element within the specified element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        internal static void SetLastFocusedElement(DependencyObject element, DependencyObject value)
        {
            var weakref = WeakReferencePool.Instance.Retrieve();
            weakref.Target = value;
            element.SetValue(LastFocusedElementProperty, weakref);
        }

        /// <summary>
        /// This dependency property is used to keep track of the "last focused element" within the element on which it is set.
        /// This is necessary to properly implement <see cref="KeyboardNavigationMode.Once"/>: according to MSDN, "either the first 
        /// tree child or the or the *last focused element* in the group receives focus."
        /// </summary>
        internal static readonly DependencyProperty LastFocusedElementProperty = DependencyProperty.RegisterAttached("LastFocusedElement", typeof(WeakReference), typeof(KeyboardNavigation),
            new PropertyMetadata<WeakReference>(null, PropertyMetadataOptions.None));
    }
}
