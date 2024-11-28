using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Classes.Domain
{
    public class PromoCode
    {
        public string Code { get; set; }
        public string Category { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            Code = code;
            Category = category;
            Discount = discount;
            ExpirationDate = expirationDate;
        }

        public bool IsValid(string productCategory, DateTime currDate)
        {
            return Category.Equals(productCategory, StringComparison.OrdinalIgnoreCase) && ExpirationDate > currDate;

        }
    }
}
