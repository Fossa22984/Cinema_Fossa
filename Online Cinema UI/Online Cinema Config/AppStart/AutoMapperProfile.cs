using AutoMapper;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;

namespace Online_Cinema_Config.AppStart
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, RegisterViewModel>();
            CreateMap<RegisterViewModel, User>();

            CreateMap<MovieViewModel, Movie>();
            CreateMap<Movie, MovieViewModel>();

            CreateMap<CinemaRoomViewModel, CinemaRoom>();
            CreateMap<CinemaRoom, CinemaRoomViewModel>();

            CreateMap<SessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>();

            CreateMap<GenreViewModel, Genre>();
            CreateMap<Genre, GenreViewModel>();

            CreateMap<RoomViewModel, Room>();
            CreateMap<Room, RoomViewModel>().ForMember(x => x.Owner, c => c.MapFrom(xc => xc.Owner.UserName));
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
