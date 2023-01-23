using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Domain;

namespace CinemaHub.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewUserRequest, ApplicationUser>();
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<MovieTheaterRequest, MovieTheater>();
            CreateMap<MovieTheater, MovieTheaterResponse>();
            CreateMap<GenreRequest, Genre>();
            CreateMap<Genre, GenreResponse>();
            CreateMap<MovieRequest, Movie>()
                .ForMember(x => x.ImageUrl, o => o.Ignore())
                .ForMember(x => x.MovieGenres, o => o.Ignore());
            CreateMap<Movie, MovieResponse>()
                .ForMember(x => x.Genres, o => o.MapFrom(m=>m.MovieGenres.Select(p=>p.Genre.Name)))
                .Include<Movie, MovieDetailResponse>();

            CreateMap<Movie, MovieDetailResponse>();

            CreateMap<MovieScreening, MovieScreeningResponse>()
                .ForMember(x=>x.Type, o=>o.MapFrom(m=>m.Type == ScreeningType._2D? "2D": "3D"));
            CreateMap<MovieScreeningRequest, MovieScreening>()
                .ForMember(x=>x.Type, o=>o.MapFrom(m=>m.Type == "2D"? ScreeningType._2D:ScreeningType._3D ));

            CreateMap<TicketPrice, TicketPriceResponse>()
                .ForMember(x => x.ScreeningType, o => o.MapFrom(m => m.ScreeningType == ScreeningType._2D? "2D" : "3D"));
        }
      
    }
}
