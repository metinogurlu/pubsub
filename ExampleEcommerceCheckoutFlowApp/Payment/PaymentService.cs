using System;
using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Message;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Payment
{
    public class PaymentService : IPaymentService, IDisposable
    {
        private readonly IPublisherSubscriber _publisherSubscriber;
        private List<Subscription> _subscriptions;
        
        public PaymentService(IPublisherSubscriber publisherSubscriber)
        {
            _publisherSubscriber = publisherSubscriber ?? throw new ArgumentNullException(nameof(publisherSubscriber));
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _subscriptions = new List<Subscription>
            {
                _publisherSubscriber.Subscribe(nameof(OrderStartedMessage),
                    async (OrderStartedMessage message) =>
                    {
                        AcceptPayment(message);
                    })
            };
        }
        
        public void AcceptPayment(OrderStartedMessage orderStartedMessage)
        {
            Console.WriteLine(
                $"{nameof(PaymentService)}: - User: ({orderStartedMessage.UserId}) payment request received");

            var paymentId = Guid.NewGuid().ToString();
            var message = new PaymentSucceededMessage(orderStartedMessage.UserId, paymentId, orderStartedMessage.Items);
            _publisherSubscriber.Publish(nameof(PaymentSucceededMessage), message);
            
        }

        public void Dispose()
        {
            _subscriptions.ForEach(s => _publisherSubscriber.Unsubscribe(s));
        }
    }
}