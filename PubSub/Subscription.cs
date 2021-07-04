using System;
using System.Threading.Tasks;

namespace PubSub
{
    public class Subscription
    {
        public string SubscriptionId { get; private set; }
        public string Topic { get; private set; }
        public Func<string, Task> Handler { get; private set; }

        public Subscription(string topic, Func<string, Task> handler)
        {
            SubscriptionId = Guid.NewGuid().ToString();
            Topic = topic;
            Handler = handler;
        }
    }
}