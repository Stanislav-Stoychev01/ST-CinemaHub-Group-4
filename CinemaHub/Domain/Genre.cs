using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class Genre : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<MovieGenres> MovieGenres { get; set; }
    }
}
