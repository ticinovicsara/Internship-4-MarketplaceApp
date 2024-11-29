using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Domain.Repositories
{
    public class ProductRepository
    {
        private readonly Context _context;

        public ProductRepository(Context context)
        {
            _context = context;
        }

        public void AddProduct(Seller seller, string title, string description, decimal price, string category)
        {
            Product newProduct = new Product(title, description, price, category, seller);
            seller.Products.Add(newProduct);
            _context.Products.Add(newProduct);
        }

        public void ChangePrice(Product product, decimal newPrice)
        {
            product.Price = newPrice;
        }

        public List<Product> GetAvailableProducts()
        {
            return _context.Products.Where(p => p.Status == Product.ProductStatus.OnSale).ToList();
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return _context.Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && p.Status == Product.ProductStatus.OnSale).ToList();
        }

        private int ratingCount = 0;
        private decimal totalRating = 0;

        public void AddRating(Product product, decimal newRating)
        {
            ratingCount++;
            totalRating += newRating;
            product.AverageRating = totalRating / ratingCount;
        }
    }
}
