namespace MyHandmadeWebServer.ByTheCake.Services.Contracts
{
    using ViewModels.Products;

    using System.Collections.Generic;

    public interface IProductService
    {
        IEnumerable<ProductListingViewModel> All(string searchTerm = null);

        void Create(string name, decimal price, string imageUrl);

        bool Exists(int id);

        ProductDetailsViewModel Find(int id);

        IEnumerable<ProductInCartViewModel> FindProductsInCart(IEnumerable<int> ids);
    }
}
