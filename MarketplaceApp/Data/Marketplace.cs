using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

using MarketplaceApp.Domain;
using static MarketplaceApp.Domain.Product;

namespace MarketplaceApp.Data
{
    public class Marketplace
    {
        private List<Buyer> Buyers { get; set; } = new List<Buyer>();
        private List<Seller> Sellers { get; set; } = new List<Seller>();
        private List<Product> Products { get; set; } = new List<Product>();
        private List<Transaction> Transactions { get; set; } = new List<Transaction>();
        private List<PromoCode> PromoCodes { get; set; } = new List<PromoCode>();
        private decimal maketplaceComission;

        public void RegisterBuyer(string name, string email, decimal balance)
        {
            var buyer = new Buyer(name, email, balance);
            Buyers.Add(buyer);
        }

        public void RegisterSeller(string name, string email)
        {
            var seller = new Seller(name, email);
            Sellers.Add(seller);
        }


        public User LoginUser(string email)
        {
            var buyer = Buyers.FirstOrDefault(b => b.Email == email);
            if(buyer != null)
                return buyer;

            var seller = Sellers.FirstOrDefault(s => s.Email == email);
            if (seller != null)
                return seller;

            throw new InvalidOperationException("Korisnik nije pronadjen\n");
        }


        public void AddProduct(Seller seller, string title, string description, decimal price, string category)
        {
            var product = new Product(title, description, price, seller, category);
            Products.Add(product);
            seller.AddNewProduct(product);
        }


        public bool TryBuyProduct(Buyer buyer, Product product, string promoCode = null)
        {
            decimal finalPrice = product.Price;
            if (!string.IsNullOrEmpty(promoCode))
            {
                var code = PromoCodes.FirstOrDefault(pc => pc.Code.Equals(promoCode, StringComparison.OrdinalIgnoreCase));
                if(code == null || !code.IsValid(product.Category, DateTime.Now))
                {
                    throw new InvalidOperationException("Promotivni kod nije valjan\n");
                }

                finalPrice += finalPrice * (code.Discount / 100);
            }

            if (buyer.Balance < product.Price || product.Status != ProductStatus.OnSale)
            {
                return false;
            }

            buyer.DeductBalance(finalPrice);
            product.ChangeStatus(ProductStatus.Sold);
            buyer.PurchasedProducts.Add(product);
            maketplaceComission += finalPrice * 0.05m;
            product.Seller.Earnings += finalPrice * 0.95m;
            LogTransaction(buyer, product.Seller, finalPrice - maketplaceComission);
            return true; 
        }


        public List<Product> GetAvailableProducts()
        {
            return Products.Where(p => p.Status == ProductStatus.OnSale).ToList();
        }


        public List<string> GetCategories()
        {
            return Products.Select(p => p.Category).Distinct().ToList();
        }
        

        public List<Product> GetProductsByCategory(string category)
        {
            return Products.Where(p =>p.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && p.Status == ProductStatus.OnSale).ToList();
        }


        public bool ReturnProduct(Buyer buyer, Product product)
        {
            if (!buyer.PurchasedProducts.Contains(product))
            {
                return false;
            }

            buyer.PurchasedProducts.Remove(product);
            product.Status = ProductStatus.OnSale;
            product.Seller.Earnings -= product.Price * 0.95m;
            buyer.Balance += product.Price * 0.8m;
            return true;
        }


        public void LogTransaction(Buyer buyer, Seller seller, decimal amount)
        {
            var transaction = new Transaction(buyer, seller, amount);
            Transactions.Add(transaction);
        }


        public decimal GetEarningsForTmePeriod(Seller seller, DateTime startDate, DateTime endDate)
        {
            decimal totalEarnings = 0;

            var sallerTransactions = Transactions.Where(t => t.SellerName == seller.Name && t.DateOfTransacton >= startDate && t.DateOfTransacton <= endDate).ToList();

            foreach (var transaction in sallerTransactions)
            {
                totalEarnings += transaction.Amount;
            }


            return totalEarnings;
        }


        public void UpdateProductPrice(Seller seller, Product product, decimal newprice)
        {
            product.ChangePrice(newprice, seller);
        }

        public void AddPromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            if(PromoCodes.Any(pc => pc.Code.Equals(code, StringComparison.OrdinalIgnoreCase))){
                throw new InvalidCastException("Promotivni kod vec postoji\n");
            }

            PromoCode newCode = new PromoCode(code, category, discount, expirationDate);
            PromoCodes.Add(newCode);
        }

        public List<PromoCode> GetPromoCodesByCategory(string category)
        {
            return PromoCodes.Where(pc => pc.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void AddRating(Product product, decimal rating)
        { 
            product.AddRating(rating);
        }
    }
}
