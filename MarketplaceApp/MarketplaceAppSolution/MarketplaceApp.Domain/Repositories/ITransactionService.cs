using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketplaceApp.Domain.Repositories
{
    public interface ITransactionService
    {
        void LogTransaction(Buyer buyer, Seller seller, decimal amount);
    }
}
