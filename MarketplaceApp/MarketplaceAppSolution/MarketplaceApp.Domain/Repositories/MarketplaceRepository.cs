using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;
using MarketplaceApp.Domain.Repositories;
using System.Transactions;

namespace MarketplaceApp.Domain.Repositories
{
    public class MarketplaceRepository : ITransactionService
    {
        private readonly UserRepository _userRepository;
        private readonly SellerRepository _sellerRepository;
        private readonly BuyerRepository _buyerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PromoCodeRepository _promoCodeRepository;
        private readonly ITransactionService _transactionService;
        private readonly Context _context;

        decimal marketplaceCommission = 0;

        public MarketplaceRepository(
            ITransactionService transactionService,
            UserRepository userRepository,
            SellerRepository sellerRepository,
            ProductRepository productRepository,
            PromoCodeRepository promoCodeRepository,
            Context context)
        {
            _userRepository = userRepository;
            _sellerRepository = sellerRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
            _transactionService = transactionService;
            _context = context;
        }


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

            var seller = new Seller(name, email);

            _userRepository.AddSeller(seller);
        }


        public void RegisterBuyer(string name, string email, decimal balance)
        {
            if (_userRepository.UserExists(email))
                throw new InvalidOperationException("Kupac s tim emailom već postoji.");

            var buyer = new Buyer(name, email, balance);

            _userRepository.AddBuyer(buyer);
        }


        public bool TryBuyProduct(Buyer buyer, Product product, string? promoCode = null)
        {
            decimal finalPrice = product.Price;

            if (!string.IsNullOrEmpty(promoCode))
            {
                var code = _context.PromoCodes.FirstOrDefault(pc => pc.Code.Equals(promoCode, StringComparison.OrdinalIgnoreCase));
                if (code == null || !IsValid(product.Category, DateTime.Now))
                {
                    Console.WriteLine("\nPromotivni kod nije valjan\n");
                }
                finalPrice -= finalPrice * (code.Discount / 100);
            }

            if (buyer.Balance < product.Price || product.Status != Product.ProductStatus.OnSale)
            {
                return false;
            }

            buyer.Balance -= finalPrice;

            product.Status = Product.ProductStatus.Sold;
            buyer.PurchasedProducts.Add(product);

            marketplaceCommission += finalPrice * 0.05m;
            product.Seller.Earnings += finalPrice * 0.95m;
            LogTransaction(buyer, product.Seller, finalPrice - marketplaceCommission);
            return true;
        }


        public void LogTransaction(Buyer buyer, Seller seller, decimal amount)
        {
            _transactionService.LogTransaction(buyer, seller, amount);
        }


        public bool IsValid(string productCategory, DateTime currDate)
        {
            var product = _context.Products.FirstOrDefault(p => p.Category.Equals(productCategory, StringComparison.OrdinalIgnoreCase));

            return product != null;
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


        public void AddPromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            _promoCodeRepository.AddPromoCode(code, category, discount, expirationDate);
        }

        public List<PromoCode> GetPromoCodesByCategory(string category)
        {
            return _context.PromoCodes.Where(pc => pc.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }


        public bool ReturnProduct(Buyer buyer, Product product)
        { 
            return _buyerRepository.ReturnProduct(buyer, product);
        }


        public void AddRating(Product product, decimal rating)
        {
            _productRepository.AddRating(product, rating);
        }

    }
}
