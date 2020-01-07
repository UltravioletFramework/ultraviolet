using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model with a single-index indexer used by the object loader tests.
    /// </summary>
    public class ObjectLoader_ArrayIndexerModel : DataObject
    {
        public ObjectLoader_ArrayIndexerModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public Int32[] this[Int32 ix]
        {
            get { return values[ix]; }
            set { values[ix] = value; }
        }

        private Int32[][] values = new Int32[100][];
    }
}
