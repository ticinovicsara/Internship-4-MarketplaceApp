using MarketplaceApp.Data;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.UserActions
{
    public class HandleRegistration
    {
        private readonly Context _context;
        private readonly MarketplaceRepository _marketplaceRepository;

        public HandleRegistration(Context context, MarketplaceRepository marketplaceRepository)
        {
            _context = context;
            _marketplaceRepository = marketplaceRepository;
        }

        public void RegisterBuyer()
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
                if (decimal.TryParse(Console.ReadLine(), out balance) && balance > 0)
                {
                    break;
                }
                Console.WriteLine("Iznos mora biti veci od 0\n");
            }


            _marketplaceRepository.RegisterBuyer(name, email, balance);
            Console.WriteLine("\nKupac uspjesno registriran\n");

            Console.WriteLine("\nPritisnite bilo sto za povratak...\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        public void RegisterSeller()
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

            _marketplaceRepository.RegisterSeller(name, email);
            Console.WriteLine("\nProdavac uspjesno registriran\n");

            Console.WriteLine("\nPritisnite bilo sto za povratak...\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }


        private string GetValidName()
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
    }
}

