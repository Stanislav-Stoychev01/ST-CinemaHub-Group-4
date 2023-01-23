using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class MovieScreening : Entity
    {
        public DateTime DateTime { get; set; }
        public Movie Movie { get; set; }
        public MovieTheater Theater { get; set; }

        [Column(TypeName = "decimal(19,5)")]
        public bool IsPremiere { get; set; }
        public ScreeningType Type  { get; set; }
    }

    public enum ScreeningType
    {
        _2D,
        _3D
    }
}
