using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an entity that hosts an <see cref="IUltravioletApplicationAdapterHost"/>
    /// </summary>
    public interface IUltravioletApplicationAdapterHost
    {
        /// <summary>
        /// Exits the application.
        /// </summary>
        void Exit();

        /// <summary>
        /// Gets the directory that contains the application's local configuration files.
        /// If the directory does not already exist, it will be created.
        /// </summary>
        /// <returns>The directory that contains the application's local configuration files.</returns>
        String GetLocalApplicationSettingsDirectory();

        /// <summary>
        /// Gets the directory that contains the application's roaming configuration files.
        /// If the directory does not already exist, it will be created.
        /// </summary>
        /// <returns>The directory that contains the application's roaming configuration files.</returns>
        String GetRoamingApplicationSettingsDirectory();

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        UltravioletContext Ultraviolet { get; }
    }
}
