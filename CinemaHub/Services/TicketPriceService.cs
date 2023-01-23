using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Data;
using CinemaHub.Domain;

namespace CinemaHub.Services
{
    public class TicketPriceService : CrudServiceBase<TicketPrice>, ITicketPriceService
    {
        public TicketPriceService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<TicketPrice> Query => dbContext.TicketPrices.AsQueryable();
    }
}
