using NUnit.Framework;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests
{
    [TestFixture]
    [Category("Content")]
    public class CursorCollectionTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        public void CursorCollection_LoadsCorrectly_FromXml()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var cursors = content.Load<CursorCollection>("Cursors/Cursors.xml");

                    TheResultingObject(cursors["Normal"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 0);

                    TheResultingObject(cursors["NormalDisabled"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 0);

                    TheResultingObject(cursors["IBeam"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 14);

                    TheResultingObject(cursors["IBeamDisabled"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 14);

                    TheResultingObject(cursors["NotACursor"])
                        .ShouldBeNull();
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        public void CursorCollection_LoadsCorrectly_FromJson()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var cursors = content.Load<CursorCollection>("Cursors/CursorsJson.json");

                    TheResultingObject(cursors["Normal"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 0);

                    TheResultingObject(cursors["NormalDisabled"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 0);

                    TheResultingObject(cursors["IBeam"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 14);

                    TheResultingObject(cursors["IBeamDisabled"])
                        .ShouldNotBeNull()
                        .ShouldSatisfyTheCondition(x => x.Width == 32)
                        .ShouldSatisfyTheCondition(x => x.Height == 32)
                        .ShouldSatisfyTheCondition(x => x.HotspotX == 0)
                        .ShouldSatisfyTheCondition(x => x.HotspotY == 14);

                    TheResultingObject(cursors["NotACursor"])
                        .ShouldBeNull();
                })
                .RunForOneFrame();
        }
    }
}
