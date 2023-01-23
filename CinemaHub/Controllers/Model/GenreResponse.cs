using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class GenreResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn  { get; set; }
    }
}
