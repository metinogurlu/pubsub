using System.Collections.Generic;
using ExampleEcommerceCheckoutFlowApp.Basket;

namespace ExampleEcommerceCheckoutFlowApp.Inventory
{
    public interface IInventoryService
    {
        void ReduceStockLevel(IEnumerable<BasketItem> items);
    }
}