namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Initializes factory methods for the FMOD implementation of the audio subsystem.
    /// </summary>
    public sealed class FMODAndroidFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<FMODPlatformSpecificImplementationDetailsFactory>((uv) => new FMODAndroidSpecificImplementationDetails());
        }
    }
}
