using ExampleEcommerceCheckoutFlowApp.Message;

namespace ExampleEcommerceCheckoutFlowApp.Payment
{
    public interface IPaymentService
    {
        void AcceptPayment(OrderStartedMessage orderStartedMessage);
    }
}