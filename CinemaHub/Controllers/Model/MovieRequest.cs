using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieRequest
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<string> Genres { get; set; }
        public string Actors { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public string TrailerId { get; set; }
    }
}
