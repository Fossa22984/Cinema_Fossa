using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using System.IO;
using System.Collections.Generic;

namespace Online_Cinema_UI.Controllers
{
    public class RoomController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RoomController(IAdminService adminService, IMapper mapper, UserManager<User> userManager)
        {
            _adminService = adminService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() => await Task.Run(() => { return View(); });

        public async Task<IActionResult> _RoomCard()
        {
            return PartialView(_mapper.Map<IList<Room>, IEnumerable<RoomViewModel>>(await _adminService.GetListRoomsAsync()));
        }

        [HttpGet]
        public async Task<IActionResult> Room(int id)
        {
            return View(_mapper.Map<Room, RoomViewModel>(await _adminService.GetRoomAsync(id)));
        }

        [HttpGet]
        public async Task<IActionResult> _ChangeRoom()
        {

            var room = await _adminService.GetRoomAsync((await _userManager.GetUserAsync(User)).Id);

            if (room != null)
            {
                var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
                return View(roomViewModel);

            }
            return View("/Room/Index");
        }

        public async Task ChangeRoom(CinemaRoomViewModel cinemaRoom)
        {
            if (cinemaRoom.ImageFile != null)
            {
                if (cinemaRoom.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        cinemaRoom.ImageFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        cinemaRoom.CinemaRoomImage = fileBytes;
                    }
                }
            }
            var res = _mapper.Map<CinemaRoomViewModel, CinemaRoom>(cinemaRoom);

            await _adminService.ChangeCinemaRoomAsync(res);
        }

    }
}