using System;
using System.Collections.Generic;
using System.Linq;
using ExampleEcommerceCheckoutFlowApp.Message;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Analytics
{
    public class AnalyticsService : IAnalyticsService, IDisposable
    {
        private readonly IPublisherSubscriber _publisherSubscriber;
        private List<Subscription> _subscriptions;
        
        public AnalyticsService(IPublisherSubscriber publisherSubscriber)
        {
            _publisherSubscriber = publisherSubscriber ?? throw new ArgumentNullException(nameof(publisherSubscriber));
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _subscriptions = new List<Subscription>
            {
                _publisherSubscriber.Subscribe(nameof(UserAddedNewItemToBasketMessage),
                    async (UserAddedNewItemToBasketMessage message) => { HandleUserAddedNewItemMessage(message); }),
                
                _publisherSubscriber.Subscribe(nameof(OrderStartedMessage),
                    async (OrderStartedMessage message) => { HandleOrderStartedMessage(message); }),
                
                _publisherSubscriber.Subscribe(nameof(PaymentSucceededMessage),
                    async (PaymentSucceededMessage message) => { HandlePaymentSucceededMessage(message); })
            };
        }
        
        public void HandleUserAddedNewItemMessage(UserAddedNewItemToBasketMessage message)
        {
            // Run business code here 
            Console.WriteLine($"{nameof(AnalyticsService)}: - User: ({message.UserId}) added a new product to basket");
        }

        public void HandleOrderStartedMessage(OrderStartedMessage message)
        {
            // Run business code here
            Console.WriteLine($"{nameof(AnalyticsService)}: - User: ({message.UserId}) started an order");
        }

        public void HandlePaymentSucceededMessage(PaymentSucceededMessage message)
        {
            // Run business code here
            var totalAmount = message.Items.Sum(i => i.UnitPrice);
            Console.WriteLine($"{nameof(AnalyticsService)}: - User: ({message.UserId}) " +
                              $"paid ({message.PaymentId}) total {totalAmount} USD successfully");
        }

        public void Dispose()
        {
            _subscriptions.ForEach(s => _publisherSubscriber.Unsubscribe(s));
        }
    }
}