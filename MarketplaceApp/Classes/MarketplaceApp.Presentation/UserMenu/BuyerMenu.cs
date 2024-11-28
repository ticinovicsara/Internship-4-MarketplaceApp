using MarketplaceApp.Classes.Data;
using MarketplaceApp.Classes.MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Classes.Presentation.UserMenu
{
    public class BuyerMenu
    {
        public static void ShowBuyerMenu(Marketplace marketplace, Buyer buyer)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Kupac: {buyer.Name}\n");
                Console.WriteLine("1 - Pregled dostupnih proizvoda");
                Console.WriteLine("2 - Kupnja proizvoda");
                Console.WriteLine("3 - Povratak proizvoda");
                Console.WriteLine("4 - Dodavanje prizvoda u omiljene");
                Console.WriteLine("5 - Pregled povijesti kupljenih proizvoda");
                Console.WriteLine("6 - Pregled omiljenih proizvoda");
                Console.WriteLine("7 - Ocjenjivanje proizvoda");
                Console.WriteLine("\n0 - Povratak / Odjava\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();

                        var products = marketplace.GetAvailableProducts();
                        Console.WriteLine("Dostupni proizvodi:\n");
                        foreach (var product_ in products)
                        {
                            Console.WriteLine($"Naziv: {product_.Title} - Cijena: {product_.Price}");
                        }
                        Console.WriteLine("\nPritisnite bilo što za povratak...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "2":
                        BuyerService.BuyProduct(marketplace, buyer);
                        break;

                    case "3":
                        BuyerService.ReturnProduct(marketplace, buyer);
                        break;

                    case "4":
                        BuyerService.AddToFavourites(marketplace, buyer);
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("Povijest kupljenih proizvoda:\n");

                        foreach (var purchasedProduct in buyer.PurchasedProducts)
                        {
                            Console.WriteLine($"\nNaziv: {purchasedProduct.Title}\nCijena: {purchasedProduct.Price}");
                            Console.WriteLine($"ID: {purchasedProduct.Id}");
                        }

                        if (!buyer.PurchasedProducts.Any())
                        {
                            Console.WriteLine("\nNemate povijest kupljenih proizvoda.");
                        }
                        Console.WriteLine("\nPritisnite bilo što za povratak...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("Omiljeni proizvodi:\n");

                        foreach (var favourites in buyer.FavouriteProducts)
                        {
                            Console.WriteLine($"\nNaziv: {favourites.Title}\nCijena: {favourites.Price}");
                            Console.WriteLine($"ID: {favourites.Id}");
                        }

                        if (buyer.FavouriteProducts.Count == 0)
                        {
                            Console.WriteLine("\nNemate omiljenih proizvoda.");
                        }
                        Console.WriteLine("\nPritisnite bilo što za povratak...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "7":
                        BuyerService.RateProduct(marketplace, buyer);
                        break;

                    case "0":
                        Console.Clear();
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
