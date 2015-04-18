using System.Collections.Generic;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a node in a UVML object tree.
    /// </summary>
    internal class UvmlObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlObject"/> class.
        /// </summary>
        /// <param name="xml">The XML element that corresponds to this node.</param>
        /// <param name="instance">The object instance that corresponds to this node.</param>
        public UvmlObject(XElement xml, UIElement instance)
        {
            this.xml = xml;
            this.instance  = instance;
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> that corresponds to this node.
        /// </summary>
        public XElement Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Gets the <see cref="Instance"/> that corresponds to this node.
        /// </summary>
        public UIElement Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the node's list of children.
        /// </summary>
        public List<UvmlObject> Children
        {
            get { return children; }
        }

        // Property values.
        private readonly XElement xml;
        private readonly UIElement instance;
        private readonly List<UvmlObject> children = new List<UvmlObject>();
    }
}
