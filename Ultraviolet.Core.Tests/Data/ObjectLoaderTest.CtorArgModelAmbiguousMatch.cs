using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test failure states for constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModelAmbiguousMatch : DataObject
    {
        public ObjectLoader_CtorArgModelAmbiguousMatch(String key, Guid globalID)
            : base(key, globalID)
        {

        }
    }
}
