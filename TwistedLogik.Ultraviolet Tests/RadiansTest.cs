using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class RadiansTest : UltravioletTestFramework
    {
        /// <summary>
        /// Tests the Radians struct' Parse functionality when given raw values in radians.
        /// </summary>
        [TestMethod]
        public void RadiansTest_Parse_RawValue()
        {
            var result = Radians.Parse("3.14159");

            TheResultingValue(result).ShouldBe(3.14159f);
        }

        /// <summary>
        /// Tests the Radians struct' Parse functionality when given values relative to pi.
        /// </summary>
        [TestMethod]
        public void RadiansTest_Parse_FromPi()
        {
            var value1 = Radians.Parse("1 pi");
            TheResultingValue(value1).ShouldBe((float)Math.PI);
            
            var value1sym = Radians.Parse("1π");
            TheResultingValue(value1sym).ShouldBe((float)Math.PI);

            var value2 = Radians.Parse("2 pi");
            TheResultingValue(value2).ShouldBe((float)(2.0 * Math.PI));

            var value2sym = Radians.Parse("2π");
            TheResultingValue(value2sym).ShouldBe((float)(2.0 * Math.PI));

            var value3 = Radians.Parse("1/2 pi");
            TheResultingValue(value3).ShouldBe((float)(0.5 * Math.PI));

            var value3sym = Radians.Parse("1/2π");
            TheResultingValue(value3sym).ShouldBe((float)(0.5 * Math.PI));
        }

        /// <summary>
        /// Tests the Radians struct' Parse functionality when given negative values relative to pi.
        /// </summary>
        [TestMethod]
        public void RadiansTest_Parse_FromPi_Negative()
        {
            var value1 = Radians.Parse("-1 pi");
            TheResultingValue(value1).ShouldBe(-(float)Math.PI);

            var value1sym = Radians.Parse("-1π");
            TheResultingValue(value1sym).ShouldBe(-(float)Math.PI);
            
            var value2 = Radians.Parse("-2 pi");
            TheResultingValue(value2).ShouldBe(-(float)(2.0 * Math.PI));
            
            var value2sym = Radians.Parse("-2π");
            TheResultingValue(value2sym).ShouldBe(-(float)(2.0 * Math.PI));
            
            var value3 = Radians.Parse("-1/2 pi");
            TheResultingValue(value3).ShouldBe(-(float)(0.5 * Math.PI));

            var value3sym = Radians.Parse("-1/2π");
            TheResultingValue(value3sym).ShouldBe(-(float)(0.5 * Math.PI));
        }

        /// <summary>
        /// Tests the Radians struct' Parse functionality when given values relative to tau.
        /// </summary>
        [TestMethod]
        public void RadiansTest_Parse_FromTau()
        {
            var value1 = Radians.Parse("1 tau");
            TheResultingValue(value1).ShouldBe((float)(2.0 * Math.PI));

            var value1sym = Radians.Parse("1τ");
            TheResultingValue(value1sym).ShouldBe((float)(2.0 * Math.PI));

            var value2 = Radians.Parse("2 tau");
            TheResultingValue(value2).ShouldBe((float)(4.0 * Math.PI));

            var value2sym = Radians.Parse("2τ");
            TheResultingValue(value2sym).ShouldBe((float)(4.0 * Math.PI));

            var value3 = Radians.Parse("1/2 tau");
            TheResultingValue(value3).ShouldBe((float)Math.PI);

            var value3sym = Radians.Parse("1/2τ");
            TheResultingValue(value3sym).ShouldBe((float)Math.PI);
        }

        /// <summary>
        /// Tests the Radians struct' Parse functionality when given negative values relative to tau.
        /// </summary>
        [TestMethod]
        public void RadiansTest_Parse_FromTau_Negative()
        {
            var value1 = Radians.Parse("-1 tau");
            TheResultingValue(value1).ShouldBe(-(float)(2.0 * Math.PI));

            var value1sym = Radians.Parse("-1τ");
            TheResultingValue(value1sym).ShouldBe(-(float)(2.0 * Math.PI));

            var value2 = Radians.Parse("-2 tau");
            TheResultingValue(value2).ShouldBe(-(float)(4.0 * Math.PI));
            
            var value2sym = Radians.Parse("-2τ");
            TheResultingValue(value2sym).ShouldBe(-(float)(4.0 * Math.PI));

            var value3 = Radians.Parse("-1/2 tau");
            TheResultingValue(value3).ShouldBe(-(float)Math.PI);

            var value3sym = Radians.Parse("-1/2τ");
            TheResultingValue(value3sym).ShouldBe(-(float)Math.PI);
        }
    }
}
