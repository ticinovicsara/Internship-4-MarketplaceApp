using MarketplaceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Presentation.Menus;

namespace MarketplaceApp.Presentation.UserActions
{
    public class HandleLogin
    {
        public static void LoginUser(MarketplaceRepository marketplaceRepository)
        {
            Console.Clear();
            Console.WriteLine("Prijava korisnika:\n");
            Console.WriteLine("Unesite email za prijavu");
            string email = Console.ReadLine();

            try
            {
                var user = marketplaceRepository.LoginUser(email);
                Console.Clear();
                Console.WriteLine($"Dobrodosli, {user.Name}\n");

                if (user is Buyer buyer)
                {
                    LoginBuyer(marketplaceRepository, buyer);
                }
                else if (user is Seller seller)
                {
                    LoginSeller(marketplaceRepository, seller);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Clear();
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }

        private static void LoginBuyer(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();
            BuyerMenu.ShowBuyerMenu(marketplaceRepository, buyer);
        }

        private static void LoginSeller(MarketplaceRepository marketplaceRepository, Seller seller)
        {
            Console.Clear();
            SellerMenu.ShowSellerMenu(marketplaceRepository, seller);
        }
    }
}
