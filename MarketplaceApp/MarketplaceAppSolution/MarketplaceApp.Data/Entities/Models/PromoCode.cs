using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApp.Data.Entities.Models
{   
    public class PromoCode
    {
        public string Code { get; private set; }
        public string Category { get; private set; }
        public decimal Discount { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public PromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            Code = code;
            Category = category;
            Discount = discount;
            ExpirationDate = expirationDate;
        }

    }
}