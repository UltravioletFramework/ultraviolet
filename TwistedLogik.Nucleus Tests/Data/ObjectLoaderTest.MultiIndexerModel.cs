using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model with a multiple-index indexer used by the object loader tests.
    /// </summary>
    public class ObjectLoader_MultiIndexerModel : DataObject
    {
        public ObjectLoader_MultiIndexerModel(Guid globalID)
            : base(globalID)
        {

        }

        public Int32 this[Int32 x, Int32 y]
        {
            get { return values[y * 10 + x]; }
            set { values[y * 10 + x] = value; }
        }

        private Int32[] values = new Int32[100];
    }
}
