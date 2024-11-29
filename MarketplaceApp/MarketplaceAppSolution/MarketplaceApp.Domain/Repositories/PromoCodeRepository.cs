using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Domain.Repositories
{
    public class PromoCodeRepository
    {
        private readonly Context _context;

        public PromoCodeRepository(Context context)
        {
            _context = context;
        }

        public void AddPromoCode(string code, string category, decimal discount, DateTime expirationDate)
        {
            if (_context.PromoCodes.Any(pc => pc.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidCastException("Promotivni kod vec postoji\n");
            }

            PromoCode newCode = new PromoCode(code, category, discount, expirationDate);

            _context.PromoCodes.Add(newCode);
        }

    }
}

