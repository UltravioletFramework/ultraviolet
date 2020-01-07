using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of arrays.
    /// </summary>
    public class ObjectLoader_ArrayModel : DataObject
    {
        public ObjectLoader_ArrayModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_ArrayModel(String key, Guid globalID, Boolean createArray)
            : base(key, globalID)
        {
            if (createArray)
                ArrayValue = new Int32[50];
        }

        public readonly Int32[] ArrayValue;
    }
}
