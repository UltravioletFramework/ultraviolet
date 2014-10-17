using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model with a single-index indexer used by the object loader tests.
    /// </summary>
    public class ObjectLoader_ListIndexerModel : DataObject
    {
        public ObjectLoader_ListIndexerModel(Guid globalID)
            : base(globalID)
        {

        }

        public List<Int32> this[Int32 ix]
        {
            get { return values[ix]; }
            set { values[ix] = value; }
        }

        private List<Int32>[] values = new List<Int32>[100];
    }
}
