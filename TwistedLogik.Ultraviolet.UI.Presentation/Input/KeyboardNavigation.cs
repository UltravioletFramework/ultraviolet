using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
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
        /// <returns><c>true</c> if the specified element accepts Return; otherwise, <c>false</c>.</returns>
        public static Boolean GetAcceptsReturn(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(AcceptsReturnProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is a tab stop.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is a tab stop; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsTabStop(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsTabStopProperty);
        }

        /// <summary>
        /// Gets the tab index of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The tab index of the specified element.</returns>
        public static Int32 GetTabIndex(DependencyObject element)
        {
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

            return element.GetValue<KeyboardNavigationMode>(DirectionalNavigationProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the specified element accepts the Return character.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAcceptsReturn(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(AcceptsReturnProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the specified element is a tab stop.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsTabStop(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(IsTabStopProperty, value);
        }

        /// <summary>
        /// Sets the tab index of the specified element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The value to set.</param>
        public static void SetTabIndex(DependencyObject element, Int32 value)
        {
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

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
            Contract.Require(element, "element");

            element.SetValue(DirectionalNavigationProperty, value);
        }

        /// <summary>
        /// Identifies the AcceptsReturn attached property.
        /// </summary>
        public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.RegisterAttached("AcceptsReturn", typeof(Boolean), typeof(KeyboardNavigation),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the IsTabStop attached property.
        /// </summary>
        public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.RegisterAttached("IsTabStop", typeof(Boolean), typeof(KeyboardNavigation),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the TabIndex attached property.
        /// </summary>
        public static readonly DependencyProperty TabIndexProperty = DependencyProperty.RegisterAttached("TabIndex", typeof(Int32), typeof(KeyboardNavigation),
            new PropertyMetadata<Int32>(Int32.MaxValue, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the TabNavigation attached property.
        /// </summary>
        public static readonly DependencyProperty TabNavigationProperty = DependencyProperty.RegisterAttached("TabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the ControlNavigation attached property.
        /// </summary>
        public static readonly DependencyProperty ControlTabNavigationProperty = DependencyProperty.RegisterAttached("ControlTabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the DirectionalNavigation attached property.
        /// </summary>
        public static readonly DependencyProperty DirectionalNavigationProperty = DependencyProperty.RegisterAttached("DirectionalNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation),
            new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Continue, PropertyMetadataOptions.None));        
    }
}
