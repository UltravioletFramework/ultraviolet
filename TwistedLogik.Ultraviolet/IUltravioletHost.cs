using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents an object which hosts instances of the Ultraviolet Framework.
    /// </summary>
    public interface IUltravioletHost
    {
        /// <summary>
        /// Exits the application.
        /// </summary>
        void Exit();

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        UltravioletContext Ultraviolet
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the application's primary window is currently active.
        /// </summary>
        Boolean IsActive
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the application is running on a fixed time step.
        /// </summary>
        Boolean IsFixedTimeStep
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target time between frames when the application is running on a fixed time step.
        /// </summary>
        TimeSpan TargetElapsedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of time to sleep every frame when
        /// the application's primary window is inactive.
        /// </summary>
        TimeSpan InactiveSleepTime
        {
            get;
            set;
        }
    }
}
