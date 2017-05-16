namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="CursorService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="CursorService"/> that was created.</returns>
    public delegate CursorService CursorServiceFactory();

    /// <summary>
    /// Represents a platform service which allows the application to change the cursor.
    /// </summary>
    public abstract class CursorService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CursorService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="CursorService"/> that was created.</returns>
        public static CursorService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<CursorServiceFactory>()();
        }
        
        /// <summary>
        /// Gets or sets the current cursor.
        /// </summary>
        public abstract Cursor Cursor { get; set; }
    }
}
