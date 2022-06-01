#if IMGUI
using Ultraviolet.Core;

namespace UvDebug.UI.Screens
{
    /// <summary>
    /// Represents the view model for <see cref="ImGuiScreen"/>.
    /// </summary>
    public sealed class ImGuiViewModel 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiViewModel"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ImGuiScreen"/> that owns this view model.</param>
        public ImGuiViewModel(ImGuiScreen owner)
        {
            Contract.Require(owner, nameof(owner));

            this.owner = owner;
        }
        
        // Property values.
        private readonly ImGuiScreen owner;        
    }
}
#endif