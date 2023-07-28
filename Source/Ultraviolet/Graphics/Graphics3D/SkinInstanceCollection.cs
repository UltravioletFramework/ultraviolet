using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a <see cref="SkinnedModelInstance"/>'s collection of skin instances.
    /// </summary>
    public class SkinInstanceCollection : ModelResourceCollection<SkinInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinInstanceCollection"/> class.
        /// </summary>
        /// <param name="skins">The skin instances to add to the collection.</param>
        public SkinInstanceCollection(IEnumerable<SkinInstance> skins)
            : base(skins, (skin, ix) => skin.Template.LogicalIndex)
        { }
    }
}
