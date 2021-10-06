using AutoMapper;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Cinema_BLL.Services;
using Online_Cinema_Domain.Models;
using Online_Cinema_UI.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly MoviesService _moviesService;

        private readonly IMapper _mapper;
        public AdminController(AdminService adminService, MoviesService moviesService, IMapper mapper)
        {
            _adminService = adminService;
            _moviesService = moviesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index() => await Task.Run(() => { return View(); });
        public async Task<JsonResult> GetListGenre() => await Task.Run(() => { return Json(_adminService.GetListStringGenreAsync().OrderBy(x => x).ToArray()); });
        public async Task<JsonResult> GetListMovies() => await Task.Run(() => { return Json(_adminService.GetDictionaryMoviesAsync().OrderBy(x => x.Value).ToArray()); });
        public async Task<JsonResult> GetListCinemaRooms() => await Task.Run(() => { return Json(_adminService.GetDictionaryCinemaRoomsAsync().OrderBy(x => x.Value).ToArray()); });
        public async Task<JsonResult> GetListSessions() => await Task.Run(() => { return Json(_adminService.GetDictionarySessionsAsync().OrderBy(x => x.Value).ToArray()); });
        public async Task<JsonResult> GetMovieDuration(int? movieId, DateTime? start)
        {
            if (movieId != null && start != null)
                return Json(start.Value.Add((await _adminService.GetMovieAsync(movieId.Value)).Duration.Value).ToString("yyyy-MM-ddTHH:mm:ss"));

            return Json("");
        }




        #region Movie Settings
        [HttpGet] public async Task<IActionResult> _MovieSettings(string returnUrl = "") => await Task.Run(() => { return PartialView("Movie Settings/_MovieSettings"); });
        [HttpGet] public async Task<IActionResult> _AddMovie(string returnUrl = "") => await Task.Run(() => { return PartialView("Movie Settings/_AddMovie"); });
        [HttpGet]
        public async Task<IActionResult> _ChangeMovie(int? movie, string returnUrl = "")
        {
            if (movie != null)
            {
                var res = await _adminService.GetMovieAsync(movie.Value);
                if (res != null)
                {
                    var ress = _mapper.Map<Movie, MovieViewModel>(res);
                    return PartialView("Movie Settings/_ChangeMovie", ress);
                }
            }
            return PartialView("Movie Settings/_ChangeMovie");
        }
        #endregion

        #region CinemaRoom Settings
        [HttpGet] public async Task<IActionResult> _CinemaRoomSettings(string returnUrl = "") => await Task.Run(() => { return PartialView("CinemaRoom Settings/_CinemaRoomSettings"); });
        [HttpGet] public async Task<IActionResult> _AddCinemaRoom(string returnUrl = "") => await Task.Run(() => { return PartialView("CinemaRoom Settings/_AddCinemaRoom"); });
        [HttpGet]
        public async Task<IActionResult> _ChangeCinemaRoom(int? cinemaRoom, string returnUrl = "")
        {
            if (cinemaRoom != null)
            {
                var res = await _adminService.GetCinemaRoomAsync(cinemaRoom.Value);
                if (res != null)
                {
                    var ress = _mapper.Map<CinemaRoom, CinemaRoomViewModel>(res);
                    return PartialView("CinemaRoom Settings/_ChangeCinemaRoom", ress);
                }
            }
            return PartialView("CinemaRoom Settings/_ChangeCinemaRoom");
        }
        #endregion

        #region Session Setting
        [HttpGet]
        public async Task<IActionResult> _AddSession(string returnUrl = "")
        {
            return PartialView("Session Setting/_AddSession");
        }

        [HttpGet]
        public async Task<IActionResult> _ChangeSession(int? session, string returnUrl = "")
        {
            if (session != null)
            {
                var res = await _adminService.GetSessionByIdAsync(session.Value);
                if (res != null)
                {
                    var ress = _mapper.Map<Session, SessionViewModel>(res);
                    return PartialView("Session Setting/_ChangeSession", ress);
                }
            }
            return PartialView("Session Setting/_ChangeSession");


        }
        [HttpGet]
        public async Task<IActionResult> _ListSessions(int? cinemaRoomId, DateTime? dateSession, string returnUrl = "")
        {
            if (cinemaRoomId != null && dateSession != null)
                return PartialView("Session Setting/_ListSessions", await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value, dateSession.Value));

            return PartialView("Session Setting/_ListSessions", await _adminService.GetSessionsForACinemaRoomsAsync(cinemaRoomId.Value));
        }
        [HttpGet]
        public async Task<IActionResult> _SessionSettings(string returnUrl = "")
        {
            return PartialView("Session Setting/_SessionSettings");
        }
        #endregion





        public async Task AddMovie(MovieViewModel movie, string genre)
        {
            if (movie.ImageFile != null)
            {
                if (movie.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        movie.ImageFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        movie.Image = fileBytes;
                    }
                }
            }
            else
            {
                var fileInfo = new FileInfo(@".\wwwroot\Images\background-fon.jpg");
                if (fileInfo.Length > 0)
                {
                    movie.Image = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(movie.Image, 0, movie.Image.Length);
                    }

                }
            }
            var res = _mapper.Map<MovieViewModel, Movie>(movie);

            var inputFile = new MediaFile { Filename = @".\wwwroot" + movie.MoviePath };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                res.Duration = inputFile.Metadata.Duration;
            }
            //await _moviesService.AddMovieAsync(res, genre);
            await _adminService.AddFilmAsync(res, genre);
        }
        public async Task ChangeMovie(MovieViewModel movie, string genre)
        {
            if (movie.ImageFile != null)
            {
                if (movie.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        movie.ImageFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        movie.Image = fileBytes;
                    }
                }
            }
            var res = _mapper.Map<MovieViewModel, Movie>(movie);

            var inputFile = new MediaFile { Filename = @".\wwwroot" + movie.MoviePath };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                res.Duration = inputFile.Metadata.Duration;
            }
            //await _moviesService.ChangeMovieAsync(res, genre);
            await _adminService.ChangeFilmAsync(res, genre);

        }

        public async Task AddCinemaRoom(CinemaRoomViewModel cinemaRoom)
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
            else
            {
                var fileInfo = new FileInfo(@".\wwwroot\Images\background-fon.jpg");
                if (fileInfo.Length > 0)
                {
                    cinemaRoom.CinemaRoomImage = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(cinemaRoom.CinemaRoomImage, 0, cinemaRoom.CinemaRoomImage.Length);
                    }

                }
            }
            var res = _mapper.Map<CinemaRoomViewModel, CinemaRoom>(cinemaRoom);
            await _adminService.AddCinemaRoomAsync(res);
        }
        public async Task ChangeCinemaRoom(CinemaRoomViewModel cinemaRoom)
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


        public async Task AddSession(SessionViewModel session)
        {
            var res = _mapper.Map<SessionViewModel, Session>(session);
            await _adminService.AddSessionAsync(res);
        }
        public async Task ChangeSession(SessionViewModel session)
        {
            var res = _mapper.Map<SessionViewModel, Session>(session);
            await _adminService.ChangeSessionAsync(res);
        }
    }
}