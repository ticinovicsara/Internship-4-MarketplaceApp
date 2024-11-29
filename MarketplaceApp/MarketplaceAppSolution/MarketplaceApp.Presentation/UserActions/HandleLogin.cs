using MarketplaceApp.Data;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Presentation.Menus;
using MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.UserActions
{
    public class HandleLogin
    {
        private readonly Context _context;
        private readonly MarketplaceRepository _marketplaceRepository;

        public HandleLogin(Context context, MarketplaceRepository marketplaceRepository)
        {
            _context = context;
            _marketplaceRepository = marketplaceRepository;
        }

        public void LoginUser()
        {
            Console.Clear();
            Console.WriteLine("Prijava korisnika:\n");
            Console.WriteLine("Unesite email za prijavu");
            string email = Console.ReadLine();

            try
            {
                var user = _marketplaceRepository.LoginUser(email);
                Console.Clear();
                Console.WriteLine($"Dobrodosli, {user.Name}\n");

                if (user is Buyer buyer)
                {
                    LoginBuyer(_marketplaceRepository, buyer);
                }
                else if (user is Seller seller)
                {
                    LoginSeller(_marketplaceRepository, seller);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Clear();
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }

        private void LoginBuyer(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();
            BuyerMenu.ShowBuyerMenu(marketplaceRepository, buyer);
        }

        private void LoginSeller(MarketplaceRepository marketplaceRepository, Seller seller)
        {
            Console.Clear();
            SellerMenu.ShowSellerMenu(marketplaceRepository, seller);
        }
    }
}
