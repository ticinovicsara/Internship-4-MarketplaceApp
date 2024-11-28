using MarketplaceApp.Classes.Data;
using MarketplaceApp.Classes.MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Classes.MarketplaceApp.Data
{
    public class Seed
    {
        public static readonly List<Buyer> Buyers = new List<Buyer>()
        {
            new Buyer("Ana Anic", "ana@gmail.com", 5000)
        };

        public static readonly List<Seller> Sellers = new List<Seller>()
        {
            new Seller("Marko Markic", "marko@gmail.com")
        };

        

        Marketplace marketplace = new Marketplace();

        marketplace.AddProduct(marko, "Laptop", "Brzi laptop za rad", 300, "Elektronika");
        marketplace.AddProduct(marko, "Pametni telefon", "Pametni telefon", 450, "Elektronika");
        marketplace.AddProduct(marko, "Kosulja", "Odjevni predmet", 300, "Roba");
        marketplace.AddProduct(marko, "Sat", "Otkucava", 450, "Nakit");

        marketplace.AddPromoCode("ELEC10", "Elektronika", 10, DateTime.Now.AddDays(10));
        marketplace.AddPromoCode("SALE20", "Roba", 20, DateTime.Now.AddDays(5));

        bool anaKupila = marketplace.TryBuyProduct(ana, marko.Products.First(p => p.Title == "Pametni telefon"));
        anaKupila = marketplace.TryBuyProduct(ana, marko.Products.First(p => p.Title == "Kosulja"));

    }
}
