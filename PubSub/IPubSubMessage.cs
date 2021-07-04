using System;

namespace PubSub
{
    public interface IPubSubMessage
    {
        string MessageId { get; set; }
        DateTime PublishTime { get; set; }
    }
}