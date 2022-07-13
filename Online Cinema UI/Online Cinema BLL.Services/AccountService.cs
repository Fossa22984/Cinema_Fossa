using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using OnlineCinema_Core.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class AccountService : IAccountService
    {
        IMapper _mapper;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        RoleManager<Role> _roleManager;
        public AccountService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var user = _mapper.Map<RegisterViewModel, User>(model);

            var fileInfo = new FileInfo(DefaultRootHelper.Current.DefaultIconPath);
            if (fileInfo.Length > 0)
            {
                user.Photo = new byte[fileInfo.Length];
                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(user.Photo, 0, user.Photo.Length);
                }
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return false;

            if (await _roleManager.FindByNameAsync("user") == null)
            {
                var role = await _roleManager.CreateAsync(new Role() { Name = "user" });
                if (role.Succeeded)
                    await _userManager.AddToRoleAsync(user, "user");
            }
            else await _userManager.AddToRoleAsync(user, "user");

            await _signInManager.SignInAsync(user, false);

            return true;

            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var link = Url.Action("Confirm", "Account",
            //    new { guid = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value);
            //await _emailSender.SendEmailAsync(user.Email, "Link ->>>", link);
        }


        public async Task<bool> ConfirmResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ResetPasswordAsync(user, model.Guid, model.Password);

            return result.Succeeded ? true : false;
        }

        public async Task<bool> ConfirmAsync(string guid, string userEmail)
        {

            var user = await _userManager.FindByEmailAsync(email: userEmail);
            var result = await _userManager.ConfirmEmailAsync(user, guid);

            return result.Succeeded ? true : false;
        }
    }
}
