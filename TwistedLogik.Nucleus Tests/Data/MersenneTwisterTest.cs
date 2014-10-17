using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.Data
{
    [TestClass]
    public class MersenneTwisterTest : NucleusTestFramework
    {
        [TestMethod]
        public void MersenneTwister_SettingSeedResultsInSameSequence()
        {
            var rng = new MersenneTwister(12345);
            var result1 = new[]
            {
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
            };

            rng.Reseed(12345);
            var result2 = new[]
            {
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
                rng.Next(0, 1000),
            };

            TheResultingCollection(result2).ShouldBeExactly(result1);
        }
    }
}
