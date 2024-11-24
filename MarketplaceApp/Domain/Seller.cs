using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain
{
    public class Seller : User
    {
        public List<Product> Products { get; private set; }
        public decimal Earnings { get; set; }

        public Seller(string name, string email) : base(name, email) 
        {
            Earnings = 0;
            Products = new List<Product>();
        }

        public void AddNewProduct(Product product)
        {
            Products.Add(product);
        }

        public decimal GetTotalEarnings() 
        {
            return Products.Where(p => p.Status == Product.ProductStatus.Sold).Sum(p => p.Price * 0.95m);
        }
    }
}
