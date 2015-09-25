using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains methods for performing keyboard navigation.
    /// </summary>
    internal static class KeyboardNavigator
    {
        /// <summary>
        /// Attempts to perform navigation as a result of the specified key press.
        /// </summary>
        /// <param name="view">The view for which to perform navigation.</param>
        /// <param name="device">The keyboard device that raised the key press event.</param>
        /// <param name="key">The key that was pressed.</param>
        /// <param name="ctrl">A value indicating whether the Ctrl modifier is active.</param>
        /// <param name="alt">A value indicating whether the Alt modifier is active.</param>
        /// <param name="shift">A value indicating whether the Shift modifier is active.</param>
        /// <param name="repeat">A value indicating whether this is a repeated key press.</param>
        /// <returns><c>true</c> if navigation was performed; otherwise, <c>false</c>.</returns>
        public static Boolean PerformNavigation(PresentationFoundationView view, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            Contract.Require(view, "view");

            var element = (view.ElementWithFocus ?? view.LayoutRoot) as UIElement;
            if (element == null)
                return false;

            switch (key)
            {
                case Key.Tab:
                    return PerformNavigation(view, element, shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next, ctrl);

                case Key.Left:
                    return PerformNavigation(view, element, FocusNavigationDirection.Left, false);

                case Key.Up:
                    return PerformNavigation(view, element, FocusNavigationDirection.Up, false);

                case Key.Right:
                    return PerformNavigation(view, element, FocusNavigationDirection.Right, false);

                case Key.Down:
                    return PerformNavigation(view, element, FocusNavigationDirection.Down, false);
            }

            return false;
        }

        /// <summary>
        /// Attempts to navigate focus in the specified direction.
        /// </summary>
        /// <param name="view">The view for which to perform navigation.</param>
        /// <param name="element">The element at which to begin navigation.</param>
        /// <param name="direction">The direction in which to navigate focus.</param>
        /// <param name="ctrl">A value indicating whether the Ctrl modifier is active.</param>
        /// <returns><c>true</c> if navigation was performed; otherwise, <c>false</c>.</returns>
        public static Boolean PerformNavigation(PresentationFoundationView view, UIElement element, FocusNavigationDirection direction, Boolean ctrl)
        {
            if (!PrepareNavigation(view, ref element, ref direction))
                return false;

            var navprop      = GetNavigationProperty(direction, ctrl);
            var navContainer = default(DependencyObject);
            var destination  = default(IInputElement);
            
            switch (direction)
            {
                case FocusNavigationDirection.Next:
                    navContainer = FindNavigationContainer(element, navprop);
                    destination  = FindNextNavigationStop(view, navContainer, element, navprop, false) as IInputElement;
                    break;

                case FocusNavigationDirection.Previous:
                    navContainer = FindNavigationContainer(element, navprop);
                    destination  = FindPrevNavigationStop(view, navContainer, element, navprop, false) as IInputElement;
                    break;

                case FocusNavigationDirection.First:
                    destination = FindNextNavigationStop(view, element, null, navprop, true) as IInputElement;
                    break;

                case FocusNavigationDirection.Last:
                    destination = FindPrevNavigationStop(view, element, null, navprop, true) as IInputElement;
                    break;

                case FocusNavigationDirection.Left:
                case FocusNavigationDirection.Right:
                case FocusNavigationDirection.Up:
                case FocusNavigationDirection.Down:
                    navContainer = FindNavigationContainer(element, navprop);
                    destination  = FindNavigationStopInDirection(view, navContainer, element, navprop, direction) as IInputElement;
                    break;
            }            

            if (destination != null)
            {
                view.FocusElement(destination);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prepares for keyboard navigation and potentially changes the current element and direction.
        /// </summary>
        /// <param name="view">The view for which to perform navigation.</param>
        /// <param name="element">The element at which to begin navigation.</param>
        /// <param name="direction">The direction in which to navigate focus.</param>
        /// <returns><c>true</c> if navigation can be performed; otherwise, false.</returns>
        private static Boolean PrepareNavigation(PresentationFoundationView view, ref UIElement element, ref FocusNavigationDirection direction)
        {
            if (element != null)
                return true;

            if (IsDirectionalNavigation(direction))
                return false;

            element = view.LayoutRoot;
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="FocusNavigationDirection"/> value represents tab navigation.
        /// </summary>
        /// <param name="direction">The <see cref="FocusNavigationDirection"/> value to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="direction"/> represents tab navigation; otherwise, <c>false</c>.</returns>
        private static Boolean IsTabNavigation(FocusNavigationDirection direction)
        {
            return
                direction == FocusNavigationDirection.Next ||
                direction == FocusNavigationDirection.Previous ||
                direction == FocusNavigationDirection.First ||
                direction == FocusNavigationDirection.Last;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="FocusNavigationDirection"/> value represents directional navigation.
        /// </summary>
        /// <param name="direction">The <see cref="FocusNavigationDirection"/> value to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="direction"/> represents directional navigation; otherwise, <c>false</c>.</returns>
        private static Boolean IsDirectionalNavigation(FocusNavigationDirection direction)
        {
            return
                direction == FocusNavigationDirection.Up ||
                direction == FocusNavigationDirection.Down ||
                direction == FocusNavigationDirection.Left ||
                direction == FocusNavigationDirection.Right;
        }

        /// <summary>
        /// Gets a value indicating whether the specified object is a valid navigation stop. To be a navigation stop,
        /// the element must be a tab stop, and it must also be focusable, enabled, and visible.
        /// </summary>
        /// <param name="navElement">The object to evaluate.</param>
        /// <returns><c>true</c> if the specified object is a navigation stop; otherwise, <c>false</c>.</returns>
        private static Boolean IsNavigationStop(DependencyObject navElement)
        {
            if (!KeyboardNavigation.GetIsTabStop(navElement))
                return false;

            var uiElement = navElement as UIElement;
            if (uiElement != null)
            {
                if (!uiElement.Focusable || !uiElement.IsEnabled || !uiElement.IsVisible)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified object is a navigation container, that is, whether it has
        /// a <see cref="KeyboardNavigationMode"/> other than <see cref="KeyboardNavigationMode.Continue"/>.
        /// </summary>
        /// <param name="navElement">The object to evaluate.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns><c>true</c> if the specified object is a navigation container; otherwise, <c>false</c>.</returns>
        private static Boolean IsNavigationContainer(DependencyObject navElement, DependencyProperty navProp)
        {
            return navElement.GetValue<KeyboardNavigationMode>(navProp) != KeyboardNavigationMode.Continue;
        }

        /// <summary>
        /// Gets a value indicating whether the specified candidate element is in the specified direction from the current element.
        /// </summary>
        /// <param name="navElement">The element from which navigation is being moved.</param>
        /// <param name="navCandidate">The candidate to which navigation might be moved.</param>
        /// <param name="direction">The direction in which focus is moving.</param>
        /// <returns><c>true</c> if the candidate is in the specified direction; otherwise, <c>false</c>.</returns>
        private static Boolean IsInDirection(DependencyObject navElement, DependencyObject navCandidate, FocusNavigationDirection direction)
        {
            var uiElement = navElement as UIElement;
            if (uiElement == null)
                return false;

            var uiCandidate = navCandidate as UIElement;
            if (uiCandidate == null)
                return false;

            switch (direction)
            {
                case FocusNavigationDirection.Left:
                    return uiCandidate.TransformedVisualBounds.Left < uiElement.TransformedVisualBounds.Left;

                case FocusNavigationDirection.Right:
                    return uiCandidate.TransformedVisualBounds.Right > uiElement.TransformedVisualBounds.Right;

                case FocusNavigationDirection.Up:
                    return uiCandidate.TransformedVisualBounds.Top < uiElement.TransformedVisualBounds.Top;

                case FocusNavigationDirection.Down:
                    return uiCandidate.TransformedVisualBounds.Bottom > uiElement.TransformedVisualBounds.Bottom;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified candidate is a better match for directional navigation than the current best match.
        /// </summary>
        /// <param name="navElement">The element from which focus is being moved.</param>
        /// <param name="navCurrentBest">The current best match for directional navigation.</param>
        /// <param name="navCandidate">The candidate match for directional navigation.</param>
        /// <returns><c>true</c> if the specified candidate is a better match than the current best match; otherwise, <c>false</c>.</returns>
        private static Boolean IsBetterDirectionalMatch(DependencyObject navElement, DependencyObject navCurrentBest, DependencyObject navCandidate)
        {
            var uiElement = navElement as UIElement;
            if (uiElement == null)
                return false;

            var uiCurrentBest = navCurrentBest as UIElement;
            if (uiCurrentBest == null)
                return false;

            var uiCandidate = navCandidate as UIElement;
            if (uiCandidate == null)
                return false;

            var boundsElement = uiElement.TransformedVisualBounds;
            var boundsCurrentBest = uiCurrentBest.TransformedVisualBounds;
            var boundsCandidate = uiCandidate.TransformedVisualBounds; 

            var distCurrentBest = 
                Math.Pow(boundsElement.Center.X - boundsCurrentBest.Center.X, 2) + 
                Math.Pow(boundsElement.Center.Y - boundsCurrentBest.Center.Y, 2);
            var distCandidate = 
                Math.Pow(boundsElement.Center.X - boundsCandidate.Center.X, 2) +
                Math.Pow(boundsElement.Center.Y - boundsCandidate.Center.Y, 2);

            return distCandidate < distCurrentBest;
        }

        /// <summary>
        /// Gets the navigation property which corresponds to the specified navigation direction.
        /// </summary>
        /// <param name="direction">The navigation direction for which to retrieve the corresponding navigation property.</param>
        /// <param name="ctrl">A value specifying whether the Ctrl keyboard modifier is currently active.</param>
        /// <returns>A <see cref="DependencyProperty"/> which identifies the navigation property corresponding to <paramref name="direction"/>.</returns>
        private static DependencyProperty GetNavigationProperty(FocusNavigationDirection direction, Boolean ctrl)
        {
            if (IsTabNavigation(direction))
            {
                return ctrl ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty;
            }
            return KeyboardNavigation.DirectionalNavigationProperty;
        }

        /// <summary>
        /// Moves forward through the visual tree, returning the next element after <paramref name="navElement"/>.
        /// </summary>
        /// <param name="navElement">The element which represents the current position in the visual tree.</param>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The next element in the visual tree after <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject TraverseVisualTreeNext(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp)
        {
            if (!IsNavigationContainer(navElement, navProp) || navElement == navContainer)
            {
                var candidate = VisualTreeHelper.GetFirstChild<UIElement>(navElement, x => x.IsVisible);
                if (candidate != null)
                    return candidate;
            }

            if (navElement == navContainer)
                return null;

            var element = navElement;
            var sibling = default(DependencyObject);

            while (true)
            {
                sibling = VisualTreeHelper.GetNextSibling<UIElement>(element, x => x.IsVisible);

                if (sibling != null)
                    return sibling;

                element = VisualTreeHelper.GetParent(element);

                if (element == null || element == navContainer)
                    break;
            }
            
            return null;
        }
        
        /// <summary>
        /// Gets the first navigation stop within the specified navigation container.
        /// </summary>
        /// <param name="navContainer">The navigation container to search for a navigation stop.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The first navigation stop within the specified navigation container, or <c>null</c>.</returns>
        private static DependencyObject GetFirstNavigationStop(DependencyObject navContainer, DependencyProperty navProp)
        {
            var min = Int32.MaxValue;

            var match = default(DependencyObject);
            var current = navContainer;

            while (true)
            {
                current = TraverseVisualTreeNext(navContainer, current, navProp);
                if (current == null)
                    break;

                if (!IsNavigationStop(current) && !IsNavigationContainer(current, navProp))
                    continue;

                var index = KeyboardNavigation.GetTabIndex(current);
                if (index < min || match == null)
                {
                    min = index;
                    match = current;
                }
            }

            return match;
        }

        /// <summary>
        /// Searches the visual tree for the next element in the tab order after <paramref name="navElement"/>.
        /// </summary>
        /// <param name="view">The view for which navigation is being performed.</param>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="local">A value indicating whether the search is restricted only to the visual subtree of <paramref name="navContainer"/>.</param>
        /// <returns>The next element in the tab order after <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject FindNextNavigationStop(PresentationFoundationView view, DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, Boolean local)
        {
            if (navElement == null && IsNavigationStop(navContainer))
                return navContainer;

            var navMode = navContainer.GetValue<KeyboardNavigationMode>(navProp);

            if (navElement != null && (navMode == KeyboardNavigationMode.None || navMode == KeyboardNavigationMode.Once))
            {
                if (local || navContainer == view.LayoutRoot)
                    return null;

                var navContainerContainer = FindNavigationContainer(navContainer, navProp, false) ?? view.LayoutRoot;
                return FindNextNavigationStop(view, navContainerContainer, navContainer, navProp, local);
            }

            var first = default(DependencyObject);
            var next = navElement;

            while (true)
            {
                next = FindNextVisualElementWithinContainer(navContainer, next, navProp, navMode);

                if (next == null || next == first)
                    break;

                if (first == null)
                    first = next;

                var descendant = FindNextNavigationStop(view, next, null, navProp, true);
                if (descendant != null)
                    return descendant;

                if (navMode == KeyboardNavigationMode.Once)
                    navMode = KeyboardNavigationMode.Contained;
            }

            if (local)
                return null;

            if (navMode != KeyboardNavigationMode.Contained)
            {
                if (VisualTreeHelper.GetParent(navContainer) != null)
                {
                    var navContainerContainer = FindNavigationContainer(navContainer, navProp, false);
                    return FindNextNavigationStop(view, navContainerContainer, navContainer, navProp, false);
                }
                return GetFirstNavigationStop(navContainer, navProp);
            }

            return null;
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navContainer"/> for the next element in the tab order after <paramref name="navElement"/>.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="navMode">The currently active navigation mode.</param>
        /// <returns>The next element in the tab order after <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject FindNextVisualElementWithinContainer(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, KeyboardNavigationMode navMode)
        {
            if (navMode == KeyboardNavigationMode.None)
                return null;

            if ((navElement ?? navContainer) == navContainer)
                return GetFirstNavigationStop(navContainer, navProp);

            if (navMode == KeyboardNavigationMode.Once)
                return null;

            return
                FindNextVisualElementWithSameTabIndex(navContainer, navElement, navProp) ??
                FindNextVisualElementWithHigherTabIndex(navContainer, navElement, navProp, navMode);
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navContainer"/> for the next element in the tab order after <paramref name="navElement"/>
        /// which has the same tab index.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The next element in the tab order after <paramref name="navElement"/> which has the same tab index, or <c>null</c>.</returns>
        private static DependencyObject FindNextVisualElementWithSameTabIndex(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp)
        {
            var targetTabIndex = KeyboardNavigation.GetTabIndex(navElement);

            var current = navElement;

            while (true)
            {
                current = TraverseVisualTreeNext(navContainer, current, navProp);
                if (current == null)
                    break;

                if ((IsNavigationStop(current) || IsNavigationContainer(current, navProp)) && KeyboardNavigation.GetTabIndex(current) == targetTabIndex)
                {
                    return current;
                }
            }

            return null;
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navContainer"/> for the next element in the tab order after <paramref name="navElement"/>
        /// which has a higher tab index.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="navMode">The currently active navigation mode.</param>
        /// <returns>The next element in the tab order after <paramref name="navElement"/> which has a higher tab index, or <c>null</c>.</returns>
        private static DependencyObject FindNextVisualElementWithHigherTabIndex(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, KeyboardNavigationMode navMode)
        {
            var targetTabIndex = KeyboardNavigation.GetTabIndex(navElement);

            var minIndex = Int32.MaxValue;
            var minElement = default(DependencyObject);

            var firstIndex = Int32.MaxValue;
            var firstElement = default(DependencyObject);

            var current = navContainer;

            while (true)
            {
                current = TraverseVisualTreeNext(navContainer, current, navProp);
                if (current == null)
                    break;

                if (IsNavigationStop(current) || IsNavigationContainer(current, navProp))
                {
                    var currentTabIndex = KeyboardNavigation.GetTabIndex(current);
                    if (currentTabIndex > targetTabIndex && (minElement == null || currentTabIndex < minIndex))
                    {
                        minIndex = currentTabIndex;
                        minElement = current;
                    }

                    if (firstElement == null || currentTabIndex < firstIndex)
                    {
                        firstIndex = currentTabIndex;
                        firstElement = current;
                    }
                }
            }
            
            return (minElement == null && navMode == KeyboardNavigationMode.Cycle) ? firstElement : minElement;
        }

        /// <summary>
        /// Moves backwards through the visual tree, returning the previous element before <paramref name="navElement"/>.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element which represents the current position in the visual tree.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The previous element in the visual tree before <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject TraverseVisualTreePrev(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp)
        {
            if (navContainer == navElement)
                return null;

            var sibling = VisualTreeHelper.GetPreviousSibling<UIElement>(navElement, x => x.IsVisible);
            if (sibling != null)
            {
                return IsNavigationContainer(sibling, navProp) ? sibling : FindLastVisualElementInTree(sibling, navProp);
            }

            return VisualTreeHelper.GetParent(navElement);
        }

        /// <summary>
        /// Gets the last navigation stop within the specified navigation container.
        /// </summary>
        /// <param name="navContainer">The navigation container to search for a navigation stop.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The last navigation stop within the specified navigation container, or <c>null</c>.</returns>
        private static DependencyObject GetLastNavigationStop(DependencyObject navContainer, DependencyProperty navProp)
        {
            var max = Int32.MinValue;

            var match = default(DependencyObject);
            var current = FindLastVisualElementInTree(navContainer, navProp);

            while (true)
            {
                if (current == null || current == navContainer)
                    break;

                if (IsNavigationStop(current) || IsNavigationContainer(current, navProp))
                {
                    var index = KeyboardNavigation.GetTabIndex(current);
                    if (index > max || match == null)
                    {
                        max = index;
                        match = current;
                    }
                }

                current = TraverseVisualTreePrev(navContainer, current, navProp);
            }

            return match;
        }

        /// <summary>
        /// Searches the visual tree for the previous element in the tab order before <paramref name="navElement"/>.
        /// </summary>
        /// <param name="view">The view for which navigation is being performed.</param>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="local">A value indicating whether the search is restricted only to the visual subtree of <paramref name="navContainer"/>.</param>
        /// <returns>The previous element in the tab order before <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject FindPrevNavigationStop(PresentationFoundationView view, DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, Boolean local)
        {
            var navMode = navContainer.GetValue<KeyboardNavigationMode>(navProp);

            if (navElement != null && (navMode == KeyboardNavigationMode.Once || navMode == KeyboardNavigationMode.None))
            {
                if (local || navContainer == navElement)
                    return null;

                if (IsNavigationStop(navContainer))
                    return navContainer;

                var navContainerContainer = FindNavigationContainer(navContainer, navProp, false);
                return FindPrevNavigationStop(view, navContainerContainer, navContainer, navProp, false);
            }

            if (navElement == null && (navMode == KeyboardNavigationMode.Once))
            {
                var descendant = FindNextVisualElementWithinContainer(navContainer, null, navProp, navMode);
                if (descendant != null)
                {
                    return FindPrevNavigationStop(view, descendant, null, navProp, true);
                }
                else
                {
                    if (IsNavigationStop(navContainer))
                        return navContainer;

                    if (local)
                        return null;
                    
                    var navContainerContainer = FindNavigationContainer(navContainer, navProp, false);
                    return FindPrevNavigationStop(view, navContainerContainer, navContainer, navProp, false);
                }
            }

            var first = default(DependencyObject);
            var prev = navElement;

            while (true)
            {
                prev = FindPrevVisualElementWithinContainer(navContainer, prev, navProp, navMode);
                if (prev == null || (prev == navContainer && (navMode == KeyboardNavigationMode.Local || navMode == KeyboardNavigationMode.Contained)))
                    break;

                if (IsNavigationStop(prev) && !IsNavigationContainer(prev, navProp))
                    return prev;

                if (first == prev)
                    break;

                if (first == null)
                    first = prev;

                var last = FindPrevNavigationStop(view, prev, null, navProp, true);
                if (last != null)
                    return last;
            }

            if (navMode != KeyboardNavigationMode.Contained)
            {
                if (navElement != navContainer && IsNavigationStop(navContainer))
                    return navContainer;

                if (local)
                    return null;

                if (VisualTreeHelper.GetParent(navContainer) != null)
                {
                    var navContainerContainer = FindNavigationContainer(navContainer, navProp, false);
                    return FindPrevNavigationStop(view, navContainerContainer, navContainer, navProp, false);
                }

                var lastNavStopInContainer = GetLastNavigationStop(navContainer, navProp);
                return ((lastNavStopInContainer == null) ? null : FindPrevVisualElementWithinContainer(lastNavStopInContainer, null, navProp, navMode)) ?? lastNavStopInContainer;
            }

            return null;
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navcontainer"/> for the previous element in the tab order before <paramref name="navElement"/>.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The previous element in the tab order before <paramref name="navElement"/>, or <c>null</c>.</returns>
        private static DependencyObject FindPrevVisualElementWithinContainer(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, KeyboardNavigationMode navMode)
        {
            if (navMode == KeyboardNavigationMode.None)
                return null;

            if (navElement == null)
                return GetLastNavigationStop(navContainer, navProp);

            if (navMode == KeyboardNavigationMode.Once || navElement == navContainer)
                return null;

            return
                FindPrevVisualElementWithSameTabIndex(navContainer, navElement, navProp) ??
                FindPrevVisualElementWithLowerTabIndex(navContainer, navElement, navProp, navMode);
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navContainer"/> for the previous element in the tab order before <paramref name="navElement"/>
        /// which has the same tab index.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The previous element in the tab order before <paramref name="navElement"/> which has the same tab index, or <c>null</c>.</returns>
        private static DependencyObject FindPrevVisualElementWithSameTabIndex(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp)
        {
            var targetTabIndex = KeyboardNavigation.GetTabIndex(navElement);

            var current = navElement;

            while (true)
            {
                current = TraverseVisualTreePrev(navContainer, current, navProp);
                if (current == null)
                    break;

                if (current == navContainer)
                    continue;

                if ((IsNavigationStop(current) || IsNavigationContainer(current, navProp)) && KeyboardNavigation.GetTabIndex(current) == targetTabIndex)
                {
                    return current;
                }
            }

            return null;
        }

        /// <summary>
        /// Searches the visual subtree contained by <paramref name="navContainer"/> for the previous element in the tab order before <paramref name="navElement"/>
        /// which has a lower tab index.
        /// </summary>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="navMode">The currently active navigation mode.</param>
        /// <returns>The previous element in the tab order before <paramref name="navElement"/> which has a lower tab index, or <c>null</c>.</returns>
        private static DependencyObject FindPrevVisualElementWithLowerTabIndex(DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, KeyboardNavigationMode navMode)
        {
            var targetTabIndex = KeyboardNavigation.GetTabIndex(navElement);

            var maxIndex = Int32.MinValue;
            var maxElement = default(DependencyObject);

            var lastIndex = Int32.MinValue;
            var lastElement = default(DependencyObject);

            var current = FindLastVisualElementInTree(navContainer, navProp);

            while (true)
            {
                if (IsNavigationStop(current) || IsNavigationContainer(current, navProp))
                {
                    var currentTabIndex = KeyboardNavigation.GetTabIndex(current);
                    if (currentTabIndex < targetTabIndex && (maxElement == null || currentTabIndex > maxIndex))
                    {
                        maxIndex = currentTabIndex;
                        maxElement = current;
                    }

                    if (lastElement == null || currentTabIndex > lastIndex)
                    {
                        lastIndex = currentTabIndex;
                        lastElement = current;
                    }
                }
                
                current = TraverseVisualTreePrev(navContainer, current, navProp);
                if (current == null)
                    break;
            }

            return (maxElement == null && navMode == KeyboardNavigationMode.Cycle) ? lastElement : maxElement;
        }

        /// <summary>
        /// Finds the last visual element within the specified visual subtree.
        /// </summary>
        /// <param name="navElement">The element at the root of the visual subtree to evaluate.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <returns>The last visual element within the specified visual subtree.</returns>
        private static DependencyObject FindLastVisualElementInTree(DependencyObject navElement, DependencyProperty navProp)
        {
            var previous = navElement;
            var current = navElement;

            while (true)
            {
                previous = current;
                current = VisualTreeHelper.GetLastChild<UIElement>(current, x => x.IsVisible);

                if (current == null || IsNavigationContainer(current, navProp))
                    break;
            }

            return current ?? previous;
        }

        /// <summary>
        /// Finds the navigation container which contains the specified object.
        /// </summary>
        /// <param name="navElement">The object to evaluate.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="includeSelf">A value indicating whether to include <paramref name="navElement"/> as a potential result.</param>
        /// <returns>The navigation container that contains the specified object.</returns>
        private static DependencyObject FindNavigationContainer(DependencyObject navElement, DependencyProperty navProp, Boolean includeSelf = true)
        {
            var previous = default(DependencyObject);
            var current = includeSelf ? navElement : VisualTreeHelper.GetParent(navElement);

            while (current != null)
            {
                if (IsNavigationContainer(current, navProp))
                    return current;

                previous = current;
                current = VisualTreeHelper.GetParent(current);
            }

            return previous;
        }

        /// <summary>
        /// Finds the next navigation stop in the specified direction.
        /// </summary>
        /// <param name="view">The view for which navigation is being performed.</param>
        /// <param name="navContainer">The navigation container that contains <paramref name="navElement"/>.</param>
        /// <param name="navElement">The element at which to begin the search.</param>
        /// <param name="navProp">The navigation property to evaluate.</param>
        /// <param name="direction">The direction in which to navigate.</param>
        /// <returns>The next navigation stop in the specified direction, or <c>null</c>.</returns>
        private static DependencyObject FindNavigationStopInDirection(PresentationFoundationView view, DependencyObject navContainer, DependencyObject navElement, DependencyProperty navProp, FocusNavigationDirection direction)
        {
            var bestMatch = default(DependencyObject);
            var bestMatchScore = Int32.MaxValue;

            var navMode = navContainer.GetValue<KeyboardNavigationMode>(navProp);
            var current = FindNextVisualElementWithinContainer(navContainer, null, navProp, navMode);

            while (current != null)
            {
                if (IsInDirection(navElement, current, direction) && (IsNavigationStop(current) || IsNavigationContainer(current, navProp)))
                {
                    var score = EvaluateDistanceScore(navElement, current, direction);
                    if (score < bestMatchScore)
                    {
                        bestMatch = current;
                        bestMatchScore = score;
                    }
                    else
                    {
                        if (score == bestMatchScore && IsBetterDirectionalMatch(navElement, bestMatch, current))
                        {
                            bestMatch = current;
                        }
                    }
                }
                current = FindNextVisualElementWithinContainer(navContainer, current, navProp, navMode);
            }

            // TODO: Look outside of container

            return bestMatch;
        }

        /// <summary>
        /// Calculates the directional navigation score of the specified candidate element. Lower is better.
        /// </summary>
        /// <param name="navElement">The element from which navigation is being moved.</param>
        /// <param name="navCandidate">The candidate to which navigation might be moved.</param>
        /// <param name="direction">The direction in which focus is moving.</param>
        /// <returns>A score used to rank potential navigation candidates.</returns>
        private static Int32 EvaluateDistanceScore(DependencyObject navElement, DependencyObject navCandidate, FocusNavigationDirection direction)
        {
            var uiElement = navElement as UIElement;
            if (uiElement == null)
                return Int32.MaxValue;

            var uiCandidate = navCandidate as UIElement;
            if (uiCandidate == null)
                return Int32.MaxValue;

            switch (direction)
            {
                case FocusNavigationDirection.Left:
                    return (Int32)Math.Abs(uiCandidate.TransformedVisualBounds.Right - uiElement.TransformedVisualBounds.Left);

                case FocusNavigationDirection.Right:
                    return (Int32)Math.Abs(uiCandidate.TransformedVisualBounds.Left - uiElement.TransformedVisualBounds.Right);

                case FocusNavigationDirection.Up:
                    return (Int32)Math.Abs(uiCandidate.TransformedVisualBounds.Bottom - uiElement.TransformedVisualBounds.Top);

                case FocusNavigationDirection.Down:
                    return (Int32)Math.Abs(uiCandidate.TransformedVisualBounds.Top - uiElement.TransformedVisualBounds.Bottom);
            }

            return Int32.MaxValue;
        }        
    }
}
