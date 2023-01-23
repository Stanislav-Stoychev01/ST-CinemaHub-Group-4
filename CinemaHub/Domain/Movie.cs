using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class Movie : Entity
    {
        public Movie()
        {
            MovieGenres = new List<MovieGenres>();
        }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Duration { get; set; }
        public ICollection<MovieGenres> MovieGenres { get; set; }
        public string Actors { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string TrailerId { get; set; }
    }
}
