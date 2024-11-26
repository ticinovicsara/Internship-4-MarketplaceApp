using MarketplaceApp.Data;
using MarketplaceApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MarketplaceApp.Presentation.UserMenu;
namespace MarketplaceApp.Presentation
{
    public class HandleLogin
    {
        public static void LoginUser(Marketplace marketplace)
        {
            Console.Clear();
            Console.WriteLine("Prijava korisnika:\n");
            Console.WriteLine("Unesite email za prijavu");
            string email = Console.ReadLine();

            try
            {
                var user = marketplace.LoginUser(email);
                Console.Clear();
                Console.WriteLine($"Dobrodosli, {user.Name}\n");

                if (user is Buyer buyer)
                {
                    LoginBuyer(marketplace, buyer);
                }
                else if (user is Seller seller)
                {
                    LoginSeller(marketplace, seller);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Clear();
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }

        private static void LoginBuyer(Marketplace marketplace, Buyer buyer)
        {
            Console.Clear();
            BuyerMenu.ShowBuyerMenu(marketplace, buyer);
        }

        private static void LoginSeller(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            SellerMenu.ShowSellerMenu(marketplace, seller);
        }
    }
}
