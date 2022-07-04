using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models;
using System;
using System.Threading.Tasks;

namespace Online_Cinema_UI.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAdminService _adminService;

        public MovieController(ILogger<MovieController> logger, IEmailSender emailSender, IAdminService adminService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _CinemaRoomCard()
        {
            return PartialView(await _adminService.GetListCinemaRoomAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CinemaRoom(int id)
        {
            return View(await _adminService.GetCinemaRoomAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> _ListSessions(int? cinemaRoomId, DateTime? dateSession, string returnUrl = "")
        {
            if (cinemaRoomId != null && dateSession != null)
                return PartialView(await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value, dateSession.Value));

            return PartialView(await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value));
        }

        public async Task<JsonResult> GetSession(int? sessionId, string returnUrl = "")
        {
            var res = await _adminService.GetSessionByIdAsync(sessionId.Value);
            Session session = new Session()
            {
                Id = res.Id,
                Start = res.Start,
                End = res.End,
                Movie = new Movie()
                {
                    MovieTitle = res.Movie.MovieTitle,
                    MoviePath = res.Movie.MoviePath
                }
            };
            return Json(session);
        }
    }
}
