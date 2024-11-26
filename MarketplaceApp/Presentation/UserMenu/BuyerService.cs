using MarketplaceApp.Data;
using MarketplaceApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.UserMenu
{
    public interface BuyerService
    {
        public static void AddToFavourites(Marketplace marketplace, Buyer buyer)
        {

            Console.Clear();
            Console.WriteLine("Dodavanje proizvoda u omiljene:\n");

            Product productToFavourite = null;
            while (true)
            {
                Console.WriteLine("Dostupni proizvodi:\n");
                foreach (var availableProduct in marketplace.GetAvailableProducts())
                {
                    Console.WriteLine($"\nNaziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                    Console.WriteLine($"ID: {availableProduct.Id}");
                }

                Console.WriteLine("\nUnesite naziv proizvoda koji zelite dodati u omiljene: ");
                string favouriteProduct = Console.ReadLine();

                productToFavourite = marketplace.GetAvailableProducts().FirstOrDefault(p => p.Title.Equals(favouriteProduct, StringComparison.OrdinalIgnoreCase));

                if (productToFavourite == null)
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod s unesenim nazivom nije pronadjen\n");
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

        public static void BuyProduct(Marketplace marketplace, Buyer buyer)
        {
            Console.Clear();
            Console.WriteLine("Kupnja prozvoda:\n");

            Console.WriteLine("Dostupni proizvodi:");
            foreach (var availableProduct in marketplace.GetAvailableProducts())
            {
                Console.WriteLine($"\nNaziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                Console.WriteLine($"ID: {availableProduct.Id}");
            }

            Guid productId;
            while (true)
            {
                Console.WriteLine("\nUnesite ID proizvoda ili 'exit' za povratak:");
                string input = Console.ReadLine();

                if (Guid.TryParse(input, out productId))
                {
                    break;
                }
                else if (input.ToLower() == "exit")
                {
                    Console.Clear();
                    return;
                }
                Console.WriteLine("Neispravan unos, unesite valjani ID proizvoda\n");
            }

            var product = marketplace.GetAvailableProducts().FirstOrDefault(p => p.Id == productId);

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

                    var promo = marketplace.GetPromoCodesByCategory(product.Category).FirstOrDefault(pc => pc.Code.Equals(promoCode, StringComparison.OrdinalIgnoreCase));

                    if (promo == null || promo.IsValid(product.Category, DateTime.Now))
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

            if (product != null && marketplace.TryBuyProduct(buyer, product))
            {
                Console.Clear();
                Console.WriteLine($"\nUspjesno ste kupili proizvod '{product.Title}'\n");
            }
            else
            {
                Console.WriteLine("Neodovljan iznos na racunu ili proivod nije dostupan\n");
            }
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ReturnProduct(Marketplace marketplace, Buyer buyer)
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

                productToReturn = buyer.PurchasedProducts.FirstOrDefault(p => p.Id.ToString() == selectedProductId);

                if (productToReturn == null)
                {
                    Console.Clear();
                    Console.WriteLine("Proizvod s unesenim ID-om nije pronadjen, pokusajte ponovno");
                }
                else if (string.IsNullOrEmpty(selectedProductId))
                {
                    Console.WriteLine("Neispravan unos, unesite ponovno\n");
                }
                else
                {
                    break;
                }
            }

            try
            {
                marketplace.ReturnProduct(buyer, productToReturn);
                Console.Clear();
                Console.WriteLine($"\nProizvod '{productToReturn.Title}' je uspjesno vracen.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
            Console.Clear();
        }


        public static void RateProduct(Marketplace marketplace, Buyer buyer)
        {
            Console.Clear();

            Console.WriteLine("Ocijeni proizvod:\n");

            var availableProducts = marketplace.GetAvailableProducts();

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
                marketplace.AddRating(selectedProduct, rating);
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
