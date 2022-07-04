using AutoMapper;
using Online_Cinema_Domain.Models;
using Online_Cinema_Models.View;

namespace Online_Cinema_Config.AppStart
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


        public static IMapper GetMapper()
        {
            var mapplineConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapplineConfig.CreateMapper();

            return mapper;
        }
    }
}
