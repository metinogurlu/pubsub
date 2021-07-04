using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PubSub
{
    public static class PubSubEventHandlerGenerator
    {
        public static Func<string, Task> GetEventHandlerFromDelegate<TEvent>(Func<TEvent, Task> func)
        {
            return async serializedEvent =>
            {
                var eventMessage = default(TEvent);

                try
                {
                    if (!string.IsNullOrWhiteSpace(serializedEvent))
                    {
                        eventMessage = JsonConvert.DeserializeObject<TEvent>(serializedEvent);
                    }
                }
                catch (Exception ex)
                {
                    throw new ConsumeException($"Event data could not be cast to type {typeof(TEvent)}", ex);
                }

                try
                {
                    await func(eventMessage);
                }
                catch (Exception ex)
                {
                    throw new ConsumeException($"Could not invoke subscriber delegate for {typeof(TEvent)}", ex);
                }
            };
        }
    }
}