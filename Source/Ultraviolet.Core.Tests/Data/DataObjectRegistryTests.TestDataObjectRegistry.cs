using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.Tests.Data
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
