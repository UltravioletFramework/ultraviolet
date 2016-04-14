using NUnit.Framework;
using System;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.IO
{
    [TestFixture]
    public class PooledObjectScopeTest : NucleusTestFramework
    {
        private class PoolableDataObject { public String Data { get; set; } }

        [Test]
        public void PooledObjectScope_ScopeRetrievesAndReleasesObject()
        {
            var pool = new ExpandingPool<PoolableDataObject>(1);

            using (var scope = pool.RetrieveScoped())
            {
                TheResultingValue(pool.Capacity).ShouldBe(1);
                TheResultingValue(pool.Count).ShouldBe(1);

                scope.Object.Data = "Hello, world!";
            }

            TheResultingValue(pool.Capacity).ShouldBe(1);
            TheResultingValue(pool.Count).ShouldBe(0);
        }
    }
}
