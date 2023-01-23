using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieTheaterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int NumberOfSeatsPerRow { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
