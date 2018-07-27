namespace MyHandmadeWebServer.ByTheCake.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }
        
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
