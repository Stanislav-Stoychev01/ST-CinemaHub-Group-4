using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class MovieTheater : Entity
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int NumberOfSeatsPerRow { get; set; }
    }
}
