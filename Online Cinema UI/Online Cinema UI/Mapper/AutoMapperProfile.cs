using AutoMapper;
using Online_Cinema_Domain.Models;
using Online_Cinema_UI.Models;

namespace Online_Cinema_UI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieViewModel, Movie>();
            CreateMap<Movie, MovieViewModel>();

            CreateMap<CinemaRoomViewModel, CinemaRoom>();
            CreateMap<CinemaRoom, CinemaRoomViewModel>();

            CreateMap<SessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>();
        }
    }
}