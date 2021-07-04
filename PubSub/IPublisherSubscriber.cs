using System;
using System.Threading.Tasks;

namespace PubSub
{
    public interface IPublisherSubscriber
    {
        /// <summary>
        /// Publish message to topic
        /// </summary>
        /// <param name="topic">The topic to publish the message to</param>
        /// <param name="message">The message to publish</param>
        /// <typeparam name="TEvent">PubSubMessage</typeparam>
        void Publish<TEvent>(string topic, TEvent message) where TEvent : PubSubMessage;
        
        /// <summary>
        /// Subscribe a message on specific topic
        /// </summary>
        /// <param name="topic">The topic to subscribe the message</param>
        /// <param name="onEventReceived">The delegate to invoke after the message consumed</param>
        /// <typeparam name="TEvent">PubSubMessage</typeparam>
        /// <returns>Subscription</returns>
        Subscription Subscribe<TEvent>(string topic, Func<TEvent, Task> onEventReceived) where TEvent : PubSubMessage;
        
        /// <summary>
        /// Unsubscribe from message on specific topic
        /// </summary>
        /// <param name="subscription">The subscription info to use for unsubscription</param>
        void Unsubscribe(Subscription subscription);
    }
}