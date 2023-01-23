using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;
using CinemaHub.Services;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/admin/ticket-prices")]
    [ApiController]
    [Authorize(policy: "AdminOnly")]
    public class TicketPriceAdminController : ControllerBase
    {
        private readonly ITicketPriceService ticketPriceService;
        private readonly IMapper mapper;

        public TicketPriceAdminController(ITicketPriceService ticketPriceService, IMapper mapper)
        {
            this.ticketPriceService = ticketPriceService;
            this.mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<TicketPriceResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllAsync()
        {

            var all = await ticketPriceService.FindAllAsync(null);

            var ticketPrices = all.Select(x => mapper.Map<TicketPriceResponse>(x)).ToList();
            var response = new PaginatedResponse<TicketPriceResponse>()
                {Count = ticketPrices.Count, StartAt = 0, TotalCount = ticketPrices.Count, Data = ticketPrices};
            return Ok(response);
        }

        [HttpPut("{id}")]
        [SwaggerResponse(200, "Success", typeof(TicketPriceResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateTicketPriceAsync(Guid id, [FromBody] TicketPriceRequest request)
        {
            var ticketPrice = await ticketPriceService.GetByIdAsync(id);
            if (ticketPrice == null) throw new ValidationException(new ValidationError("id", "Ticket price not found"));

            ticketPrice.Price = request.Price;
            ticketPrice = await ticketPriceService.UpdateAsync(id, ticketPrice);
            var response = mapper.Map<TicketPriceResponse>(ticketPrice);
            return Ok(response);
        }
    }
}
