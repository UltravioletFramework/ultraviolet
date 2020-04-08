namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents an implementation of the <see cref="FMODPlatformSpecificImplementationDetails"/> class which does nothing.
    /// </summary>
    public class FMODGenericPlatformImplementationDetails : FMODPlatformSpecificImplementationDetails
    {
        /// <inheritdoc/>
        public override void OnInitialized() { }

        /// <inheritdoc/>
        public override void OnApplicationCreated() { }

        /// <inheritdoc/>
        public override void OnApplicationTerminating() { }
    }
}
