using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MarketplaceApp.Domain.Repositories
{
    public class TransactionService : ITransactionService
    {
        private readonly Context _context;

        public TransactionService(Context context)
        {
            _context = context;
        }

        public void LogTransaction(Buyer buyer, Seller seller, decimal amount)
        {
            Transaction transaction = new Transaction(buyer, seller, amount);

            _context.Transactions.Add(transaction);
        }
    }
}

