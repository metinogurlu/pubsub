using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PubSub
{
    public class PublisherSubscriber : IPublisherSubscriber
    {
        private readonly List<Subscription> _subscriptions = new List<Subscription>();
        
        /// <summary>
        /// Publish message to topic
        /// </summary>
        /// <param name="topic">The topic to publish the message to</param>
        /// <param name="message">The message to publish</param>
        /// <typeparam name="TEvent">PubSubMessage</typeparam>
        public void Publish<TEvent>(string topic, TEvent message) where TEvent : PubSubMessage
        {
            var subscriptionsToBeNotified 
                = _subscriptions.Where(s => s.Topic.Equals(topic)).ToList();
         
            var msg = JsonConvert.SerializeObject(message);

            subscriptionsToBeNotified.ForEach(s =>
                Task.Run(async () => await s.Handler(msg)));
        }
        
        /// <summary>
        /// Subscribe a message on specific topic
        /// </summary>
        /// <param name="topic">The topic to subscribe the message</param>
        /// <param name="onEventReceived">The delegate to invoke after the message consumed</param>
        /// <typeparam name="TEvent">PubSubMessage</typeparam>
        /// <returns>Subscription</returns>
        public Subscription Subscribe<TEvent>(string topic, Func<TEvent, Task> onEventReceived) where TEvent : PubSubMessage
        {
            var subscription = new Subscription(topic, 
                PubSubEventHandlerGenerator.GetEventHandlerFromDelegate(onEventReceived));
            _subscriptions.Add(subscription);
            
            return subscription;
        }
        
        /// <summary>
        /// Unsubscribe from message on specific topic
        /// </summary>
        /// <param name="subscription">The subscription info to use for unsubscription</param>
        public void Unsubscribe(Subscription subscription)
        {
            _subscriptions.Remove(subscription);
        }
    }
}