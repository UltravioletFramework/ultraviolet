using NUnit.Framework;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.Data
{
    [TestFixture]
    public class MersenneTwisterTest : NucleusTestFramework
    {
        [Test]
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
