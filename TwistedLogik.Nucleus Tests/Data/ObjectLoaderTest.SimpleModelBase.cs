using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a simple model used by the object loader tests.
    /// </summary>
    public abstract class ObjectLoader_SimpleModelBase : DataObject
    {
        public ObjectLoader_SimpleModelBase(String key, Guid globalID)
            : base(key, 
            globalID)
        {

        }

        public readonly String StringValueOnBaseClass;
    }
}
