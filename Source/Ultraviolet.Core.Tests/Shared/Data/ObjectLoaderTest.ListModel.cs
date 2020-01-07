using System;
using System.Collections.Generic;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of lists.
    /// </summary>
    public class ObjectLoader_ListModel : DataObject
    {
        public ObjectLoader_ListModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_ListModel(String key, Guid globalID, Boolean createList)
            : base(key, globalID)
        {
            if (createList)
                ListValue = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        public readonly List<Int32> ListValue;
    }
}
