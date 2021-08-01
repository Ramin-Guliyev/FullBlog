using BlogApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public interface IEmailService
    {
        Task AddAsync(string email);
        IEnumerable<Email> GetAllEmails(); 
    }
}
