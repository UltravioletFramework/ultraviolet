using System;
using Ultraviolet.Core.Data;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Tests.Data
{
    partial class DataObjectRegistryTests
    {
        public class TestDataObject : DataObject
        {
            [JsonConstructor]
            public TestDataObject(String key, Guid id)
                : base(key, id)
            { }

            [JsonProperty(PropertyName = "foo")]
            public String Foo { get; protected set; }
            [JsonProperty(PropertyName = "bar")]
            public String Bar { get; protected set; }
        }
    }
}
