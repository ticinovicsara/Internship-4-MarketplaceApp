using MarketplaceApp.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Classes.MarketplaceApp.Data.Entities.Models
{
    public class Buyer : User
    {
        public decimal Balance { get; set; }
        public List<Product> FavouriteProducts { get; set; } = new List<Product>();
        public List<Product> PurchasedProducts { get; set; } = new List<Product>();

        public Buyer(string name, string email, decimal balance) : base(name, email)
        {
            Balance = balance;
            FavouriteProducts = new List<Product>();
            PurchasedProducts = new List<Product>();
        }

        public void DeductBalance(decimal amount)
        {
            if (Balance > amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Nedovoljan iznos na racunu");
            }
        }

    }
}
