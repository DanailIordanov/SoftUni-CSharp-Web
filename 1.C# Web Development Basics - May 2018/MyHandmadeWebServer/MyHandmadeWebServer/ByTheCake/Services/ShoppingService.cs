namespace MyHandmadeWebServer.ByTheCake.Services
{
    using Data;
    using Data.Models;
    using Services.Contracts;
    using ViewModels.Shopping;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, IEnumerable<int> productIds)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var order = new Order
                {
                    UserId = userId,
                    CreationDate = DateTime.UtcNow,
                    Products = productIds.Select(id => new OrderProduct { ProductId = id }).ToList()
                };

                db.Orders.Add(order);
                db.SaveChanges();
            }
        }

        public OrderDetailsViewModel GetOrder(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return this
                    .GetOrders(
                        db.Users
                        .Where(u => u.Orders.Any(o => o.Id == id))
                        .SingleOrDefault()
                        .Username)
                    .Where(ovm => ovm.OrderId == id)
                    .Select(ovm => new OrderDetailsViewModel
                    {
                        OrderId = ovm.OrderId,
                        CreationDate = ovm.CreationDate,
                        Products = db
                            .OrderProducts
                            .Where(op => op.OrderId == id)
                            .Select(p => p.Product)
                            .ToList()
                    })
                    .SingleOrDefault();
            }
        }

        public IEnumerable<OrderViewModel> GetOrders(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Orders
                    .Where(o => o.User.Username == username)
                    .Select(o => new OrderViewModel
                    {
                        OrderId = o.Id,
                        CreationDate = o.CreationDate,
                        TotalSum = o.Products.Sum(p => p.Product.Price)
                    })
                    .ToList();
            }
        }
    }
}
