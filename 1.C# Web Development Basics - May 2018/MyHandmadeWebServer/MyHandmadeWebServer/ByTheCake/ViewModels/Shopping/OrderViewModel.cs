namespace MyHandmadeWebServer.ByTheCake.ViewModels.Shopping
{
    using System;

    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public DateTime CreationDate { get; set; }

        public decimal TotalSum { get; set; }
    }
}
