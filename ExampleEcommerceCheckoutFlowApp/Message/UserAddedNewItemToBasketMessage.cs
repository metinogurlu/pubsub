using PubSub;

namespace ExampleEcommerceCheckoutFlowApp.Message
{
    public class UserAddedNewItemToBasketMessage : PubSubMessage
    {
        public string UserId { get; set; }   
        public string ProductId { get; set; }

        public UserAddedNewItemToBasketMessage(string userId, string productId)
        {
            UserId = userId;
            ProductId = productId;
            PrepareToPublish();
        }
    }
}