using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model with a single-index indexer used by the object loader tests.
    /// </summary>
    public class ObjectLoader_ComplexIndexerModel : DataObject
    {
        public ObjectLoader_ComplexIndexerModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_ComplexRefObject this[Int32 ix]
        {
            get { return values[ix]; }
            set { values[ix] = value; }
        }

        private ObjectLoader_ComplexRefObject[] values = new ObjectLoader_ComplexRefObject[100];
    }
}
