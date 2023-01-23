using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class PaginatedRequest
    {
        public int StartAt { get; set; } = 0;
        public int Count { get; set; } = 100;
        public IEnumerable<string> SortFields { get; set; } = null;
    }
}
