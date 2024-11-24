using MarketplaceApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public Seller Seller { get; set; }
        public string Category { get; set; }
        public decimal AverageRating { get; set; }

        public enum ProductStatus
        {
            Sold,
            OnSale
        }

        public Product(string title, string description, decimal price, Seller seller, string category)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Price = price;
            Seller = seller;
            Status = ProductStatus.OnSale;
            Category = category;
            AverageRating = 0.0m;
        }


        public void ChangePrice(decimal newprice, Seller seller)
        {
            if (this.Seller == seller)
            {
                this.Price = newprice;
            }
            else
            {
                throw new InvalidOperationException("Samo prodavac moze promijeniti cijenu\n");
            }
        }

        public void ChangeStatus(ProductStatus newstatus)
        {
            Status = newstatus;
        }

        public void AddRating(decimal newrating)
        {
            AverageRating = (AverageRating + newrating) / 2;
        }


    }
}
