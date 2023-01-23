﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;

namespace CinemaHub.Services
{
    public interface IMovieService : ITextSearchService<MovieTextSearchRequest, Movie>
    {
        Task<Movie> UpdateGenresAsync(Movie movie, List<Genre> genres);
    }
}
