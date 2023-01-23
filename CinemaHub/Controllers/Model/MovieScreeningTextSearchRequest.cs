using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieScreeningTextSearchRequest : TextSearchRequest
    {
        public Guid? MovieId { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public Guid? TheaterId { get; set; }
        public bool? IsPremiere { get; set; }
    }
}
