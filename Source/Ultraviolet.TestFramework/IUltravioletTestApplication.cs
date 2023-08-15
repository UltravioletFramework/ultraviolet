using System;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents an Ultraviolet application used in unit tests.
    /// </summary>
    public interface IUltravioletTestApplication : IDisposable
    {
        /// <summary>
        /// Gets the adapter
        /// </summary>
        IUltravioletTestApplicationAdapter Adapter { get; }
    }
}
