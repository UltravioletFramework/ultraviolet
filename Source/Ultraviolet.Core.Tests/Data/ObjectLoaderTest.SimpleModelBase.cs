using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
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
