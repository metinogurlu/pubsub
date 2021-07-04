using System;
using System.Runtime.Serialization;

namespace PubSub
{
    [Serializable]
    public class ConsumeException : Exception
    {
        public ConsumeException()
        {
        }

        public ConsumeException(string message) : base(message)
        {
        }

        public ConsumeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConsumeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}