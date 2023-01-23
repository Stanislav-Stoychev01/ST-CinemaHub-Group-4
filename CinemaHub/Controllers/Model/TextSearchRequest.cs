using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class TextSearchRequest : PaginatedRequest
    {
        public string SearchText { get; set; }
    }
}
