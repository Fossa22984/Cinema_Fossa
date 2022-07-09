using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using OnlineCinema_Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public RoomService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IList<RoomViewModel>> GetListRoomsAsync()
        {
            var rooms = (await _unitOfWork.Room.GetRoomByConditionAsync(x => x.IsRemoved != true && x.IsOpen == true)).ToList();
            rooms.ForEach(x => x.Owner = _userManager.Users.FirstOrDefault(u => u.Id == x.OwnerId));



            //return _mapper.Map<List<Room>, List<RoomViewModel>>((await _unitOfWork.Room.GetRoomByConditionAsync(x => x.IsRemoved != true && x.IsOpen == true)).ToList());
            return _mapper.Map<List<Room>, List<RoomViewModel>>(rooms);

        }
        public async Task<RoomViewModel> GetRoomAsync(int roomId) =>
            _mapper.Map<Room, RoomViewModel>(await _unitOfWork.Room.GetRoomByIdAsync(roomId));
        public async Task<RoomViewModel> GetRoomAsync(Guid userId) =>
            _mapper.Map<Room, RoomViewModel>((await _unitOfWork.Room.GetRoomByConditionAsync(x => x.OwnerId == userId)).FirstOrDefault());


        public async Task<IDictionary<int, string>> GetDictionaryMoviesAsync()
        {
            var listMovies = await _unitOfWork.Movie.GetAllMovieAsync();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listMovies)
                dictionary.Add(item.Id, $"{item.MovieTitle} (#{item.Id})");

            return dictionary;
        }

        public async Task<MovieViewModel> GetMovieAsync(int movieId) =>
            _mapper.Map<Movie, MovieViewModel>(await _unitOfWork.Movie.GetMovieByIdAsync(movieId));


        public async Task ChangeRoomAsync(RoomViewModel roomView)
        {
            if (roomView.ImageFile != null)
            {
                if (roomView.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        roomView.ImageFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        roomView.RoomImage = fileBytes;
                    }
                }
            }
            var room = _mapper.Map<RoomViewModel, Room>(roomView);

            if (room.RoomImage.Length == 0)
                room.RoomImage = (await _unitOfWork.Room.GetRoomByIdAsync(room.Id)).RoomImage;

            await _unitOfWork.Room.UpdateRoom(room);
            await _unitOfWork.SaveAsync();
            Log.Current.Debug($"Change room -> {room.RoomName} room id-> {room.Id}");
        }

    }
}
