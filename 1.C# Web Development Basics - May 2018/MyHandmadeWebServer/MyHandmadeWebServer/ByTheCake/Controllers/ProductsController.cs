namespace MyHandmadeWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Services.Contracts;
    using ViewModels.Shopping;
    using ViewModels.Products;

    using System;
    using System.Linq;

    public class ProductsController : Controller
    {
        private const string AddView = @"Products\add";
        private const string SearchView = @"Products\search";
        private const string DetailsView = @"Products\details";
        
        private readonly IProductService products;

        public ProductsController()
        {
            this.products = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Add(AddProductViewModel viewModel)
        {
            if (viewModel.Name.Length < 4 ||
                viewModel.Name.Length > 30)
            {
                this.AddError("The name should be between 4 and 30 symbols.");
            }

            if (viewModel.ImageUrl.Length < 3 ||
                viewModel.ImageUrl.Length > 2000)
            {
                this.AddError("Invalid url.");

                return this.FileViewResponse(AddView);
            }

            this.products.Create(viewModel.Name, viewModel.Price, viewModel.ImageUrl);

            this.ViewData["name"] = viewModel.Name;
            this.ViewData["price"] = viewModel.Price.ToString();
            this.ViewData["imageUrl"] = viewModel.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchKey = "term";

            var urlParameters = request.UrlParameters;

            var results = string.Empty;
            this.ViewData["results"] = string.Empty;
            
            var searchTerm = urlParameters.ContainsKey(searchKey) ? urlParameters[searchKey] : null;

            this.ViewData["searchTerm"] = searchTerm;

            var foundProducts = this.products.All(searchTerm);

            if (!foundProducts.Any())
            {
                this.ViewData["results"] = "<div>No cakes found.</div>";
            }
            else
            {
                var allProducts = foundProducts.Select(c => $@"<div><a href=""/products/{c.Id}"">{c.Name}</a> - ${c.Price:f2} <a href=""/shopping/add/{c.Id}?term={searchTerm}"">Order</a></div>");

                this.ViewData["results"] = string.Join(Environment.NewLine, allProducts);
            }
            
            this.ViewData["showCart"] = "none";

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                var totalProducts = shoppingCart.ProductIds.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }

            return this.FileViewResponse(SearchView);
        }

        public IHttpResponse Details(int id)
        {
            var product = this.products.Find(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = $"{product.Price:f2}";
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse(DetailsView);
        }
    }
}
