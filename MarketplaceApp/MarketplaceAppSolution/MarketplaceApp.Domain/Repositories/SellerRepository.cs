using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Domain.Repositories
{
    public class SellerRepository
    {
        private readonly Context _context;
        public SellerRepository(Context context)
        {
            _context = context;
        }

        public void AddNewProduct(Seller seller, Product product)
        {
            seller.Products.Add(product);
        }


        public decimal GetEarningsForTimePeriod(Seller seller, DateTime startDate, DateTime endDate)
        {
            decimal totalEarnings = 0;

            var sallerTransactions = _context.Transactions.Where(t => t.SellerName == seller.Name && t.DateOfTransaction >= startDate && t.DateOfTransaction <= endDate).ToList();

            foreach (var transaction in sallerTransactions)
            {
                totalEarnings += transaction.Amount;
            }

            return totalEarnings;
        }
    }

}
