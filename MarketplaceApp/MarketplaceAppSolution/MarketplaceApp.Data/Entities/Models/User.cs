using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data;

namespace MarketplaceApp.Data.Entities.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }

}
