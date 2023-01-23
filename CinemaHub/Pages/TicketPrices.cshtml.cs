using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Domain;
using CinemaHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaHub.Pages
{
    public class TicketPricesModel : PageModel
    {
        private readonly ITicketPriceService ticketPriceService;

        public TicketPricesModel(ITicketPriceService ticketPriceService)
        {
            this.ticketPriceService = ticketPriceService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await ticketPriceService.FindAllAsync(null);
            var weekdays3D = all.FirstOrDefault(x =>
                x.ScreeningType == ScreeningType._3D && x.DaysOfTheWeek == DaysOfTheWeek.Weekdays);
            if (weekdays3D != null)
            {
                ViewData["Weekdays3D"] = weekdays3D.Price;
            }
            var weekend3D = all.FirstOrDefault(x =>
                x.ScreeningType == ScreeningType._3D && x.DaysOfTheWeek == DaysOfTheWeek.Weekend);
            if (weekend3D != null)
            {
                ViewData["Weekend3D"] = weekend3D.Price;
            }

            var weekdays2D = all.FirstOrDefault(x =>
                x.ScreeningType == ScreeningType._2D && x.DaysOfTheWeek == DaysOfTheWeek.Weekdays);
            if (weekdays2D != null)
            {
                ViewData["Weekdays2D"] = weekdays2D.Price;
            }
            var weekend2D = all.FirstOrDefault(x =>
                x.ScreeningType == ScreeningType._2D && x.DaysOfTheWeek == DaysOfTheWeek.Weekend);
            if (weekend2D != null)
            {
                ViewData["Weekend2D"] = weekend2D.Price;
            }

            return Page();
        }
    }
}
