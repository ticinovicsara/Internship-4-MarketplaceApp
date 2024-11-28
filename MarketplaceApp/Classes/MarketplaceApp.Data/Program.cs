using MarketplaceApp.Classes.Domain;
using MarketplaceApp.Classes.MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Classes.MarketplaceApp.Data
{
    public static class Program
    {
        public class Context
        {
            public List<Buyer> Buyers { get; set; } = new List<Buyer>();
            public List<Seller> Sellers { get; set; } = new List<Seller>();
            public List<Product> Products { get; set; } = new List<Product>();
            public List<Transaction> Transactions { get; set; } = new List<Transaction>();
            public List<PromoCode> PromoCodes { get; set; } = new List<PromoCode>();
        }
    }
}
