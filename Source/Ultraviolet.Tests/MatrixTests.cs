using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class MatrixTests : UltravioletTestFramework
    {
        [Test]
        public void Matrix_ConstructorSetsValues()
        {
            var matrix = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);

            TheResultingValue(matrix).ShouldBe(
                11, 12, 13, 14,
                21, 22, 23, 24,
                31, 32, 33, 34,
                41, 42, 43, 44
            );
        }

        [Test]
        public void Matrix_EqualsTestsCorrectly()
        {
            var matrix1 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34,
                41, 42, 43, 44);
            
            var matrix2 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);
            
            var matrix3 = Matrix.Identity;

            TheResultingValue(matrix1.Equals(matrix2)).ShouldBe(true);
            TheResultingValue(matrix1.Equals(matrix3)).ShouldBe(false);
        }

        [Test]
        public void Matrix_OperatorEqualsTestsCorrectly()
        {
            var matrix1 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24,
                31, 32, 33, 34, 
                41, 42, 43, 44);

            var matrix2 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);
            
            var matrix3 = Matrix.Identity;

            TheResultingValue(matrix1 == matrix2).ShouldBe(true);
            TheResultingValue(matrix1 == matrix3).ShouldBe(false);
        }

        [Test]
        public void Matrix_OperatorNotEqualsTestsCorrectly()
        {
            var matrix1 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);
            
            var matrix2 = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);
            
            var matrix3 = Matrix.Identity;

            TheResultingValue(matrix1 != matrix2).ShouldBe(false);
            TheResultingValue(matrix1 != matrix3).ShouldBe(true);
        }

        [Test]
        public void Matrix_DeterminantCalculatedCorrectly()
        {
            var matrix = new Matrix(
                3, 2, 0, 1, 
                4, 0, 1, 2, 
                3, 0, 2, 1, 
                9, 2, 3, 1);

            var determinant = matrix.Determinant();

            TheResultingValue(determinant)
                .WithinDelta(0.1f).ShouldBe(24.0f);
        }

        [Test]
        public void Matrix_InterpolatesCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);
            
            var result = matrix1.Interpolate(matrix2, 0.66f);

            TheResultingValue(result).ShouldBe(
                  69.4f,  138.8f,  208.2f,  277.6f,
                 347.0f,  416.4f,  485.8f,  555.2f,
                 624.6f,  694.0f,  763.4f,  832.8f,
                 902.2f,  971.6f, 1041.0f, 1110.4f
            );
        }

        [Test]
        public void Matrix_LerpCalculatesCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80,
                 90, 100, 110, 120, 
                130, 140, 150, 160);

            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Lerp(matrix1, matrix2, 0.66f);

            TheResultingValue(result).ShouldBe(
                  69.4f,  138.8f,  208.2f,  277.6f,
                 347.0f,  416.4f,  485.8f,  555.2f,
                 624.6f,  694.0f,  763.4f,  832.8f,
                 902.2f,  971.6f, 1041.0f, 1110.4f
            );
        }

        [Test]
        public void Matrix_LerpCalculatesCorrectlyWithOutParam()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);

            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Identity;
            Matrix.Lerp(ref matrix1, ref matrix2, 0.66f, out result);

            TheResultingValue(result).ShouldBe(
                  69.4f,  138.8f,  208.2f,  277.6f,
                 347.0f,  416.4f,  485.8f,  555.2f,
                 624.6f,  694.0f,  763.4f,  832.8f,
                 902.2f,  971.6f, 1041.0f, 1110.4f
            );
        }
        
        [Test]
        public void Matrix_AddsCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);

            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);
            
            var result = Matrix.Add(matrix1, matrix2);

            TheResultingValue(result).ShouldBe(
                  110.0f,  220.0f,  330.0f,  440.0f,
                  550.0f,  660.0f,  770.0f,  880.0f,
                  990.0f, 1100.0f, 1210.0f, 1320.0f,
                 1430.0f, 1540.0f, 1650.0f, 1760.0f
            );
        }

        [Test]
        public void Matrix_AddsCorrectlyWithOutParam()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80,
                 90, 100, 110, 120,
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);
            
            var result = Matrix.Identity;
            Matrix.Add(ref matrix1, ref matrix2, out result);

            TheResultingValue(result).ShouldBe(
                  110.0f, 220.0f, 330.0f, 440.0f,
                  550.0f, 660.0f, 770.0f, 880.0f,
                  990.0f, 1100.0f, 1210.0f, 1320.0f,
                 1430.0f, 1540.0f, 1650.0f, 1760.0f
            );
        }

        [Test]
        public void Matrix_OperatorAddsCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = matrix1 + matrix2;

            TheResultingValue(result).ShouldBe(
                  110.0f, 220.0f, 330.0f, 440.0f,
                  550.0f, 660.0f, 770.0f, 880.0f,
                  990.0f, 1100.0f, 1210.0f, 1320.0f,
                 1430.0f, 1540.0f, 1650.0f, 1760.0f
            );
        }

        [Test]
        public void Matrix_SubtractsCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800,
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Subtract(matrix1, matrix2);

            TheResultingValue(result).ShouldBe(
                  -90f,  -180f,  -270f,  -360f,
                 -450f,  -540f,  -630f,  -720f,
                 -810f,  -900f,  -990f, -1080f,
                -1170f, -1260f, -1350f, -1440f
            );
        }

        [Test]
        public void Matrix_SubtractsCorrectlyWithOutParam()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);
            
            var result = Matrix.Identity;
            Matrix.Subtract(ref matrix1, ref matrix2, out result);

            TheResultingValue(result).ShouldBe(
                  -90f,  -180f,  -270f,  -360f,
                 -450f,  -540f,  -630f,  -720f,
                 -810f,  -900f,  -990f, -1080f,
                -1170f, -1260f, -1350f, -1440f
            );
        }

        [Test]
        public void Matrix_OperatorSubtractsCorrectly()
        {
            var matrix1 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var matrix2 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = matrix1 - matrix2;

            TheResultingValue(result).ShouldBe(
                  -90f,  -180f,  -270f,  -360f,
                 -450f,  -540f,  -630f,  -720f,
                 -810f,  -900f,  -990f, -1080f,
                -1170f, -1260f, -1350f, -1440f
            );
        }

        [Test]
        public void Matrix_DividesCorrectly()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var result = Matrix.Divide(matrix1, matrix2);

            TheResultingValue(result).ShouldBe(
                10f, 10f, 10f, 10f, 
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f
            );
        }

        [Test]
        public void Matrix_DividesCorrectlyWithOutParam()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80,
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var result = Matrix.Identity;
            Matrix.Divide(ref matrix1, ref matrix2, out result);

            TheResultingValue(result).ShouldBe(
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f
            );
        }

        [Test]
        public void Matrix_OperatorDividesCorrectly()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200,
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var result = matrix1 / matrix2;

            TheResultingValue(result).ShouldBe(
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f,
                10f, 10f, 10f, 10f
            );
        }

        [Test]
        public void Matrix_DividesByFactorCorrectly()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Divide(matrix, 10f);

            TheResultingValue(result).ShouldBe(
                 10f,  20f,  30f,  40f,
                 50f,  60f,  70f,  80f,
                 90f, 100f, 110f, 120f,
                130f, 140f, 150f, 160f
            );
        }

        [Test]
        public void Matrix_DividesByFactorCorrectlyWithOutParam()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Identity;
            Matrix.Divide(ref matrix, 10f, out result);

            TheResultingValue(result).ShouldBe(
                 10f, 20f, 30f, 40f,
                 50f, 60f, 70f, 80f,
                 90f, 100f, 110f, 120f,
                130f, 140f, 150f, 160f
            );
        }

        [Test]
        public void Matrix_OperatorDividesByFactorCorrectly()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = matrix1 / 10;

            TheResultingValue(result).ShouldBe(
                 10f, 20f, 30f, 40f,
                 50f, 60f, 70f, 80f,
                 90f, 100f, 110f, 120f,
                130f, 140f, 150f, 160f
            );
        }

        [Test]
        public void Matrix_MultipliesCorrectly()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40, 
                 50,  60,  70,  80, 
                 90, 100, 110, 120, 
                130, 140, 150, 160);
            
            var result = Matrix.Multiply(matrix1, matrix2);

            TheResultingValue(result).ShouldBe(
                 90000f, 100000f, 110000f, 120000f,
                202000f, 228000f, 254000f, 280000f,
                314000f, 356000f, 398000f, 440000f,
                426000f, 484000f, 542000f, 600000f
            );
        }

        [Test]
        public void Matrix_MultipliesCorrectlyWithOutParam()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400,
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40,
                 50,  60,  70,  80,
                 90, 100, 110, 120,
                130, 140, 150, 160);

            var result = Matrix.Identity;
            Matrix.Multiply(ref matrix1, ref matrix2, out result);

            TheResultingValue(result).ShouldBe(
                 90000f, 100000f, 110000f, 120000f,
                202000f, 228000f, 254000f, 280000f,
                314000f, 356000f, 398000f, 440000f,
                426000f, 484000f, 542000f, 600000f
            );
        }

        [Test]
        public void Matrix_OperatorMultipliesCorrectly()
        {
            var matrix1 = new Matrix(
                 100,  200,  300,  400,
                 500,  600,  700,  800,
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var matrix2 = new Matrix(
                 10,  20,  30,  40,
                 50,  60,  70,  80, 
                 90, 100, 110, 120,
                130, 140, 150, 160);

            var result = matrix1 * matrix2;

            TheResultingValue(result).ShouldBe(
                 90000f, 100000f, 110000f, 120000f,
                202000f, 228000f, 254000f, 280000f,
                314000f, 356000f, 398000f, 440000f,
                426000f, 484000f, 542000f, 600000f
            );
        }

        [Test]
        public void Matrix_MultipliesByFactorCorrectly()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200,
                1300, 1400, 1500, 1600);

            var result = Matrix.Multiply(matrix, 10f);

            TheResultingValue(result).ShouldBe(
                 1000f,  2000f,  3000f,  4000f,
                 5000f,  6000f,  7000f,  8000f,
                 9000f, 10000f, 11000f, 12000f,
                13000f, 14000f, 15000f, 16000f
            );
        }

        [Test]
        public void Matrix_MultipliesByFactorCorrectlyWithOutParam()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Identity;
            Matrix.Multiply(ref matrix, 10f, out result);

            TheResultingValue(result).ShouldBe(
                 1000f, 2000f, 3000f, 4000f,
                 5000f, 6000f, 7000f, 8000f,
                 9000f, 10000f, 11000f, 12000f,
                13000f, 14000f, 15000f, 16000f
            );
        }

        [Test]
        public void Matrix_OperatorMultipliesByFactorCorrectly()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400,
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = matrix * 10f;

            TheResultingValue(result).ShouldBe(
                 1000f, 2000f, 3000f, 4000f,
                 5000f, 6000f, 7000f, 8000f,
                 9000f, 10000f, 11000f, 12000f,
                13000f, 14000f, 15000f, 16000f
            );
        }

        [Test]
        public void Matrix_ParseHandlesValidInput()
        {
            var result = Matrix.Parse("11 12 13 14 21 22 23 24 31 32 33 34 41 42 43 44");

            TheResultingValue(result).ShouldBe(
                11f, 12f, 13f, 14f,
                21f, 22f, 23f, 24f,
                31f, 32f, 33f, 34f,
                41f, 42f, 43f, 44f
            );
        }

        [Test]
        public void Matrix_ParseRejectsInvalidInput()
        {
            Assert.That(() => Matrix.Parse("sfdjhkfsh"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Matrix_TryParseHandlesValidInput()
        {
            var result    = Matrix.Identity;
            var succeeded = Matrix.TryParse("11 12 13 14 21 22 23 24 31 32 33 34 41 42 43 44", out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).ShouldBe(
                11f, 12f, 13f, 14f,
                21f, 22f, 23f, 24f,
                31f, 32f, 33f, 34f,
                41f, 42f, 43f, 44f
            );
        }

        [Test]
        public void Matrix_TryParseRejectsInvalidInput()
        {
            var result    = Matrix.Identity;
            var succeeded = Matrix.TryParse("asdfasdfasdfs", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Matrix_NegateCalculatedCorrectly()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400,
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Negate(matrix);

            TheResultingValue(result).ShouldBe(
                 -100f,  -200f,  -300f,  -400f,
                 -500f,  -600f,  -700f,  -800f,
                 -900f, -1000f, -1100f, -1200f,
                -1300f, -1400f, -1500f, -1600f
            );
        }

        [Test]
        public void Matrix_NegateCalculatedCorrectlyWithOutParam()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = Matrix.Identity;
            Matrix.Negate(ref matrix, out result);

            TheResultingValue(result).ShouldBe(
                 -100f,  -200f,  -300f,  -400f,
                 -500f,  -600f,  -700f,  -800f,
                 -900f, -1000f, -1100f, -1200f,
                -1300f, -1400f, -1500f, -1600f
            );
        }

        [Test]
        public void Matrix_OperatorNegateCalculatedCorrectly()
        {
            var matrix = new Matrix(
                 100,  200,  300,  400, 
                 500,  600,  700,  800, 
                 900, 1000, 1100, 1200, 
                1300, 1400, 1500, 1600);

            var result = -matrix;

            TheResultingValue(result).ShouldBe(
                 -100f,  -200f,  -300f,  -400f,
                 -500f,  -600f,  -700f,  -800f,
                 -900f, -1000f, -1100f, -1200f,
                -1300f, -1400f, -1500f, -1600f
            );
        }

        [Test]
        public void Matrix_TransposeCalculatedCorrectly()
        {
            var matrix = new Matrix(
                11, 12, 13, 14, 
                21, 22, 23, 24, 
                31, 32, 33, 34, 
                41, 42, 43, 44);

            var result = Matrix.Transpose(matrix);

            TheResultingValue(result).ShouldBe(
                11f, 21f, 31f, 41f,
                12f, 22f, 32f, 42f,
                13f, 23f, 33f, 43f,
                14f, 24f, 34f, 44f);
        }

        [Test]
        public void Matrix_TransposeCalculatedCorrectlyWithOutParam()
        {
            var matrix = new Matrix(
                11, 12, 13, 14,
                21, 22, 23, 24,
                31, 32, 33, 34,
                41, 42, 43, 44);

            var result = Matrix.Identity;
            Matrix.Transpose(ref matrix, out result);

            TheResultingValue(result).ShouldBe(
                11f, 21f, 31f, 41f,
                12f, 22f, 32f, 42f,
                13f, 23f, 33f, 43f,
                14f, 24f, 34f, 44f);
        }

        [Test]
        public void Matrix_InvertCalculatedCorrectly()
        {
            var matrix = new Matrix(
                4, 0, 0, 0,
                0, 0, 2, 0, 
                0, 1, 2, 0, 
                1, 0, 0, 1);

            var result = Matrix.Invert(matrix);

            TheResultingValue(result).ShouldBe(
                 0.25f,  0.00f,  0.00f,  0.00f,
                 0.00f, -1.00f,  1.00f,  0.00f,
                 0.00f,  0.50f,  0.00f,  0.00f,
                -0.25f,  0.00f,  0.00f,  1.00f
            );
        }

        [Test]
        public void Matrix_InvertCalculatedCorrectlyWithOutParam()
        {
            var matrix = new Matrix(
                4, 0, 0, 0,
                0, 0, 2, 0, 
                0, 1, 2, 0, 
                1, 0, 0, 1);

            var result = Matrix.Identity;
            Matrix.Invert(ref matrix, out result);

            TheResultingValue(result).ShouldBe(
                 0.25f,  0.00f,  0.00f,  0.00f,
                 0.00f, -1.00f,  1.00f,  0.00f,
                 0.00f,  0.50f,  0.00f,  0.00f,
                -0.25f,  0.00f,  0.00f,  1.00f
            );
        }

        [Test]
        public void Matrix_TryInvertCalculatedCorrectly()
        {
            var matrix = new Matrix(
                4, 0, 0, 0,
                0, 0, 2, 0,
                0, 1, 2, 0,
                1, 0, 0, 1);

            Matrix result;
            var succeeded = Matrix.TryInvert(matrix, out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).ShouldBe(
                 0.25f, 0.00f, 0.00f, 0.00f,
                 0.00f, -1.00f, 1.00f, 0.00f,
                 0.00f, 0.50f, 0.00f, 0.00f,
                -0.25f, 0.00f, 0.00f, 1.00f
            );
        }

        [Test]
        public void Matrix_TryInvertRefCalculatedCorrectly()
        {
            var matrix = new Matrix(
                4, 0, 0, 0,
                0, 0, 2, 0,
                0, 1, 2, 0,
                1, 0, 0, 1);

            Matrix result;
            var succeeded = Matrix.TryInvertRef(ref matrix, out result);

            TheResultingValue(succeeded).ShouldBe(true);
            TheResultingValue(result).ShouldBe(
                 0.25f, 0.00f, 0.00f, 0.00f,
                 0.00f, -1.00f, 1.00f, 0.00f,
                 0.00f, 0.50f, 0.00f, 0.00f,
                -0.25f, 0.00f, 0.00f, 1.00f
            );
        }

        [Test]
        public void Matrix_TryInvertFailsForSingularMatrix()
        {
            var matrix = new Matrix(
                16,  2,  3, 13,
                 5, 11, 10,  8,
                 9,  7,  6, 12,
                 4, 14, 15,  1);

            Matrix result;
            var succeeded = Matrix.TryInvert(matrix, out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Matrix_TryInvertRefFailsForSingularMatrix()
        {
            var matrix = new Matrix(
                16,  2,  3, 13,
                 5, 11, 10,  8,
                 9,  7,  6, 12,
                 4, 14, 15,  1);

            Matrix result;
            var succeeded = Matrix.TryInvertRef(ref matrix, out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Matrix_CreateFromAxisAngleCalculatedCorrectly()
        {
            var axis  = Vector3.Normalize(new Vector3(5.1f, 3.2f, 7.4f));
            var angle = (float)Math.PI;

            var result = Matrix.CreateFromAxisAngle(axis, angle);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                -0.4284f,  0.3586f, 0.8293f, 0.0000f,
                 0.3586f, -0.7749f, 0.5203f, 0.0000f,
                 0.8293f,  0.5203f, 0.2033f, 0.0000f,
                 0.0000f,  0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateFromAxisAngleCalculatedCorrectlyWithOutParam()
        {
            var axis   = Vector3.Normalize(new Vector3(5.1f, 3.2f, 7.4f));
            var angle  = (float)Math.PI;
            
            var result = Matrix.Identity;
            Matrix.CreateFromAxisAngle(ref axis, angle, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                -0.4284f,  0.3586f, 0.8293f, 0.0000f,
                 0.3586f, -0.7749f, 0.5203f, 0.0000f,
                 0.8293f,  0.5203f, 0.2033f, 0.0000f,
                 0.0000f,  0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateLookAtCalculatedCorrectly()
        {
            var cameraPosition = new Vector3(10, 10, 10);
            var cameraTarget   = new Vector3(0, 0, 0);
            var cameraUp       = new Vector3(0, 1, 0);

            var result = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUp);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.7071f, -0.4082f,   0.5773f, 0.0000f,
                 0.0000f,  0.8164f,   0.5773f, 0.0000f,
                -0.7071f, -0.4082f,   0.5773f, 0.0000f,
                 0.0000f,  0.0000f, -17.3205f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateLookAtCalculatedCorrectlyWithOutParam()
        {
            var cameraPosition = new Vector3(10, 10, 10);
            var cameraTarget   = new Vector3(0, 0, 0);
            var cameraUp       = new Vector3(0, 1, 0);
            
            var result = Matrix.Identity;
            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUp, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.7071f, -0.4082f,   0.5773f, 0.0000f,
                 0.0000f,  0.8164f,   0.5773f, 0.0000f,
                -0.7071f, -0.4082f,   0.5773f, 0.0000f,
                 0.0000f,  0.0000f, -17.3205f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateOrthographicCalculatedCorrectly()
        {
            var result = Matrix.CreateOrthographic(1024f, 768f, 1f, 1000f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0019f, 0.0000f,  0.0000f, 0.0000f,
                0.0000f, 0.0026f,  0.0000f, 0.0000f,
                0.0000f, 0.0000f, -0.0010f, 0.0000f,
                0.0000f, 0.0000f, -0.0010f, 1.000f);
        }

        [Test]
        public void Matrix_CreateOrthographicCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateOrthographic(1024f, 768f, 1f, 1000f, out result);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0019f, 0.0000f,  0.0000f, 0.0000f,
                0.0000f, 0.0026f,  0.0000f, 0.0000f,
                0.0000f, 0.0000f, -0.0010f, 0.0000f,
                0.0000f, 0.0000f, -0.0010f, 1.000f);
        }

        [Test]
        public void Matrix_CreateOrthographicOffCenterCalculatedCorrectly()
        {
            var result = Matrix.CreateOrthographicOffCenter(128f, 1024f, 768f, 64f, 1f, 1000f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.0022f,  0.0000f,  0.0000f, 0.0000f,
                 0.0000f, -0.0028f,  0.0000f, 0.0000f,
                 0.0000f,  0.0000f, -0.0010f, 0.0000f,
                -1.2857f,  1.1818f, -0.0010f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateOrthographicOffCenterCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateOrthographicOffCenter(128f, 1024f, 768f, 64f, 1f, 1000f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.0022f,  0.0000f,  0.0000f, 0.0000f,
                 0.0000f, -0.0028f,  0.0000f, 0.0000f,
                 0.0000f,  0.0000f, -0.0010f, 0.0000f,
                -1.2857f,  1.1818f, -0.0010f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveCalculatedCorrectly()
        {
            var result = Matrix.CreatePerspective(1024f, 768f, 1f, 1000f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0019f, 0.0000f,  0.0000f,  0.0000f,
                0.0000f, 0.0026f,  0.0000f,  0.0000f,
                0.0000f, 0.0000f, -1.0010f, -1.0000f,
                0.0000f, 0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreatePerspective(1024f, 768f, 1f, 1000f, out result);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0019f, 0.0000f,  0.0000f,  0.0000f,
                0.0000f, 0.0026f,  0.0000f,  0.0000f,
                0.0000f, 0.0000f, -1.0010f, -1.0000f,
                0.0000f, 0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveFieldOfViewCalculatedCorrectly()
        {
            var result = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2f, 1024f / 768f, 1f, 1000f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.7500f, 0.0000f,  0.0000f,  0.0000f,
                0.0000f, 1.0000f,  0.0000f,  0.0000f,
                0.0000f, 0.0000f, -1.0010f, -1.0000f,
                0.0000f, 0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveFieldOfViewCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2f, 1024f / 768f, 1f, 1000f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.7500f, 0.0000f,  0.0000f,  0.0000f,
                0.0000f, 1.0000f,  0.0000f,  0.0000f,
                0.0000f, 0.0000f, -1.0010f, -1.0000f,
                0.0000f, 0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveOffCenterCalculatedCorrectly()
        {
            var result = Matrix.CreatePerspectiveOffCenter(128f, 1024f, 768f, 64f, 1f, 1000f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0022f,  0.0000f,  0.0000f,  0.0000f,
                0.0000f, -0.0028f,  0.0000f,  0.0000f,
                1.2857f, -1.1818f, -1.0010f, -1.0000f,
                0.0000f,  0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreatePerspectiveOffCenterCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreatePerspectiveOffCenter(128f, 1024f, 768f, 64f, 1f, 1000f, out result);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.0022f,  0.0000f,  0.0000f,  0.0000f,
                0.0000f, -0.0028f,  0.0000f,  0.0000f,
                1.2857f, -1.1818f, -1.0010f, -1.0000f,
                0.0000f,  0.0000f, -1.0010f,  0.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationXCalculatedCorrectly()
        {
            var result = Matrix.CreateRotationX(0.1234f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                1.0000f,  0.0000f, 0.0000f, 0.0000f, 
                0.0000f,  0.9923f, 0.1230f, 0.0000f,
                0.0000f, -0.1230f, 0.9923f, 0.0000f,
                0.0000f,  0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationXCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateRotationX(0.1234f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                1.0000f,  0.0000f, 0.0000f, 0.0000f, 
                0.0000f,  0.9923f, 0.1230f, 0.0000f,
                0.0000f, -0.1230f, 0.9923f, 0.0000f,
                0.0000f,  0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationYCalculatedCorrectly()
        {
            var result = Matrix.CreateRotationY(0.1234f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.9923f, 0.0000f, -0.1230f, 0.0000f,
                0.0000f, 1.0000f,  0.0000f, 0.0000f,
                0.1230f, 0.0000f,  0.9923f, 0.0000f,
                0.0000f, 0.0000f,  0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationYCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateRotationY(0.1234f, out result);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                0.9923f, 0.0000f, -0.1230f, 0.0000f,
                0.0000f, 1.0000f,  0.0000f, 0.0000f,
                0.1230f, 0.0000f,  0.9923f, 0.0000f,
                0.0000f, 0.0000f,  0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationZCalculatedCorrectly()
        {
            var result = Matrix.CreateRotationZ(0.1234f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.9923f, 0.1230f, 0.0000f, 0.0000f,
                -0.1230f, 0.9923f, 0.0000f, 0.0000f,
                 0.0000f, 0.0000f, 1.0000f, 0.0000f,
                 0.0000f, 0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateRotationZCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateRotationZ(0.1234f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 0.9923f, 0.1230f, 0.0000f, 0.0000f,
                -0.1230f, 0.9923f, 0.0000f, 0.0000f,
                 0.0000f, 0.0000f, 1.0000f, 0.0000f,
                 0.0000f, 0.0000f, 0.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateScaleCalculatedCorrectly()
        {
            var result = Matrix.CreateScale(111f, 222f, 333f);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                111f,   0f,   0f,   0f,
                  0f, 222f,   0f,   0f,
                  0f,   0f, 333f,   0f,
                  0f,   0f,   0f,   1f
            );
        }

        [Test]
        public void Matrix_CreateScaleCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateScale(111f, 222f, 333f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                111f,   0f,   0f, 0f,
                  0f, 222f,   0f, 0f,
                  0f,   0f, 333f, 0f,
                  0f,   0f,   0f, 1f
            );
        }

        [Test]
        public void Matrix_CreateTranslationCalculatedCorrectly()
        {
            var result = Matrix.CreateTranslation(111f, 222f, 333f);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                  1f,   0f,   0f, 0f,
                  0f,   1f,   0f, 0f,
                  0f,   0f,   1f, 0f,
                111f, 222f, 333f, 1f
            );
        }

        [Test]
        public void Matrix_CreateTranslationCalculatedCorrectlyWithOutParam()
        {
            var result = Matrix.Identity;
            Matrix.CreateTranslation(111f, 222f, 333f, out result);

            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                  1f,   0f,   0f, 0f,
                  0f,   1f,   0f, 0f,
                  0f,   0f,   1f, 0f,
                111f, 222f, 333f, 1f
            );
        }

        [Test]
        public void Matrix_CreateWorldCalculatedCorrectly()
        {
            var position = new Vector3(11, 12, 13);
            var forward  = new Vector3(0, 0, 1);
            var up       = new Vector3(0, 1, 0);
            
            var result = Matrix.CreateWorld(position, forward, up);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 -1.0000f,  0.0000f,  0.0000f, 0.0000f,
                  0.0000f,  1.0000f,  0.0000f, 0.0000f,
                  0.0000f,  0.0000f, -1.0000f, 0.0000f,
                 11.0000f, 12.0000f, 13.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_CreateWorldCalculatedCorrectlyWithOutParam()
        {
            var position = new Vector3(11, 12, 13);
            var forward  = new Vector3(0, 0, 1);
            var up       = new Vector3(0, 1, 0);
            
            var result = Matrix.Identity;
            Matrix.CreateWorld(ref position, ref forward, ref up, out result);
            
            TheResultingValue(result).WithinDelta(0.001f).ShouldBe(
                 -1.0000f,  0.0000f,  0.0000f, 0.0000f,
                  0.0000f,  1.0000f,  0.0000f, 0.0000f,
                  0.0000f,  0.0000f, -1.0000f, 0.0000f,
                 11.0000f, 12.0000f, 13.0000f, 1.0000f
            );
        }

        [Test]
        public void Matrix_SerializesToJson()
        {
            var matrix = Matrix.Identity;
            var json = JsonConvert.SerializeObject(matrix, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"[1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0]");
        }

        [Test]
        public void Matrix_SerializesToJson_WhenNullable()
        {
            var matrix = Matrix.Identity;
            var json = JsonConvert.SerializeObject((Matrix?)matrix, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"[1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0]");
        }

        [Test]
        public void Matrix_DeserializesFromJson()
        {
            const String json1 = @"[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16]";
            
            var matrix1 = JsonConvert.DeserializeObject<Matrix?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(matrix1.Value)
                .ShouldBe(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            const String json2 = @"null";

            var matrix2 = JsonConvert.DeserializeObject<Matrix?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(matrix2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Matrix_ConvertsFromSystemNumericsCorrectly()
        {
            var matrix1 = new Matrix(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f);
            System.Numerics.Matrix4x4 matrix2 = matrix1;

            TheResultingValue(matrix1.M11).ShouldBe(matrix2.M11);
            TheResultingValue(matrix1.M12).ShouldBe(matrix2.M12);
            TheResultingValue(matrix1.M13).ShouldBe(matrix2.M13);
            TheResultingValue(matrix1.M14).ShouldBe(matrix2.M14);

            TheResultingValue(matrix1.M21).ShouldBe(matrix2.M21);
            TheResultingValue(matrix1.M22).ShouldBe(matrix2.M22);
            TheResultingValue(matrix1.M23).ShouldBe(matrix2.M23);
            TheResultingValue(matrix1.M24).ShouldBe(matrix2.M24);

            TheResultingValue(matrix1.M31).ShouldBe(matrix2.M31);
            TheResultingValue(matrix1.M32).ShouldBe(matrix2.M32);
            TheResultingValue(matrix1.M33).ShouldBe(matrix2.M33);
            TheResultingValue(matrix1.M34).ShouldBe(matrix2.M34);

            TheResultingValue(matrix1.M41).ShouldBe(matrix2.M41);
            TheResultingValue(matrix1.M42).ShouldBe(matrix2.M42);
            TheResultingValue(matrix1.M43).ShouldBe(matrix2.M43);
            TheResultingValue(matrix1.M44).ShouldBe(matrix2.M44);
        }

        [Test]
        public void Matrix_ConvertsToSystemNumericsCorrectly()
        {
            var matrix1 = new System.Numerics.Matrix4x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f);
            Matrix matrix2 = matrix1;

            TheResultingValue(matrix1.M11).ShouldBe(matrix2.M11);
            TheResultingValue(matrix1.M12).ShouldBe(matrix2.M12);
            TheResultingValue(matrix1.M13).ShouldBe(matrix2.M13);
            TheResultingValue(matrix1.M14).ShouldBe(matrix2.M14);

            TheResultingValue(matrix1.M21).ShouldBe(matrix2.M21);
            TheResultingValue(matrix1.M22).ShouldBe(matrix2.M22);
            TheResultingValue(matrix1.M23).ShouldBe(matrix2.M23);
            TheResultingValue(matrix1.M24).ShouldBe(matrix2.M24);

            TheResultingValue(matrix1.M31).ShouldBe(matrix2.M31);
            TheResultingValue(matrix1.M32).ShouldBe(matrix2.M32);
            TheResultingValue(matrix1.M33).ShouldBe(matrix2.M33);
            TheResultingValue(matrix1.M34).ShouldBe(matrix2.M34);

            TheResultingValue(matrix1.M41).ShouldBe(matrix2.M41);
            TheResultingValue(matrix1.M42).ShouldBe(matrix2.M42);
            TheResultingValue(matrix1.M43).ShouldBe(matrix2.M43);
            TheResultingValue(matrix1.M44).ShouldBe(matrix2.M44);
        }
    }
}
