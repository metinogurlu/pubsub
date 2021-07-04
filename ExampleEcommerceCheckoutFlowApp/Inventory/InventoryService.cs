using System;
using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Basket;
using ExampleEcommerceCheckoutFlowApp.Message;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Inventory
{
    public class InventoryService : IInventoryService, IDisposable
    {
        private readonly IPublisherSubscriber _publisherSubscriber;
        private List<Subscription> _subscriptions;
        
        public InventoryService(IPublisherSubscriber publisherSubscriber)
        {
            _publisherSubscriber = publisherSubscriber ?? throw new ArgumentNullException(nameof(publisherSubscriber));
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _subscriptions = new List<Subscription>
            {
                _publisherSubscriber.Subscribe(nameof(PaymentSucceededMessage),
                    async (PaymentSucceededMessage message) =>
                    {
                        ReduceStockLevel(message.Items);
                    })
            };
        }

        public void ReduceStockLevel(IEnumerable<BasketItem> items)
        {
            // Run business code here 
            foreach (var item in items)
            {
                Console.WriteLine(
                    $"{nameof(InventoryService)}: - Item: ({item.ProductId}) stock has been reduced by one");
            }
        }

        public void Dispose()
        {
            _subscriptions.ForEach(s => _publisherSubscriber.Unsubscribe(s));
        }
    }
}