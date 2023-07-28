using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Vector3Tests : UltravioletTestFramework
    {
        [Test]
        public void Vector3_ConstructorSetsValues()
        {
            var result = new Vector3(123.4f, 567.8f, 901.2f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, 567.8f, 901.2f);
        }

        [Test]
        public void Vector3_EqualsTestsCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector3 = new Vector3(901.2f, 567.8f, 123.4f);

            TheResultingValue(vector1.Equals(vector2)).ShouldBe(true);
            TheResultingValue(vector1.Equals(vector3)).ShouldBe(false);
        }

        [Test]
        public void Vector3_OperatorEqualsTestsCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector3 = new Vector3(901.2f, 567.8f, 123.4f);

            TheResultingValue(vector1 == vector2).ShouldBe(true);
            TheResultingValue(vector1 == vector3).ShouldBe(false);
        }

        [Test]
        public void Vector3_OperatorNotEqualsTestsCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector3 = new Vector3(567.8f, 123.4f, 901.2f);

            TheResultingValue(vector1 != vector2).ShouldBe(false);
            TheResultingValue(vector1 != vector3).ShouldBe(true);
        }

        [Test]
        public void Vector3_CalculatesLengthSquaredCorrectly()
        {
            var vectorX = 123.4f;
            var vectorY = 567.8f;
            var vectorZ = 901.2f;
            var vector  = new Vector3(vectorX, vectorY, vectorZ);
            
            TheResultingValue(vector.LengthSquared()).WithinDelta(0.1f)
                .ShouldBe((vectorX * vectorX) + (vectorY * vectorY) + (vectorZ * vectorZ));
        }

        [Test]
        public void Vector3_CalculatesLengthCorrectly()
        {
            var vector = new Vector3(123.4f, 567.8f, 901.2f);

            TheResultingValue(vector.Length()).WithinDelta(0.1f)
                .ShouldBe((float)Math.Sqrt((123.4f * 123.4f) + (567.8f * 567.8f) + (901.2f * 901.2f)));
        }

        [Test]
        public void Vector3_InterpolatesCorrectly()
        {
            var vector1 = new Vector3(25, 25, 25);
            var vector2 = new Vector3(100, 50, 75);
            
            var result = vector1.Interpolate(vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 50.0f);
        }

        [Test]
        public void Vector3_AddsCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Add(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f);
        }

        [Test]
        public void Vector3_AddsCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);
            
            var result = Vector3.Zero;            
            Vector3.Add(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f);
        }

        [Test]
        public void Vector3_OperatorAddsCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = vector1 + vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f);
        }

        [Test]
        public void Vector3_SubtractsCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Subtract(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f);
        }

        [Test]
        public void Vector3_SubtractsCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Zero;
            Vector3.Subtract(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f);
        }

        [Test]
        public void Vector3_OperatorSubtractsCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = vector1 - vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f);
        }

        [Test]
        public void Vector3_MultipliesCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Multiply(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f);
        }

        [Test]
        public void Vector3_MultipliesCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Zero;            
            Vector3.Multiply(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f);
        }

        [Test]
        public void Vector3_OperatorMultipliesCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = vector1 * vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f, 700.0f);
        }

        [Test]
        public void Vector3_MultipliesByFactorCorrectly()
        {
            var vector = new Vector3(25, 20, 10);
            
            var result = Vector3.Multiply(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f);
        }

        [Test]
        public void Vector3_MultipliesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector3(25, 20, 10);

            var result = Vector3.Zero;            
            Vector3.Multiply(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f);
        }

        [Test]
        public void Vector3_OperatorMultipliesByFactorCorrectly()
        {
            var vector = new Vector3(25, 20, 10);

            var result = vector * 1000;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f);
        }

        [Test]
        public void Vector3_DividesCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Divide(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f);
        }

        [Test]
        public void Vector3_DividesCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Zero;
            Vector3.Divide(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f);
        }

        [Test]
        public void Vector3_OperatorDividesCorrectly()
        {
            var vector1 = new Vector3(25, 20, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = vector1 / vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f, 0.14f);
        }

        [Test]
        public void Vector3_DividesByFactorCorrectly()
        {
            var vector = new Vector3(25, 20, 10);
            
            var result = Vector3.Divide(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f);
        }

        [Test]
        public void Vector3_DividesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector3(25, 20, 10);

            var result = Vector3.Zero;
            Vector3.Divide(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f);
        }

        [Test]
        public void Vector3_OperatorDividesByFactorCorrectly()
        {
            var vector = new Vector3(25, 20, 10);

            var result = vector / 1000f;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f, 0.010f);
        }

        [Test]
        public void Vector3_CalculatesDotProductCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);

            var result = Vector3.Dot(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(998293.3f);
        }

        [Test]
        public void Vector3_CalculatesDotProductCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);

            var result = 0f;            
            Vector3.Dot(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(998293.3f);
        }

        [Test]
        public void Vector3_ParseHandlesValidInput()
        {
            var result = Vector3.Parse("123.4 -567.8 901.2");

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f);
        }

        [Test]
        public void Vector3_ParseRejectsInvalidInput()
        {
            Assert.That(() => Vector3.Parse("sfdjhkfsh"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Vector3_TryParseHandlesValidInput()
        {
            var result    = Vector3.Zero;
            var succeeded = Vector3.TryParse("123.4 -567.8 901.2", out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f);
        }

        [Test]
        public void Vector3_TryParseRejectsInvalidInput()
        {
            var result    = Vector3.Zero;
            var succeeded = Vector3.TryParse("asdfasdfasdfs", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }
        
        [Test]
        public void Vector3_LerpCalculatesCorrectly()
        {
            var vector1 = new Vector3(25, 25, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Lerp(vector1, vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f);
        }

        [Test]
        public void Vector3_LerpCalculatesCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(25, 25, 10);
            var vector2 = new Vector3(100, 50, 70);

            var result = Vector3.Zero;
            Vector3.Lerp(ref vector1, ref vector2, 0.5f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f);
        }

        [Test]
        public void Vector3_ClampToMin()
        {
            var source = new Vector3(1, 2, 3);
            var min    = new Vector3(10, 20, 30);
            var max    = new Vector3(100, 200, 300);

            var result = Vector3.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y, min.Z);
        }

        [Test]
        public void Vector3_ClampToMinWithOutParam()
        {
            var source = new Vector3(1, 2, 3);
            var min    = new Vector3(10, 20, 30);
            var max    = new Vector3(100, 200, 300);

            var result = Vector3.Zero;
            Vector3.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y, min.Z);
        }

        [Test]
        public void Vector3_ClampToMax()
        {
            var source = new Vector3(1000, 2000, 3000);
            var min    = new Vector3(10, 20, 30);
            var max    = new Vector3(100, 200, 300);

            var result = Vector3.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y, max.Z);
        }

        [Test]
        public void Vector3_ClampToMaxWithOutParam()
        {
            var source = new Vector3(1000, 2000, 3000);
            var min = new Vector3(10, 20, 30);
            var max = new Vector3(100, 200, 300);

            var result = Vector3.Zero;
            Vector3.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y, max.Z);
        }

        [Test]
        public void Vector3_MinCalculatedCorrectly()
        {
            var vector1 = new Vector3(1, 200, 3);
            var vector2 = new Vector3(300, 2, 400);

            var result = Vector3.Min(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f, 3.0f);
        }

        [Test]
        public void Vector3_MinCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(1, 200, 3);
            var vector2 = new Vector3(300, 2, 400);

            var result = Vector3.Zero;            
            Vector3.Min(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f, 3.0f);
        }

        [Test]
        public void Vector3_MaxCalculatedCorrectly()
        {
            var vector1 = new Vector3(1, 200, 3);
            var vector2 = new Vector3(300, 2, 400);

            var result = Vector3.Max(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f, 400.0f);
        }

        [Test]
        public void Vector3_MaxCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(1, 200, 3);
            var vector2 = new Vector3(300, 2, 400);
            
            var result = Vector3.Zero;            
            Vector3.Max(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f, 400.0f);
        }

        [Test]
        public void Vector3_NegateCalculatedCorrectly()
        {
            var vector = new Vector3(123.4f, -567.8f, 901.2f);
            
            var result = Vector3.Negate(vector);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f);
        }

        [Test]
        public void Vector3_NegateCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector3(123.4f, -567.8f, 901.2f);

            var result = Vector3.Zero;            
            Vector3.Negate(ref vector, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f);
        }

        [Test]
        public void Vector3_OperatorNegateCalculateCorrectly()
        {
            var vector = new Vector3(123.4f, -567.8f, 901.2f);
            
            var result = -vector;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f);
        }

        [Test]
        public void Vector3_NormalizeCalculatedCorrectly()
        {
            var vector = new Vector3(123.4f, 567.8f, 901.2f);

            var result = Vector3.Normalize(vector);

            TheResultingValue(result.Length()).ShouldBe(1f);
            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1150f, 0.5295f, 0.840f);
        }

        [Test]
        public void Vector3_NormalizeCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector3(123.4f, 567.8f, 901.2f);

            var result = Vector3.Zero;            
            Vector3.Normalize(ref vector, out result);

            TheResultingValue(result.Length()).ShouldBe(1f);
            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1150f, 0.5295f, 0.8404f);
        }

        [Test]
        public void Vector3_DistanceCalculatedCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);

            var result = Vector3.Distance(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(959.335f);
        }

        [Test]
        public void Vector3_DistanceCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);

            Single result;
            Vector3.Distance(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(959.335f);
        }

        [Test]
        public void Vector3_DistanceSquaredCalculatedCorrectly()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);
            
            var result = Vector3.DistanceSquared(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(920323.3f);
        }

        [Test]
        public void Vector3_DistanceSquaredCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(123.4f, 567.8f, 901.2f);
            var vector2 = new Vector3(901.2f, 876.5f, 432.1f);

            Single result;
            Vector3.DistanceSquared(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(920323.3f);
        }

        [Test]
        public void Vector3_ReflectCalculatedCorrectly()
        {
            var vector = new Vector3(123.4f, 567.8f, 901.2f);
            var normal = new Vector3(0, 1, 0);

            var result = Vector3.Reflect(vector, normal);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f);
        }

        [Test]
        public void Vector3_ReflectCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector3(123.4f, 567.8f, 901.2f);
            var normal = new Vector3(0, 1, 0);

            var result = Vector3.Zero;            
            Vector3.Reflect(ref vector, ref normal, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f);
        }

        [Test]
        public void Vector3_BarycentricCalculatedCorrectly()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var vector3 = new Vector3(456, 789, 300);

            var result = Vector3.Barycentric(vector1, vector2, vector3, 0.75f, 0.22f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f, 219.0f);
        }

        [Test]
        public void Vector3_BarycentricCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var vector3 = new Vector3(456, 789, 300);

            var result = Vector3.Zero;
            Vector3.Barycentric(ref vector1, ref vector2, ref vector3, 0.75f, 0.22f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f, 219.0f);
        }

        [Test]
        public void Vector3_CatmullRomCalculatedCorrectly()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var vector3 = new Vector3(456, 789, 300);
            var vector4 = new Vector3(-456, 986, 400);

            var result = Vector3.CatmullRom(vector1, vector2, vector3, vector4, 0.55f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f, 255.0f);
        }

        [Test]
        public void Vector3_CatmullRomCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var vector3 = new Vector3(456, 789, 300);
            var vector4 = new Vector3(-456, 986, 400);
            
            var result = Vector3.Zero;            
            Vector3.CatmullRom(ref vector1, ref vector2, ref vector3, ref vector4, 0.55f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f, 255.0f);
        }

        [Test]
        public void Vector3_HermiteCalculatedCorrectly()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var tangent1 = new Vector3(1, 0, 0);
            var tangent2 = new Vector3(0, 1, 0);

            var result = Vector3.Hermite(vector1, tangent1, vector2, tangent2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f, 173.1f);
        }

        [Test]
        public void Vector3_HermiteCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(234, -345, 200);
            var tangent1 = new Vector3(1, 0, 0);
            var tangent2 = new Vector3(0, 1, 0);

            var result = Vector3.Zero;            
            Vector3.Hermite(ref vector1, ref tangent1, ref vector2, ref tangent2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f, 173.1f);
        }

        [Test]
        public void Vector3_SmoothStepCalculatedCorrectly()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(-456, 986, 200);

            var result = Vector3.SmoothStep(vector1, vector2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f, 173.1f);
        }

        [Test]
        public void Vector3_SmoothStepCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector3(-123, -234, 100);
            var vector2 = new Vector3(-456, 986, 200);

            var result = Vector3.Zero;
            Vector3.SmoothStep(ref vector1, ref vector2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f, 173.1f);
        }

        [Test]
        public void Vector3_TransformTranslate()
        {
            var vector1   = new Vector3(123, 456, 789);
            var transform = Matrix.CreateTranslation(100, 200, 300);

            var result = Vector3.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(223.0f, 656.0f, 1089.0f);
        }

        [Test]
        public void Vector3_TransformTranslateWithOutParam()
        {
            var vector1   = new Vector3(123, 456, 789);
            var transform = Matrix.CreateTranslation(100, 200, 300);
            
            var result = Vector3.Zero;            
            Vector3.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(223.0f, 656.0f, 1089.0f);
        }

        [Test]
        public void Vector3_TransformsCorrectly_WithMatrix()
        {
            var vector1   = new Vector3(123, 456, 768);
            var transform = Matrix.CreateRotationZ((float)Math.PI);
            var result    = Vector3.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768.0f);
        }

        [Test]
        public void Vector3_TransformsCorrectly_WithMatrix_WithOutParam()
        {
            var vector1 = new Vector3(123, 456, 768);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Vector3.Zero;            
            Vector3.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 768.0f);
        }

        [Test]
        public void Vector3_TransformsCorrectly_WithQuaternion()
        {
            var vector1 = new Vector3(123, 456, 789);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Vector3.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789.0f);
        }

        [Test]
        public void Vector3_TransformsForrectly_WithQuaternion_WithOutParam()
        {
            var vector1 = new Vector3(123, 456, 789);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Vector3.Transform(ref vector1, ref transform, out Vector3 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f, 789.0f);
        }

        [Test]
        public void Vector3_SerializesToJson()
        {
            var vector = new Vector3(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject(vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4}");
        }

        [Test]
        public void Vector3_SerializesToJson_WhenNullable()
        {
            var vector = new Vector3(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject((Vector3?)vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4}");
        }

        [Test]
        public void Vector3_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3,""z"":3.4}";
            
            var vector = JsonConvert.DeserializeObject<Vector3>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector)
                .ShouldBe(1.2f, 2.3f, 3.4f);
        }

        [Test]
        public void Vector3_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3,""z"":3.4}";

            var vector1 = JsonConvert.DeserializeObject<Vector3?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector1.Value)
                .ShouldBe(1.2f, 2.3f, 3.4f);

            const String json2 = @"null";

            var vector2 = JsonConvert.DeserializeObject<Vector3?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Vector3_ConvertsFromSystemNumericsCorrectly()
        {
            var vector1 = new Vector3(1.2f, 3.4f, 5.6f);
            System.Numerics.Vector3 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
            TheResultingValue(vector1.Z).ShouldBe(vector2.Z);
        }

        [Test]
        public void Vector3_ConvertsToSystemNumericsCorrectly()
        {
            var vector1 = new System.Numerics.Vector3(1.2f, 3.4f, 5.6f);
            Vector3 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
            TheResultingValue(vector1.Z).ShouldBe(vector2.Z);
        }
    }
}
