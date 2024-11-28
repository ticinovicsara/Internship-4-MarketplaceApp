using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Buyer : User
    {
        public decimal Balance { get; set; }
        public List<Product> FavouriteProducts { get; set; } = new List<Product>();
        public List<Product> PurchasedProducts { get; set; } = new List<Product>();

        public Buyer(string name, string email, decimal balance) : base(name, email)
        {
            Balance = balance;
        }
    }
}

