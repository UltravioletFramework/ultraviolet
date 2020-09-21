using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class CurveTests : UltravioletTestFramework
    {
        private static String GetResourcePath(String filename) => Path.Combine("Resources", "Expected", filename);

        private static void AssertCurveMatchesSavedSamples(Curve<Single> curve, String filename)
        {
            var expectedSamples = CurveTestUtils.ReadSamplesFromCsv<Single>(GetResourcePath(filename));
            foreach (var expectedSample in expectedSamples)
            {
                var actualSampleValue = curve.Evaluate(expectedSample.Position, default);
                Assert.AreEqual(expectedSample.Value, actualSampleValue, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
            }
        }

        private static void AssertCurveMatchesSavedSamples(Curve<Vector3> curve, String filename)
        {
            var expectedSamples = CurveTestUtils.ReadSamplesFromCsv<Vector3>(GetResourcePath(filename));
            foreach (var expectedSample in expectedSamples)
            {
                var expectedSampleValue = Vector3.Normalize(expectedSample.Value);
                var actualSampleValue = Vector3.Normalize(curve.Evaluate(expectedSample.Position, default));
                Assert.AreEqual(expectedSampleValue.X, actualSampleValue.X, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
                Assert.AreEqual(expectedSampleValue.Y, actualSampleValue.Y, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
                Assert.AreEqual(expectedSampleValue.Z, actualSampleValue.Z, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
            }
        }

        private static void AssertCurveMatchesSavedSamples(Curve<Quaternion> curve, String filename)
        {
            var expectedSamples = CurveTestUtils.ReadSamplesFromCsv<Quaternion>(GetResourcePath(filename));
            foreach (var expectedSample in expectedSamples)
            {
                var expectedSampleValue = Quaternion.Normalize(expectedSample.Value);
                var actualSampleValue = Quaternion.Normalize(curve.Evaluate(expectedSample.Position, default));
                Assert.AreEqual(expectedSampleValue.X, actualSampleValue.X, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
                Assert.AreEqual(expectedSampleValue.Y, actualSampleValue.Y, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
                Assert.AreEqual(expectedSampleValue.Z, actualSampleValue.Z, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
                Assert.AreEqual(expectedSampleValue.W, actualSampleValue.W, 0.0001f, $"Sample mismatch at curve position {expectedSample.Position}.");
            }
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsSingle_AndContinuityIsStep()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("SingleCurve_Step_keys.csv"), SingleCurveStepSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "SingleCurve_Step_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsSingle_AndContinuityIsLinear()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("SingleCurve_Linear_keys.csv"), SingleCurveLinearSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "SingleCurve_Linear_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsSingle_AndContinuityIsCubicSpline()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("SingleCurve_CubicSpline_keys.csv"), SingleCurveCubicSplineSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "SingleCurve_CubicSpline_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsVector3_AndContinuityIsStep()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("Vector3Curve_Step_keys.csv"), Vector3CurveStepSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "Vector3Curve_Step_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsVector3_AndContinuityIsLinear()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("Vector3Curve_Linear_keys.csv"), Vector3CurveLinearSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "Vector3Curve_Linear_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsVector3_AndContinuityIsCubicSpline()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("Vector3Curve_CubicSpline_keys.csv"), Vector3CurveCubicSplineSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "Vector3Curve_CubicSpline_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsQuaternion_AndContinuityIsStep()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("QuaternionCurve_Step_keys.csv"), QuaternionCurveStepSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "QuaternionCurve_Step_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsQuaternion_AndContinuityIsLinear()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("QuaternionCurve_Linear_keys.csv"), QuaternionCurveLinearSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "QuaternionCurve_Linear_samples.csv");
        }

        [Test]
        public void Curve_MatchesExpectedSampleValues_WhenTypeIsQuaternion_AndContinuityIsCubicSpline()
        {
            var curve = CurveTestUtils.ReadCurveFromCsv(GetResourcePath("QuaternionCurve_CubicSpline_keys.csv"), QuaternionCurveCubicSplineSampler.Instance);
            AssertCurveMatchesSavedSamples(curve, "QuaternionCurve_CubicSpline_samples.csv");
        }
    }
}
