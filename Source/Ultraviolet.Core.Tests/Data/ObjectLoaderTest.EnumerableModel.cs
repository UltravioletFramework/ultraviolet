using System;
using System.Collections.Generic;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of enumerables.
    /// </summary>
    public class ObjectLoader_EnumerableModel : DataObject
    {
        public ObjectLoader_EnumerableModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_EnumerableModel(String key, Guid globalID, Boolean createList)
            : base(key, globalID)
        {
            if (createList)
                EnumerableValue = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        public readonly IEnumerable<Int32> EnumerableValue;
    }
}
