using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.Data
{
    [TestFixture]
    public partial class DataObjectRegistryTests : CoreTestFramework
    {
        [Test]
        public void DataObjectRegistry_LoadsFromXml()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var dobj1 = DataObjectRegistries.Get<TestDataObject>().GetObjectByKey("TEST_OBJECT_1");
                TheResultingObject(dobj1)
                    .ShouldSatisfyTheCondition(x => x.Key == "TEST_OBJECT_1")
                    .ShouldSatisfyTheCondition(x => x.GlobalID == new Guid("3bd956ab-24cc-49e7-a178-99111c69d24f"))
                    .ShouldSatisfyTheCondition(x => x.Foo == "Hello")
                    .ShouldSatisfyTheCondition(x => x.Bar == "World");

                var dobj2 = DataObjectRegistries.Get<TestDataObject>().GetObjectByKey("TEST_OBJECT_2");
                TheResultingObject(dobj2)
                    .ShouldSatisfyTheCondition(x => x.Key == "TEST_OBJECT_2")
                    .ShouldSatisfyTheCondition(x => x.GlobalID == new Guid("285c472e-a184-49a6-9639-d5b127ebc74a"))
                    .ShouldSatisfyTheCondition(x => x.Foo == "Goodbye")
                    .ShouldSatisfyTheCondition(x => x.Bar == "Universe");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void DataObjectRegistry_LoadsFromJson()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.json") });
                DataObjectRegistries.Load();

                var dobj1 = DataObjectRegistries.Get<TestDataObject>().GetObjectByKey("TEST_OBJECT_1");
                TheResultingObject(dobj1)
                    .ShouldSatisfyTheCondition(x => x.Key == "TEST_OBJECT_1")
                    .ShouldSatisfyTheCondition(x => x.GlobalID == new Guid("3bd956ab-24cc-49e7-a178-99111c69d24f"))
                    .ShouldSatisfyTheCondition(x => x.Foo == "Hello")
                    .ShouldSatisfyTheCondition(x => x.Bar == "World");

                var dobj2 = DataObjectRegistries.Get<TestDataObject>().GetObjectByKey("TEST_OBJECT_2");
                TheResultingObject(dobj2)
                    .ShouldSatisfyTheCondition(x => x.Key == "TEST_OBJECT_2")
                    .ShouldSatisfyTheCondition(x => x.GlobalID == new Guid("285c472e-a184-49a6-9639-d5b127ebc74a"))
                    .ShouldSatisfyTheCondition(x => x.Foo == "Goodbye")
                    .ShouldSatisfyTheCondition(x => x.Bar == "Universe");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_SerializesToJson()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = DataObjectRegistries.ResolveReference("@test:TEST_OBJECT_1");
                var json = JsonConvert.SerializeObject(reference,
                    CoreJsonSerializerSettings.Instance);

                TheResultingString(json)
                    .ShouldBe(@"""@test:TEST_OBJECT_1""");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_SerializesToJson_WhenNullable()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = DataObjectRegistries.ResolveReference("@test:TEST_OBJECT_1");
                var json1 = JsonConvert.SerializeObject((ResolvedDataObjectReference?)reference,
                    CoreJsonSerializerSettings.Instance);

                TheResultingString(json1)
                    .ShouldBe(@"""@test:TEST_OBJECT_1""");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_SerializesToJson_Guid()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = new ResolvedDataObjectReference(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f"));
                var json = JsonConvert.SerializeObject(reference,
                    CoreJsonSerializerSettings.Instance);

                TheResultingString(json)
                    .ShouldBe(@"""3bd956ab-24cc-49e7-a178-99111c69d24f""");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_SerializesToJson_Guid_WhenNullable()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = new ResolvedDataObjectReference(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f"));
                var json = JsonConvert.SerializeObject((ResolvedDataObjectReference?)reference,
                    CoreJsonSerializerSettings.Instance);

                TheResultingString(json)
                    .ShouldBe(@"""3bd956ab-24cc-49e7-a178-99111c69d24f""");
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_DeserializesFromJson()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json = @"""@test:TEST_OBJECT_1""";
 
                var reference = JsonConvert.DeserializeObject<ResolvedDataObjectReference>(json, 
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f")));
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_DeserializesFromJson_WhenNullable()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json1 = @"""@test:TEST_OBJECT_1""";
                const String json2 = @"null";

                var reference1 = JsonConvert.DeserializeObject<ResolvedDataObjectReference?>(json1,
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference1.Value)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f")));

                var reference2 = JsonConvert.DeserializeObject<ResolvedDataObjectReference?>(json2, 
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference2.HasValue)
                    .ShouldBe(false);
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_DeserializesFromJson_Guid()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json = @"""3bd956ab-24cc-49e7-a178-99111c69d24f""";
                
                var reference = JsonConvert.DeserializeObject<ResolvedDataObjectReference>(json, 
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f")));
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }

        [Test]
        public void ResolvedDataObjectReference_DeserializesFromJson_Guid_WhenNullable()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Resources", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json1 = @"""3bd956ab-24cc-49e7-a178-99111c69d24f""";
                const String json2 = @"null";

                var reference1 = JsonConvert.DeserializeObject<ResolvedDataObjectReference?>(json1, 
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference1.Value)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("3bd956ab-24cc-49e7-a178-99111c69d24f")));

                var reference2 = JsonConvert.DeserializeObject<ResolvedDataObjectReference?>(json2, 
                    CoreJsonSerializerSettings.Instance);

                TheResultingValue(reference2.HasValue)
                    .ShouldBe(false);
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }
    }
}
