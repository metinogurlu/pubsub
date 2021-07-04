using System;
using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Message;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IPublisherSubscriber _publisherSubscriber;
        private string _activeUserId;
        private List<BasketItem> _basket;

        public BasketService(IPublisherSubscriber publisherSubscriber)
        {
            _publisherSubscriber = publisherSubscriber ?? throw new ArgumentNullException(nameof(publisherSubscriber));
            _basket = new List<BasketItem>();
        }

        public IBasketService StartNewSession()
        {
            _activeUserId = Guid.NewGuid().ToString();
            _basket = new List<BasketItem>();

            return this;
        }

        public IBasketService AddNewItem()
        {
            var rand = new Random();
            var item = new BasketItem
            {
                ProductId = Guid.NewGuid().ToString(),
                UnitPrice = rand.Next(0, 1000),
            };
            
            _basket.Add(item);

            var message = new UserAddedNewItemToBasketMessage(_activeUserId, item.ProductId);
            _publisherSubscriber.Publish(nameof(UserAddedNewItemToBasketMessage), message);

            return this;
        }

        public IBasketService GoToCheckout()
        {
            var message = new OrderStartedMessage(_activeUserId, _basket);
            _publisherSubscriber.Publish(nameof(OrderStartedMessage), message);

            return this;
        }
    }
}