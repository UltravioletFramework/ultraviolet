
namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents the types of data supported by the object loader.
    /// </summary>
    public enum ObjectLoaderDataType
    {
        /// <summary>
        /// Attempt to auto-detect the type of data in the definition file.
        /// </summary>
        Detect,

        /// <summary>
        /// The definition file contains XML data.
        /// </summary>
        Xml,

        /// <summary>
        /// The definition file contains JSON data.
        /// </summary>
        Json,
    }
}
