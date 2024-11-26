using MarketplaceApp.Data;
using MarketplaceApp.Domain;
using MarketplaceApp.Presentation;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


class Program
{
    static void Main()
    {
        Marketplace marketplace = new Marketplace();

        marketplace.RegisterBuyer("Ana Anic", "ana@gmail.com", 5000);

        marketplace.RegisterSeller("Marko Markic", "marko@gmail.com");

        var ana = (Buyer)marketplace.LoginUser("ana@gmail.com");
        var marko = (Seller)marketplace.LoginUser("marko@gmail.com");

        marketplace.AddProduct(marko, "Laptop", "Brzi laptop za rad", 300, "Elektronika");
        marketplace.AddProduct(marko, "Pametni telefon", "Pametni telefon", 450, "Elektronika");
        marketplace.AddProduct(marko, "Kosulja", "Odjevni predmet", 300, "Roba");
        marketplace.AddProduct(marko, "Sat", "Otkucava", 450, "Nakit");

        marketplace.AddPromoCode("ELEC10", "Elektronika", 10, DateTime.Now.AddDays(10));
        marketplace.AddPromoCode("SALE20", "Roba", 20, DateTime.Now.AddDays(5));

        bool anaKupila = marketplace.TryBuyProduct(ana, marko.Products.First(p => p.Title == "Pametni telefon"));
        anaKupila = marketplace.TryBuyProduct(ana, marko.Products.First(p => p.Title == "Kosulja"));



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
                            HandleRegistration.RegisterBuyer(marketplace);
                            break;
                        }
                        else if (role == "2")
                        {
                            HandleRegistration.RegisterSeller(marketplace);
                            break;
                        }
                        else if(role == "0")
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
                    HandleLogin.LoginUser(marketplace);
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