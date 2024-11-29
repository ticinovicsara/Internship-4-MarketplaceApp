using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;

namespace MarketplaceApp.Data
{
    public class Context
    {
        public List<Product> Products { get; set; }
        public List<Buyer> Buyers { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<PromoCode> PromoCodes { get; set; }

        public Context()
        {
            Products = new List<Product>();
            Buyers = new List<Buyer>();
            Sellers = new List<Seller>();
            Transactions = new List<Transaction>();
            PromoCodes = new List<PromoCode>();
        }
    }
}
