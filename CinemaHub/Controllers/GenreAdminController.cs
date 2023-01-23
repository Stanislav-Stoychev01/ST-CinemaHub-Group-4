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
    [Route("api/admin/genres")]
    [Authorize(policy: "AdminOnly")]
    public class GenreAdminController : Controller
    {
        private readonly IGenreService genreService;
        private readonly IMapper mapper;

        public GenreAdminController(IGenreService genreService, IMapper mapper)
        {
            this.genreService = genreService;
            this.mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<GenreResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextSearchRequest request)
        {
            var totalCount = await genreService.CountAsync(request);
            var searchResults = await genreService.SearchAsync(request);

            var mappedResponse = searchResults.Select(x => mapper.Map<GenreResponse>(x)).ToList();

            var response = new PaginatedResponse<GenreResponse>(mappedResponse,
                request?.StartAt ?? 0,
                totalCount,
                request?.SortFields);

            return Ok(response);
        }
        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(GenreResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateGenreAsync([FromBody] GenreRequest request)
        {
            var genre = mapper.Map<Genre>(request);

            genre = await genreService.AddAsync(genre);
            var response = mapper.Map<GenreResponse>(genre);

            return Ok(response);

        }
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Success", typeof(GenreResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateGenreAsync(Guid id, [FromBody] GenreRequest request)
        {
            var genre = await genreService.GetByIdAsync(id);
            if (genre == null)
                throw new ValidationException(new ValidationError("id", "Genre is not found"));

            mapper.Map(request, genre);

            genre = await genreService.UpdateAsync(id, genre);
            var response = mapper.Map<GenreResponse>(genre);

            return Ok(response);

        }

        [HttpDelete("{id}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> DeleteGenreAsync(Guid id)
        {
            var movieTheater = await genreService.GetByIdAsync(id);
            if (movieTheater == null)
                return BadRequest(
                    new ValidationFailedResponse(new ValidationError("id", "Genre is not found")));
            await genreService.DeleteAsync(id);

            return Ok();

        }
    }
}
