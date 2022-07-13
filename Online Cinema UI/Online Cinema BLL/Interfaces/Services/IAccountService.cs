using Online_Cinema_Models.View;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<bool> ConfirmResetPasswordAsync(ResetPasswordViewModel model);
        Task<bool> ConfirmAsync(string guid, string userEmail);
    }
}
