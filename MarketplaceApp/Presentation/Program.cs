using MarketplaceApp.Data;
using MarketplaceApp.Domain;
using System;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main()
    {
        Marketplace marketplace = new Marketplace();

        marketplace.RegisterBuyer("Ivan Ivic", "ivan@gmail.com", 500m);
        marketplace.RegisterBuyer("Ana Anic", "ana@gmail.com", 50m);

        marketplace.RegisterSeller("Marko Markic", "marko@gmail.com");
        marketplace.RegisterSeller("Jana Voda", "jana@gmail.com");

        var ivan = (Buyer)marketplace.LoginUser("ivan@gmail.com");
        var ana = (Buyer)marketplace.LoginUser("ana@gmail.com");
        var marko = (Seller)marketplace.LoginUser("marko@gmail.com");
        var jana = (Seller)marketplace.LoginUser("jana@gmail.com");

        marketplace.AddProduct(marko, "Laptop", "Brzi laptop za rad", 300m, "Elektronika");
        marketplace.AddProduct(jana, "Pametan telefon", "Pametan telefon", 450m, "Elektronika");

        bool ivanKupio = marketplace.TryBuyProduct(ivan, marko.Products.First(p => p.Title == "Laptop"));
        bool anaKupila = marketplace.TryBuyProduct(ana, jana.Products.First(p => p.Title == "Pametan telefon"));

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

                        string role = Console.ReadLine();

                        if (role == "1")
                        {
                            RegisterBuyer(marketplace);
                            break;
                        }
                        else if (role == "2")
                        {
                            RegisterSeller(marketplace);
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Neispravan odabir unesite ponovno\n");
                        }
                    }
                    break;

                case "2":
                    LoginUser(marketplace);
                    break;

                case "3":
                    Console.WriteLine("Byee!\n");
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Neispravan unos, pokusajte ponovno\n");
                    break;
            }
        }

        static void RegisterBuyer(Marketplace marketplace)
        {
            string name = GetValidName();

            string email;
            while (true)
            {
                Console.WriteLine("Unesite email:");
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                break;
            }

            decimal balance;
            while (true)
            {
                Console.WriteLine("Unesite pocetni iznos:");
                if(decimal.TryParse(Console.ReadLine(), out balance) && balance > 0)
                {
                    break;
                }
                Console.WriteLine("Iznos mora biti veci od 0\n");
            }
            

            marketplace.RegisterBuyer(name, email, balance);
            Console.WriteLine("Kupac uspjesno registriran\n");

            Console.WriteLine("\nPritisnite bilo sto za povratak...\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }


        static void RegisterSeller(Marketplace marketplace)
        {
            string name = GetValidName();

            string email;
            while (true)
            {
                Console.WriteLine("Unesite email:");
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                break;
            }

            marketplace.RegisterSeller(name, email);
            Console.WriteLine("Prodavac uspjesno registriran\n");

            Console.WriteLine("\nPritisnite bilo sto za povratak...\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }


        static string GetValidName()
        {
            while (true)
            {
                Console.WriteLine("\nUnesite ime:");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }

                if (name.Any(char.IsDigit))
                {
                    Console.Clear();
                    Console.WriteLine("Ime ne moze sadrzavati brojeve, pokusajte ponovno\n");
                    continue;
                }

                return name;
            }
        }

        static void LoginUser(Marketplace marketplace)
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

                if(user is Buyer buyer)
                {
                    ShowBuyerMenu(marketplace, buyer);
                }
                else if (user is Seller seller)
                {
                    ShowSellerMenu(marketplace, seller);
                }
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
                Console.WriteLine("\nPritisnite bilo sto za povratak...");
                Console.ReadKey();
            }
        }

        static void ShowBuyerMenu(Marketplace marketplace, Buyer buyer)
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
                Console.WriteLine("\n0 - Povratak / Odjava\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();

                        var products = marketplace.GetAvailableProducts();
                        Console.WriteLine("Dostupni proizvodi:");
                        foreach(var product_ in products)
                        {
                            Console.WriteLine($"Naziv: {product_.Title} - Cijena: {product_.Price}");
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("Kupnja prozvoda:\n");

                        Console.WriteLine("Dostupni proizvodi:");
                        foreach (var availableProduct in marketplace.GetAvailableProducts())
                        {
                            Console.WriteLine($"Naziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                        }

                        Guid productId;
                        while (true)
                        {
                            Console.WriteLine("Unesite ID proizvoda:");
                            string input = Console.ReadLine();

                            if (Guid.TryParse(input, out productId))
                            {
                                break;
                            }
                            Console.WriteLine("Neispravan unos, unesite valjani ID proizvoda.");
                        }

                        var product = marketplace.GetAvailableProducts().FirstOrDefault(p => p.Id == productId);

                        if(product != null && marketplace.TryBuyProduct(buyer, product))
                        {
                            Console.WriteLine($"Uspjesno ste kupili proizvod '{product.Title}'\n");
                        }
                        else
                        {
                            Console.WriteLine("Neodovljan iznos na racunu ili proivod nije dostupan\n");
                        }

                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("Povratak kupljenog prozvoda:\n");

                        Console.WriteLine("Kupljeni proizvodi:");
                        foreach (var product_ in buyer.PurchasedProducts)
                        {
                            Console.WriteLine($"ID: {product_.Id} - Naziv: {product_.Title} - Cijena: {product_.Price}");
                        }

                        if (buyer.PurchasedProducts.Count == 0) 
                        {
                            Console.Clear();
                            Console.WriteLine("\nNemate kupljenih prozvoda");
                            Console.WriteLine("\nPritisnite bilo sto za povratak...");
                            Console.ReadKey();
                            break;
                        }

                        Console.WriteLine("\nUnesite ID proizvoda za povrat:");
                        string selectedProductId = Console.ReadLine();

                        var productToReturn = buyer.PurchasedProducts.FirstOrDefault(p => p.Id.ToString()  ==  selectedProductId);

                        if (productToReturn == null)
                        {
                            Console.WriteLine("Proizvod s unesenim ID-om nije pronadjen.");
                            Console.WriteLine("\nPritisnite bilo sto za povratak...");
                            Console.ReadKey();
                            break;
                        }

                        try
                        {
                            marketplace.ReturnProduct(buyer, productToReturn);
                            Console.WriteLine($"Proizvod {productToReturn.Title} uspjesno vracen.");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;


                    case "4":
                        Console.Clear();
                        Console.WriteLine("Dodavanje proizvoda u omiljene:\n");

                        Console.WriteLine("Dostupni proizvodi:");
                        foreach(var availableProduct in marketplace.GetAvailableProducts())
                        {
                            Console.WriteLine($"ID: {availableProduct.Id} - Naziv: {availableProduct.Title} - Cijena: {availableProduct.Price}");
                        }

                        Console.WriteLine("Unesite naziv proizvoda koji zelite dodati u omiljene: ");
                        string favouriteProduct = Console.ReadLine();

                        var productToFavourite = marketplace.GetAvailableProducts().FirstOrDefault(p => p.Title.Equals(favouriteProduct, StringComparison.OrdinalIgnoreCase));

                        if (productToFavourite == null)
                        {
                            Console.WriteLine("Proizvod s unesenim nazivom nije pronadjen.");
                        }
                        else
                        {
                            buyer.FavouriteProducts.Add(productToFavourite);
                            Console.WriteLine($"Proizvod '{productToFavourite.Title}' je uspjesno dodan u omiljene\n");
                        }

                        break;


                    case "5":
                        Console.Clear();
                        Console.WriteLine("Povijest kupljenih proizvoda:\n");

                        foreach (var purchasedProduct in buyer.PurchasedProducts)
                        {
                            Console.WriteLine($"ID: {purchasedProduct.Id} - Naziv: {purchasedProduct.Title} - Cijena: {purchasedProduct.Price}");
                        }

                        if(!buyer.PurchasedProducts.Any())
                        {
                            Console.WriteLine("\nNemate povijest kupljenih proizvoda.");
                        }
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("Povijest kupljenih proizvoda:\n");

                        foreach (var favourites in buyer.FavouriteProducts)
                        {
                            Console.WriteLine($"ID: {favourites.Id} - Naziv: {favourites.Title} - Cijena: {favourites.Price}");
                        }

                        if (buyer.FavouriteProducts.Count == 0)
                        {
                            Console.WriteLine("\nNemate omiljenih proizvoda.");
                        }

                        break;

                    case "0":
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Neispravan unos, pokusajte ponovno\n");
                        break;
                }

                Console.WriteLine("Pritisnite bilo sto za povratak...");
                Console.ReadKey();
                Console.Clear();
                break;
            }
        }

        static void ShowSellerMenu(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Prodavac: {seller.Name}\n");
                Console.WriteLine("1 - Dodavanje novog proizvoda");
                Console.WriteLine("2 - pregled svih proizvoda u vlasnistvu");
                Console.WriteLine("3 - Pregled ukupne zarade");
                Console.WriteLine("4 - Pregled prodanih proizvoda po kategoriji");
                Console.WriteLine("5 - Pregled zarade po vremenskom razdoblju");
                Console.WriteLine("\n0 - Povratak / Odjava\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewProduct(marketplace, seller);
                        break;

                    case "2":
                        ViewAllProducts(marketplace, seller);
                        break;

                    case "3":
                        ViewTotalEarnings(marketplace, seller);
                        break;

                    case "4":
                        ViewSoldProductsByCategory(marketplace, seller);
                        break;

                    case "5":
                        ViewEarningsForTimePeriod(marketplace, seller);
                        break;

                    case "0":
                        Console.Clear();
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Neispavan unos, pokusajte ponovno\n");
                        break;
                }

                Console.WriteLine("\nPritisnite bilo sto za povratak...");
                Console.ReadKey();
                break;
            }
        }

        static void AddNewProduct(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            Console.WriteLine("Dodaj novi proizvod:\n");

            string title;
            var currentProducts = marketplace.GetAvailableProducts();
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
                else if(currentProducts.Any(p => p.Title == title))
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
                Console.WriteLine("Unesite cijenu proizvoda");
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

            marketplace.AddProduct(seller, title, desctiption, price, category);

            Console.Clear();
            Console.WriteLine("Proizvod uspjesno dodan!\n");
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
        }


        static void ViewAllProducts(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled svih proizvoda u vlasnistvu: {seller.Name}\n");

            foreach (var product in seller.Products)
            {
                Console.WriteLine($"ID: {product.Id} - Naziv: {product.Title} - Cijena: {product.Price} - Opis: {product.Description} - Status: {product.Status} - Kategorija: {product.Category} - Prosjecna ocjena: {product.AverageRating}");
            }

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
        }

        static void ViewTotalEarnings(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled ukupne zarade prodavaca: {seller.Name}");

            Console.WriteLine($"Ukupna zarada: {seller.Earnings}\n");
            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
        }

        static void ViewSoldProductsByCategory(Marketplace marketplace, Seller seller)
        {
            Console.Clear();
            Console.WriteLine($"Pregled prodanih proizvoda po kategoriji\n");

            string input;
            var categories = marketplace.GetCategories();

            while (true)
            {
                Console.WriteLine("Dostupne kategorije:\n");
                foreach (var category in categories)
                {
                    Console.WriteLine($"\t{category}");
                }

                Console.WriteLine("\nUnesite jednu kategoriju\n");
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    Console.WriteLine("Ne mozete unijeti prazno, pokusajte ponovno\n");
                    continue;
                }
                else if(!categories.Contains(input))
                {
                    Console.Clear();
                    Console.WriteLine("Kategorija ne postoji, pokusajte ponovno\n");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"Proizvodi kategorije '{input}'");

            var productsInCategories = marketplace.GetProductsByCategory(input);
            foreach (var product in productsInCategories)
            {
                Console.WriteLine($"\tNaziv: {product.Title} - Cijena: {product.Price}");
            }

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
        }

        static void ViewEarningsForTimePeriod(Marketplace marketplace, Seller seller)
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

            decimal earnings = marketplace.GetEarningsForTmePeriod(seller, startDate, endDate);

            Console.WriteLine($"Ukupna zarada od {startDate:dd.MM.yyyy} do {endDate:dd.MM.yyyy} iznosi: {earnings}");

            Console.WriteLine("\nPritisnite bilo sto za povratak...");
            Console.ReadKey();
        }
    }
}