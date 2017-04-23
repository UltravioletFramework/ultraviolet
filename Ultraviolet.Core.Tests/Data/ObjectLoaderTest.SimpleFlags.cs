using System;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a set of flags used by the object loader test.
    /// </summary>
    [Flags]
    public enum ObjectLoader_SimpleFlags
    {
        ValueOne = 0x01,
        ValueTwo = 0x02,
        ValueThree = 0x04,
    }
}
