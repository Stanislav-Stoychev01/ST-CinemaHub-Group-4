using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Blobs;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;
using CinemaHub.Services;
using CinemaHub.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SkiaSharp;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/admin/movies")]
    [Authorize(policy: "AdminOnly")]
    public class MovieAdminController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<MovieAdminController> logger;

        public MovieAdminController(IMovieService movieService, IMapper mapper, IGenreService genreService, IHostingEnvironment environment, ILogger<MovieAdminController> logger)
        {
            this.movieService = movieService;
            this.mapper = mapper;
            this.genreService = genreService;
            _environment = environment;
            this.logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<MovieResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllMoviesAsync([FromQuery] MovieTextSearchRequest request)
        {

            var count = await movieService.CountAsync(request);
            var search = await movieService.SearchAsync(request);

            var movies = search.Select(x => mapper.Map<MovieResponse>(x)).ToList();
            
            var response = new PaginatedResponse<MovieResponse>(movies,
                request?.StartAt ?? 0,
                count,
                request?.SortFields);

            return Ok(response);
        }

        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(MovieResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateMovieAsync([FromBody] MovieRequest movieRequest)
        {
            
            var movie = mapper.Map<Movie>(movieRequest);
            var genres = await GetGenresAsync(movieRequest.Genres);
            movie = await movieService.AddAsync(movie);
            movie = await movieService.UpdateGenresAsync(movie, genres);
            if (!string.IsNullOrEmpty(movieRequest.ImageUrl))
            {
                var url = await SavePostImageAsync(movieRequest.ImageUrl);
                movie.ImageUrl = url;
                await movieService.UpdateAsync(movie.Id, movie);
            }
            var response = mapper.Map<MovieResponse>(movie);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [SwaggerResponse(200, "Success", typeof(MovieResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateMovieAsync(Guid id, [FromBody] MovieRequest movieRequest)
        {
            var movie =await movieService.GetByIdAsync(id);
            if (movie == null)
                throw new ValidationException(new ValidationError("id", "Movie not found"));

            mapper.Map(movieRequest, movie);

            var genres = await GetGenresAsync(movieRequest.Genres);
            movie = await movieService.UpdateGenresAsync(movie, genres);
            if (!string.IsNullOrEmpty(movieRequest.ImageUrl) && movieRequest.ImageUrl.StartsWith("data:image"))
            {
                if (!string.IsNullOrEmpty(movie.ImageUrl))
                {
                   // DeleteImage(movie.ImageUrl);
                }
                var url = await SavePostImageAsync(movieRequest.ImageUrl);
                movie.ImageUrl = url;
                await movieService.UpdateAsync(id, movie);
            }

            var response = mapper.Map<MovieResponse>(movie);

            return Ok(response);
        }

        private async Task<List<Genre>> GetGenresAsync(List<string> genreNames)
        {
            var genres = new List<Genre>();
            foreach (var genreName in genreNames)
            {
                var name = genreName.Trim();
                if(string.IsNullOrEmpty(name)) continue;
                var results = await genreService.SearchAsync(new TextSearchRequest() { SearchText = name });
                var genre = results.FirstOrDefault();
                if (genre != null) genres.Add(genre);
            }

            return genres;
        } 

        private async Task<string> SavePostImageAsync(string image)
        {
            var folderName = Application.BlobStorageUrl;
            var base64 = GetImageTypeFromJavaScriptImage(image);
            var uniqueFileName = FileHelper.GetUniqueFileName() + ".jpg";
            
            try
            {
                var container = new BlobContainerClient(Application.BlobStorageConnectionString, "image-container");
                
                var imageBytes = Convert.FromBase64String(base64);
                using (var img=  Image.Load(imageBytes))
                {
                    var blob = container.GetBlobClient(uniqueFileName);
                    var stream = new MemoryStream();
                    await img.SaveAsync(stream, new JpegEncoder());
                    stream.Position = 0;
                    await blob.UploadAsync(stream);
                }
                return Path.Combine(folderName, uniqueFileName);
            }
            catch (Exception e)
            {
                logger.LogError(new EventId(1), e, "Saving image has failed");
                throw new ValidationException(new ValidationError("image", "Saving image has failed"));
            }

        }

        private void DeleteImage(string path)
        {
            var filePath = Path.Combine(_environment.WebRootPath, path);
            System.IO.File.Delete(filePath);
        }
        private String GetImageTypeFromJavaScriptImage(String javaScriptBase64String)
        {
            return Regex.Match(javaScriptBase64String, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        }

        }
}
