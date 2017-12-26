
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a type which defines the layout of a vertex format.
    /// </summary>
    public interface IVertexType
    {
        /// <summary>
        /// Gets the type's vertex declaration.
        /// </summary>
        VertexDeclaration VertexDeclaration { get; }
    }
}
