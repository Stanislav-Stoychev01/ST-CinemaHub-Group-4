using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieTextSearchRequest: TextSearchRequest

    {
        public string Genre { get; set; }
        public bool? IncludeInactive { get; set; }
    }
}
