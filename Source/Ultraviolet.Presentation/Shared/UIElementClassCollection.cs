using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a collection of classes associated with a UI element.
    /// </summary>
    public sealed partial class UIElementClassCollection : IEnumerable<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementClassCollection"/> class.
        /// </summary>
        /// <param name="owner">The element that owns the collection.</param>
        internal UIElementClassCollection(UIElement owner)
        {
            Contract.Require(owner, nameof(owner));

            this.Owner = owner;
        }

        /// <summary>
        /// Removes all items from the collection and invalidates the
        /// styles of the element that owns the collection.
        /// </summary>
        public void Clear()
        {
            foreach (var existingClassName in classes)
                Owner.OnClassRemoved(existingClassName);

            classes.Clear();
            Owner.InvalidateStyle();
        }

        /// <summary>
        /// Adds the specified class to the collection, if it is not already in the collection,
        /// and removes all other classes. If this operation results in the collection changing,
        /// then the styles of the element that owns the collection are invalidated.
        /// </summary>
        /// <param name="className">The name of the class to add to the collection.</param>
        public void Set(String className)
        {
            Contract.RequireNotEmpty(className, nameof(className));

            var wasAlreadySet = false;
            var wasCleared = false;

            for (int i = 0; i < classes.Count; i++)
            {
                var existingClassName = classes[i];
                if (String.Equals(existingClassName, className, StringComparison.OrdinalIgnoreCase))
                {
                    wasAlreadySet = true;
                    continue;
                }
                wasCleared = true;
                Owner.OnClassRemoved(existingClassName);
            }

            if (wasCleared)
                classes.Clear();

            if (!wasAlreadySet)
            {
                classes.Add(className);
                Owner.OnClassAdded(className);
            }

            if (wasCleared || !wasAlreadySet)
                Owner.InvalidateStyle();
        }

        /// <summary>
        /// Attempts to add the specified class to the collection. If the class is added to the collection,
        /// then the styles of the element that owns the collection are invalidated.
        /// </summary>
        /// <param name="className">The name of the element to add to the collection.</param>
        /// <returns><see langword="true"/> if the class was added to the collection;
        /// otherwise, <see langword="false"/>.</returns>
        public Boolean Add(String className)
        {
            Contract.RequireNotEmpty(className, nameof(className));

            return AddInternal(className);
        }
        
        /// <summary>
        /// Attempts to remove the specified class from the collection. If the class is removed
        /// from the collection, then the styles of the element that owns the collection are invalidated.
        /// </summary>
        /// <param name="className">The name of thhe class to remove from the collection.</param>
        /// <returns><see langword="true"/> if the class was removed from the collection;
        /// otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(String className)
        {
            Contract.RequireNotEmpty(className, nameof(className));

            return RemoveInternal(className);
        }

        /// <summary>
        /// Adds the specified class to the collection if it does not already exist.
        /// Otherwise, the class is removed from the collection. In either case, the 
        /// styles of the element that owns the collection are invalidated.
        /// </summary>
        /// <param name="className">The name of the class to toggle.</param>
        /// <returns><see langword="true"/> if the class was added to the collection;
        /// <see langword="false"/> if the class was removed from the collection.</returns>
        public Boolean Toggle(String className)
        {
            Contract.RequireNotEmpty(className, nameof(className));

            if (RemoveInternal(className))
                return false;

            AddInternal(className);
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified class.
        /// </summary>
        /// <param name="className">The name of the class to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified class;
        /// otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(String className)
        {
            Contract.RequireNotEmpty(className, nameof(className));

            return ContainsInternal(className);
        }

        /// <summary>
        /// Gets the UI element that owns the collection.
        /// </summary>
        public UIElement Owner
        {
            get;
        }

        /// <summary>
        /// Gets the number of classes in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return classes.Count; }
        }

        /// <summary>
        /// Attempts to add the specified class to the collection. If the class is added to the collection,
        /// then the styles of the element that owns the collection are invalidated.
        /// </summary>
        public Boolean AddInternal(String className)
        {
            if (ContainsInternal(className))
                return false;

            classes.Add(className);

            Owner.OnClassAdded(className);
            Owner.InvalidateStyle();

            return true;
        }

        /// <summary>
        /// Attempts to remove the specified class from the collection. If the class is removed
        /// from the collection, then the styles of the element that owns the collection are invalidated.
        /// </summary>
        public Boolean RemoveInternal(String className)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                if (String.Equals(classes[i], className, StringComparison.OrdinalIgnoreCase))
                {
                    classes.RemoveAt(i);

                    Owner.OnClassRemoved(className);
                    Owner.InvalidateStyle();

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified class.
        /// </summary>
        private Boolean ContainsInternal(String className)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                if (String.Equals(classes[i], className, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        
        // The underlying storage for the collection.
        private readonly List<String> classes = new List<String>();
    }
}
