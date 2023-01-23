using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;
using CinemaHub.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/movies")]
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IMovieScreeningService movieScreeningService;
        private readonly IMapper mapper;

        public MovieController(IMovieService movieService, IMovieScreeningService movieScreeningService, IMapper mapper)
        {
            this.movieService = movieService;
            this.movieScreeningService = movieScreeningService;
            this.mapper = mapper;
        }

        [HttpGet("premiere")]
        [SwaggerResponse(200, "Success", typeof(MovieResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetPremiereMovieAsync()
        {
            var premiere = await movieScreeningService.SearchAsync(new MovieScreeningTextSearchRequest()
            {
                IsPremiere = true,
                SortFields = new List<string>() {"-CreatedOn"},
                Count = 1
            });
            if (!premiere.Any()) return Ok();

            var movie = mapper.Map<MovieResponse>(premiere.First().Movie);

            return Ok(movie);
        }

        [HttpGet("latest")]
        [SwaggerResponse(200, "Success", typeof(List<MovieResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetLatestMovieAsync()
        {
            var movies = await movieService.SearchAsync(new MovieTextSearchRequest()
            {
                SortFields = new List<string>() { "-CreatedOn" },
                Count = 6
            });
            if (!movies.Any()) return Ok();

            var response = movies.Select(x=>mapper.Map<MovieResponse>(x));

            return Ok(response);
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(List<MovieResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllMovieAsync()
        {
            var movies = await movieService.SearchAsync(new MovieTextSearchRequest()
            {
                SortFields = new List<string>() { "-CreatedOn" }
            });
            if (!movies.Any()) return Ok();

            var response = movies.Select(x => mapper.Map<MovieResponse>(x));

            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, "Success", typeof(List<MovieResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetMovieAsync(Guid id)
        {
            var movie =await movieService.GetByIdAsync(id);
            if (movie == null) return Ok();
           
            var response = mapper.Map<MovieDetailResponse>(movie);

            var premiere = await movieScreeningService.SearchAsync(new MovieScreeningTextSearchRequest()
            {
                IsPremiere = true,
                MovieId = id
            });
            if (premiere.Any())
            {
                response.Premiere = premiere.First().DateTime.ToString("dd.MM.yyyy");
            }

            var screenings = await movieScreeningService.SearchAsync(new MovieScreeningTextSearchRequest()
            {
                MovieId = id
            });
            var array = new List<string>();
            if (screenings.Any(x => x.Type == ScreeningType._2D))
            {
                array.Add("2D");
            }
            if (screenings.Any(x => x.Type == ScreeningType._3D))
            {
                array.Add("3D");
            }

            response.ScreeningTypes = string.Join(',', array);

            return Ok(response);
        }

        [HttpGet("{id}/screenings")]
        [SwaggerResponse(200, "Success", typeof(List<MovieScreeningResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetMovieScreeningsAsync(Guid id, [FromQuery] PaginatedRequest paginatedRequest)
        {
            var request = new MovieScreeningTextSearchRequest()
            {
                SortFields = new List<string>() {"+DateTime"},
                Count = paginatedRequest.Count,
                StartAt = paginatedRequest.StartAt,
                MovieId = id,
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
