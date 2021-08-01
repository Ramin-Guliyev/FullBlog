using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public interface IMessageService
    {
        Task EmailSenderAsync(string email, string subject, string htmlMessage);
    }
}
