namespace PubSub.UnitTests
{
    public class TestUserLoggedInMessage : PubSubMessage
    {
        public string UserId { get; set; }

        public TestUserLoggedInMessage()
        {
        }

        public TestUserLoggedInMessage(string userId)
        {
            UserId = userId;
            PrepareToPublish();
        }
    }
}