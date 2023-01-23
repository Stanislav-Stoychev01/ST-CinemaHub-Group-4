using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class MovieScreeningRequest
    {
        public DateTime DateTime { get; set; }
        public Guid MovieId { get; set; }
        public Guid TheaterId { get; set; }
        public bool IsPremiere { get; set; }
        public string Type { get; set; }
    }
}
