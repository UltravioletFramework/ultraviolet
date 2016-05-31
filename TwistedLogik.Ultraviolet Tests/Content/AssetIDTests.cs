using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Content
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
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Content", "Manifests", "Test.manifest"));
                    
                    var id = content.Ultraviolet.GetContent().Manifests["Test"]["Textures"]["Triangle"].CreateAssetID();
                    var json = JsonConvert.SerializeObject(id);

                    TheResultingString(json)
                        .ShouldBe(@"""#Test:Textures:Triangle""");
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        public void AssetID_DeserializesFromJson()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Content", "Manifests", "Test.manifest"));
                    
                    var id = JsonConvert.DeserializeObject<AssetID>(@"""#Test:Textures:Triangle""");

                    TheResultingValue(id)
                        .ShouldBe(AssetID.Parse("#Test:Textures:Triangle"));
                })
                .RunForOneFrame();
        }
    }
}
