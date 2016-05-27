using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    partial class DataObjectRegistryTests
    {
        public class TestDataObject : DataObject
        {
            public TestDataObject(String key, Guid globalID)
                : base(key, globalID)
            { }

            public String Foo { get; protected set; }
            public String Bar { get; protected set; }
        }
    }
}
