using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Online_Cinema_UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IEmailSender emailSender)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._emailSender = emailSender;
        }

        //[HttpGet]
        public async Task<IActionResult> Settings() => await Task.Run(() => { return View(); });

        [HttpGet] public async Task<IActionResult> _ProfileSettings() { return PartialView(await _userManager.GetUserAsync(User)); }
        [HttpGet] public async Task<IActionResult> _SecurityPrivacySettings() { return PartialView(await _userManager.GetUserAsync(User)); }
        [HttpGet] public async Task<IActionResult> _NotificationSettings() { return PartialView(await _userManager.GetUserAsync(User)); }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Add Default Admin
            if (!_userManager.Users.Where(x => x.UserName == "Admin").Any())
            {
                var admin = new User() { Email = "my.code.fossa@gmail.com", EmailConfirmed = true, UserName = "Admin" };
                var fileInfo = new FileInfo(@".\wwwroot\Images\background-fon.jpg");
                if (fileInfo.Length > 0)
                {
                    admin.Photo = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(admin.Photo, 0, admin.Photo.Length);
                    }
                }
                var res = await _userManager.CreateAsync(admin, "31415926535@qAZ");
                await _userManager.AddToRolesAsync(admin, new List<string>() { "Admin", "User" });
            }

            if (User.Identity.IsAuthenticated)
                return Redirect("/CinemaRoom/Index");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _Register() => await Task.Run(() => { return PartialView(); });

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!TryValidateModel(model)) return View("Index");

            var user = new User() { Email = model.Email, UserName = model.Username, Birthday = model.Birthday };

            var fileInfo = new FileInfo(@".\wwwroot\Images\background-fon.jpg");
            if (fileInfo.Length > 0)
            {
                user.Photo = new byte[fileInfo.Length];
                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(user.Photo, 0, user.Photo.Length);
                }

            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return View("Index");

            if (await _roleManager.FindByNameAsync("user") == null)
            {
                var role = await _roleManager.CreateAsync(new Role() { Name = "user" });
                if (role.Succeeded)
                    await _userManager.AddToRoleAsync(user, "user");
            }
            else await _userManager.AddToRoleAsync(user, "user");

            await _signInManager.SignInAsync(user, false);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("Confirm", "Account",
                new { guid = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value);
            await _emailSender.SendEmailAsync(user.Email, "Link ->>>", link);

            return Redirect("/CinemaRoom/Index");
        }

        [HttpGet]
        public async Task<IActionResult> _Login() => await Task.Run(() => { return PartialView(); });

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!TryValidateModel(model)) return View("Index");

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "CinemaRoom");
            }
            return View("Index");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("ChangePassword", "Account",
                new { guid = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value);
            await _emailSender.SendEmailAsync(user.Email, "Link ->>>", link);

            // add Send View 
            return Redirect("/CinemaRoom/Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePasswordAsync(string userEmail, string guid) => await Task.Run(() => { return View(new ResetPasswordViewModel() { Email = userEmail, Guid = guid }); });

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ConfirmResetPasswordAsync(ResetPasswordViewModel model)
        {
            if (!TryValidateModel(model)) return View();

            var user = await _userManager.FindByEmailAsync(model.Email);
            var res = await _userManager.ResetPasswordAsync(user, model.Guid, model.Password);
            //todo add view changePassword Success
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmAsync(string guid, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(email: userEmail);
            var res = await _userManager.ConfirmEmailAsync(user, guid);
            if (res.Succeeded)
            {
                return RedirectToAction("Index", "CinemaRoom");
            }
            //todo add ERROR PAGE
            return View();
        }
    }


    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}