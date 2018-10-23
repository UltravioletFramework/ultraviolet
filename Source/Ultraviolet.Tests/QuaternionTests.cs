using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class QuaternionTests : UltravioletTestFramework
    {
        [Test]
        public void Quaternion_ConstructorSetsValues()
        {
            var result = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, 567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_EqualsTestsCorrectly()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion3 = new Quaternion(345.6f, 901.2f, 567.8f, 123.4f);

            TheResultingValue(quaternion1.Equals(quaternion2)).ShouldBe(true);
            TheResultingValue(quaternion1.Equals(quaternion3)).ShouldBe(false);
        }

        [Test]
        public void Quaternion_OperatorEqualsTestsCorrectly()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion3 = new Quaternion(345.6f, 901.2f, 567.8f, 123.4f);

            TheResultingValue(quaternion1 == quaternion2).ShouldBe(true);
            TheResultingValue(quaternion1 == quaternion3).ShouldBe(false);
        }

        [Test]
        public void Quaternion_OperatorNotEqualsTestsCorrectly()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion3 = new Quaternion(345.6f, 567.8f, 123.4f, 901.2f);

            TheResultingValue(quaternion1 != quaternion2).ShouldBe(false);
            TheResultingValue(quaternion1 != quaternion3).ShouldBe(true);
        }

        [Test]
        public void Quaternion_CalculatesLengthSquaredCorrectly()
        {
            var quaternionX = 123.4f;
            var quaternionY = 567.8f;
            var quaternionZ = 901.2f;
            var quaternionW = 345.6f;
            var quaterinon = new Quaternion(quaternionX, quaternionY, quaternionZ, quaternionW);            

            TheResultingValue(quaterinon.LengthSquared()).WithinDelta(0.1f)
                .ShouldBe((quaternionX * quaternionX) + (quaternionY * quaternionY) + (quaternionZ * quaternionZ) + (quaternionW * quaternionW));
        }

        [Test]
        public void Quaternion_CalculatesLengthCorrectly()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            TheResultingValue(quaternion.Length()).WithinDelta(0.1f)
                .ShouldBe((float)Math.Sqrt((123.4f * 123.4f) + (567.8f * 567.8f) + (901.2f * 901.2f) + (345.6f * 345.6f)));
        }

        [Test]
        public void Quternion_InterpolatesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 25, 25, 25);
            var quaternion2 = new Quaternion(100, 50, 75, 150);

            var result = quaternion1.Interpolate(quaternion2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 50.0f, 87.5f);
        }
        
        [Test]
        public void Quaternion_ParseHandlesValidInput()
        {
            var result = Quaternion.Parse("123.4 -567.8 901.2 345.6");

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_ParseRejectsInvalidInput()
        {
            Assert.That(() => Quaternion.Parse("sfdjhkfsh"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Quaternion_TryParseHandlesValidInput()
        {
            var succeeded = Quaternion.TryParse("123.4 -567.8 901.2 345.6", out Quaternion result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(123.4f, -567.8f, 901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_TryParseRejectsInvalidInput()
        {
            var succeeded = Quaternion.TryParse("asdfasdfasdfs", out Quaternion result);

            TheResultingValue(succeeded).ShouldBe(false);
        }
        
        [Test]
        public void Quaternion_CreateFromAxisAngleCalculatedCorrectly()
        {
            var axis = Vector3.Normalize(new Vector3(5.1f, 3.2f, 7.4f));
            var angle = (float)Math.PI;

            var result = Quaternion.CreateFromAxisAngle(axis, angle);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.5345f, 0.3354f, 0.7756f, 0.000f);
        }

        [Test]
        public void Quaternion_CreateFromAxisAngleCalculatedCorrectly_WithOutParam()
        {
            var axis = Vector3.Normalize(new Vector3(5.1f, 3.2f, 7.4f));
            var angle = (float)Math.PI;

            Quaternion.CreateFromAxisAngle(ref axis, angle, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.5345f, 0.3354f, 0.7756f, 0.000f);
        }

        [Test]
        public void Quaternion_CreateFromYawPitchRollCalculatedCorrectly()
        {
            var result = Quaternion.CreateFromYawPitchRoll(12.3f, 23.4f, 34.5f);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1076f, -0.7524f, -0.6386f, -0.1196f);
        }

        [Test]
        public void Quaternion_CreateFromYawPitchRollCalculatedCorrectly_WithOutParam()
        {
            Quaternion.CreateFromYawPitchRoll(12.3f, 23.4f, 34.5f, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1076f, -0.7524f, -0.6386f, -0.1196f);
        }

        [Test]
        public void Quaternion_CreateFromRotationMatrixCalculatedCorrectly()
        {
            var matrix = Matrix.CreateRotationZ(12.3f);
            var result = Quaternion.CreateFromRotationMatrix(matrix);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0f, 0f, -0.1327f, 0.9911f);
        }

        [Test]
        public void Quaternion_CreateFromRotationMatrixCalculatedCorrectly_WithOutParam()
        {
            var matrix = Matrix.CreateRotationZ(12.3f);
            Quaternion.CreateFromRotationMatrix(ref matrix, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0f, 0f, -0.1327f, 0.9911f);
        }
        
        [Test]
        public void Quaternion_AddsCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Add(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Quaternion_AddsCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);
            
            Quaternion.Add(ref quaternion1, ref quaternion2, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Quaternion_OperatorAddsCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = quaternion1 + quaternion2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(125.0f, 70.0f, 80.0f, 110.0f);
        }

        [Test]
        public void Quaternion_SubtractsCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Subtract(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Quaternion_SubtractsCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);
            
            Quaternion.Subtract(ref quaternion1, ref quaternion2, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Quaternion_OperatorSubtractsCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = quaternion1 - quaternion2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-75.0f, -30.0f, -60.0f, -50.0f);
        }

        [Test]
        public void Quaternion_MultipliesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Multiply(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(5900f, 2350f, 2150f, -1800f);
        }

        [Test]
        public void Quaternion_MultipliesCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);
            
            Quaternion.Multiply(ref quaternion1, ref quaternion2, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(5900f, 2350f, 2150f, -1800f);
        }

        [Test]
        public void Quaternion_OperatorMultipliesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = quaternion1 * quaternion2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(5900f, 2350f, 2150f, -1800f);
        }

        [Test]
        public void Quaternion_MultipliesByFactorCorrectly()
        {
            var quaternion = new Quaternion(25, 20, 10, 30);

            var result = Quaternion.Multiply(quaternion, 1000);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Quaternion_MultipliesByFactorCorrectly_WithOutParam()
        {
            var quaternion = new Quaternion(25, 20, 10, 30);
            
            Quaternion.Multiply(ref quaternion, 1000, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Quaternion_OperatorMultipliesByFactorCorrectly()
        {
            var quaternion = new Quaternion(25, 20, 10, 30);

            var result = quaternion * 1000;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(25000.0f, 20000.0f, 10000.0f, 30000.0f);
        }

        [Test]
        public void Quaternion_DividesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Divide(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-0.0798f, 0.0357f, -0.02310f, 0.2773f);
        }

        [Test]
        public void Quaternion_DividesCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);
            
            Quaternion.Divide(ref quaternion1, ref quaternion2, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-0.0798f, 0.0357f, -0.02310f, 0.2773f);
        }

        [Test]
        public void Quaternion_OperatorDividesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 20, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = quaternion1 / quaternion2;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-0.0798f, 0.0357f, -0.02310f, 0.2773f);
        }
        
        [Test]
        public void Quaternion_LerpCalculatesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 25, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Lerp(quaternion1, quaternion2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.6269f, 0.3761f, 0.4012f, 0.5517f);
        }

        [Test]
        public void Quaternion_LerpCalculatesCorrectlyWithOutParam()
        {
            var quaternion1 = new Quaternion(25, 25, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);
            
            Quaternion.Lerp(ref quaternion1, ref quaternion2, 0.5f, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.6269f, 0.3761f, 0.4012f, 0.5517f);
        }

        [Test]
        public void Quaternion_SlerpCalculatesCorrectly()
        {
            var quaternion1 = new Quaternion(25, 25, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            var result = Quaternion.Slerp(quaternion1, quaternion2, 0.5f);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f, 55.0f);
        }

        [Test]
        public void Quaternion_SlerpCalculatesCorrectlyWithOutParam()
        {
            var quaternion1 = new Quaternion(25, 25, 10, 30);
            var quaternion2 = new Quaternion(100, 50, 70, 80);

            Quaternion.Slerp(ref quaternion1, ref quaternion2, 0.5f, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(62.5f, 37.5f, 40.0f, 55.0f);
        }
        
        [Test]
        public void Quaternion_NormalizeCalculatedCorrectly()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            var result = Quaternion.Normalize(quaternion);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0.1095f, 0.5039f, 0.7999f, 0.3067f);
        }

        [Test]
        public void Quaternion_NormalizeCalculatedCorrectly_WithOutParam()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            
            Quaternion.Normalize(ref quaternion, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.001f)
                .ShouldBe(0.1095f, 0.5039f, 0.7999f, 0.3067f);
        }
        
        [Test]
        public void Quaternion_NegateCalculatedCorrectly()
        {
            var quaternion = new Quaternion(123.4f, -567.8f, 901.2f, -345.6f);

            var result = Quaternion.Negate(quaternion);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_NegateCalculatedCorrectlyWithOutParam()
        {
            var quaternion = new Quaternion(123.4f, -567.8f, 901.2f, -345.6f);
            
            Quaternion.Negate(ref quaternion, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_OperatorNegateCalculateCorrectly()
        {
            var quaternion = new Quaternion(123.4f, -567.8f, 901.2f, -345.6f);

            var result = -quaternion;

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, 567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_CalculatesInverseCorrectly()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            var result = Quaternion.Inverse(quaternion);
            
            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0f, -0.0004f, -0.0007f, 0.0002f);
        }

        [Test]
        public void Quaternion_CalculatesInverseCorrectly_WithOutParam()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            Quaternion.Inverse(ref quaternion, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(0f, -0.0004f, -0.0007f, 0.0002f);
        }

        [Test]
        public void Quaternion_CalculatesDotProductCorrectly()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(345.6f, 901.2f, 876.5f, 432.1f);

            var result = Quaternion.Dot(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1493584.0f);
        }

        [Test]
        public void Quaternion_CalculatesDotProductCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(345.6f, 901.2f, 876.5f, 432.1f);
            
            Quaternion.Dot(ref quaternion1, ref quaternion2, out Single result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(1493584.0f);
        }

        [Test]
        public void Quaternion_CalculatesConcatenateCorrectly()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(345.6f, 901.2f, 876.5f, 432.1f);

            var result = Quaternion.Concatenate(quaternion1, quaternion2);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(487245.3f, 353506.5f, 777350.5f, -1194916.5f);
        }

        [Test]
        public void Quaternion_CalculatesConcatenateCorrectly_WithOutParam()
        {
            var quaternion1 = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);
            var quaternion2 = new Quaternion(345.6f, 901.2f, 876.5f, 432.1f);

            Quaternion.Concatenate(ref quaternion1, ref quaternion2, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(487245.3f, 353506.5f, 777350.5f, -1194916.5f);
        }

        [Test]
        public void Quaternion_CalculatesConjugateCorrectly()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            var result = Quaternion.Conjugate(quaternion);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, -567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_CalculatesConjugateCorrectly_WithOutParam()
        {
            var quaternion = new Quaternion(123.4f, 567.8f, 901.2f, 345.6f);

            Quaternion.Conjugate(ref quaternion, out Quaternion result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.4f, -567.8f, -901.2f, 345.6f);
        }

        [Test]
        public void Quaternion_SerializesToJson()
        {
            var quaternion = new Quaternion(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject(quaternion,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4,""w"":4.5}");
        }

        [Test]
        public void Quaternion_SerializesToJson_WhenNullable()
        {
            var quaternion = new Quaternion(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject((Quaternion?)quaternion, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""z"":3.4,""w"":4.5}");
        }

        [Test]
        public void Quaternion_DeserializesFromJson()
        {
            const String json = @"{ ""x"": 1.2, ""y"": 2.3, ""z"": 3.4, ""w"": 4.5 }";

            var quaternion = JsonConvert.DeserializeObject<Quaternion>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(quaternion)
                .ShouldBe(1.2f, 2.3f, 3.4f, 4.5f);
        }

        [Test]
        public void Quaternion_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""x"": 1.2, ""y"": 2.3, ""z"": 3.4, ""w"": 4.5 }";

            var quaternion1 = JsonConvert.DeserializeObject<Quaternion?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(quaternion1.Value)
                .ShouldBe(1.2f, 2.3f, 3.4f, 4.5f);

            const String json2 = @"null";

            var quaternion2 = JsonConvert.DeserializeObject<Quaternion?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(quaternion2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Quaternion_ConvertsFromSystemNumericsCorrectly()
        {
            var quaternion1 = new Quaternion(1.2f, 3.4f, 5.6f, 7.8f);
            System.Numerics.Quaternion quaternion2 = quaternion1;

            TheResultingValue(quaternion1.X).ShouldBe(quaternion2.X);
            TheResultingValue(quaternion1.Y).ShouldBe(quaternion2.Y);
            TheResultingValue(quaternion1.Z).ShouldBe(quaternion2.Z);
            TheResultingValue(quaternion1.W).ShouldBe(quaternion2.W);
        }

        [Test]
        public void Quaternion_ConvertsToSystemNumericsCorrectly()
        {
            var quaternion1 = new System.Numerics.Quaternion(1.2f, 3.4f, 5.6f, 7.8f);
            Quaternion quaternion2 = quaternion1;

            TheResultingValue(quaternion1.X).ShouldBe(quaternion2.X);
            TheResultingValue(quaternion1.Y).ShouldBe(quaternion2.Y);
            TheResultingValue(quaternion1.Z).ShouldBe(quaternion2.Z);
            TheResultingValue(quaternion1.W).ShouldBe(quaternion2.W);
        }
    }
}
