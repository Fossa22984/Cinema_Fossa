using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Cinema_BLL.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Online_Cinema_UI.Controllers
{
    [Authorize]
    public class CinemaRoomController : Controller
    {
        private readonly ICinemaRoomService _cinemaRoomService;
        public CinemaRoomController(ICinemaRoomService cinemaRoomService)
        {
            _cinemaRoomService = cinemaRoomService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _CinemaRoomCard()
        {
            return PartialView(await _cinemaRoomService.GetListCinemaRoomAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CinemaRoom(int id)
        {
            return View(await _cinemaRoomService.GetCinemaRoomAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> _ListSessions(int? cinemaRoomId, DateTime? dateSession)
        {
            if (cinemaRoomId != null && dateSession != null)
                return PartialView(await _cinemaRoomService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value, dateSession.Value));

            return PartialView(await _cinemaRoomService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value));
        }

        public async Task<JsonResult> GetSession(int? sessionId)
        {
            var sessionView = await _cinemaRoomService.GetSessionByIdAsync(sessionId.Value);
            return Json(sessionView);
        }
    }
}
