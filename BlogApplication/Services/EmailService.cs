using BlogApplication.Data;
using BlogApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationContext _applicationDbContext;

        public EmailService(ApplicationContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(string email)
        {
            var addEmail = new Email() { EmailAddress = email };
            await _applicationDbContext.Emails.AddAsync(addEmail);
            await _applicationDbContext.SaveChangesAsync();
        }

        public IEnumerable<Email> GetAllEmails()
        {
            var emails = _applicationDbContext.Emails;
            return emails;
        }
    }
}
