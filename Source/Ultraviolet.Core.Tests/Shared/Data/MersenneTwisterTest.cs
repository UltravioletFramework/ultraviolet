using NUnit.Framework;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.Data
{
    [TestFixture]
    public class MersenneTwisterTest : CoreTestFramework
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
