using NUnit.Framework;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.IO
{
    [TestFixture]
    public class ExpandingPoolRegistryTest : NucleusTestFramework
    {
        private class PoolableObject1 { }
        private class PoolableObject2 { }

        [Test]
        public void ExpandingPoolRegistry_CanCreatePools()
        {
            var registry = new ExpandingPoolRegistry();
            registry.Create<PoolableObject1>(32);
            registry.Create<PoolableObject2>(64);

            var pool1 = registry.Get<PoolableObject1>();
            var pool2 = registry.Get<PoolableObject2>();

            TheResultingObject(pool1)
                .ShouldNotBeNull()
                .ShouldSatisfyTheCondition(x => x.Capacity == 32);

            TheResultingObject(pool2)
                .ShouldNotBeNull()
                .ShouldSatisfyTheCondition(x => x.Capacity == 64);
        }

        [Test]
        public void ExpandingPoolRegistry_CanCreatePoolsOnTheFly()
        {
            var registry = new ExpandingPoolRegistry();

            var pool1 = registry.Get<PoolableObject1>(32);
            var pool2 = registry.Get<PoolableObject1>(64);

            TheResultingObject(pool1)
                .ShouldNotBeNull()
                .ShouldBe(pool2);
        }
    }
}
