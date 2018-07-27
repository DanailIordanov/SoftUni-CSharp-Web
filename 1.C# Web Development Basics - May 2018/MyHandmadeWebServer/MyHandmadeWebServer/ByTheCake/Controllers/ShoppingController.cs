namespace MyHandmadeWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Services.Contracts;
    using ViewModels.Shopping;

    using System;
    using System.Linq;

    public class ShoppingController : Controller
    {
        private const string CartView = @"Shopping\cart";
        private const string FinishOrderView = @"Shopping\finish-order";
        private const string OrdersView = @"Shopping\orders";

        private readonly IProductService products;
        private readonly IUserService users;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.users = new UserService();
            this.products = new ProductService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest request)
        {
            var id = int.Parse(request.UrlParameters["id"]);
            
            if (!this.products.Exists(id))
            {
                return new NotFoundResponse();
            }

            request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).ProductIds.Add(id);

            const string searchKey = "term";

            var redirectUrl = "/search";

            if (request.UrlParameters.ContainsKey(searchKey))
            {
                redirectUrl = $"{redirectUrl}?{searchKey}={request.UrlParameters[searchKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse Orders(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var orders = this.shopping.GetOrders(username);

            if (!orders.Any())
            {
                this.ViewData["showTable"] = "none";
                this.ViewData["orders"] = "<tr></tr>";
                this.ViewData["showError"] = "block";
                this.ViewData["error"] = "You don't have any orders.";

                return this.FileViewResponse(OrdersView);
            }

            var orderDivs = orders
                .Select(o => $@"<tr><td><a href=""/orders/{o.OrderId}"">{o.OrderId}</a></td><td>{o.CreationDate.ToShortDateString()}</td><td>${o.TotalSum}</td></tr>");

            this.ViewData["orders"] = string.Join(Environment.NewLine, orderDivs);
            this.ViewData["showTable"] = "inline-block";
            this.ViewData["showError"] = "none";

            return this.FileViewResponse(OrdersView);
        }

        public IHttpResponse OrderDetails(int id)
        {
            var order = this.shopping.GetOrder(id);

            var productDivs = order
                .Products
                .Select(p => $@"<tr><td><a href=""/products/{p.Id}"">{p.Name}</a></td><td>${p.Price:f2}</td></tr>");

            this.ViewData["orderId"] = order.OrderId.ToString();
            this.ViewData["products"] = string.Join(Environment.NewLine, productDivs);
            this.ViewData["creationDate"] = order.CreationDate.ToShortDateString();

            return this.FileViewResponse(@"Shopping\order-details");
        }

        public IHttpResponse ShowCart(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productsInCart = this.products.FindProductsInCart(shoppingCart.ProductIds);

                var cartItems = productsInCart.Select(p => $"<div>{p.Name} - ${p.Price:f2}</div><br />");

                var totalCost = productsInCart.Sum(p => p.Price);

                this.ViewData["cartItems"] = string.Join(string.Empty, cartItems);
                this.ViewData["totalCost"] = $"{totalCost:f2}";
            }

            return this.FileViewResponse(CartView);
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);
            var userId = this.users.GetUserId(username);

            if (userId == null)
            {
                throw new InvalidOperationException($"User {username} does not exist.");
            }

            var productIds = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).ProductIds;

            if (!productIds.Any())
            {
                return new RedirectResponse("/cart");
            }

            this.shopping.CreateOrder(userId.Value, productIds);
            productIds.Clear();

            return this.FileViewResponse(FinishOrderView);
        }
    }
}
