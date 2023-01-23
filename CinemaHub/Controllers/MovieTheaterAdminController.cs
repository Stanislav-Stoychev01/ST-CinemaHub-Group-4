using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;
using CinemaHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/admin/movie-theaters")]
    [Authorize(policy: "AdminOnly")]
    public class MovieTheaterAdminController : Controller
    {
        private readonly IMovieTheaterService movieTheaterService;
        private readonly IMapper mapper;
        public MovieTheaterAdminController(IMovieTheaterService movieTheaterService, IMapper mapper)
        {
            this.movieTheaterService = movieTheaterService;
            this.mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<MovieTheaterResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextSearchRequest request)
        {
            var totalCount = await movieTheaterService.CountAsync(request);
            var searchResults = await movieTheaterService.SearchAsync(request);

            var mappedResponse = searchResults.Select(x => mapper.Map<MovieTheaterResponse>(x)).ToList();
        
            var response = new PaginatedResponse<MovieTheaterResponse>(mappedResponse,
                request?.StartAt ?? 0,
                totalCount,
                request?.SortFields);

            return Ok(response);
        }
        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(MovieTheaterResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateMovieTheaterAsync([FromBody] MovieTheaterRequest request)
        {
            var movieTheater = mapper.Map<MovieTheater>(request);

            movieTheater = await movieTheaterService.AddAsync(movieTheater);
            var response = mapper.Map<MovieTheaterResponse>(movieTheater);

            return Ok(response);

        }
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Success", typeof(MovieTheaterResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateMovieTheaterAsync(Guid id, [FromBody] MovieTheaterRequest request)
        {
            var movieTheater = await movieTheaterService.GetByIdAsync(id);
            if (movieTheater == null)
                return BadRequest(
                    new ValidationFailedResponse(new ValidationError("id", "Movie Theater is not found")));

            mapper.Map(request, movieTheater);

            movieTheater = await movieTheaterService.UpdateAsync(id, movieTheater);
            var response = mapper.Map<MovieTheaterResponse>(movieTheater);

            return Ok(response);

        }

        [HttpDelete("{id}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> DeleteMovieTheaterAsync(Guid id)
        {
            var movieTheater = await movieTheaterService.GetByIdAsync(id);
            if (movieTheater == null)
                return BadRequest(
                    new ValidationFailedResponse(new ValidationError("id", "Movie Theater is not found")));
            await movieTheaterService.DeleteAsync(id);

            return Ok();

        }


    }
}
