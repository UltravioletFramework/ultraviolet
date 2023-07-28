using System;
using System.Collections.Generic;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of lists of complex objects.
    /// </summary>
    public class ObjectLoader_ListComplexModel : DataObject
    {
        public ObjectLoader_ListComplexModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public readonly List<ObjectLoader_ComplexRefObject> ListValue;
    }
}
