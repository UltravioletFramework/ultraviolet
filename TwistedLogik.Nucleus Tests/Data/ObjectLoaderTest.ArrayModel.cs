using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of arrays.
    /// </summary>
    public class ObjectLoader_ArrayModel : DataObject
    {
        public ObjectLoader_ArrayModel(Guid globalID)
            : base(globalID)
        {

        }

        public ObjectLoader_ArrayModel(Guid globalID, Boolean createArray)
            : base(globalID)
        {
            if (createArray)
                ArrayValue = new Int32[50];
        }

        public readonly Int32[] ArrayValue;
    }
}
