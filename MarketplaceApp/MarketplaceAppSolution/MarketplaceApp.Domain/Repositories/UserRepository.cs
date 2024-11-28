﻿using System;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;

namespace MarketplaceApp.Domain.Repostories
{
    public class UserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public Buyer GetBuyerByEmail(string email)
        {
            return _context.Buyers.FirstOrDefault(b => b.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Seller GetSellerByEmail(string email)
        {
            return _context.Sellers.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void AddBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            _context.SaveChanges();
        }

        public void AddSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        public bool UserExists(string email)
        {
            var buyerExists = _context.Buyers.Any(b => b.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            var sellerExists = _context.Sellers.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return buyerExists || sellerExists;
        }
    }
}