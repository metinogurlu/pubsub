using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Basket;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Message
{
    public class PaymentSucceededMessage : PubSubMessage
    {
        public string UserId { get; set; }   
        public string PaymentId { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }

        public PaymentSucceededMessage()
        {
            
        }

        public PaymentSucceededMessage(string userId, string paymentId, IEnumerable<BasketItem> items)
        {
            UserId = userId;
            PaymentId = paymentId;
            Items = items;
        }
    }
}