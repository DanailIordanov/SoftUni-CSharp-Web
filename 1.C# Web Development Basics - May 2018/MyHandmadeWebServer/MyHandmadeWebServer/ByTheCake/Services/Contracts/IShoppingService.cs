namespace MyHandmadeWebServer.ByTheCake.Services.Contracts
{
    using ViewModels.Shopping;

    using System.Collections.Generic;

    public interface IShoppingService
    {
        void CreateOrder(int userId, IEnumerable<int> productIds);

        OrderDetailsViewModel GetOrder(int id);

        IEnumerable<OrderViewModel> GetOrders(string username);
    }
}
