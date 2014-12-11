using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
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
