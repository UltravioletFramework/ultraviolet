using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test failure states for constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModelNoMatch : DataObject
    {
        public ObjectLoader_CtorArgModelNoMatch(Guid globalID)
            : base(globalID)
        {

        }
    }
}
