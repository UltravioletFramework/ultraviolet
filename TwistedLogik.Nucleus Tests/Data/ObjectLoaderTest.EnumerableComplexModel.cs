using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to validate loading of enumerables of complex objects.
    /// </summary>
    public class ObjectLoader_EnumerableComplexModel : DataObject
    {
        public ObjectLoader_EnumerableComplexModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public readonly IEnumerable<ObjectLoader_ComplexRefObject> EnumerableValue;
    }
}
