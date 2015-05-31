using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a collection of classes associated with a UI element.
    /// </summary>
    public sealed partial class UIElementClassCollection  : IEnumerable<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementClassCollection"/> class.
        /// </summary>
        /// <param name="owner">The UI element that owns the collection.</param>
        internal UIElementClassCollection(UIElement owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }

        /// <summary>
        /// Adds a class to the collection.
        /// </summary>
        /// <param name="className">The name of the class to add to the collection.</param>
        /// <returns><c>true</c> if the class was added to the collection; otherwise, <c>false</c>.</returns>
        public Boolean Add(String className)
        {
            if (classes.Add(className))
            {
                owner.OnClassAdded(className);
                owner.InvalidateStyle();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Toggles a class. The class is removed if it exists, and added if it does not.
        /// </summary>
        /// <param name="className">The name of the class to toggle.</param>
        /// <returns><c>true</c> if the class was added to the collection; <c>false</c> if the class was removed from the collection.</returns>
        public Boolean Toggle(String className)
        {
            if (classes.Contains(className))
            {
                classes.Remove(className);
                owner.OnClassRemoved(className);
                return false;
            }
            classes.Add(className);
            owner.OnClassAdded(className);
            owner.InvalidateStyle();
            return true;
        }

        /// <summary>
        /// Removes a class from the collection.
        /// </summary>
        /// <param name="className">The name of the class to remove from the collection.</param>
        /// <returns><c>true</c> if the class was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(String className)
        {
            if (classes.Remove(className))
            {
                owner.OnClassRemoved(className);
                owner.InvalidateStyle();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified class.
        /// </summary>
        /// <param name="className">The name of the class to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified class; otherwise, <c>false</c>.</returns>
        public Boolean Contains(String className)
        {
            return classes.Contains(className);
        }

        /// <summary>
        /// Gets the UI element that owns the collection.
        /// </summary>
        public UIElement Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// Gets the number of classes in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return classes.Count; }
        }

        // Property values.
        private readonly UIElement owner;

        // State values.
        private readonly HashSet<String> classes = new HashSet<String>(StringComparer.InvariantCultureIgnoreCase);
    }
}
