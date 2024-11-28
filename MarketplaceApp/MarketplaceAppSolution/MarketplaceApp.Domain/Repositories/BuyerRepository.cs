using MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain.Repositories
{
    public class BuyerRepository
    {
        private readonly Context _context;

        public BuyerRepository(Context context)
        {
            _context = context;
        }

        public bool ReturnProduct(Buyer buyer, Product product)
        {
            if (!buyer.PurchasedProducts.Contains(product))
            {
                return false;
            }

            buyer.PurchasedProducts.Remove(product);
            product.Status = Product.ProductStatus.OnSale;
            buyer.Balance += product.Price * 0.8m;
            product.Seller.Earnings -= product.Price * 0.95m;

            LogTransaction(buyer, product.Seller, -(product.Price * 0.8m));

            return true;
        }
    }
}

