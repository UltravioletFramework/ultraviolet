using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test failure states for constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModelInvalidMatch : DataObject
    {
        public ObjectLoader_CtorArgModelInvalidMatch(Int32 x, Int32 y, Int32 z)
            : base(String.Empty, Guid.Empty)
        {

        }
    }
}
