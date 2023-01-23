using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieScreeningResponse
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public MovieResponse Movie { get; set; }
        public MovieTheaterResponse Theater { get; set; }
        public decimal TicketPrice { get; set; }
        public bool IsPremiere { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Type { get; set; }

    }
}
