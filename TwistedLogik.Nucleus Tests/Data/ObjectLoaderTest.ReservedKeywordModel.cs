using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test the population of properties with the same
    /// names as the loader's reserved keywords.
    /// </summary>
    public class ObjectLoader_ReservedKeywordModel : DataObject
    {
        public ObjectLoader_ReservedKeywordModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_ReservedKeywordModel(String key, Guid globalID, String setByConstructor)
            : this(key, globalID)
        {
            this.SetByConstructor = setByConstructor;
        }

        public String SetByConstructor
        {
            get;
            private set;
        }

        public String Class
        {
            get;
            set;
        }

        public String ID
        {
            get;
            set;
        }

        public String Type
        {
            get;
            set;
        }
    }
}
