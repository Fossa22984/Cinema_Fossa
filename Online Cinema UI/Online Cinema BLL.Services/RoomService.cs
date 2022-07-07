using AutoMapper;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using Online_Cinema_Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<RoomViewModel>> GetListRoomsAsync() =>
            _mapper.Map<List<Room>, List<RoomViewModel>>((await _unitOfWork.Room.GetRoomByConditionAsync(x => x.IsRemoved != true)).ToList());
        public async Task<RoomViewModel> GetRoomAsync(int roomId) =>
            _mapper.Map<Room, RoomViewModel>(await _unitOfWork.Room.GetRoomByIdAsync(roomId));
        public async Task<RoomViewModel> GetRoomAsync(Guid userId) =>
            _mapper.Map<Room, RoomViewModel>((await _unitOfWork.Room.GetRoomByConditionAsync(x => x.OwnerId == userId)).FirstOrDefault());
    }
}
