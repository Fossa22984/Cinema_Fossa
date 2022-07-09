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
        public async Task<JsonResult> GetListMovies() => Json((await _roomService.GetDictionaryMoviesAsync()).OrderBy(x => x.Value).ToArray());
        public async Task<IActionResult> _RoomCard()
        {
            var res = await _roomService.GetListRoomsAsync();
            return PartialView(await _roomService.GetListRoomsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Room(int id)
        {
            return View("Room", await _roomService.GetRoomAsync(id));
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

        [HttpGet]
        public async Task<IActionResult> _FilmCard(int? movie)
        {
            if (movie != null)
            {
                var film = await _roomService.GetMovieAsync(movie.Value);
                if (film != null)
                {
                    return PartialView("_FilmCard", film);
                }
            }
            return PartialView("_FilmCard");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangeRoom(RoomViewModel room)
        {
            await _roomService.ChangeRoomAsync(room);
            return RedirectToAction("Room", "Room", new { id = room.Id });
        }
    }
}