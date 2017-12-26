using System;
using System.Xml.Linq;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents an input binding which corresponds to no device and which is never pressed.
    /// </summary>
    public sealed class DummyInputBinding : InputBinding
    {
        /// <summary>
        /// Updates the binding's state.
        /// </summary>
        public override void Update()
        {

        }

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same button configuration as the specified input binding.
        /// </summary>
        /// <param name="binding">The input binding to compare against this input binding.</param>
        /// <returns>true if the specified input binding uses the same device and the same button 
        /// configuration as this input binding; otherwise, false.</returns>
        public override Boolean UsesSameButtons(InputBinding binding)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same primary buttons as the specified input binding.
        /// </summary>
        /// <param name="binding">The input binding to compare against this input binding.</param>
        /// <returns>true if the specified input binding uses the same device and the same primary 
        /// buttons as this input binding; otherwise, false.</returns>
        public override Boolean UsesSamePrimaryButtons(InputBinding binding)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the binding is down.
        /// </summary>
        /// <returns>true if the binding is down; otherwise, false.</returns>
        public override Boolean IsDown()
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the binding is up.
        /// </summary>
        /// <returns>true if the binding is up; otherwise, false.</returns>
        public override Boolean IsUp()
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the binding was pressed this frame.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>true if the binding was pressed this frame; otherwise, false.</returns>
        public override Boolean IsPressed(Boolean ignoreRepeats = true)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the binding was released this frame.
        /// </summary>
        /// <returns>true if the binding was released this frame; otherwise, false.</returns>
        public override Boolean IsReleased()
        {
            return false;
        }

        /// <summary>
        /// Creates an XML element that represents the binding.
        /// </summary>
        /// <param name="name">The binding's name.</param>
        /// <returns>An XML element that represents the binding.</returns>
        internal override XElement ToXml(String name = null)
        {
            return new XElement("Binding", name == null ? null : new XAttribute("Name", name));
        }

        /// <summary>
        /// Calculates the binding's priority relative to other bindings with the same primary buttons.
        /// </summary>
        /// <returns>The binding's priority relative to other bindings with the same primary buttons.</returns>
        protected override Int32 CalculatePriority()
        {
            return 0;
        }
    }
}
