using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains helper methods for interacting with the logical tree.
    /// </summary>
    public static class LogicalTreeHelper
    {
        /// <summary>
        /// Gets the index of the specified child within a given parent element's collection of logical children.
        /// </summary>
        /// <param name="parent">The parent element to evaluate.</param>
        /// <param name="child">The child element to evaluate.</param>
        /// <returns>The index of the specified child, or -1 if <paramref name="child"/> is not a logical child of <paramref name="parent"/>.</returns>
        public static Int32 GetIndexOfChild(DependencyObject parent, DependencyObject child)
        {
            var children = GetChildrenCount(parent);
            for (int i = 0; i < children; i++)
            {
                if (GetChild(parent, i) == child)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Performs an action for each of the specified object's logical children.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's logical children.</param>
        public static void ForEachChild(DependencyObject dobj, Object state, Action<DependencyObject, Object> action)
        {
            ForEachChild<DependencyObject>(dobj, state, action);
        }

        /// <summary>
        /// Performs an action for each of the specified object's logical children which are of the specified type.
        /// </summary>
        /// <typeparam name="TChild">The type of child to retrieve.</typeparam>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's logical children.</param>
        public static void ForEachChild<TChild>(DependencyObject dobj, Object state, Action<TChild, Object> action) where TChild : class
        {
            Contract.Require(dobj, nameof(dobj));
            Contract.Require(action, nameof(action));

            var children = GetChildrenCount(dobj);
            for (int i = 0; i < children; i++)
            {
                var child = GetChild(dobj, i) as TChild;
                if (child != null)
                {
                    action(child, state);
                }
            }
        }

        /// <summary>
        /// Gets the parent of the specified logical object.
        /// </summary>
        /// <param name="dobj">The object for which to retrieve a parent.</param>
        /// <returns>The parent of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetParent(DependencyObject dobj)
        {
            Contract.Require(dobj, nameof(dobj));

            var element = dobj as UIElement;
            if (element != null)
            {
                return element.Parent;
            }

            return null;
        }

        /// <summary>
        /// Gets the specified logical child of a specified parent object.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="childIndex">The index of the child to retrieve.</param>
        /// <returns>The specified logical child of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetChild(DependencyObject dobj, Int32 childIndex)
        {
            Contract.Require(dobj, nameof(dobj));

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.GetLogicalChild(childIndex);
            }

            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the first logical child of <paramref name="dobj"/> which matches the specified predicate.
        /// </summary>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate children. If <paramref name="predicate"/> is not null,
        /// any children which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The first child of <paramref name="dobj"/> in the logical tree which matches the specified predicate, or <see langword="null"/> if no such child exists.</returns>
        public static DependencyObject GetFirstChild(DependencyObject dobj, Predicate<DependencyObject> predicate = null)
        {
            return GetFirstChild<DependencyObject>(dobj, predicate);
        }

        /// <summary>
        /// Gets the first logical child of <paramref name="dobj"/> which matches the specified predicate.
        /// </summary>
        /// <typeparam name="TChild">The type of child to retrieve.</typeparam>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate children. If <paramref name="predicate"/> is not null,
        /// any children which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The first child of <paramref name="dobj"/> in the logical tree which matches the specified predicate, or <see langword="null"/> if no such child exists.</returns>
        public static DependencyObject GetFirstChild<TChild>(DependencyObject dobj, Predicate<TChild> predicate = null) where TChild : DependencyObject
        {
            Contract.Require(dobj, nameof(dobj));

            var children = GetChildrenCount(dobj);
            if (children == 0)
                return null;

            for (int i = 0; i < children; i++)
            {
                var child = GetChild(dobj, i) as TChild;
                if (child != null && (predicate == null || predicate(child)))
                    return child;
            }

            return null;
        }

        /// <summary>
        /// Gets the last logical child of <paramref name="dobj"/> which matches the specified predicate.
        /// </summary>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate children. If <paramref name="predicate"/> is not null,
        /// any children which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The last child of <paramref name="dobj"/> in the logical tree which matches the specified predicate, or <see langword="null"/> if no such child exists.</returns>
        public static DependencyObject GetLastChild(DependencyObject dobj, Predicate<DependencyObject> predicate = null)
        {
            return GetLastChild<DependencyObject>(dobj, predicate);
        }

        /// <summary>
        /// Gets the last logical child of <paramref name="dobj"/> which matches the specified predicate.
        /// </summary>
        /// <typeparam name="TChild">The type of child to retrieve.</typeparam>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate children. If <paramref name="predicate"/> is not null,
        /// any children which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The last child of <paramref name="dobj"/> in the logical tree which matches the specified predicate, or <see langword="null"/> if no such child exists.</returns>
        public static DependencyObject GetLastChild<TChild>(DependencyObject dobj, Predicate<TChild> predicate = null) where TChild : DependencyObject
        {
            Contract.Require(dobj, nameof(dobj));

            var children = GetChildrenCount(dobj);
            if (children == 0)
                return null;

            for (int i = children - 1; i >= 0; i--)
            {
                var child = GetChild(dobj, i) as TChild;
                if (child != null && (predicate == null || predicate(child)))
                    return child;
            }

            return null;
        }

        /// <summary>
        /// Gets the next sibling object after the specified object in the logical tree.
        /// </summary>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate siblings. If <paramref name="predicate"/> is not null,
        /// any siblings which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The next sibling of <paramref name="dobj"/> in the logical tree, or <see langword="null"/> if no such sibling exists.</returns>
        public static DependencyObject GetNextSibling(DependencyObject dobj, Predicate<DependencyObject> predicate = null)
        {
            return GetNextSibling<DependencyObject>(dobj, predicate);
        }

        /// <summary>
        /// Gets the next sibling object with the specified type after the specified object in the logical tree.
        /// </summary>
        /// <typeparam name="TSibling">The type of sibling to retrieve.</typeparam>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate siblings. If <paramref name="predicate"/> is not null,
        /// any siblings which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The next sibling of <paramref name="dobj"/> in the logical tree, or <see langword="null"/> if no such sibling exists.</returns>
        public static DependencyObject GetNextSibling<TSibling>(DependencyObject dobj, Predicate<TSibling> predicate = null) where TSibling : DependencyObject
        {
            Contract.Require(dobj, nameof(dobj));

            var parent = GetParent(dobj);
            if (parent == null)
                return null;

            var childIndex = GetIndexOfChild(parent, dobj);
            if (childIndex < 0)
                return null;

            var children = GetChildrenCount(parent);
            for (int i = childIndex + 1; i < children; i++)
            {
                var sibling = GetChild(parent, i) as TSibling;
                if (sibling != null && (predicate == null || predicate(sibling)))
                    return sibling;
            }

            return null;
        }

        /// <summary>
        /// Gets the previous sibling object before the specified object in the logical tree.
        /// </summary>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate siblings. If <paramref name="predicate"/> is not null,
        /// any siblings which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The previous sibling of <paramref name="dobj"/> in the logical tree, or <see langword="null"/> if no such sibling exists.</returns>
        public static DependencyObject GetPreviousSibling(DependencyObject dobj, Predicate<DependencyObject> predicate = null)
        {
            return GetPreviousSibling<DependencyObject>(dobj, predicate);
        }

        /// <summary>
        /// Gets the previous sibling object with the specified type after the specified object in the logical tree.
        /// </summary>
        /// <typeparam name="TSibling">The type of sibling to retrieve.</typeparam>
        /// <param name="dobj">The object to evaluate.</param>
        /// <param name="predicate">A predicate with which to evaluate siblings. If <paramref name="predicate"/> is not null,
        /// any siblings which cause it to return <see langword="false"/> will be skipped over by this method.</param>
        /// <returns>The previous sibling of <paramref name="dobj"/> in the logical tree, or <see langword="null"/> if no such sibling exists.</returns>
        public static DependencyObject GetPreviousSibling<TSibling>(DependencyObject dobj, Predicate<TSibling> predicate = null) where TSibling : DependencyObject
        {
            Contract.Require(dobj, nameof(dobj));

            var parent = GetParent(dobj);
            if (parent == null)
                return null;

            var childIndex = GetIndexOfChild(parent, dobj);
            if (childIndex < 0)
                return null;

            for (int i = childIndex - 1; i >= 0; i--)
            {
                var sibling = GetChild(parent, i) as TSibling;
                if (sibling != null && (predicate == null || predicate(sibling)))
                    return sibling;
            }

            return null;
        }

        /// <summary>
        /// Gets the number of logical children belonging to the specified parent.
        /// </summary>
        /// <param name="dobj">The parent object to evaluate.</param>
        /// <returns>The number of logical children belonging to <paramref name="dobj"/>.</returns>
        public static Int32 GetChildrenCount(DependencyObject dobj)
        {
            Contract.Require(dobj, nameof(dobj));

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.LogicalChildrenCount;
            }

            return 0;
        }
    }
}
