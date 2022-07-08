using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Interfaces.Observers;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_BLL.Observers.Base;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_Core.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Observers
{
    public class GetSessionObserver : BaseObserver, IGetSessionObserver
    {
        private readonly ICinemaRoomCacheManager _roomCacheManager;
        private readonly ISessionCacheManager _sessionCacheManager;
        private readonly IAdminService _adminService;
        private readonly ChatHub _chatHub;
        private readonly IUnitOfWork _unitOfWork;
        private bool initializeCache = false;
        public GetSessionObserver(IServiceProvider serviceProvider, ICinemaRoomCacheManager roomCacheManager,
            ISessionCacheManager sessionCacheManager) : base(serviceProvider, 1000, "SessionObserver")
        {
            _roomCacheManager = roomCacheManager;
            _sessionCacheManager = sessionCacheManager;
            _adminService = CreateService<IAdminService>();
            _chatHub = CreateService<ChatHub>();
            _unitOfWork = CreateService<IUnitOfWork>();
        }

        public async Task InitilizeCache()
        {
            var sessions = await _unitOfWork.Session.GetAllSessionAsync();
            _sessionCacheManager.Set(sessions.ToList());

            var rooms = await _unitOfWork.CinemaRoom.GetAllCinemaRoomAsync();
            _roomCacheManager.Set(rooms.ToList());

            initializeCache = true;
        }

        protected override async Task Process()
        {
            if (!initializeCache) await InitilizeCache();

            foreach (var room in _roomCacheManager.GetAll())
            {
                var session = await _adminService.GetSessionAsync(room.Id);
                if (session != null) await _chatHub.SendSession(session.Id, room.Id.ToString());
            }
        }
    }
}
