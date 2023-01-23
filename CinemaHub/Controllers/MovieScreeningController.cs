using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/movie-screenings")]
    [ApiController]
    public class MovieScreeningController : ControllerBase
    {
        private readonly IMovieScreeningService movieScreeningService;
        private readonly IMapper mapper;

        public MovieScreeningController(IMapper mapper, IMovieScreeningService movieScreeningService)
        {
            this.mapper = mapper;
            this.movieScreeningService = movieScreeningService;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(List<MovieScreeningResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetMovieScreeningsAsync([FromQuery] PaginatedRequest paginatedRequest)
        {
            var request = new MovieScreeningTextSearchRequest()
            {
                SortFields = new List<string>() { "+DateTime" },
                Count = paginatedRequest.Count,
                StartAt = paginatedRequest.StartAt,
                FromDateTime = DateTime.Now
            };
            var screenings = await movieScreeningService.SearchAsync(request);
            var count = await movieScreeningService.CountAsync(request);

            var response = screenings.Select(x => mapper.Map<MovieScreeningResponse>(x));
            var paginatedResponse =
                new PaginatedResponse<MovieScreeningResponse>(response, paginatedRequest.StartAt, count);

            return Ok(paginatedResponse);
        }
    }
}
