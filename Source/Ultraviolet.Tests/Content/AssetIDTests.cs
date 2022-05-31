using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Content;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Content
{
    [TestFixture]
    public class AssetIDTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        public void AssetID_SerializesToJson()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Resources", "Content", "Manifests", "Test.manifest"));
                    
                    var id = content.Ultraviolet.GetContent().Manifests["Test"]["Textures"]["Triangle"].CreateAssetID();
                    var json = JsonConvert.SerializeObject(id, 
                        UltravioletJsonSerializerSettings.Instance);

                    TheResultingString(json)
                        .ShouldBe(@"""#Test:Textures:Triangle""");
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        public void AssetID_DeserializesFromJson_WithXmlManifest()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Resources", "Content", "Manifests", "Test.manifest"));
                    
                    var id = JsonConvert.DeserializeObject<AssetID>(@"""#Test:Textures:Triangle""", 
                        UltravioletJsonSerializerSettings.Instance);

                    TheResultingValue(id)
                        .ShouldBe(AssetID.Parse("#Test:Textures:Triangle"));
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        public void AssetID_DeserializesFromJson_WithJsonManifest()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Resources", "Content", "Manifests", "TestJson.jsmanifest"));

                    var id = JsonConvert.DeserializeObject<AssetID>(@"""#Test:Textures:Triangle""",
                        UltravioletJsonSerializerSettings.Instance);

                    TheResultingValue(id)
                        .ShouldBe(AssetID.Parse("#Test:Textures:Triangle"));
                })
                .RunForOneFrame();
        }
    }
}
