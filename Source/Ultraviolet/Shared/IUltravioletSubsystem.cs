using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the method that is called when an Ultraviolet subsystem updates its state.
    /// </summary>
    /// <param name="subsystem">The Ultraviolet subsystem.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
    public delegate void UltravioletSubsystemUpdateEventHandler(IUltravioletSubsystem subsystem, UltravioletTime time);

    /// <summary>
    /// Represents one of Ultraviolet's subsystems.
    /// </summary>
    public interface IUltravioletSubsystem : IUltravioletComponent, IDisposable
    {
        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        void Update(UltravioletTime time);

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        Boolean Disposed { get; }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        event UltravioletSubsystemUpdateEventHandler Updating;
    }
}
