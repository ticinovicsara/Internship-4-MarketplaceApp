using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;

namespace MarketplaceApp.Data
{
    public class Seed
    {
        private readonly Context _context;

        public Seed(Context context)
        {
            _context = context;
        }

        public void InitializeData()
        {
            Buyer ana = new Buyer("Ana Anic", "ana@gmail.com", 5000);
            Seller marko = new Seller("Marko Markic", "marko@gmail.com");

            Product product1 = new Product("Laptop", "Brzi laptop za rad", 300, "Elektronika", marko);
            Product product2 = new Product("Pametni telefon", "Pametni telefon", 450, "Elektronika", marko);
            Product product3 = new Product("Kosulja", "Odjevni predmet", 300, "Roba", marko);
            Product product4 = new Product("Sat", "Otkucava", 450, "Nakit", marko);

            PromoCode promo1 = new PromoCode("ELEC10", "Elektronika", 10, DateTime.Now.AddDays(10));
            PromoCode promo2 = new PromoCode("SALE20", "Roba", 20, DateTime.Now.AddDays(5));

            ana.PurchasedProducts.Add(product1);
            ana.PurchasedProducts.Add(product2);
            marko.Earnings += product1.Price - 0.05*product1.Price;
            marko.Earnings += product2.Price - 0.05 * product2.Price;

            _context.Buyers.Add(ana);
            _context.Sellers.Add(marko);

            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.Products.Add(product4);

            marko.Products.Add(product1);
            marko.Products.Add(product2);
            marko.Products.Add(product3);
            marko.Products.Add(product4);

            _context.PromoCodes.Add(promo1);
            _context.PromoCodes.Add(promo2);

            Transaction transaction1 = new Transaction(ana, marko, product1.Price);
            Transaction transaction2 = new Transaction(ana, marko, product2.Price);

            _context.Transactions.Add(transaction1);
            _context.Transactions.Add(transaction2);
        }
    }
}
