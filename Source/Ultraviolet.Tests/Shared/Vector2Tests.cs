using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Vector2Tests : UltravioletTestFramework
    {
        [Test]
        public void Vector2_ConstructorSetsValues()
        {
            var result = new Vector2(123.4f, 567.8f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, 567.8f);
        }

        [Test]
        public void Vector2_EqualsTestsCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(123.4f, 567.8f);
            var vector3 = new Vector2(567.8f, 123.4f);

            TheResultingValue(vector1.Equals(vector2)).ShouldBe(true);
            TheResultingValue(vector1.Equals(vector3)).ShouldBe(false);
        }

        [Test]
        public void Vector2_OperatorEqualsTestsCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(123.4f, 567.8f);
            var vector3 = new Vector2(567.8f, 123.4f);

            TheResultingValue(vector1 == vector2).ShouldBe(true);
            TheResultingValue(vector1 == vector3).ShouldBe(false);
        }

        [Test]
        public void Vector2_OperatorNotEqualsTestsCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(123.4f, 567.8f);
            var vector3 = new Vector2(567.8f, 123.4f);

            TheResultingValue(vector1 != vector2).ShouldBe(false);
            TheResultingValue(vector1 != vector3).ShouldBe(true);
        }

        [Test]
        public void Vector2_CalculatesLengthSquaredCorrectly()
        {
            var vector = new Vector2(123.4f, 567.8f);

            TheResultingValue(vector.LengthSquared()).WithinDelta(0.1f)
                .ShouldBe((123.4f * 123.4f) + (567.8f * 567.8f));
        }

        [Test]
        public void Vector2_CalculatesLengthCorrectly()
        {
            var vector = new Vector2(123.4f, 567.8f);

            TheResultingValue(vector.Length()).WithinDelta(0.1f)
                .ShouldBe((float)Math.Sqrt((123.4f * 123.4f) + (567.8f * 567.8f)));
        }

        [Test]
        public void Vector2_InterpolatesCorrectly()
        {
            var vector1 = new Vector2(25, 25);
            var vector2 = new Vector2(100, 50);

            var result = vector1.Interpolate(vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f);
        }

        [Test]
        public void Vector2_AddsCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);
            
            var result = Vector2.Add(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f);
        }

        [Test]
        public void Vector2_AddsCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Zero;            
            Vector2.Add(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f);
        }

        [Test]
        public void Vector2_OperatorAddsCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = vector1 + vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f);
        }

        [Test]
        public void Vector2_SubtractsCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Subtract(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f);
        }

        [Test]
        public void Vector2_SubtractsCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Zero;            
            Vector2.Subtract(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f);
        }

        [Test]
        public void Vector2_OperatorSubtractsCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = vector1 - vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f);
        }

        [Test]
        public void Vector2_MultipliesCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Multiply(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f);
        }

        [Test]
        public void Vector2_MultipliesCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Zero;            
            Vector2.Multiply(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f);
        }

        [Test]
        public void Vector2_OperatorMultipliesCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = vector1 * vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(2500.0f, 1000.0f);
        }

        [Test]
        public void Vector2_MultipliesByFactorCorrectly()
        {
            var vector = new Vector2(25, 20);
            
            var result = Vector2.Multiply(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f);
        }

        [Test]
        public void Vector2_MultipliesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector2(25, 20);
            
            var result = Vector2.Zero;            
            Vector2.Multiply(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f);
        }

        [Test]
        public void Vector2_OperatorMultipliesByFactorCorrectly()
        {
            var vector = new Vector2(25, 20);

            var result = vector * 1000;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f);
        }

        [Test]
        public void Vector2_DividesCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Divide(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f);
        }

        [Test]
        public void Vector2_DividesCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = Vector2.Zero;
            Vector2.Divide(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f);
        }

        [Test]
        public void Vector2_OperatorDividesCorrectly()
        {
            var vector1 = new Vector2(25, 20);
            var vector2 = new Vector2(100, 50);

            var result = vector1 / vector2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.25f, 0.40f);
        }

        [Test]
        public void Vector2_DividesByFactorCorrectly()
        {
            var vector = new Vector2(25, 20);
            
            var result = Vector2.Divide(vector, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f);
        }

        [Test]
        public void Vector2_DividesByFactorCorrectlyWithOutParam()
        {
            var vector = new Vector2(25, 20);

            var result = Vector2.Zero;
            Vector2.Divide(ref vector, 1000, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f);
        }

        [Test]
        public void Vector2_OperatorDividesByFactorCorrectly()
        {
            var vector = new Vector2(25, 20);

            var result = vector / 1000f;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.025f, 0.020f);
        }

        [Test]
        public void Vector2_CalculatesDotProductCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);

            var result = Vector2.Dot(vector1, vector2);

            TheResultingValue(result).ShouldBe(353506.48f);
        }

        [Test]
        public void Vector2_CalculatesDotProductCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);

            var result = 0f;
            Vector2.Dot(ref vector1, ref vector2, out result);

            TheResultingValue(result).ShouldBe(353506.48f);
        }

        [Test]
        public void Vector2_ParseHandlesValidInput()
        {
            var result = Vector2.Parse("123.4 -567.8");

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f);
        }

        [Test]
        public void Vector2_ParseRejectsInvalidInput()
        {
            Assert.That(() => Vector2.Parse("sfdjhkfsh"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Vector2_TryParseHandlesValidInput()
        {
            var result    = Vector2.Zero;
            var succeeded = Vector2.TryParse("123.4 -567.8", out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f);
        }

        [Test]
        public void Vector2_TryParseRejectsInvalidInput()
        {
            var result    = Vector2.Zero;
            var succeeded = Vector2.TryParse("asdfasdfasdfs", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }
        
        [Test]
        public void Vector2_LerpCalculatesCorrectly()
        {
            var vector1 = new Vector2(25, 25);
            var vector2 = new Vector2(100, 50);
            
            var result = Vector2.Lerp(vector1, vector2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f);
        }

        [Test]
        public void Vector2_LerpCalculatesCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(25, 25);
            var vector2 = new Vector2(100, 50);
            
            var result = Vector2.Zero;            
            Vector2.Lerp(ref vector1, ref vector2, 0.5f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f);
        }

        [Test]
        public void Vector2_ClampToMin()
        {
            var source = new Vector2(1, 2);
            var min    = new Vector2(10, 20);
            var max    = new Vector2(100, 200);

            var result = Vector2.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y);
        }

        [Test]
        public void Vector2_ClampToMinWithOutParam()
        {
            var source = new Vector2(1, 2);
            var min    = new Vector2(10, 20);
            var max    = new Vector2(100, 200);

            var result = Vector2.Zero;
            Vector2.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(min.X, min.Y);
        }

        [Test]
        public void Vector2_ClampToMax()
        {
            var source = new Vector2(1000, 2000);
            var min    = new Vector2(10, 20);
            var max    = new Vector2(100, 200);

            var result = Vector2.Clamp(source, min, max);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y);
        }

        [Test]
        public void Vector2_ClampToMaxWithOutParam()
        {
            var source = new Vector2(1000, 2000);
            var min    = new Vector2(10, 20);
            var max    = new Vector2(100, 200);

            var result = Vector2.Zero;
            Vector2.Clamp(ref source, ref min, ref max, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(max.X, max.Y);
        }

        [Test]
        public void Vector2_MinCalculatedCorrectly()
        {
            var vector1 = new Vector2(1, 200);
            var vector2 = new Vector2(300, 2);

            var result = Vector2.Min(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f);
        }

        [Test]
        public void Vector2_MinCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(1, 200);
            var vector2 = new Vector2(300, 2);
            
            var result = Vector2.Zero;            
            Vector2.Min(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1.0f, 2.0f);
        }

        [Test]
        public void Vector2_MaxCalculatedCorrectly()
        {
            var vector1 = new Vector2(1, 200);
            var vector2 = new Vector2(300, 2);
            
            var result = Vector2.Max(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f);
        }

        [Test]
        public void Vector2_MaxCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(1, 200);
            var vector2 = new Vector2(300, 2);
           
            var result = Vector2.Zero;            
            Vector2.Max(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(300.0f, 200.0f);
        }

        [Test]
        public void Vector2_NegateCalculatedCorrectly()
        {
            var vector = new Vector2(123.4f, -567.8f);
            
            var result = Vector2.Negate(vector);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f);
        }

        [Test]
        public void Vector2_NegateCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector2(123.4f, -567.8f);
            
            var result = Vector2.Zero;            
            Vector2.Negate(ref vector, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f);
        }

        [Test]
        public void Vector2_OperatorNegateCalculateCorrectly()
        {
            var vector = new Vector2(123.4f, -567.8f);
            
            var result = -vector;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f);
        }

        [Test]
        public void Vector2_NormalizeCalculatedCorrectly()
        {
            var vector = new Vector2(123.4f, 567.8f);
            
            var result = Vector2.Normalize(vector);

            TheResultingValue(result.Length()).WithinDelta(0.001f)
                .ShouldBe(1f);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.2123f, 0.9771f);
        }

        [Test]
        public void Vector2_NormalizeCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector2(123.4f, 567.8f);
            
            var result = Vector2.Zero;            
            Vector2.Normalize(ref vector, out result);

            TheResultingValue(result.Length()).WithinDelta(0.001f)
                .ShouldBe(1f);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.2123f, 0.9771f);
        }

        [Test]
        public void Vector2_DistanceCalculatedCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);
           
            var result = Vector2.Distance(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(765.2281f);
        }

        [Test]
        public void Vector2_DistanceCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);

            Single result;
            Vector2.Distance(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(765.2281f);
        }

        [Test]
        public void Vector2_DistanceSquaredCalculatedCorrectly()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);
            
            var result = Vector2.DistanceSquared(vector1, vector2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(585574.1f);
        }

        [Test]
        public void Vector2_DistanceSquaredCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(123.4f, 567.8f);
            var vector2 = new Vector2(876.5f, 432.1f);

            Single result;
            Vector2.DistanceSquared(ref vector1, ref vector2, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(585574.1f);
        }

        [Test]
        public void Vector2_ReflectCalculatedCorrectly()
        {
            var vector = new Vector2(123.4f, 567.8f);
            var normal = new Vector2(0, 1);
            
            var result = Vector2.Reflect(vector, normal);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f);
        }

        [Test]
        public void Vector2_ReflectCalculatedCorrectlyWithOutParam()
        {
            var vector = new Vector2(123.4f, 567.8f);
            var normal = new Vector2(0, 1);

            var result = Vector2.Zero;            
            Vector2.Reflect(ref vector, ref normal, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f);
        }

        [Test]
        public void Vector2_BarycentricCalculatedCorrectly()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var vector3 = new Vector2(456, 789);
            
            var result = Vector2.Barycentric(vector1, vector2, vector3, 0.75f, 0.22f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f);
        }

        [Test]
        public void Vector2_BarycentricCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var vector3 = new Vector2(456, 789);

            var result = Vector2.Zero;
            Vector2.Barycentric(ref vector1, ref vector2, ref vector3, 0.75f, 0.22f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(272.13f, -92.19f);
        }

        [Test]
        public void Vector2_CatmullRomCalculatedCorrectly()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var vector3 = new Vector2(456, 789);
            var vector4 = new Vector2(-456, 986);
            
            var result = Vector2.CatmullRom(vector1, vector2, vector3, vector4, 0.55f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f);
        }

        [Test]
        public void Vector2_CatmullRomCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var vector3 = new Vector2(456, 789);
            var vector4 = new Vector2(-456, 986);

            var result = Vector2.Zero;
            Vector2.CatmullRom(ref vector1, ref vector2, ref vector3, ref vector4, 0.55f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(440.8f, 273.1f);
        }

        [Test]
        public void Vector2_HermiteCalculatedCorrectly()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var tangent1 = new Vector2(1, 0);
            var tangent2 = new Vector2(0, 1);
            
            var result = Vector2.Hermite(vector1, tangent1, vector2, tangent2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f);
        }

        [Test]
        public void Vector2_HermiteCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(234, -345);
            var tangent1 = new Vector2(1, 0);
            var tangent2 = new Vector2(0, 1);
            
            var result = Vector2.Zero;            
            Vector2.Hermite(ref vector1, ref tangent1, ref vector2, ref tangent2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(138.3f, -315.3f);
        }

        [Test]
        public void Vector2_SmoothStepCalculatedCorrectly()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(-456, 986);
            
            var result = Vector2.SmoothStep(vector1, vector2, 0.66f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f);
        }

        [Test]
        public void Vector2_SmoothStepCalculatedCorrectlyWithOutParam()
        {
            var vector1 = new Vector2(-123, -234);
            var vector2 = new Vector2(-456, 986);
            
            var result = Vector2.Zero;            
            Vector2.SmoothStep(ref vector1, ref vector2, 0.66f, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-366.6f, 658.8f);
        }

        [Test]
        public void Vector2_TransformTranslate()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateTranslation(100, 200, 300);

            var result = Vector2.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(223.0f, 656.0f);
        }

        [Test]
        public void Vector2_TransformTranslateWithOutParam()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateTranslation(100, 200, 300);
            
            var result = Vector2.Zero;            
            Vector2.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(223.0f, 656.0f);
        }

        [Test]
        public void Vector2_TransformsCorrectly_WithMatrix()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);
           
            var result = Vector2.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Vector2_TransformsCorrectly_WithMatrix_WithOutParam()
        {
            var vector1 = new Vector2(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);
            
            var result = Vector2.Zero;            
            Vector2.Transform(ref vector1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Vector2_TransformsCorrectly_WithQuaternion()
        {
            var vector1 = new Vector2(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Vector2.Transform(vector1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Vector2_TransformsForrectly_WithQuaternion_WithOutParam()
        {
            var vector1 = new Vector2(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Vector2.Transform(ref vector1, ref transform, out Vector2 result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }
        
        [Test]
        public void Vector2_SerializesToJson()
        {
            var vector = new Vector2(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject(vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Vector2_SerializesToJson_WhenNullable()
        {
            var vector = new Vector2(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject((Vector2?)vector,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Vector2_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3}";
            
            var vector = JsonConvert.DeserializeObject<Vector2>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector)
                .ShouldBe(1.2f, 2.3f);
        }

        [Test]
        public void Vector2_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3}";

            var vector1 = JsonConvert.DeserializeObject<Vector2?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector1.Value)
                .ShouldBe(1.2f, 2.3f);

            const String json2 = @"null";

            var vector2 = JsonConvert.DeserializeObject<Vector2?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(vector2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Vector2_ConvertsFromSystemNumericsCorrectly()
        {
            var vector1 = new Vector2(1.2f, 3.4f);
            System.Numerics.Vector2 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
        }

        [Test]
        public void Vector2_ConvertsToSystemNumericsCorrectly()
        {
            var vector1 = new System.Numerics.Vector2(1.2f, 3.4f);
            Vector2 vector2 = vector1;

            TheResultingValue(vector1.X).ShouldBe(vector2.X);
            TheResultingValue(vector1.Y).ShouldBe(vector2.Y);
        }
    }
}
