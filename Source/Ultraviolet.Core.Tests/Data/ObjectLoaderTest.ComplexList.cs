using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a complex list used to test the object loader.
    /// </summary>
    public class ObjectLoader_ComplexList : List<Int32>
    {
        public readonly Int32 X;

        public readonly Int32 Y;

        public readonly Int32 Z;
    }
}
