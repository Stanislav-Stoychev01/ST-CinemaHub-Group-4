using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class MovieGenres : Entity
    {
        public Movie Movie { get; set; }
        public Guid MovieId { get; set; }
        public Genre Genre { get; set; }
        public Guid GenreId { get; set; }
    }
}
