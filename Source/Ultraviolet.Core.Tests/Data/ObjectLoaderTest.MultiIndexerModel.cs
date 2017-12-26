using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a model with a multiple-index indexer used by the object loader tests.
    /// </summary>
    public class ObjectLoader_MultiIndexerModel : DataObject
    {
        public ObjectLoader_MultiIndexerModel(String key, Guid globalID)
            : base(key, globalID)
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
