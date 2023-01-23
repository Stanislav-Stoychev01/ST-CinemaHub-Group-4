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
    [Route("api/admin/movie-screenings")]
    public class MovieScreeningAdminController :Controller
    {
        private readonly IMovieScreeningService movieScreeningService;
        private readonly IMovieTheaterService movieTheaterService;
        private readonly IMovieService movieService;
        private readonly IMapper mapper;

        public MovieScreeningAdminController(IMapper mapper, IMovieScreeningService movieScreeningService, IMovieTheaterService movieTheaterService, IMovieService movieService)
        {
            this.mapper = mapper;
            this.movieScreeningService = movieScreeningService;
            this.movieTheaterService = movieTheaterService;
            this.movieService = movieService;
        }


        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<MovieScreeningResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllAsync([FromQuery] MovieScreeningTextSearchRequest request)
        {
            var totalCount = await movieScreeningService.CountAsync(request);
            var searchResults = await movieScreeningService.SearchAsync(request);

            var mappedResponse = searchResults.Select(x => mapper.Map<MovieScreeningResponse>(x)).ToList();

            var response = new PaginatedResponse<MovieScreeningResponse>(mappedResponse,
                request?.StartAt ?? 0,
                totalCount,
                request?.SortFields);

            return Ok(response);
        }

        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(MovieTheaterResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateMovieScreeningAsync([FromBody] MovieScreeningRequest request)
        {
            var movieScreening = mapper.Map<MovieScreening>(request);
            var theater = await movieTheaterService.GetByIdAsync(request.TheaterId);
            if (theater == null) throw new ValidationException(new ValidationError("theaterId", "Theater not found"));
            movieScreening.Theater = theater;

            var movie = await movieService.GetByIdAsync(request.MovieId);
            if (movie == null) throw new ValidationException(new ValidationError("movieId", "Theater not found"));
            movieScreening.Movie = movie;

            movieScreening = await movieScreeningService.AddAsync(movieScreening);
            var response = mapper.Map<MovieScreeningResponse>(movieScreening);

            return Ok(response);

        }

        [HttpPut("{id}")]
        [SwaggerResponse(200, "Success", typeof(MovieTheaterResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateMovieScreeningAsync(Guid id, [FromBody] MovieScreeningRequest request)
        {
            var movieScreening = await movieScreeningService.GetByIdAsync(id);
            if (movieScreening == null)
                throw new ValidationException(new ValidationError("id", "Movie Screening not found"));
            
            mapper.Map(request, movieScreening);

            if (movieScreening.Movie.Id != request.MovieId)
            {
                var movie = await movieService.GetByIdAsync(request.MovieId);
                if (movie == null) throw new ValidationException(new ValidationError("movieId", "Theater not found"));
                movieScreening.Movie = movie;
            }

            if (movieScreening.Theater.Id != request.TheaterId)
            {
                var theater = await movieTheaterService.GetByIdAsync(request.TheaterId);
                if (theater == null)
                    throw new ValidationException(new ValidationError("theaterId", "Theater not found"));
                movieScreening.Theater = theater;
            }
            
            movieScreening = await movieScreeningService.UpdateAsync(id, movieScreening);
            var response = mapper.Map<MovieScreeningResponse>(movieScreening);

            return Ok(response);
        }
    }
}
