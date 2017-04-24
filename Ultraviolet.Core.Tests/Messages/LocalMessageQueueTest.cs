using System;
using NUnit.Framework;
using Ultraviolet.Core.Messages;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.IO
{
    [TestFixture]
    public class LocalMessageQueueTest : CoreTestFramework
    {
        private class MockMessageSubscriber : IMessageSubscriber<Int32>
        {
            public Boolean ReceivedMessage { get; private set; }

            public void ReceiveMessage(Int32 type, MessageData data)
            {
                ReceivedMessage = true;
            }
        }

        [Test]
        public void LocalMessageQueue_SubscriberReceivesMessages()
        {
            var queue = new LocalMessageQueue<Int32>();
            var subscriber = new MockMessageSubscriber();
            
            queue.Subscribe(subscriber, 1);
            queue.Publish(1, null);
            queue.Process();

            TheResultingValue(subscriber.ReceivedMessage).ShouldBe(true);
        }

        [Test]
        public void LocalMessageQueue_SubscriberIgnoresMessages()
        {
            var queue = new LocalMessageQueue<Int32>();
            var subscriber = new MockMessageSubscriber();
            
            queue.Subscribe(subscriber, 1);
            queue.Publish(2, null);
            queue.Process();

            TheResultingValue(subscriber.ReceivedMessage).ShouldBe(false);
        }

        [Test]
        public void LocalMessageQueue_UnsubscribeRemovesSubscriber()
        {
            var queue = new LocalMessageQueue<Int32>();
            var subscriber = new MockMessageSubscriber();

            queue.Subscribe(subscriber, 1);
            queue.Unsubscribe(subscriber, 1);
            queue.Publish(1, null);
            queue.Process();

            TheResultingValue(subscriber.ReceivedMessage).ShouldBe(false);
        }

        [Test]
        public void LocalMessageQueue_UnsubscribeAllRemovesSubscriber()
        {
            var queue = new LocalMessageQueue<Int32>();
            var subscriber = new MockMessageSubscriber();

            queue.Subscribe(subscriber, 1);
            queue.Unsubscribe(subscriber);
            queue.Publish(1, null);
            queue.Process();

            TheResultingValue(subscriber.ReceivedMessage).ShouldBe(false);
        }
    }
}
