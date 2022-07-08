using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<User> _userManager;
        public AdminController(IAdminService adminService, UserManager<User> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() => await Task.Run(() => { return View(); });

        #region Movie Settings
        [DisableRequestSizeLimit]
        public async Task AddMovie(MovieViewModel movie, string genre)
        {
            var id = (await _userManager.GetUserAsync(User)).UserName;
            await _adminService.AddFilmAsync(movie, genre, movie.VideoFile, id);
        }

        public async Task ChangeMovie(MovieViewModel movie, string genre)
        {
            await _adminService.ChangeFilmAsync(movie, genre);
        }

        [HttpGet] public async Task<IActionResult> _MovieSettings() => await Task.Run(() => { return PartialView("Movie Settings/_MovieSettings"); });
        [HttpGet] public async Task<IActionResult> _AddMovie() => await Task.Run(() => { return PartialView("Movie Settings/_AddMovie"); });
        [HttpGet]
        public async Task<IActionResult> _ChangeMovie(int? movie)
        {
            if (movie != null)
            {
                var movieView = await _adminService.GetMovieAsync(movie.Value);
                if (movieView != null)
                    return PartialView("Movie Settings/_ChangeMovie", movieView);
            }
            return PartialView("Movie Settings/_ChangeMovie");
        }
        #endregion

        #region CinemaRoom Settings
        public async Task AddCinemaRoom(CinemaRoomViewModel cinemaRoom)
        {
            await _adminService.AddCinemaRoomAsync(cinemaRoom);
        }

        public async Task ChangeCinemaRoom(CinemaRoomViewModel cinemaRoom)
        {
            await _adminService.ChangeCinemaRoomAsync(cinemaRoom);
        }


        [HttpGet] public async Task<IActionResult> _CinemaRoomSettings() => await Task.Run(() => { return PartialView("CinemaRoom Settings/_CinemaRoomSettings"); });
        [HttpGet] public async Task<IActionResult> _AddCinemaRoom() => await Task.Run(() => { return PartialView("CinemaRoom Settings/_AddCinemaRoom"); });
        [HttpGet]
        public async Task<IActionResult> _ChangeCinemaRoom(int? cinemaRoom)
        {
            if (cinemaRoom != null)
            {
                var cinemaRoomView = await _adminService.GetCinemaRoomAsync(cinemaRoom.Value);
                if (cinemaRoomView != null)
                    return PartialView("CinemaRoom Settings/_ChangeCinemaRoom", cinemaRoomView);
            }
            return PartialView("CinemaRoom Settings/_ChangeCinemaRoom");
        }
        #endregion

        #region Session Setting
        public async Task AddSession(SessionViewModel session)
        {
            await _adminService.AddSessionAsync(session);
        }

        public async Task ChangeSession(SessionViewModel session)
        {
            await _adminService.ChangeSessionAsync(session);
        }

        [HttpGet]
        public async Task<IActionResult> _AddSession()
        {
            return await Task.FromResult(PartialView("Session Setting/_AddSession"));
        }

        [HttpGet]
        public async Task<IActionResult> _ChangeSession(int? session)
        {
            if (session != null)
            {
                var sessionView = await _adminService.GetSessionByIdAsync(session.Value);
                if (sessionView != null)
                    return PartialView("Session Setting/_ChangeSession", sessionView);

            }
            return PartialView("Session Setting/_ChangeSession");


        }
        [HttpGet]
        public async Task<IActionResult> _ListSessions(int? cinemaRoomId, DateTime? dateSession)
        {
            if (cinemaRoomId != null && dateSession != null)
                return PartialView("Session Setting/_ListSessions", await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value, dateSession.Value));

            return PartialView("Session Setting/_ListSessions", await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value));
        }
        [HttpGet]
        public async Task<IActionResult> _SessionSettings()
        {
            return await Task.FromResult(PartialView("Session Setting/_SessionSettings"));
        }
        #endregion

        #region Json Response

        public async Task<JsonResult> GetListGenre() => Json(await _adminService.GetListStringGenreAsync());
        public async Task<JsonResult> GetListMovies() => Json((await _adminService.GetDictionaryMoviesAsync()).OrderBy(x => x.Value).ToArray());
        public async Task<JsonResult> GetListCinemaRooms() => Json((await _adminService.GetDictionaryCinemaRoomsAsync()).OrderBy(x => x.Value).ToArray());
        public async Task<JsonResult> GetListSessions() => Json((await _adminService.GetDictionarySessionsAsync()).OrderBy(x => x.Value).ToArray());
        public async Task<JsonResult> GetMovieDuration(int? movieId, DateTime? start)
        {
            if (movieId != null && start != null)
                return Json(start.Value.Add((await _adminService.GetMovieAsync(movieId.Value)).Duration.Value).ToString("yyyy-MM-ddTHH:mm:ss"));

            return Json("");
        }
        #endregion
    }
}