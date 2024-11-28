using System;
using System.Collections.Concurrent;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

using MarketplaceApp.Domain.Repositories;
namespace MarketplaceApp.Presentation;

class Program
{
    static void Main()
    {
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
                            UserRepository.RegisterBuyer(marketplace);
                            break;
                        }
                        else if (role == "2")
                        {
                            UserRepository.RegisterSeller(marketplace);
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
                    UserRepository.LoginUser(marketplace);
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
