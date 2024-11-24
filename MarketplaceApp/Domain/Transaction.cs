using MarketplaceApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfTransacton;

        public Transaction(Buyer buyer, Seller seller, decimal amount)
        {
            BuyerName = buyer.Name;
            SellerName = seller.Name;
            DateOfTransacton = DateTime.Now;
            Amount = amount;
            DateOfTransacton = DateTime.Now;
        }
    }
}
