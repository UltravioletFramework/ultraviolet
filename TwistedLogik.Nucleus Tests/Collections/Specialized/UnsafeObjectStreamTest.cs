using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections.Specialized;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.IO
{
    [TestClass]
    public partial class UnsafeObjectStreamTest : NucleusTestFramework
    {
        [TestMethod]
        public void UnsafeObjectStream_CorrectlyAddsStructuresToStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.Advance(sizeof(UnsafeObjectTypeOne));

                    stream.Reserve(sizeof(UnsafeObjectTypeTwo));
                    *(UnsafeObjectTypeTwo*)stream.Data = new UnsafeObjectTypeTwo() { ObjectType = UnsafeObjectType.TypeTwo, Value1 = 234, Value2 = 345 };
                    stream.Advance(sizeof(UnsafeObjectTypeTwo));
                    
                    TheResultingValue(stream.LengthInBytes).ShouldBe(sizeof(UnsafeObjectTypeOne) + sizeof(UnsafeObjectTypeTwo));
                    TheResultingValue(stream.LengthInObjects).ShouldBe(2);

                    var objData0 = stream.SeekObject(0);
                    var objType0 = *(UnsafeObjectType*)objData0;
                    TheResultingValue(objType0).ShouldBe(UnsafeObjectType.TypeOne);

                    var objTypeOne = *(UnsafeObjectTypeOne*)objData0;
                    TheResultingValue(objTypeOne.Value1).ShouldBe(123);

                    var objData1 = stream.SeekObject(1);
                    var objType1 = *(UnsafeObjectType*)objData1;
                    TheResultingValue(objType1).ShouldBe(UnsafeObjectType.TypeTwo);

                    var objTypeTwo = *(UnsafeObjectTypeTwo*)objData1;
                    TheResultingValue(objTypeTwo.Value1).ShouldBe(234);
                    TheResultingValue(objTypeTwo.Value2).ShouldBe(345);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }
    }
}
