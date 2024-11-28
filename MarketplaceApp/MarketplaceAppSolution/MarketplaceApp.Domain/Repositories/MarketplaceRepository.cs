using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;

namespace MarketplaceApp.Domain.Repostories
{
    public class MarketplaceRepository
    {
        private readonly UserRepository _userRepository;
        private readonly SellerRepository _sellerRepository;
        private readonly BuyerRepository _buyerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PromoCodeRepository _promoCodeRepository;

        decimal marketplaceCommission = 0;

        public MarketplaceRepository(
            UserRepository userRepository,
            SellerRepository sellerRepository,
            BuyerRepository buyerRepository,
            ProductRepository productRepository,
            PromoCodeRepository promoCodeRepository,
            Context context)
        {
            _userRepository = userRepository;
            _sellerRepository = sellerRepository;
            _buyerRepository = buyerRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
            _context = context;
        }

        decimal marketplaceCommission = 0;


        public User LoginUser(string email)
        {
            var buyer = _userRepository.GetBuyerByEmail(email);
            if (buyer != null)
                return buyer;

            var seller = _userRepository.GetSellerByEmail(email);
            if (seller != null)
                return seller;

            throw new InvalidOperationException("Korisnik nije pronadjen\n");
        }


        public void RegisterSeller(string name, string email)
        {
            if (_userRepository.UserExists(email))
                throw new InvalidOperationException("Prodavac s tim emailom već postoji.");

            var seller = new Seller
            {
                Name = name,
                Email = email,
            };
            _userRepository.AddSeller(seller);
        }


        public void RegisterBuyer(string name, string email, decimal balance)
        {
            if (_userRepository.UserExists(email))
                throw new InvalidOperationException("Kupac s tim emailom već postoji.");

            var buyer = new Buyer
            {
                Name = name,
                Email = email,
                Balance = balance
            };
            _userRepository.AddBuyer(buyer);
        }


        public bool TryBuyProduct(Buyer buyer, Product product, string promoCode = null)
        {
            decimal finalPrice = product.Price;

            if (!string.IsNullOrEmpty(promoCode))
            {
                var code = _context.PromoCodes.FirstOrDefault(pc => pc.Code.Equals(promoCode, StringComparison.OrdinalIgnoreCase));
                if (code == null || !code.IsValid(product.Category, DateTime.Now))
                {
                    throw new InvalidOperationException("Promotivni kod nije valjan\n");
                }
                finalPrice -= finalPrice * (code.Discount / 100);
            }

            if (buyer.Balance < product.Price || product.Status != ProductStatus.OnSale)
            {
                return false;
            }

            buyer.Balance -= finalPrice;

            product.Status = ProductStatus.Sold;
            buyer.PurchasedProducts.Add(product);

            marketplaceCommission += finalPrice * 0.05m;
            product.Seller.Earnings += finalPrice * 0.95m;
            LogTransaction(buyer, product.Seller, finalPrice - marketplaceCommission);
            return true;
        }


        public void LogTransaction(Buyer buyer, Seller seller, decimal amount)
        {
            var transaction = new Transaction
            {
                BuyerName = buyer.Name,
                SellerName = seller.Name,
                Amount = amount,
                DateOfTransaction = DateTime.Now
            };
            _context.Transactions.Add(transaction);
        }


        public List<string> GetCategories()
        {
            return _context.Products.Select(p => p.Category).Distinct().ToList();
        }


        public void AddProduct(Seller seller, string title, string description, decimal price, string category)
        {
            _productRepository.AddProduct(seller, title, description, price, category);
        }


        public void UpdateProductPrice(Product product, decimal newPrice)
        {
            _productRepository.ChangePrice(product, newPrice);
        }


        public List<Product> GetAvailableProducts()
        {
            return _productRepository.GetAvailableProducts();
        }


        public List<Product> GetProductsByCategory(string category)
        {
            return _productRepository.GetProductsByCategory(category);
        }


        public decimal GetTotalEarnings()
        {
            return _sellerRepository.GetTotalEarnings();
        }
        

        public decimal GetEarningsForTimePeriod(Seller seller, DateTime startDate, DateTime endDate)
        {
            return _sellerRepository.GetEarningsForTimePeriod(seller, startDate, endDate);
        }


        public bool IsValid(string productCategory, DateTime currDate)
        {
            return Category.Equals(productCategory, StringComparison.OrdinalIgnoreCase) && ExpirationDate > currDate;

        }


        public void AddPromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            if (_context.PromoCodes.Any(pc => pc.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidCastException("Promotivni kod vec postoji\n");
            }

            PromoCode newCode = new PromoCode
            {
                Code = code,
                Category = category,
                Discount = discount,
                ExpirationDate = expirationDate
            };
            _context.PromoCodes.Add(newCode);
        }

        public List<PromoCode> GetPromoCodesByCategory(string category)
        {
            return _context.PromoCodes.Where(pc => pc.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }


        public bool ReturnProduct(Buyer buyer, Product product)
        { 
            return _buyerRepository.ReturnProduct(buyer, product);
        }


        public void AddRating(ProductRepository product, decimal rating)
        {
            product.AddRating(rating);
        }

    }
}
