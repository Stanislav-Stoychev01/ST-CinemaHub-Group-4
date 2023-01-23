using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class TicketPriceResponse
    {
        public Guid Id { get; set; }
        public string DaysOfTheWeek { get; set; }
        public string ScreeningType { get; set; }
        public double Price { get; set; }
    }
}
