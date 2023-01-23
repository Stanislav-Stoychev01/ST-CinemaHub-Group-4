using System;
using System.Collections.Generic;
using System.Linq;
using CinemaHub.Data;

namespace CinemaHub.Bootstrap
{
    public static class Config
    {
        public static IEnumerable<ApplicationRole> GetRoles()
        {
            return new[]
            {
                new ApplicationRole
                {
                    Id = new Guid("50d3f7b8-2b9e-4e17-b57b-92581a0c30d7"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "dc45397b-4968-416c-abaf-586bbd68d22e"
                },
                new ApplicationRole
                {
                    Id = new Guid("9d5fc5df-37d0-4eef-bafa-cb6909f71a0e"),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "5b3bb423-297f-408a-827a-3a6d5743b7b4"
                },
                new ApplicationRole
                {
                    Id = new Guid("dc0909b9-504f-4753-b315-8406e0967449"),
                    Name = "Cashier",
                    NormalizedName = "CASHIER",
                    ConcurrencyStamp = "1d7d2e8e-0602-488c-b0e3-fc487d2e3f47"
                }
            };
        }
        
       
    }
}