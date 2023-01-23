
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CinemaHub.Services.ServicesStartup))]
namespace CinemaHub.Services
{
    public class ServicesStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services
                    .AddTransient<IUserService, UserService>()
                    .AddTransient<IMovieTheaterService, MovieTheaterService>()
                    .AddTransient<IGenreService, GenreService>()
                    .AddTransient<IMovieService, MovieService>()
                    .AddTransient<IMovieScreeningService, MovieScreeningService>()
                    .AddTransient<ITicketPriceService, TicketPriceService>(); 

            });
        }
    }
}
