using MarketplaceApp.Data;
using MarketplaceApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.UserMenu
{
    public  class SellerMenu
    {
        public static void ShowSellerMenu(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Prodavac: {seller.Name}\n");
                Console.WriteLine("1 - Dodavanje novog proizvoda");
                Console.WriteLine("2 - Pregled svih proizvoda u vlasnistvu");
                Console.WriteLine("3 - Pregled ukupne zarade");
                Console.WriteLine("4 - Pregled prodanih proizvoda po kategoriji");
                Console.WriteLine("5 - Pregled zarade po vremenskom razdoblju");
                Console.WriteLine("6 - Pregled ocjena proizvoda");
                Console.WriteLine("\n0 - Povratak / Odjava\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SellerService.AddNewProduct(marketplace, seller);
                        break;

                    case "2":
                        SellerService.ViewAllProducts(marketplace, seller);
                        break;

                    case "3":
                        SellerService.ViewTotalEarnings(marketplace, seller);
                        break;

                    case "4":
                        SellerService.ViewSoldProductsByCategory(marketplace, seller);
                        break;

                    case "5":
                        SellerService.ViewEarningsForTimePeriod(marketplace, seller);
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("Pregled ocjena:\n");
                        var allProducts = marketplace.GetAvailableProducts();

                        foreach (var product in allProducts)
                        {
                            Console.WriteLine($"\nNaziv: {product.Title} - Prosjecna ocjena: {product.AverageRating}");
                        }
                        Console.WriteLine("\nPritisnite bilo sto za povratak...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "0":
                        Console.Clear();
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Neispavan unos, pokusajte ponovno\n");
                        break;
                }
            }
        }

    }
}
