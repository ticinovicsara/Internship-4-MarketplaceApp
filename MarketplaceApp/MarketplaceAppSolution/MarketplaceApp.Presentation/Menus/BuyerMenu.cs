using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;


namespace MarketplaceApp.Presentation.Menus
{
    public class BuyerMenu
    {
        public static void ShowBuyerMenu(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Kupac: {buyer.Name}\n");
                Console.WriteLine("1 - Pregled dostupnih proizvoda");
                Console.WriteLine("2 - Kupnja proizvoda");
                Console.WriteLine("3 - Povratak proizvoda");
                Console.WriteLine("4 - Dodavanje proizvoda u omiljene");
                Console.WriteLine("5 - Pregled povijesti kupljenih proizvoda");
                Console.WriteLine("6 - Pregled omiljenih proizvoda");
                Console.WriteLine("7 - Ocjenjivanje proizvoda");
                Console.WriteLine("\n0 - Povratak / Odjava\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();

                        var products = marketplaceRepository.GetAvailableProducts();
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
                        BuyProduct(marketplaceRepository, buyer);
                        break;

                    case "3":
                        ReturnProduct(marketplaceRepository, buyer);
                        break;

                    case "4":
                        AddToFavourites(marketplaceRepository, buyer);
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
                        RateProduct(marketplaceRepository, buyer);
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

        public static void AddToFavourites(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {

            Console.Clear();
            Console.WriteLine("Dodavanje proizvoda u omiljene:\n");

            Product productToFavourite = null;
            while (true)
            {
                Console.WriteLine("Dostupni proizvodi:\n");
                foreach (var availableProduct in marketplaceRepository.GetAvailableProducts())
                {
                    Console.WriteLine($"\nNaziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                    Console.WriteLine($"ID: {availableProduct.Id}");
                }

                Console.WriteLine("\nUnesite naziv proizvoda koji zelite dodati u omiljene: ");
                string favouriteProduct = Console.ReadLine();

                productToFavourite = marketplaceRepository.GetAvailableProducts().FirstOrDefault(p => p.Title.Equals(favouriteProduct, StringComparison.OrdinalIgnoreCase));

                if (productToFavourite == null)
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod s unesenim nazivom nije pronadjen\n");
                }
                else if (buyer.FavouriteProducts.Contains(productToFavourite))
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod je vec u omiljenima, odaberite neki drugi\n");
                }
                else
                {
                    break;
                }
            }

            if (productToFavourite != null)
            {
                buyer.FavouriteProducts.Add(productToFavourite);
                Console.WriteLine($"\nProizvod '{productToFavourite.Title}' je uspjesno dodan u omiljene\n");
            }
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static bool IsValid(string productCategory, DateTime currentDate, MarketplaceRepository marketplaceRepository)
        {
            PromoCode promo = marketplaceRepository.GetPromoCodesByCategory(productCategory).FirstOrDefault(pc => pc.Category.Equals(productCategory, StringComparison.OrdinalIgnoreCase));

            if (promo == null)
            {
                return false;
            }

            if (promo.ExpirationDate  < currentDate)
            {
                return false;
            }

            return true;
        }



        public static void BuyProduct(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();
            Console.WriteLine("Kupnja prozvoda:\n");

            Console.WriteLine("Dostupni proizvodi:");
            foreach (var availableProduct in marketplaceRepository.GetAvailableProducts())
            {
                Console.WriteLine($"\nNaziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                Console.WriteLine($"ID: {availableProduct.Id}");
            }

            Guid productId;
            while (true)
            {
                Console.WriteLine("\nUnesite ID proizvoda ili 'exit' za povratak:");
                string input = Console.ReadLine();

                
                if (input.ToLower() == "exit")
                {
                    Console.Clear();
                    return;
                }
                else if (Guid.TryParse(input, out productId))
                {
                    break;
                }
                Console.Clear();
                Console.WriteLine("Neispravan unos, unesite valjani ID proizvoda\n");
            }

            var product = marketplaceRepository.GetAvailableProducts().FirstOrDefault(p => p.Id.ToString() == productId.ToString());

            if (product == null)
            {
                Console.WriteLine("Proizvod s unesenim ID-om nije pronadjen ili nije dostupan");
                return;
            }

            string promoCode = null;
            while (true)
            {
                Console.WriteLine("\nImate li promo kod? (yes/no): ");
                string confirm = Console.ReadLine().ToLower();

                if (confirm == "yes")
                {
                    Console.WriteLine("Unesite kod: ");
                    promoCode = Console.ReadLine();

                    var promo = marketplaceRepository.GetPromoCodesByCategory(product.Category).FirstOrDefault(pc => pc.Code.Equals(promoCode, StringComparison.OrdinalIgnoreCase));

                    string product_category = product.Category;
                    if (promo == null || IsValid(product_category, DateTime.Now, marketplaceRepository))
                    {
                        Console.WriteLine("Uneseni kod nije valjan ili je istekao\n");
                        promoCode = null;
                    }
                    break;
                }
                else if (confirm == "no")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unos nije valjan, unesite ponovno\n");
                }
            }

            if (product != null && marketplaceRepository.TryBuyProduct(buyer, product))
            {
                Console.Clear();
                Console.WriteLine($"\nUspjesno ste kupili proizvod '{product.Title}'\n");
            }
            else
            {
                Console.WriteLine("\nNeodovljan iznos na racunu ili proivod nije dostupan\n");
            }
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ReturnProduct(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();
            Console.WriteLine("Povratak kupljenog prozvoda:\n");

            Console.WriteLine("Kupljeni proizvodi:");

            if (buyer.PurchasedProducts.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("\nNemate kupljenih prozvoda");
                return;
            }

            Product productToReturn = null;
            while (true)
            {
                foreach (var product in buyer.PurchasedProducts)
                {
                    Console.WriteLine($"\nNaziv: {product.Title}\nCijena: {product.Price}\nID: {product.Id}");
                }
                Console.WriteLine("\nUnesite ID proizvoda za povrat:");
                string selectedProductId = Console.ReadLine();

                if (string.IsNullOrEmpty(selectedProductId))
                {
                    Console.WriteLine("ID ne moze biti prazan, pokusajte ponovo.\n");
                    continue;
                }

                if (!Guid.TryParse(selectedProductId, out Guid parsedGuid))
                {
                    Console.WriteLine("Uneseni ID nije validan GUID, pokusajte ponovo.\n");
                    continue;
                }

                productToReturn = buyer.PurchasedProducts.FirstOrDefault(p => p.Id == parsedGuid);

                if (productToReturn == null)
                {
                    Console.WriteLine("Proizvod s unesenim ID-om nije pronadjen, pokusajte ponovo.\n");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Dosllvksdv");

            if (marketplaceRepository.ReturnProduct(buyer, productToReturn))
            {
                Console.Clear();
                Console.WriteLine($"\nProizvod '{productToReturn.Title}' je uspjesno vracen.");
            }
            else
            {
                Console.WriteLine("\nNeuspjesno vracanje proizvoda\n");
                Console.WriteLine("\nPritisnite bilo sto za povratak...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void RateProduct(MarketplaceRepository marketplaceRepository, Buyer buyer)
        {
            Console.Clear();

            Console.WriteLine("Ocijeni proizvod:\n");

            var availableProducts = marketplaceRepository.GetAvailableProducts();

            Product selectedProduct = null;
            while (true)
            {
                Console.WriteLine("Dostupni proizvodi:");
                foreach (var product in availableProducts)
                {
                    Console.WriteLine($"\tNaziv: {product.Title} - Cijena: {product.Price} - Opis: {product.Description}");
                }

                Console.WriteLine("\nUnesite naziv proizvoda kojeg zelite ocijeniti: ");
                string name = Console.ReadLine();

                selectedProduct = availableProducts.FirstOrDefault(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (selectedProduct == null)
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod nije pronadjen, pokusajte ponovno\n");
                }
                else
                {
                    break;
                }
            }

            decimal rating;
            while (true)
            {
                Console.WriteLine($"\nOdabrali ste proizvod: {selectedProduct.Title}\n");
                Console.WriteLine("\nUnesite ocjenu (1 - 5): ");

                if (!decimal.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5)
                {
                    Console.Clear();
                    Console.WriteLine("Neispravan unos, pokusajte ponovno\n");
                }
                else
                {
                    break;
                }
            }

            try
            {
                marketplaceRepository.AddRating(selectedProduct, rating);
                Console.WriteLine($"\nUspjesno ste dodali ocjenu {rating} proizvodu '{selectedProduct.Title}'\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dogodila se pogreska prilikom ocjenjivanja: {ex.Message}\n");
            }

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

