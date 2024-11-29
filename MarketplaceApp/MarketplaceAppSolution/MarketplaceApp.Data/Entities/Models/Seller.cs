using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Seller : User
    {
        public List<Product> Products { get; set; }
        public decimal Earnings { get; set; }

        public Seller(string name, string email) : base(name, email)
        {
            Earnings = 0;
            Products = new List<Product>();
        }
    }
}
