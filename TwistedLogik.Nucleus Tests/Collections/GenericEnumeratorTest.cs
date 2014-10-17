using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.IO
{
    [TestClass]
    public class GenericEnumeratorTest : NucleusTestFramework
    {
        [TestMethod]
        public void GenericEnumerator_EnumeratesCorrectly()
        {
            var collection = new[] { 5, 7, 9, 11, 13 };

            var enumerator = new GenericEnumerator<Int32>(collection,
                (Object state, Int32 ix, out Int32 result) => 
                { 
                    var l = (Int32[])state;
                    if (ix < 0 || ix >= l.Length)
                    {
                        result = 0;
                        return false;
                    }
                    result = l[ix];
                    return true;
                });

            TheResultingValue(enumerator.MoveNext()).ShouldBe(true);
            TheResultingValue(enumerator.Current).ShouldBe(5);

            TheResultingValue(enumerator.MoveNext()).ShouldBe(true);
            TheResultingValue(enumerator.Current).ShouldBe(7);

            TheResultingValue(enumerator.MoveNext()).ShouldBe(true);
            TheResultingValue(enumerator.Current).ShouldBe(9);

            TheResultingValue(enumerator.MoveNext()).ShouldBe(true);
            TheResultingValue(enumerator.Current).ShouldBe(11);

            TheResultingValue(enumerator.MoveNext()).ShouldBe(true);
            TheResultingValue(enumerator.Current).ShouldBe(13);

            TheResultingValue(enumerator.MoveNext()).ShouldBe(false);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenericEnumerator_ThrowsInvalidOperationExceptionIfSourceIsChanged()
        {
            var collection = new[] { 5, 7, 9, 11, 13 };
            var version    = 0;

            var enumerator = new GenericEnumerator<Int32>(collection,
                (Object state, Int32 ix, out Int32 result) =>
                {
                    var l = (Int32[])state;
                    if (ix < 0 || ix >= l.Length)
                    {
                        result = 0;
                        return false;
                    }
                    result = l[ix];
                    return true;
                },
                (Object state) =>
                {
                    return version;
                });

            enumerator.MoveNext();

            version++;
            collection[0] = 5;

            enumerator.MoveNext();
        }
    }
}
