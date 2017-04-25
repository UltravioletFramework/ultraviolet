using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents one of a UI element's visual states.
    /// </summary>
    public sealed class VisualState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualState"/> class.
        /// </summary>
        /// <param name="visualStateGroup">The visual state group that created the state.</param>
        /// <param name="visualStateName">The visual state's name.</param>
        internal VisualState(VisualStateGroup visualStateGroup, String visualStateName)
        {
            Contract.Require(visualStateGroup, nameof(visualStateGroup));
            Contract.RequireNotEmpty(visualStateName, nameof(name));

            this.visualStateGroup = visualStateGroup;
            this.qualifiedName    = String.Format("{0}.{1}", visualStateGroup.Name, visualStateName);
            this.name             = visualStateName;
        }

        /// <summary>
        /// Gets the visual state group that created this visual state.
        /// </summary>
        public VisualStateGroup VisualStateGroup
        {
            get { return visualStateGroup; }
        }

        /// <summary>
        /// Gets the visual state's qualified name.
        /// </summary>
        public String QualifiedName
        {
            get { return qualifiedName; }
        }

        /// <summary>
        /// Gets the visual state's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        // Property values.
        private readonly VisualStateGroup visualStateGroup;
        private readonly String qualifiedName;
        private readonly String name;
    }
}
