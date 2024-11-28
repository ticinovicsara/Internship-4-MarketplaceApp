using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Marketplace
    {
        public List<User> Users { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public List<PromoCode> PromoCodes { get; private set; }
        public double TotalTransactionFee { get; set; }

        public Marketplace()
        {
            var (users, products, transactions, promoCodes) = Seed.GetSeedData();
            this.AllUsers = users;
            this.AllProducts = products;
            this.AllTransactions = transactions;
            this.AllPromoCodes = promoCodes;
        }
    }

}