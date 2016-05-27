using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.Data
{
    [TestFixture]
    public partial class DataObjectRegistryTests : NucleusTestFramework
    {
        [Test]
        public void ResolvedDataObjectReference_SerializesToJson()
        {
            try
            {
                DataObjectRegistries.Reset();
                DataObjectRegistries.Register(GetType().Assembly);

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Data", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = DataObjectRegistries.ResolveReference("@test:TEST_OBJECT_1");
                var converter = new NucleusJsonConverter();
                var json = JsonConvert.SerializeObject(reference, converter);

                TheResultingString(json)
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

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Data", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                var reference = new ResolvedDataObjectReference(Guid.Parse("32758c5b-0a91-4c25-a092-c4e65754346d"));
                var converter = new NucleusJsonConverter();
                var json = JsonConvert.SerializeObject(reference, converter);

                TheResultingString(json)
                    .ShouldBe(@"""32758c5b-0a91-4c25-a092-c4e65754346d""");
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

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Data", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json = @"""@test:TEST_OBJECT_1""";

                var converter = new NucleusJsonConverter();
                var reference = JsonConvert.DeserializeObject<ResolvedDataObjectReference>(json, converter);

                TheResultingValue(reference)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("32758c5b-0a91-4c25-a092-c4e65754346d")));
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

                DataObjectRegistries.Get<TestDataObject>().SetSourceFiles(new[] { Path.Combine("Data", "DataObjectRegistryData.xml") });
                DataObjectRegistries.Load();

                const String json = @"""32758c5b-0a91-4c25-a092-c4e65754346d""";

                var converter = new NucleusJsonConverter();
                var reference = JsonConvert.DeserializeObject<ResolvedDataObjectReference>(json, converter);

                TheResultingValue(reference)
                    .ShouldSatisfyTheCondition(x => x.Value.Equals(Guid.Parse("32758c5b-0a91-4c25-a092-c4e65754346d")));
            }
            finally
            {
                DataObjectRegistries.Reset();
            }
        }
    }
}
