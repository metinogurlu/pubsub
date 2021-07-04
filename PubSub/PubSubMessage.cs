using System;

namespace PubSub
{
    /// <summary>
    /// Base class for PubSub Messages
    /// </summary>
    public abstract class PubSubMessage : IPubSubMessage
    {
        public string MessageId { get; set; }
        public DateTime PublishTime { get; set; }

        protected void PrepareToPublish()
        {
            MessageId = Guid.NewGuid().ToString();
            PublishTime = DateTime.UtcNow;
        }
    }
}