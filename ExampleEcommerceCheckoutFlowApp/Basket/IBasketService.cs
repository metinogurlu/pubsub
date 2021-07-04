namespace ExampleEcommerceCheckoutFlowApp.Basket
{
    public interface IBasketService
    {
        IBasketService StartNewSession();
        IBasketService AddNewItem();
        IBasketService GoToCheckout();
    }
}