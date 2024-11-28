using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApp.Data
{
    public class Seed
    {
        public class Seed
        {
            private readonly UserRepository _userRepository;
            private readonly MarketplaceRepository _marketplaceRepository;
            private readonly ProductRepository _productRepository;
            private readonly PromoCodeRepository _promoCodeRepository;

            public Seed(UserRepository userRepository, MarketplaceRepository marketplaceRepository, ProductRepository productRepository, PromoCodeRepository promoCodeRepository)
            {
                _userRepository = userRepository;
                _marketplaceRepository = marketplaceRepository;
                _productRepository = productRepository;
                _promoCodeRepository = promoCodeRepository;
            }
        }


        public void InitializeData()
        {
            _userRepository.RegisterBuyer("Ana Anic", "ana@gmail.com", 5000);
            _userRepository.RegisterSeller("Marko Markic", "marko@gmail.com");

            var marko = _marketplaceRepository.LoginUser("marko@gmail.com") as Seller;

            _productRepository.AddProduct(marko, "Laptop", "Brzi laptop za rad", 300, "Elektronika");
            _productRepository.AddProduct(marko, "Pametni telefon", "Pametni telefon", 450, "Elektronika");
            _productRepository.AddProduct(marko, "Kosulja", "Odjevni predmet", 300, "Roba");
            _productRepository.AddProduct(marko, "Sat", "Otkucava", 450, "Nakit");

            _promoCodeRepository.AddPromoCode("ELEC10", "Elektronika", 10, DateTime.Now.AddDays(10));
            _promoCodeRepository.AddPromoCode("SALE20", "Roba", 20, DateTime.Now.AddDays(5));

            var ana = _userRepository.LoginUser("ana@gmail.com") as Buyer;
            var product = _context.Products.FirstOrDefault(p => p.Title == "Pametni telefon");
            bool kupovina = _marketplaceRepository.TryBuyProduct(ana, product, "ELEC10");
        }

    }

}
