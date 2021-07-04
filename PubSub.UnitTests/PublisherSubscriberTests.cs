using System.Collections.Concurrent;
using System.Threading;
using Xunit;

namespace PubSub.UnitTests
{
    public class PublisherSubscriberTests
    {
        [Fact]
        public void Should_PublishMessage_When_NoSubscriber()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            var message = new TestUserLoggedInMessage("User");
            
            // Act
            publisherSubscriber.Publish(topic, message);
        }
        
        [Fact]
        public void Should_Return_Subscription_When_Subscribed_Successfully()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            
            // Act
            var subscription = publisherSubscriber.Subscribe(topic, 
                async (TestUserLoggedInMessage message) => { });
            
            // Assert
            Assert.NotEmpty(subscription.SubscriptionId);
        }
        
        [Fact]
        public void Should_Consume_When_Message_Published()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            var pause = new ManualResetEvent(false);
            var messageToPublish = new TestUserLoggedInMessage("User");
            var messageToConsume = new TestUserLoggedInMessage();
            
            // Act
            publisherSubscriber.Subscribe(topic,
                async (TestUserLoggedInMessage message) =>
                {
                    messageToConsume = message;
                    pause.Set();
                });
            
            publisherSubscriber.Publish(topic, messageToPublish);

            // Assert
            Assert.True(pause.WaitOne(500));
            Assert.Equal(messageToPublish.MessageId, messageToConsume.MessageId);
            Assert.Equal(messageToPublish.UserId, messageToConsume.UserId);
            Assert.Equal(messageToPublish.PublishTime, messageToConsume.PublishTime);
        }
        
        [Fact]
        public void Should_ConsumeAll_When_Message_Published()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            var messageToPublished = new TestUserLoggedInMessage("User");
            var messagesToConsumed = new ConcurrentBag<TestUserLoggedInMessage>();
            var manualEvents =
                new[]
                {
                    new ManualResetEvent(false),
                    new ManualResetEvent(false),
                };
            // Act
            publisherSubscriber.Subscribe(topic,
                async (TestUserLoggedInMessage message) =>
                {
                    messagesToConsumed.Add(message);
                    manualEvents[0].Set();
                });
            
            publisherSubscriber.Subscribe(topic,
                async (TestUserLoggedInMessage message) =>
                {
                    messagesToConsumed.Add(message);
                    manualEvents[1].Set();
                });
            
            publisherSubscriber.Publish(topic, messageToPublished);
            WaitHandle.WaitAll(manualEvents, 500);
            
            // Assert
            Assert.Equal(2, messagesToConsumed.Count);
            Assert.All(messagesToConsumed, m => Assert.Equal(messageToPublished.MessageId, m.MessageId));
            Assert.All(messagesToConsumed, m => Assert.Equal(messageToPublished.UserId, m.UserId));
            Assert.All(messagesToConsumed, m => Assert.Equal(messageToPublished.PublishTime, m.PublishTime));
        }

        [Fact]
        public void Should_NotConsume_When_Message_Published_AnotherTopic()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            var anotherTopic = "AnotherTestTopic";
            var manualEvent = new ManualResetEvent(false);
            var messageToPublish = new TestUserLoggedInMessage("User");
            var messageToConsume = new TestUserLoggedInMessage();
            
            // Act
            publisherSubscriber.Subscribe(topic,
                async (TestUserLoggedInMessage message) =>
                {
                    messageToConsume = message;
                    manualEvent.Set();
                });
            
            publisherSubscriber.Publish(anotherTopic, messageToPublish);

            // Assert
            Assert.False(manualEvent.WaitOne(100));
            Assert.Null(messageToConsume.UserId);
            Assert.Null(messageToConsume.MessageId);
        }

        [Fact]
        public void Should_NotConsume_When_Unsubscribed_FromTopic()
        {
            // Arrange
            var publisherSubscriber = new PublisherSubscriber();
            var topic = "TestTopic";
            var manualEvent = new ManualResetEvent(false);
            var messageToPublish = new TestUserLoggedInMessage("User");
            var messageToConsume = new TestUserLoggedInMessage();
            
            // Act
            var subscription = publisherSubscriber.Subscribe(topic,
                async (TestUserLoggedInMessage message) =>
                {
                    messageToConsume = message;
                    manualEvent.Set();
                });
            publisherSubscriber.Unsubscribe(subscription);
            publisherSubscriber.Publish(topic, messageToPublish);

            // Assert
            Assert.False(manualEvent.WaitOne(100));
            Assert.Null(messageToConsume.UserId);
            Assert.Null(messageToConsume.MessageId);
        }
    }
}