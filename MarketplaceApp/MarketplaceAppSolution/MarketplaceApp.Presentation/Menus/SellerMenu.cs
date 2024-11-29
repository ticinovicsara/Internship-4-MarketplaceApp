using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;

namespace MarketplaceApp.Presentation.Menus
{
    public class SellerMenu
    {
        public static void ShowSellerMenu(MarketplaceRepository marketplaceRepository, Seller seller)
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
                        AddNewProduct(marketplaceRepository, seller);
                        break;

                    case "2":
                        ViewAllProducts(seller);
                        break;

                    case "3":
                        ViewTotalEarnings(seller);
                        break;

                    case "4":
                        ViewSoldProductsByCategory(marketplaceRepository, seller);
                        break;

                    case "5":
                        ViewEarningsForTimePeriod(marketplaceRepository, seller);
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("Pregled ocjena:\n");
                        var allProducts = marketplaceRepository.GetAvailableProducts();

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


        public static void AddNewProduct(MarketplaceRepository marketplaceRepository, Seller seller)
        {
            Console.Clear();
            Console.WriteLine("Dodaj novi proizvod:\n");

            string title;
            var currentProducts = marketplaceRepository.GetAvailableProducts();
            while (true)
            {
                Console.WriteLine("Unesite naziv proizvoda");
                title = Console.ReadLine();

                if (string.IsNullOrEmpty(title))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                else if (title.Any(char.IsDigit))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti brojeve, pokusajte ponovno\n");
                    continue;
                }
                else if (currentProducts.Any(p => p.Title == title))
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod vec postoji, pokusajte ponovno\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            string desctiption;
            while (true)
            {
                Console.WriteLine("Unesite opis proizvoda");
                desctiption = Console.ReadLine();

                if (string.IsNullOrEmpty(desctiption))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            decimal price;
            while (true)
            {
                Console.WriteLine("Unesite cijenu proizvoda:");
                if (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Neispravan unos, pokusajte ponovno\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            string category;
            while (true)
            {
                Console.WriteLine("Unesite kategoriju proizvoda:");
                category = Console.ReadLine();

                if (string.IsNullOrEmpty(category))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            marketplaceRepository.AddProduct(seller, title, desctiption, price, category);

            Console.Clear();
            Console.WriteLine("Proizvod uspjesno dodan!\n");
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }


        public static void ViewAllProducts(Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled svih proizvoda u vlasnistvu: {seller.Name}\n");

            if (seller.Products == null || seller.Products.Count == 0)
            {
                Console.WriteLine("Nemate proizvode u svojoj ponudi.");
            }
            else
            {
                foreach (var product in seller.Products)
                {
                    Console.WriteLine($"\nNaziv: {product.Title}\nCijena: {product.Price}\nStatus: {product.Status}\nKategorija: {product.Category}");
                    Console.WriteLine($"Opis: {product.Description}\nProsjecna ocjena: {product.AverageRating}\nID: {product.Id}");
                }
            }

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ViewTotalEarnings(Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled ukupne zarade prodavaca: {seller.Name}");

            Console.WriteLine($"Ukupna zarada: {seller.Earnings}\n");
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ViewSoldProductsByCategory(MarketplaceRepository marketplaceRepository, Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled prodanih proizvoda po kategoriji\n");

            string input;
            var categories = marketplaceRepository.GetCategories();

            while (true)
            {
                Console.WriteLine("Dostupne kategorije:\n");
                foreach (var category in categories)
                {
                    Console.WriteLine($"\t{category}");
                }

                Console.WriteLine("\nUnesite jednu kategoriju\n");
                input = Console.ReadLine();

                var selectedCategory = categories.Any(category => category.Equals(input, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                else if (!selectedCategory)
                {
                    Console.Clear();
                    Console.WriteLine("Kategorija ne postoji, pokusajte ponovno\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"\nProizvodi kategorije '{input}'");

            var productsInCategories = marketplaceRepository.GetProductsByCategory(input);
            foreach (var product in productsInCategories)
            {
                Console.WriteLine($"\tNaziv: {product.Title} - Cijena: {product.Price}");
            }

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ViewEarningsForTimePeriod(MarketplaceRepository marketplaceRepository,Seller seller)
        {
            Console.Clear();
            Console.WriteLine("Pregled zarade za vremensko razdoblje\n");

            DateTime startDate;
            DateTime endDate;

            while (true)
            {
                Console.WriteLine("Unesite pocetni datum (format: dd.MM.yyyy): ");
                string startInput = Console.ReadLine();

                if (DateTime.TryParseExact(startInput, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Neispravan datum, pokusajte ponovno.");
                }
            }

            while (true)
            {
                Console.WriteLine("Unesite zavrsni datum (format: dd.MM.yyyy): ");
                string endInput = Console.ReadLine();

                if (DateTime.TryParseExact(endInput, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
                {
                    if (endDate >= startDate)
                    {
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Zavrsni datum mora biti veci ili jednak pocetnom datumu, pokusajte ponovno.");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Neispravan datum, pokusajte ponovno.");
                }
            }

            decimal earnings = marketplaceRepository.GetEarningsForTimePeriod(seller, startDate, endDate);

            Console.WriteLine($"Ukupna zarada od {startDate:dd.MM.yyyy} do {endDate:dd.MM.yyyy} iznosi: {earnings}");

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

