namespace MyHandmadeWebServer.ByTheCake.ViewModels.Shopping
{
    using Data.Models;

    using System;
    using System.Collections.Generic;

    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
