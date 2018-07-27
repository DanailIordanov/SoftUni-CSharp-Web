namespace MyHandmadeWebServer.ByTheCake
{
    using Controllers;
    using Data;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using ViewModels.Account;
    using ViewModels.Products;

    using Microsoft.EntityFrameworkCore;

    public class ByTheCakeApplication : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get(
                    "/",
                    req => new HomeController().Index());

            appRouteConfig
                .Get(
                    "/about",
                    req => new HomeController().About());

            appRouteConfig
                .Get(
                    "/add",
                    req => new ProductsController().Add());

            appRouteConfig
                .Post(
                    "/add",
                    req => new ProductsController().Add(
                        new AddProductViewModel
                        {
                            Name = req.FormData["name"],
                            Price = decimal.Parse(req.FormData["price"]),
                            ImageUrl = req.FormData["imageUrl"]
                        }));
            
            appRouteConfig
                .Get(
                    "/products/{(?<id>[0-9]+)}",
                    req => new ProductsController().Details(int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Get(
                    "/search",
                    req => new ProductsController().Search(req));

            appRouteConfig
                .Get(
                    "/register",
                    req => new AccountController().Register());

            appRouteConfig
                .Post(
                    "/register",
                    req => new AccountController().Register(
                        req,
                        new RegisterViewModel
                        {
                            Username = req.FormData["username"],
                            Password = req.FormData["password"],
                            ConfirmPassword = req.FormData["confirm-password"]
                        }));

            appRouteConfig
                .Get(
                    "/login",
                    req => new AccountController().Login());

            appRouteConfig
                .Post(
                    "/login",
                    req => new AccountController().Login(
                        req,
                        new LoginViewModel
                        {
                            Username = req.FormData["username"],
                            Password = req.FormData["password"]
                        }));

            appRouteConfig
                .Get(
                    "/profile",
                    req => new AccountController().Profile(req));

            appRouteConfig
                .Post(
                    "/logout",
                    req => new AccountController().Logout(req));

            appRouteConfig
                .Get(
                    "/shopping/add/{(?<id>[0-9]+)}",
                    req => new ShoppingController().AddToCart(req));

            appRouteConfig
                .Get(
                    "/orders",
                    req => new ShoppingController().Orders(req));

            appRouteConfig
                .Get(
                    "/orders/{(?<id>[0-9]+)}",
                    req => new ShoppingController().OrderDetails(int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Get(
                    "/shopping/cart",
                    req => new ShoppingController().ShowCart(req));

            appRouteConfig
                .Post(
                    "/shopping/finish-order",
                    req => new ShoppingController().FinishOrder(req));
        }
    }
}
