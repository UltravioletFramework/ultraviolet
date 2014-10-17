using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests
{
    [TestClass]
    public class MathUtilTest : NucleusTestFramework
    {
        [TestMethod]
        public void MathUtil_ClampByte()
        {
            var result1 = MathUtil.Clamp((byte)123, (byte)0, (byte)44);
            TheResultingValue(result1).ShouldBe((byte)44);

            var result2 = MathUtil.Clamp((byte)0, (byte)22, (byte)255);
            TheResultingValue(result2).ShouldBe((byte)22);
        }

        [TestMethod]
        public void MathUtil_ClampInt16()
        {
            var result1 = MathUtil.Clamp((short)123, (short)0, (short)44);
            TheResultingValue(result1).ShouldBe((short)44);

            var result2 = MathUtil.Clamp((short)0, (short)22, (short)255);
            TheResultingValue(result2).ShouldBe((short)22);

            var result3 = MathUtil.Clamp((short)-44, (short)-33, (short)255);
            TheResultingValue(result3).ShouldBe((short)-33);

            var result4 = MathUtil.Clamp((short)123, (short)-1000, (short)-25);
            TheResultingValue(result4).ShouldBe((short)-25);
        }

        [TestMethod]
        public void MathUtil_ClampInt32()
        {
            var result1 = MathUtil.Clamp((int)123, (int)0, (int)44);
            TheResultingValue(result1).ShouldBe((int)44);

            var result2 = MathUtil.Clamp((int)0, (int)22, (int)255);
            TheResultingValue(result2).ShouldBe((int)22);

            var result3 = MathUtil.Clamp((int)-44, (int)-33, (int)255);
            TheResultingValue(result3).ShouldBe((int)-33);

            var result4 = MathUtil.Clamp((int)123, (int)-1000, (int)-25);
            TheResultingValue(result4).ShouldBe((int)-25);
        }

        [TestMethod]
        public void MathUtil_ClampInt64()
        {
            var result1 = MathUtil.Clamp((long)123, (long)0, (long)44);
            TheResultingValue(result1).ShouldBe((long)44);

            var result2 = MathUtil.Clamp((long)0, (long)22, (long)255);
            TheResultingValue(result2).ShouldBe((long)22);

            var result3 = MathUtil.Clamp((long)-44, (long)-33, (long)255);
            TheResultingValue(result3).ShouldBe((long)-33);

            var result4 = MathUtil.Clamp((long)123, (long)-1000, (long)-25);
            TheResultingValue(result4).ShouldBe((long)-25);
        }

        [TestMethod]
        public void MathUtil_ClampUInt16()
        {
            var result1 = MathUtil.Clamp((ushort)123, (ushort)0, (ushort)44);
            TheResultingValue(result1).ShouldBe((ushort)44);

            var result2 = MathUtil.Clamp((ushort)0, (ushort)22, (ushort)255);
            TheResultingValue(result2).ShouldBe((ushort)22);
        }

        [TestMethod]
        public void MathUtil_ClampUInt32()
        {
            var result1 = MathUtil.Clamp((uint)123, (uint)0, (uint)44);
            TheResultingValue(result1).ShouldBe((uint)44);

            var result2 = MathUtil.Clamp((uint)0, (uint)22, (uint)255);
            TheResultingValue(result2).ShouldBe((uint)22);
        }

        [TestMethod]
        public void MathUtil_ClampUInt64()
        {
            var result1 = MathUtil.Clamp((ulong)123, (ulong)0, (ulong)44);
            TheResultingValue(result1).ShouldBe((ulong)44);

            var result2 = MathUtil.Clamp((ulong)0, (ulong)22, (ulong)255);
            TheResultingValue(result2).ShouldBe((ulong)22);
        }

        [TestMethod]
        public void MathUtil_ClampSingle()
        {
            var result1 = MathUtil.Clamp(123f, 0f, 44f);
            TheResultingValue(result1).ShouldBe(44f);

            var result2 = MathUtil.Clamp(0f, 22f, 255f);
            TheResultingValue(result2).ShouldBe(22f);

            var result3 = MathUtil.Clamp(-44f, -33f, 255f);
            TheResultingValue(result3).ShouldBe(-33f);

            var result4 = MathUtil.Clamp(123f, -1000f, -25f);
            TheResultingValue(result4).ShouldBe(-25f);
        }

        [TestMethod]
        public void MathUtil_ClampDouble()
        {
            var result1 = MathUtil.Clamp(123.0, 0.0, 44.0);
            TheResultingValue(result1).ShouldBe(44.0);

            var result2 = MathUtil.Clamp(0.0, 22.0, 255.0);
            TheResultingValue(result2).ShouldBe(22.0);

            var result3 = MathUtil.Clamp(-44.0, -33.0, 255.0);
            TheResultingValue(result3).ShouldBe(-33.0);

            var result4 = MathUtil.Clamp(123.0, -1000.0, -25.0);
            TheResultingValue(result4).ShouldBe(-25.0);
        }

        [TestMethod]
        public void MathUtil_LerpByte()
        {
            var result1 = MathUtil.Lerp((byte)0, (byte)100, 0.25f);
            TheResultingValue(result1).ShouldBe((byte)25);

            var result2 = MathUtil.Lerp((byte)0, (byte)100, 0.75f);
            TheResultingValue(result2).ShouldBe((byte)75);
        }

        [TestMethod]
        public void MathUtil_LerpInt16()
        {
            var result1 = MathUtil.Lerp((short)0, (short)100, 0.25f);
            TheResultingValue(result1).ShouldBe((short)25);

            var result2 = MathUtil.Lerp((short)0, (short)100, 0.75f);
            TheResultingValue(result2).ShouldBe((short)75);

            var result3 = MathUtil.Lerp((short)-50, (short)50, 0.25f);
            TheResultingValue(result3).ShouldBe((short)-25);

            var result4 = MathUtil.Lerp((short)-50, (short)50, 0.75f);
            TheResultingValue(result4).ShouldBe((short)25);
        }

        [TestMethod]
        public void MathUtil_LerpInt32()
        {
            var result1 = MathUtil.Lerp((int)0, (int)100, 0.25f);
            TheResultingValue(result1).ShouldBe((int)25);

            var result2 = MathUtil.Lerp((int)0, (int)100, 0.75f);
            TheResultingValue(result2).ShouldBe((int)75);

            var result3 = MathUtil.Lerp((int)-50, (int)50, 0.25f);
            TheResultingValue(result3).ShouldBe((int)-25);

            var result4 = MathUtil.Lerp((int)-50, (int)50, 0.75f);
            TheResultingValue(result4).ShouldBe((int)25);
        }

        [TestMethod]
        public void MathUtil_LerpInt64()
        {
            var result1 = MathUtil.Lerp((long)0, (long)100, 0.25f);
            TheResultingValue(result1).ShouldBe((long)25);

            var result2 = MathUtil.Lerp((long)0, (long)100, 0.75f);
            TheResultingValue(result2).ShouldBe((long)75);

            var result3 = MathUtil.Lerp((long)-50, (long)50, 0.25f);
            TheResultingValue(result3).ShouldBe((long)-25);

            var result4 = MathUtil.Lerp((long)-50, (long)50, 0.75f);
            TheResultingValue(result4).ShouldBe((long)25);
        }

        [TestMethod]
        public void MathUtil_LerpUInt16()
        {
            var result1 = MathUtil.Lerp((ushort)0, (ushort)100, 0.25f);
            TheResultingValue(result1).ShouldBe((ushort)25);

            var result2 = MathUtil.Lerp((ushort)0, (ushort)100, 0.75f);
            TheResultingValue(result2).ShouldBe((ushort)75);
        }

        [TestMethod]
        public void MathUtil_LerpUInt32()
        {
            var result1 = MathUtil.Lerp((uint)0, (uint)100, 0.25f);
            TheResultingValue(result1).ShouldBe((uint)25);

            var result2 = MathUtil.Lerp((uint)0, (uint)100, 0.75f);
            TheResultingValue(result2).ShouldBe((uint)75);
        }

        [TestMethod]
        public void MathUtil_LerpUInt64()
        {
            var result1 = MathUtil.Lerp((ulong)0, (ulong)100, 0.25f);
            TheResultingValue(result1).ShouldBe((ulong)25);

            var result2 = MathUtil.Lerp((ulong)0, (ulong)100, 0.75f);
            TheResultingValue(result2).ShouldBe((ulong)75);
        }

        [TestMethod]
        public void MathUtil_LerpSingle()
        {
            var result1 = MathUtil.Lerp(0f, 100f, 0.25f);
            TheResultingValue(result1).ShouldBe(25f);

            var result2 = MathUtil.Lerp(0f, 100f, 0.75f);
            TheResultingValue(result2).ShouldBe(75f);

            var result3 = MathUtil.Lerp(-50f, 50f, 0.25f);
            TheResultingValue(result3).ShouldBe(-25f);

            var result4 = MathUtil.Lerp(-50f, 50f, 0.75f);
            TheResultingValue(result4).ShouldBe(25f);
        }

        [TestMethod]
        public void MathUtil_LerpDouble()
        {
            var result1 = MathUtil.Lerp(0.0, 100.0, 0.25f);
            TheResultingValue(result1).ShouldBe(25f);

            var result2 = MathUtil.Lerp(0.0, 100.0, 0.75f);
            TheResultingValue(result2).ShouldBe(75f);

            var result3 = MathUtil.Lerp(-50.0, 50.0, 0.25f);
            TheResultingValue(result3).ShouldBe(-25f);

            var result4 = MathUtil.Lerp(-50.0, 50.0, 0.75f);
            TheResultingValue(result4).ShouldBe(25f); 
        }
    }
}
