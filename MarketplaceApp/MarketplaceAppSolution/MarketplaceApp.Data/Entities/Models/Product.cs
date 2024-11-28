using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Product
    {
        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public Seller Seller { get; set; }
        public string Category { get; set; }
        public decimal AverageRating { get; private set; }

        public enum ProductStatus
        {
            Sold,
            OnSale
        }

        public Product(string title, string description, decimal price, Seller seller, string category)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Price = price;
            Seller = seller;
            Status = ProductStatus.OnSale;
            Category = category;
            AverageRating = 0.0m;
        }

        private int ratingCount = 0;
        private decimal totalRating = 0;

        public void AddRating(decimal newRating)
        {
            ratingCount++;
            totalRating += newRating;
            AverageRating = totalRating / ratingCount;
        }

        public void ChangePrice(decimal newPrice, Seller seller)
        {
            if (seller != null && seller.Products.Contains(this))
            {
                Price = newPrice;
            }
            else
            {
                throw new InvalidOperationException("Seller is not the owner of this product.");
            }
        }
    }
}
