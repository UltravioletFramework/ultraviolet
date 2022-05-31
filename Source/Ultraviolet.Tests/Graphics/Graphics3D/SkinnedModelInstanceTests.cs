using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using NUnit.Framework;
using Ultraviolet.Graphics.Graphics3D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics3D
{
    [TestFixture]
    public class SkinnedModelInstanceTests : UltravioletApplicationTestFramework
    {
        private Dictionary<Double, Matrix[]> LoadBoneData(String name)
        {
            using (var stream = File.OpenRead(Path.Combine("Resources", "Expected", "Graphics", "Graphics3D", name)))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var records = csv.GetRecords<SkinnedModelBoneData>().ToArray();
                var result = records.GroupBy(x => x.Time).ToDictionary(k => k.Key, v =>
                {
                    var bones = new Matrix[v.Count()];

                    foreach (var record in v)
                    {
                        bones[record.Bone] = new Matrix(
                            record.M11, record.M12, record.M13, record.M14,
                            record.M21, record.M22, record.M23, record.M24,
                            record.M31, record.M32, record.M33, record.M34,
                            record.M41, record.M42, record.M43, record.M44);
                    }

                    return bones;
                });

                return result;
            }
        }

        [Test]
        [TestCase("Models/SimpleSkin", "BoneMatrices_SimpleSkin.csv")]
        [TestCase("Models/CesiumMan", "BoneMatrices_CesiumMan.csv")]
        [Category("Content")]
        public void SkinnedModelInstance_CalculatesCorrectBoneMatrices(String modelAssetFile, String modelBoneDataFile)
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    const Double TimeDelta = 0.25;

                    var expectedByTime = LoadBoneData(modelBoneDataFile);

                    var model = content.Load<SkinnedModel>(modelAssetFile);
                    var modelInstance = new SkinnedModelInstance(model);

                    var animationTrack = modelInstance.PlayAnimation(SkinnedAnimationMode.Manual, 0);
                    modelInstance.UpdateAnimationState();

                    var animationDuration = (Int32)Math.Ceiling(model.Animations[0].Duration);
                    for (var t = 0.0; t < animationDuration; t += TimeDelta)
                    {
                        var skin = animationTrack.Model.Skins[0];
                        var boneCount = skin.BoneCount;
                        var boneTransforms = skin.GetBoneTransforms();
                        var boneTransformsExpected = expectedByTime[t];

                        TheResultingValue(boneTransformsExpected.Length)
                            .ShouldBe(boneTransforms.Length);
    
                        for (var i = 0; i < boneTransforms.Length; i++)
                        {
                            TheResultingValue(boneTransformsExpected[i])
                                .WithinDelta(0.0001f).ShouldBe(boneTransforms[i]);
                        }

                        modelInstance.AdvanceTime(TimeDelta);
                    }
                })
                .RunForOneFrame();
        }
    }
}
