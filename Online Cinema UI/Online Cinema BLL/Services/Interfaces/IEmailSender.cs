using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
