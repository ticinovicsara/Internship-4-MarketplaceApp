using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;

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
            var newProduct = new Product
            {
                Title = title,
                Description = description,
                Price = price,
                Category = category,
                Seller = seller,
                Status = ProductStatus.OnSale 
            };

            _context.Products.Add(newProduct);
        }

        public void ChangePrice(Product product, decimal newPrice)
        {
            product.Price = newPrice;
        }

        public List<Product> GetAvailableProducts()
        {
            return _context.Products.Where(p => p.Status == ProductStatus.OnSale).ToList();
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return _context.Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && p.Status == ProductStatus.OnSale).ToList();
        }
    }
}
