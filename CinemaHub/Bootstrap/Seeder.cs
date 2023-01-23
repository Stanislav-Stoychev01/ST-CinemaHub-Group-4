using System;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Data;
using CinemaHub.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaHub.Bootstrap
{
    public static class Seeder
    {
        public static async Task RunDbMigrations(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            using var scope = provider.GetService<IServiceScopeFactory>().CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await applicationDbContext.Database.MigrateAsync();
        }

        public static void SeedDatabase(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider(); 
            SeedDatabase(provider);
        }

        public static void SeedDatabase(IServiceProvider provider)
        {
            using var scope = provider.GetService<IServiceScopeFactory>().CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            SeedRoles(applicationDbContext);
            SeedTicketPrices(applicationDbContext);
        }

        private static void SeedRoles(ApplicationDbContext context)
        {
            context.Roles.RemoveRange(context.Roles.ToList());
            foreach (var role in Config.GetRoles())
            {
                context.Roles.Add(role);
            }

            context.SaveChanges();
        }

        private static void SeedTicketPrices(ApplicationDbContext context)
        {
            if (!context.TicketPrices.Any())
            {
                context.TicketPrices.Add(new TicketPrice()
                    {DaysOfTheWeek = DaysOfTheWeek.Weekdays, ScreeningType = ScreeningType._2D});
                context.TicketPrices.Add(new TicketPrice()
                    { DaysOfTheWeek = DaysOfTheWeek.Weekend, ScreeningType = ScreeningType._2D });
                context.TicketPrices.Add(new TicketPrice()
                    { DaysOfTheWeek = DaysOfTheWeek.Weekdays, ScreeningType = ScreeningType._3D });
                context.TicketPrices.Add(new TicketPrice()
                    { DaysOfTheWeek = DaysOfTheWeek.Weekend, ScreeningType = ScreeningType._3D });
            }
            context.SaveChanges();
        }
    }
}