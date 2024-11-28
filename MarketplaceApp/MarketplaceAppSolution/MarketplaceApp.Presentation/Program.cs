using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Presentation.UserActions;

namespace MarketplaceApp.Presentation
{
    class Program
    {
        static void Main()
        {
            var context = new Context();
            var userRepository = new UserRepository(context);
            var productRepository = new ProductRepository(context);
            var promoCodeRepository = new PromoCodeRepository(context);
            var marketplaceRepository = new MarketplaceRepository(context);

            var seed = new Seed(context, userRepository, marketplaceRepository, productRepository, promoCodeRepository);
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
                                HandleRegistration.RegisterBuyer(marketplaceRepository);
                                break;
                            }
                            else if (role == "2")
                            {
                                HandleRegistration.RegisterSeller(marketplaceRepository);
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
                        HandleLogin.LoginUser(marketplaceRepository);
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