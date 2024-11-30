using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Presentation.UserActions;
using MarketplaceApp.Presentation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation
{
    class Program
    {
        static void Main()
        {
            var context = new Context();

            var transactionService = new TransactionService(context);

            var userRepository = new UserRepository(context);
            var sellerRepository = new SellerRepository(context);
            var productRepository = new ProductRepository(context);
            var promoCodeRepository = new PromoCodeRepository(context);

            var marketplaceRepository = new MarketplaceRepository(
                transactionService,
                userRepository,
                sellerRepository,
                productRepository,
                promoCodeRepository,
                context
            );

            var handleRegistration = new HandleRegistration(context, marketplaceRepository);
            var handleLogin = new HandleLogin(context, marketplaceRepository);


            var seed = new Seed(context);
            seed.InitializeData();


            while (true)
            {
                Console.WriteLine("Dobrodosli u Marketplace!\n");
                Console.WriteLine("1 - Registracija korisnika");
                Console.WriteLine("2 - Prijava korisnika\n");
                Console.WriteLine("3 - Izlaz\n");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        Console.Clear();

                        while (true)
                        {
                            Console.WriteLine("Registracija:\n");
                            Console.WriteLine("1 - Kupac");
                            Console.WriteLine("2 - Prodavac\n");
                            Console.WriteLine("\n0 - Povratak\n");

                            string role = Console.ReadLine();

                            if (role == "1")
                            {
                                handleRegistration.RegisterBuyer();
                                break;
                            }
                            else if (role == "2")
                            {
                                handleRegistration.RegisterSeller();
                                break;
                            }
                            else if (role == "0")
                            {
                                Console.Clear();
                                return;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Neispravan odabir unesite ponovno\n");
                            }
                        }
                        break;

                    case "2":
                        handleLogin.LoginUser();
                        break;

                    case "3":
                        Console.WriteLine("\nSee yaa!\n");
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Neispravan unos, pokusajte ponovno\n");
                        break;
                }
            }
        }
    }
}