using System;
using Microsoft.Extensions.DependencyInjection;
using ExampleEcommerceCheckoutFlowApp.Analytics;
using ExampleEcommerceCheckoutFlowApp.Basket;
using ExampleEcommerceCheckoutFlowApp.Inventory;
using ExampleEcommerceCheckoutFlowApp.Payment;
using PubSub;

namespace ExampleEcommerceCheckoutFlowApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPublisherSubscriber, PublisherSubscriber>()
                .AddSingleton<IBasketService, BasketService>()
                .AddSingleton<IAnalyticsService, AnalyticsService>()
                .AddSingleton<IInventoryService, InventoryService>()
                .AddSingleton<IPaymentService, PaymentService>()
                .BuildServiceProvider();
            
            RunEcommerceEventBasedCheckoutFlow(serviceProvider);
            Console.ReadKey();
        }

        static void RunEcommerceEventBasedCheckoutFlow(IServiceProvider serviceProvider)
        {
            var basketService = serviceProvider.GetService<IBasketService>();
            var analyticsService = serviceProvider.GetService<IAnalyticsService>();
            var inventoryService = serviceProvider.GetService<IInventoryService>();
            var paymentService = serviceProvider.GetService<IPaymentService>();

            basketService
                .StartNewSession()
                .AddNewItem()
                .AddNewItem()
                .GoToCheckout();
        }
    }
}