namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="FMODPlatformSpecificImplementationDetails"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="FMODPlatformSpecificImplementationDetails"/> that was created.</returns>
    public delegate FMODPlatformSpecificImplementationDetails FMODPlatformSpecificImplementationDetailsFactory(UltravioletContext uv);

    /// <summary>
    /// Represents platform-specific implementation details for FMOD which can be provided by an additional assembly.
    /// </summary>
    public abstract class FMODPlatformSpecificImplementationDetails
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FMODPlatformSpecificImplementationDetails"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="FMODPlatformSpecificImplementationDetails"/> that was created.</returns>
        public static FMODPlatformSpecificImplementationDetails Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            var factory = uv.TryGetFactoryMethod<FMODPlatformSpecificImplementationDetailsFactory>();
            if (factory == null)
                return new FMODGenericPlatformImplementationDetails();

            return factory(uv);
        }

        /// <summary>
        /// Called when FMOD is initialized.
        /// </summary>
        public abstract void OnInitialized();

        /// <summary>
        /// Called when the operating system creates the application.
        /// </summary>
        public abstract void OnApplicationCreated();

        /// <summary>
        /// Called when the operating system destroys the application.
        /// </summary>
        public abstract void OnApplicationTerminating();
    }
}
