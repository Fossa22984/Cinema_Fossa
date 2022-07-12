using AutoMapper;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using Online_Cinema_Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CinemaRoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<CinemaRoomViewModel>> GetListCinemaRoomAsync() =>
            _mapper.Map<List<CinemaRoom>, List<CinemaRoomViewModel>>((await _unitOfWork.CinemaRoom.GetAllCinemaRoomAsync()).ToList());

        public async Task<CinemaRoomViewModel> GetCinemaRoomAsync(int CinemaRoomId) =>
            _mapper.Map<CinemaRoom, CinemaRoomViewModel>(await _unitOfWork.CinemaRoom.GetCinemaRoomByIdAsync(CinemaRoomId));

        public async Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date)
        {
            var allSessions = await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoom.Id == cinemaRoom);
            var result = allSessions.AsEnumerable().Where(x => x.CinemaRoom.Id == cinemaRoom && x.Start.ToString("dd.MM.yyyy")
           .Contains(date.ToString("dd.MM.yyyy"), StringComparison.OrdinalIgnoreCase)).ToList();

            return _mapper.Map<List<Session>, List<SessionViewModel>>(result);
        }

        public async Task<SessionViewModel> GetSessionByIdAsync(int sessionId) =>
            _mapper.Map<Session, SessionViewModel>(await _unitOfWork.Session.GetSessionByIdAsync(sessionId));

        public async Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom)
            => _mapper.Map<List<Session>, List<SessionViewModel>>((await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoomId == cinemaRoom)).ToList());
    }
}
