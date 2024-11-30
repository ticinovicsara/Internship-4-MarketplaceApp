using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Data.Entities.Models
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

        public Product(string title, string description, decimal price, string category, Seller seller)
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
    }
}
