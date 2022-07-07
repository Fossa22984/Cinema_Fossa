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
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RoomController(IRoomService roomService, IMapper mapper, UserManager<User> userManager)
        {
            _roomService = roomService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() => await Task.Run(() => { return View(); });

        public async Task<IActionResult> _RoomCard()
        {
            return PartialView(await _roomService.GetListRoomsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Room(int id)
        {
            return View(await _roomService.GetRoomAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> _ChangeRoom()
        {

            var room = await _roomService.GetRoomAsync((await _userManager.GetUserAsync(User)).Id);

            if (room != null)
            {
                return View(room);
            }
            return View("/Room/Index");
        }
    }
}