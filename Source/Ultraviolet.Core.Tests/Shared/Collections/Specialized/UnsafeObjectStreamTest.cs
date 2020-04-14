using System;
using NUnit.Framework;
using Ultraviolet.Core.Collections.Specialized;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.IO
{
    [TestFixture]
    public partial class UnsafeObjectStreamTest : CoreTestFramework
    {
        [Test]
        public void UnsafeObjectStream_Reserve_ReservesSpaceForObject()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.Reserve(sizeof(UnsafeObjectTypeTwo));
                    *(UnsafeObjectTypeTwo*)stream.Data = new UnsafeObjectTypeTwo() { ObjectType = UnsafeObjectType.TypeTwo, Value1 = 234, Value2 = 345 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeTwo));
                    
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

        [Test]
        public void UnsafeObjectStream_Reserve_ThrowsException_WhenReservingDataBeforeEndOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.SeekBeginning();

                    Assert.That(() => stream.Reserve(sizeof(UnsafeObjectTypeOne)),
                        Throws.TypeOf<InvalidOperationException>());
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_ReserveMultiple_ReservesSpaceForMultipleObjects()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, sizeof(UnsafeObjectTypeOne) + sizeof(UnsafeObjectTypeTwo));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeTwo*)stream.Data = new UnsafeObjectTypeTwo() { ObjectType = UnsafeObjectType.TypeTwo, Value1 = 234, Value2 = 345 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeTwo));

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

        [Test]
        public void UnsafeObjectStream_ReserveMultiple_ThrowsException_WhenReservingDataBeforeEndOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.Reserve(sizeof(UnsafeObjectTypeOne));
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.SeekBeginning();

                    Assert.That(() => stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne)),
                        Throws.TypeOf<InvalidOperationException>());
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_ReserveInsert_ReservesSpaceForMultipleObjects()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.ReserveInsert(1, 2, 2 * sizeof(UnsafeObjectTypeTwo));

                    *(UnsafeObjectTypeTwo*)stream.Data = new UnsafeObjectTypeTwo() { ObjectType = UnsafeObjectType.TypeTwo, Value1 = 111, Value2 = 222 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeTwo));

                    *(UnsafeObjectTypeTwo*)stream.Data = new UnsafeObjectTypeTwo() { ObjectType = UnsafeObjectType.TypeTwo, Value1 = 333, Value2 = 444 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeTwo));
                    
                    TheResultingValue(stream.LengthInBytes).ShouldBe(2 * sizeof(UnsafeObjectTypeOne) + 2 * sizeof(UnsafeObjectTypeTwo));
                    TheResultingValue(stream.LengthInObjects).ShouldBe(4);

                    var objData0 = (UnsafeObjectTypeOne*)stream.SeekObject(0);
                    TheResultingValue(objData0->ObjectType).ShouldBe(UnsafeObjectType.TypeOne);
                    TheResultingValue(objData0->Value1).ShouldBe(123);

                    var objData1 = (UnsafeObjectTypeTwo*)stream.SeekObject(1);
                    TheResultingValue(objData1->ObjectType).ShouldBe(UnsafeObjectType.TypeTwo);
                    TheResultingValue(objData1->Value1).ShouldBe(111);
                    TheResultingValue(objData1->Value2).ShouldBe(222);

                    var objData2 = (UnsafeObjectTypeTwo*)stream.SeekObject(2);
                    TheResultingValue(objData2->ObjectType).ShouldBe(UnsafeObjectType.TypeTwo);
                    TheResultingValue(objData2->Value1).ShouldBe(333);
                    TheResultingValue(objData2->Value2).ShouldBe(444);

                    var objData3 = (UnsafeObjectTypeOne*)stream.SeekObject(3);
                    TheResultingValue(objData3->ObjectType).ShouldBe(UnsafeObjectType.TypeOne);
                    TheResultingValue(objData3->Value1).ShouldBe(234);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekBeginning_SeeksToBeginningOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));
                    
                    stream.RawSeekBeginning();

                    TheResultingValue(stream.PositionInObjects).ShouldBe(0);
                    TheResultingValue(stream.PositionInBytes).ShouldBe(0);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekEnd_SeeksToEndOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.RawSeekEnd();

                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects);
                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekForward_SeeksToNextObject_WhenNotAtEndOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.RawSeekObject(0);

                    TheResultingValue(stream.PositionInBytes).ShouldBe(0);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(0);

                    stream.SeekForward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(sizeof(UnsafeObjectTypeOne));
                    TheResultingValue(stream.PositionInObjects).ShouldBe(1);

                    stream.SeekForward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(sizeof(UnsafeObjectTypeOne) + sizeof(UnsafeObjectTypeOne));
                    TheResultingValue(stream.PositionInObjects).ShouldBe(2);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekForward_DoesNothing_WhenAtEndOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.RawSeekObject(stream.LengthInObjects);

                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects);

                    stream.SeekForward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekBackward_SeeksToPreviousObject_WhenNotAtBeginningOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.RawSeekObject(stream.LengthInObjects);

                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects);

                    stream.SeekBackward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes - (sizeof(UnsafeObjectTypeOne)));
                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects - 1);

                    stream.SeekBackward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(stream.LengthInBytes - (sizeof(UnsafeObjectTypeOne) + sizeof(UnsafeObjectTypeOne)));
                    TheResultingValue(stream.PositionInObjects).ShouldBe(stream.LengthInObjects - 2);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }

        [Test]
        public void UnsafeObjectStream_RawSeekBackward_DoesNothing_WhenAtBeginningOfStream()
        {
            var stream = new UnsafeObjectStream();

            stream.AcquirePointers();
            try
            {
                unsafe
                {
                    stream.ReserveMultiple(2, 2 * sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 123 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    *(UnsafeObjectTypeOne*)stream.Data = new UnsafeObjectTypeOne() { ObjectType = UnsafeObjectType.TypeOne, Value1 = 234 };
                    stream.FinalizeObject(sizeof(UnsafeObjectTypeOne));

                    stream.RawSeekObject(0);

                    TheResultingValue(stream.PositionInBytes).ShouldBe(0);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(0);

                    stream.SeekBackward();

                    TheResultingValue(stream.PositionInBytes).ShouldBe(0);
                    TheResultingValue(stream.PositionInObjects).ShouldBe(0);
                }
            }
            finally
            {
                stream.ReleasePointers();
            }
        }
    }
}
