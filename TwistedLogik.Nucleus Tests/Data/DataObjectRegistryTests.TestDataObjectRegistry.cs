using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    partial class DataObjectRegistryTests
    {
        public class TestDataObjectRegistry : DataObjectRegistry<TestDataObject>
        {
            public override String DataElementName => "TestDataObject";
            public override String ReferenceResolutionName => "test";
        }
    }
}
