using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Basket;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Message
{
    public class OrderStartedMessage : PubSubMessage
    {
        public string UserId { get; set; }   
        public IEnumerable<BasketItem> Items { get; set; }

        public OrderStartedMessage(string userId, IEnumerable<BasketItem> items)
        {
            UserId = userId;
            Items = items;
            PrepareToPublish();
        }
    }
}