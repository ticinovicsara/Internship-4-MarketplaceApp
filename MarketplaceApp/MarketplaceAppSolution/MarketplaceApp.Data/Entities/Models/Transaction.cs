using System;
using System.Collections.Generic;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;
using System.Linq;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public string BuyerName { get; private set; }
        public string SellerName { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime DateOfTransaction { get; private set; }

        public Transaction(Buyer buyer, Seller seller, decimal amount)
        {
            this.Id = Guid.NewGuid();
            this.BuyerName = buyer.Name;
            this.SellerName = seller.Name;
            this.DateOfTransaction = DateTime.Now;
            this.Amount = amount;
        }
    }
}