using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using System.Threading.Tasks;


namespace Online_Cinema_UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IAccountService accountService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _accountService = accountService;
        }

        public async Task<IActionResult> Settings() => await Task.Run(() => { return View(); });

        [HttpGet] public async Task<IActionResult> _ProfileSettings() { return PartialView(await _userManager.GetUserAsync(User)); }
        [HttpGet] public async Task<IActionResult> _SecurityPrivacySettings() { return PartialView(await _userManager.GetUserAsync(User)); }
        [HttpGet] public async Task<IActionResult> _NotificationSettings() { return PartialView(await _userManager.GetUserAsync(User)); }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
                return await Task.FromResult(Redirect("/CinemaRoom/Index"));

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _Register() => await Task.Run(() => { return PartialView(); });

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!TryValidateModel(model)) return View("Index");
            return await _accountService.Register(model) ? Redirect("/CinemaRoom/Index") : View("Index");
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

            return Redirect("/CinemaRoom/Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePasswordAsync(string userEmail, string guid) => await Task.Run(() =>
        {
            return View(new ResetPasswordViewModel() { Email = userEmail, Guid = guid });
        });

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ConfirmResetPasswordAsync(ResetPasswordViewModel model)
        {
            if (!TryValidateModel(model)) return View();

            await _accountService.ConfirmResetPasswordAsync(model);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmAsync(string guid, string userEmail)
        {
            if (await _accountService.ConfirmAsync(guid, userEmail))
            {
                return RedirectToAction("Index", "CinemaRoom");
            }
            return View();
        }
    }
}