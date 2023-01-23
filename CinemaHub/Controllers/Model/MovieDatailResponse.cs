using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieDetailResponse : MovieResponse
    {
        public string Premiere { get; set; }
        public string ScreeningTypes { get; set; }
    }
}
