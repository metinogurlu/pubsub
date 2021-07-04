using ExampleEcommerceCheckoutFlowApp.Message;

namespace ExampleEcommerceCheckoutFlowApp.Analytics
{
    public interface IAnalyticsService
    {
        void HandleUserAddedNewItemMessage(UserAddedNewItemToBasketMessage message);
        void HandleOrderStartedMessage(OrderStartedMessage message);
        void HandlePaymentSucceededMessage(PaymentSucceededMessage message);
    }
}