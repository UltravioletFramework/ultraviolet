using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Vector4Tests : UltravioletTestFramework
    {
        [Test]
        public void Vector4_ConstructorSetsValues()
        {
            var result = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, 567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Vector4_EqualsTestsCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector3 = new Vector4(345.6f, 901.2f, 567.8f, 123.4f);

            TheResultingValue(vector1.Equals(vector2)).ShouldBe(true);
            TheResultingValue(vector1.Equals(vector3)).ShouldBe(false);
        }

        [Test]
        public void Vector4_OperatorEqualsTestsCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector3 = new Vector4(345.6f, 901.2f, 567.8f, 123.4f);

            TheResultingValue(vector1 == vector2).ShouldBe(true);
            TheResultingValue(vector1 == vector3).ShouldBe(false);
        }

        [Test]
        public void Vector4_OperatorNotEqualsTestsCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector3 = new Vector4(345.6f, 567.8f, 123.4f, 901.2f);

            TheResultingValue(vector1 != vector2).ShouldBe(false);
            TheResultingValue(vector1 != vector3).ShouldBe(true);
        }

        [Test]
        public void Vector4_CalculatesLengthSquaredCorrectly()
        {
            var vectorX = 123.4f;
            var vectorY = 567.8f;
            var vectorZ = 901.2f;
            var vectorW = 345.6f;
            var vector  = new Vector4(vectorX, vectorY, vectorZ, vectorW);

            TheResultingValue(vector.LengthSquared()).WithinDelta(0.1f)
                .ShouldBe((vectorX * vectorX) + (vectorY * vectorY) + (vectorZ * vectorZ) + (vectorW * vectorW));
        }

        [Test]
        public void Vector4_CalculatesLengthCorrectly()
        {
            var vector = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);

            TheResultingValue(vector.Length()).WithinDelta(0.1f)
                .ShouldBe((float)Math.Sqrt((123.4f * 123.4f) + (567.8f * 567.8f) + (901.2f * 901.2f) + (345.6f * 345.6f)));
        }

        [Test]
        public void Vector4_InterpolatesCorrectly()
        {
            var vector1 = new Vector4(25, 25, 25, 25);
            var vector2 = new Vector4(100, 50, 75, 150);

            var result = vector1.Interpolate(vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 50.0f, 87.5f);
        }

        [Test]
        public void Vector4_AddsCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);
            
            var result = Vector4.Add(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Vector4_AddsCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Zero;            
            Vector4.Add(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Vector4_OperatorAddsCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = vector1 + vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Vector4_SubtractsCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Subtract(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Vector4_SubtractsCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Zero;
            Vector4.Subtract(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Vector4_OperatorSubtractsCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = vector1 - vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Vector4_MultipliesCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Multiply(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f, 2400.0f);
        }

        [Test]
        public void Vector4_MultipliesCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Zero;
            Vector4.Multiply(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f, 2400.0f);
        }

        [Test]
        public void Vector4_OperatorMultipliesCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);
            
            var result = vector1 * vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f, 2400.0f);
        }

        [Test]
        public void Vector4_MultipliesByFactorCorrectly()
        {
            var vector = new Vector4(25, 20, 10, 30);
            
            var result = Vector4.Multiply(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Vector4_MultipliesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector4(25, 20, 10, 30);
            
            var result = Vector4.Zero;            
            Vector4.Multiply(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Vector4_OperatorMultipliesByFactorCorrectly()
        {
            var vector = new Vector4(25, 20, 10, 30);

            var result = vector * 1000;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Vector4_DividesCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);
            
            var result = Vector4.Divide(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f, 0.375f);
        }

        [Test]
        public void Vector4_DividesCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Zero;
            Vector4.Divide(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f, 0.375f);
        }

        [Test]
        public void Vector4_OperatorDividesCorrectly()
        {
            var vector1 = new Vector4(25, 20, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = vector1 / vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f, 0.375f);
        }

        [Test]
        public void Vector4_DividesByFactorCorrectly()
        {
            var vector = new Vector4(25, 20, 10, 30);

            var result = Vector4.Divide(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f, 0.030f);
        }

        [Test]
        public void Vector4_DividesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector4(25, 20, 10, 30);
            
            var result = Vector4.Zero;
            Vector4.Divide(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f, 0.030f);
        }

        [Test]
        public void Vector4_OperatorDividesByFactorCorrectly()
        {
            var vector = new Vector4(25, 20, 10, 30);

            var result = vector / 1000f;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f, 0.030f);
        }

        [Test]
        public void Vector4_CalculatesDotProductCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);
            
            var result = Vector4.Dot(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1493584.0f);
        }

        [Test]
        public void Vector4_CalculatesDotProductCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);
            
            var result = 0f;            
            Vector4.Dot(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1493584.0f);
        }

        [Test]
        public void Vector4_ParseHandlesValidInput()
        {
            var result = Vector4.Parse("123.4 -567.8 901.2 345.6");

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Vector4_ParseRejectsInvalidInput()
        {
            Assert.That(() => Vector4.Parse("sfdjhkfsh"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Vector4_TryParseHandlesValidInput()
        {
            var result    = Vector4.Zero;
            var succeeded = Vector4.TryParse("123.4 -567.8 901.2 345.6", out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Vector4_TryParseRejectsInvalidInput()
        {
            var result    = Vector4.Zero;
            var succeeded = Vector4.TryParse("asdfasdfasdfs", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }
        
        [Test]
        public void Vector4_LerpCalculatesCorrectly()
        {
            var vector1 = new Vector4(25, 25, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);
            
            var result = Vector4.Lerp(vector1, vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f, 55.0f);
        }

        [Test]
        public void Vector4_LerpCalculatesCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(25, 25, 10, 30);
            var vector2 = new Vector4(100, 50, 70, 80);

            var result = Vector4.Zero;
            Vector4.Lerp(ref vector1, ref vector2, 0.5f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f, 55.0f);
        }

        [Test]
        public void Vector4_ClampToMin()
        {
            var source = new Vector4(1, 2, 3, 4);
            var min    = new Vector4(10, 20, 30, 40);
            var max    = new Vector4(100, 200, 300, 400);

            var result = Vector4.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y, min.Z, min.W);
        }

        [Test]
        public void Vector4_ClampToMinWithOutParam()
        {
            var source = new Vector4(1, 2, 3, 4);
            var min    = new Vector4(10, 20, 30, 40);
            var max    = new Vector4(100, 200, 300, 400);

            var result = Vector4.Zero;
            Vector4.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y, min.Z, min.W);
        }

        [Test]
        public void Vector4_ClampToMax()
        {
            var source = new Vector4(1000, 2000, 3000, 4000);
            var min    = new Vector4(10, 20, 30, 40);
            var max    = new Vector4(100, 200, 300, 400);

            var result = Vector4.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y, max.Z, max.W);
        }

        [Test]
        public void Vector4_ClampToMaxWithOutParam()
        {
            var source = new Vector4(1000, 2000, 3000, 4000);
            var min    = new Vector4(10, 20, 30, 40);
            var max    = new Vector4(100, 200, 300, 400);

            var result = Vector4.Zero;
            Vector4.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y, max.Z, max.W);
       }

        [Test]
        public void Vector4_MinCalculatedCorrectly()
        {
            var vector1 = new Vector4(1, 200, 3, 400);
            var vector2 = new Vector4(300, 2, 400, 4);

            var result     = Vector4.Min(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f, 3.0f, 4.0f);
        }

        [Test]
        public void Vector4_MinCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(1, 200, 3, 400);
            var vector2 = new Vector4(300, 2, 400, 4);
            
            var result = Vector4.Zero;            
            Vector4.Min(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f, 3.0f, 4.0f);
        }

        [Test]
        public void Vector4_MaxCalculatedCorrectly()
        {
            var vector1 = new Vector4(1, 200, 3, 400);
            var vector2 = new Vector4(300, 2, 400, 4);

            var result = Vector4.Max(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f, 400.0f, 400.0f);
        }

        [Test]
        public void Vector4_MaxCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(1, 200, 3, 400);
            var vector2 = new Vector4(300, 2, 400, 4);
            
            var result = Vector4.Zero;            
            Vector4.Max(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f, 400.0f, 400.0f);
        }

        [Test]
        public void Vector4_NegateCalculatedCorrectly()
        {
            var vector = new Vector4(123.4f, -567.8f, 901.2f, -345.6f);
            
            var result = Vector4.Negate(vector);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Vector4_NegateCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector4(123.4f, -567.8f, 901.2f, -345.6f);
            
            var result = Vector4.Zero;            
            Vector4.Negate(ref vector, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Vector4_OperatorNegateCalculateCorrectly()
        {
            var vector = new Vector4(123.4f, -567.8f, 901.2f, -345.6f);
            
            var result = -vector;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Vector4_NormalizeCalculatedCorrectly()
        {
            var vector = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);

            var result = Vector4.Normalize(vector);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.1095f, 0.5039f, 0.7999f, 0.3067f);
        }

        [Test]
        public void Vector4_NormalizeCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            
            var result = Vector4.Zero;
            Vector4.Normalize(ref vector, out result);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1095f, 0.5039f, 0.7999f, 0.3067f);
        }

        [Test]
        public void Vector4_DistanceCalculatedCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);
            
            var result = Vector4.Distance(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(410.635f);
        }

        [Test]
        public void Vector4_DistanceCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);

            Single result;
            Vector4.Distance(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(410.635f);
        }

        [Test]
        public void Vector4_DistanceSquaredCalculatedCorrectly()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);
            
            var result = Vector4.DistanceSquared(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(168620.8f);
        }

        [Test]
        public void Vector4_DistanceSquaredCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var vector2 = new Vector4(345.6f, 901.2f, 876.5f, 432.1f);

            Single result;
            Vector4.DistanceSquared(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(168620.8f);
        }

        [Test]
        public void Vector4_ReflectCalculatedCorrectly()
        {
            var vector = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var normal = new Vector4(0, 1, 0, 0);

            var result = Vector4.Reflect(vector, normal);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Vector4_ReflectCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector4(123.4f, 567.8f, 901.2f, 345.6f);
            var normal = new Vector4(0, 1, 0, 0);

            var result = Vector4.Zero;            
            Vector4.Reflect(ref vector, ref normal, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Vector4_BarycentricCalculatedCorrectly()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var vector3 = new Vector4(456, 789, 300, 333);

            var result = Vector4.Barycentric(vector1, vector2, vector3, 0.75f, 0.22f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f, 219.0f, 243.09f);
        }

        [Test]
        public void Vector4_BarycentricCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var vector3 = new Vector4(456, 789, 300, 333);
            
            var result = Vector4.Zero;            
            Vector4.Barycentric(ref vector1, ref vector2, ref vector3, 0.75f, 0.22f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f, 219.0f, 243.09f);
        }

        [Test]
        public void Vector4_CatmullRomCalculatedCorrectly()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var vector3 = new Vector4(456, 789, 300, 333);
            var vector4 = new Vector4(-456, 986, 400, 444);
            
            var result = Vector4.CatmullRom(vector1, vector2, vector3, vector4, 0.55f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f, 255.0f, 283.05f);
        }

        [Test]
        public void Vector4_CatmullRomCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var vector3 = new Vector4(456, 789, 300, 333);
            var vector4 = new Vector4(-456, 986, 400, 444);

            var result = Vector4.Zero;            
            Vector4.CatmullRom(ref vector1, ref vector2, ref vector3, ref vector4, 0.55f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f, 255.0f, 283.05f);
        }

        [Test]
        public void Vector4_HermiteCalculatedCorrectly()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var tangent1 = new Vector4(1, 0, 0, 0);
            var tangent2 = new Vector4(0, 1, 0, 0);

            var result = Vector4.Hermite(vector1, tangent1, vector2, tangent2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f, 173.1f, 192.23f);
        }

        [Test]
        public void Vector4_HermiteCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(234, -345, 200, 222);
            var tangent1 = new Vector4(1, 0, 0, 0);
            var tangent2 = new Vector4(0, 1, 0, 0);
            
            var result = Vector4.Zero;            
            Vector4.Hermite(ref vector1, ref tangent1, ref vector2, ref tangent2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f, 173.1f, 192.23f);
        }

        [Test]
        public void Vector4_SmoothStepCalculatedCorrectly()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(-456, 986, 200, 222);

            var result = Vector4.SmoothStep(vector1, vector2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f, 173.1f, 192.23f);
        }

        [Test]
        public void Vector4_SmoothStepCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector4(-123, -234, 100, 111);
            var vector2 = new Vector4(-456, 986, 200, 222);

            var result = Vector4.Zero;
            Vector4.SmoothStep(ref vector1, ref vector2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f, 173.1f, 192.23f);
        }

        [Test]
        public void Vector4_TransformTranslate()
        {
            var vector1 = new Vector4(123, 456, 789, 321);
            var transform = Matrix.CreateTranslation(100, 200, 300);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(32223.0f, 64656.0f, 97089.0f, 321.0f);
        }

        [Test]
        public void Vector4_TransformTranslateWithOutParam()
        {
            var vector1 = new Vector4(123, 456, 789, 321);
            var transform = Matrix.CreateTranslation(100, 200, 300);

            var result = Vector4.Zero;
            Vector4.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(32223.0f, 64656.0f, 97089.0f, 321.0f);
        }

        [Test]
        public void Vector4_TransformsVector2Correctly_WithMatrix()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 0f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector2Correctly_WithMatrix_WithOutParam()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            Vector4.Transform(ref vector1, ref transform, out Vector4 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 0f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector3Correctly_WithMatrix()
        {
            var vector1 = new Vector3(123, 456, 789);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector3Correctly_WithMatrix_WithOutParam()
        {
            var vector1 = new Vector3(123, 456, 789);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            Vector4.Transform(ref vector1, ref transform, out Vector4 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector4Correctly_WithMatrix()
        {
            var vector1 = new Vector4(123, 456, 768, 321);
            var transform = Matrix.CreateRotationZ((float)Math.PI);
            
            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768.0f, 321.0f);
        }

        [Test]
        public void Vector4_TransformsVector4Correctly_WithMatrix_WithOutParam()
        {
            var vector1 = new Vector4(123, 456, 768, 321);
            var transform = Matrix.CreateRotationZ((float)Math.PI);
            
            var result = Vector4.Zero;            
            Vector4.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768.0f, 321.0f);
        }

        [Test]
        public void Vector4_TransformsVector2Correctly_WithQuaternion()
        {
            var vector1 = new Vector2(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 0f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector2Correctly_WithQuaternion_WithOutParam()
        {
            var vector1 = new Vector2(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Vector4.Transform(ref vector1, ref transform, out Vector4 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 0f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector3Correctly_WithQuaternion()
        {
            var vector1 = new Vector3(123, 456, 789);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector3Correctly_WithQuaternion_WithOutParam()
        {
            var vector1 = new Vector3(123, 456, 789);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Vector4.Transform(ref vector1, ref transform, out Vector4 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789f, 1f);
        }

        [Test]
        public void Vector4_TransformsVector4Correctly_WithQuaternion()
        {
            var vector1 = new Vector4(123, 456, 768, 321);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Vector4.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768f, 321f);
        }

        [Test]
        public void Vector4_TransformsVector4Correctly_WithQuaternion_WithOutParam()
        {
            var vector1 = new Vector4(123, 456, 768, 321);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Vector4.Transform(ref vector1, ref transform, out Vector4 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768f, 321f);
        }

        [Test]
        public void Vector4_SerializesToJson()
        {
            var vector = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject(vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4,""w"":4.5}");
        }

        [Test]
        public void Vector4_SerializesToJson_WhenNullable()
        {
            var vector = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject((Vector4?)vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4,""w"":4.5}");
        }

        [Test]
        public void Vector4_DeserializesFromJson()
        {
            const String json = @"{ ""x"": 1.2, ""y"": 2.3, ""z"": 3.4, ""w"": 4.5 }";
            
            var vector = JsonConvert.DeserializeObject<Vector4>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector)
                .ShouldBe(1.2f, 2.3f, 3.4f, 4.5f);
        }

        [Test]
        public void Vector4_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""x"": 1.2, ""y"": 2.3, ""z"": 3.4, ""w"": 4.5 }";

            var vector1 = JsonConvert.DeserializeObject<Vector4?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector1.Value)
                .ShouldBe(1.2f, 2.3f, 3.4f, 4.5f);

            const String json2 = @"null";

            var vector2 = JsonConvert.DeserializeObject<Vector4?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Vector4_ConvertsFromSystemNumericsCorrectly()
        {
            var vector1 = new Vector4(1.2f, 3.4f, 5.6f, 7.8f);
            System.Numerics.Vector4 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
            TheResultingValue(vector1.Z).ShouldBe(vector2.Z);
            TheResultingValue(vector1.W).ShouldBe(vector2.W);
        }

        [Test]
        public void Vector3_ConvertsToSystemNumericsCorrectly()
        {
            var vector1 = new System.Numerics.Vector4(1.2f, 3.4f, 5.6f, 7.8f);
            Vector4 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
            TheResultingValue(vector1.Z).ShouldBe(vector2.Z);
            TheResultingValue(vector1.W).ShouldBe(vector2.W);
        }
    }
}
